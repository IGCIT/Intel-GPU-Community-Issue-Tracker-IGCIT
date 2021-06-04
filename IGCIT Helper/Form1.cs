using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace IGCIT_Helper {
    public partial class Form1 : Form {
        RegistryKey _localMachine;
        ToolTip _cpTooltip;

        public Form1() {
            InitializeComponent();

            _localMachine = Registry.LocalMachine;
            _cpTooltip = new ToolTip();
        }

        private void Form1_Load(object sender, EventArgs e) {
            ManagementClass cs = new ManagementClass("Win32_ComputerSystem");
            ManagementClass os = new ManagementClass("Win32_OperatingSystem");

            ActiveControl = label2; // unfocus the textbox!
            winbuild.Text = GetWindowsBuildVersion();
            cpuname.Text = GetProcessorName();
            gpudrvver.Text = GetGPUDriverVersion();

            foreach (ManagementObject mo in cs.GetInstances()) {
                dmodelT.Text = mo["Model"].ToString();
                dmanufT.Text = mo["Manufacturer"].ToString();
            }

            foreach (ManagementObject mo in os.GetInstances())
                ramT.Text = String.Format("{0:0.#} GB", (ulong)mo["TotalVisibleMemorySize"] / 1024 / 1024f);
        }

        private void Form1_Shown(object sender, EventArgs e) {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length <= 1)
                return;

            if (args[1].Equals("eminidump"))
                enableMiniDump();
        }

        private String GetRegistryPath(in String path) {
            String[] pathAr = path.Split('\\');
            String ret = "";

            for (int i = 0, l = pathAr.Length; i < l; ++i) {
                if (pathAr[i] == "" || String.Equals(pathAr[i], "registry", StringComparison.OrdinalIgnoreCase))
                    continue;
                else if (String.Equals(pathAr[i], "machine", StringComparison.OrdinalIgnoreCase))
                    ret += "HKEY_LOCAL_MACHINE";
                else if (String.Equals(pathAr[i], "system", StringComparison.OrdinalIgnoreCase))
                    ret += "\\SYSTEM";
                else
                    ret += "\\" + pathAr[i]; // wrong?
            }

            return ret;
        }

        private String GetWindowsBuildVersion() {
            object buildN = null;
            object version = null;

            try {
                RegistryKey registryWinBuildKey = _localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

                buildN = registryWinBuildKey.GetValue(@"CurrentBuildNumber");
                version = registryWinBuildKey.GetValue(@"DisplayVersion");

            } catch (Exception e) {
                MessageBox.Show(this, "Unable to get Windows build number.\n\n" + e.Message, "GetWindowsBuildVersion() Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (buildN ?? "Unknown") + " (" + (version ?? "Unknown") + ")";
        }

        private String GetProcessorName() {
            object cpu = null;

            try {
                cpu = _localMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0").GetValue("ProcessorNameString");

            } catch (Exception e) {
                MessageBox.Show(this, "Unable to get Processor name.\n\n" + e.Message, "GetProcessorName() Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return cpu == null ? "Unknown" : cpu.ToString();
        }

        private String GetGPUDriverVersion() {
            string ret = "Unknown";

            try {
                RegistryKey regGpus = _localMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\VIDEO");

                if (regGpus == null)
                    return ret;

                foreach (string k in regGpus.GetValueNames()) {
                    if (!k.Contains("Video"))
                        continue;

                    object gpuPath = regGpus.GetValue(k);
                    object gpu, drvVer;
                    string regGPUPath;

                    if (gpuPath == null)
                        continue;

                    regGPUPath = GetRegistryPath(gpuPath.ToString()).Replace("\\", "\\");

                    gpu = Registry.GetValue(regGPUPath, "DriverDesc", null);
                    if (gpu == null || gpu.ToString().IndexOf("intel", StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    drvVer = Registry.GetValue(regGPUPath, "DriverVersion", null);
                    if (drvVer == null)
                        break;

                    ret = drvVer.ToString();
                    break;
                }

            } catch (Exception e) {
                MessageBox.Show(this, "Unable to get GPU driver version.\n\n" + e.Message, "GetGPUDriverVersion() Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ret;
        }

        private void enableMiniDump() {
            try {
                RegistryKey crashControl = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl", true);
                RegistryKey memManagment = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", true);
                object crashDumpT = crashControl.GetValue("CrashDumpEnabled");
                object pageFile = memManagment.GetValue("PagingFiles");
                bool setPagefile = true;
                bool reqReboot = false;
                DialogResult ret;

                if (pageFile != null) {
                    string[] pageFileAr = (string[])pageFile;
                    bool isDefault = false;
                    bool adv = false;
                    int min = 0;
                    int max = 0;

                    if (pageFileAr.Length > 1) {
                        ret = MessageBox.Show(this, "It seems you have an advanced PageFile configuration!\n\n" +
                                                    "IGCIT Helper has no support for this, make sure you have a 2Mb or more pagefile enabled.\n\n" +
                                                    "Press No to cancel the operation.\n" +
                                                    "Press Yes to continue and enable small memory dumps.",
                                                    "Enable small memory dump", MessageBoxButtons.YesNo);

                        if (ret == DialogResult.No)
                            return;

                        adv = true;

                    } else if (pageFileAr.Length == 1 && pageFileAr[0].Equals(@"?:\pagefile.sys")) {
                        isDefault = true;

                    } else if (pageFileAr.Length == 1 && pageFileAr[0] != "") {
                        string[] pageFileData = pageFileAr[0].Split(' ');

                        if (pageFileData.Length == 3) {
                            Int32.TryParse(pageFileData[1], out min);
                            Int32.TryParse(pageFileData[2], out max);
                        }
                    }

                    setPagefile = !adv && !isDefault && (pageFileAr.Length == 0 || min < 2 || max < 2);
                }

                if (setPagefile) {
                    ret = MessageBox.Show(this, "Small memory dumps require a 2Mb or more pagefile.\n" +
                                                "IGCIT Helper will now enable automatic pagefile on your system.\n\n" +
                                                "Press No to cancel this operation.\n" +
                                                "Press Yes to continue and enable small memory dumps.",
                                                "Enable small memory dump", MessageBoxButtons.YesNo);

                    if (ret == DialogResult.No)
                        return;

                    memManagment.SetValue("PagingFiles", new string[] { @"?:\pagefile.sys" }, RegistryValueKind.MultiString);
                    reqReboot = true;
                }

                if (crashDumpT == null || (int)crashDumpT != 3) {
                    crashControl.SetValue("CrashDumpEnabled", 3, RegistryValueKind.DWord);
                    reqReboot = true;
                }

                if (!reqReboot) {
                    MessageBox.Show(this, "Windows small memory dump is already enabled!", "Enable small memory dumps", MessageBoxButtons.OK);
                    return;
                }

                ret = MessageBox.Show(this, "A reboot is required for the changes to take effect\n\nDo you want to reboot now?", "Enable small memory dump", MessageBoxButtons.YesNo);
                if (ret == DialogResult.Yes) {
                    ManagementClass w32os = new ManagementClass("Win32_OperatingSystem");

                    w32os.Scope.Options.EnablePrivileges = true;
                    w32os.GetInstances().OfType<ManagementObject>().First().InvokeMethod("Win32Shutdown", new object[] { 0x2, 0 }); // 2 = reboot
                    Application.Exit();
                }

            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Restart the application with admin rights and execute the requested operation.
        /// 
        /// Probably dirty and not the best way, but this is just a simple helper
        /// </summary>
        /// <param name="args"></param>
        private void runsAsAdmin(string args) {
            ProcessStartInfo proc = new ProcessStartInfo {
                                            UseShellExecute = true,
                                            WorkingDirectory = Environment.CurrentDirectory,
                                            FileName = Application.ExecutablePath,
                                            Arguments = args,
                                            Verb = "runas"
                                        };

            try {
                Process.Start(proc);

            } catch {
                return;
            }

            Application.Exit();
        }

        private void saveScreenshot() {
            string path = string.Format("IGCITHelper_deviceInfo-{0}.png", DateTime.Now.Ticks);
            Bitmap img = new Bitmap(Width, Height);

            try {
                DrawToBitmap(img, new Rectangle(0, 0, Width, Height));
                img.Save(path, ImageFormat.Png);
                MessageBox.Show(this, "Saved: " + path, "Success", MessageBoxButtons.OK);

            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void showCopyTooltip() {
            _cpTooltip.Show("Copied", this, Cursor.Position.X - this.Location.X - 20, Cursor.Position.Y - this.Location.Y - 20, 800);
        }

        private void cpmodel_Click(object sender, EventArgs e) {
            Clipboard.SetText(dmodelT.Text);
            showCopyTooltip();
        }

        private void cpmanuf_Click(object sender, EventArgs e) {
            Clipboard.SetText(dmanufT.Text);
            showCopyTooltip();
        }

        private void cpmem_Click(object sender, EventArgs e) {
            Clipboard.SetText(ramT.Text);
            showCopyTooltip();
        }

        private void cpgpud_Click(object sender, EventArgs e) {
            Clipboard.SetText(gpudrvver.Text);
            showCopyTooltip();
        }

        private void cpproc_Click(object sender, EventArgs e) {
            Clipboard.SetText(cpuname.Text);
            showCopyTooltip();
        }

        private void cpwinb_Click(object sender, EventArgs e) {
            Clipboard.SetText(winbuild.Text);
            showCopyTooltip();
        }

        private void goToRepositoryToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT");
        }

        private void goToWikiToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki");
        }

        private void asPNGToolStripMenuItem_Click(object sender, EventArgs e) {
            saveScreenshot();
        }

        private void windowsMiniDToolStripMenuItem_Click(object sender, EventArgs e) {
            runsAsAdmin("eminidump");
        }
    }
}
