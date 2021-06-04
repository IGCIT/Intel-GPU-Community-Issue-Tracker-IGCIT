using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;

namespace IGCIT_Helper {
    public partial class Form1 : Form {
        RegistryKey _localMachine;
        ToolTip _cpTooltip;

        public Form1() {
            InitializeComponent();

            _localMachine = Registry.LocalMachine;
            _cpTooltip = new ToolTip();
        }

        private String GetRegistryPath(in String path) {
            String[] pathAr = path.Split('\\');
            String ret = "";

            for (int i=0, l=pathAr.Length; i<l; ++i) {
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

        private void Form1_Load(object sender, EventArgs e) {
            ManagementClass cs = new ManagementClass("Win32_ComputerSystem");
            ManagementClass os = new ManagementClass("Win32_OperatingSystem");

            this.ActiveControl = label2; // unfocus the textbox!
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
            System.Diagnostics.Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT");
        }

        private void goToWikiToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki");
        }

        private void asPNGToolStripMenuItem_Click(object sender, EventArgs e) {
            saveScreenshot();
        }
    }
}
