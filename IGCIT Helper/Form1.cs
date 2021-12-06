using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using IGCIT_Helper.Events;
using System.Text.RegularExpressions;

namespace IGCIT_Helper {
    public partial class Form1 : Form {
        private enum AdminCMD:int {
            CrashDumpsSetup = 0,
            TDRSettings,
            FixWatchdogGen,
            RestoreDumpDefaults,
            ExtractDumps,
            ClearDumpFolds
        }

        private readonly Dictionary<string, CancellationTokenSource> _cancTokSources;
        private readonly RegistryKey _localMachine;
        private readonly RegistryKey _localMachine64;
        private readonly ToolTip _cpTooltip;
        private readonly Utils _util;
        private readonly bool _isAdmin;
        private Process _activeProc;

        [DllImport("shell32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsUserAnAdmin();

        public Form1() {
            InitializeComponent();

            _localMachine = Registry.LocalMachine;
            _localMachine64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            _cpTooltip = new ToolTip();
            _util = Utils.Instance;
            _isAdmin = IsUserAnAdmin();

            _cancTokSources = new Dictionary<string, CancellationTokenSource>() {
                {"comprdumps",    null }
            };

            DialogCancelEvent.OnDialogCancelClick += OnDialogCancelClicked;
        }

        private async void OnDialogCancelClicked(DialogCancelEventArgs args) {
            switch (args.EventType) {
                case DialogType.WaitForProcess: {
                    string tok = args.Token;

                    if (tok != "" && _cancTokSources[tok] != null && !_cancTokSources[tok].IsCancellationRequested)
                        _cancTokSources[tok].Cancel();

                    if (_activeProc != null && !_activeProc.HasExited) {
                        await Task.Factory.StartNew(() => {
                            _activeProc.Kill();
                            _activeProc.WaitForExit();
                        });
                    }
                }
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            ManagementClass cs = new ManagementClass("Win32_ComputerSystem");
            ManagementClass os = new ManagementClass("Win32_OperatingSystem");
            
            ActiveControl = label2; // unfocus the textbox!
            winbuild.Text = GetWindowsBuildVersion();
            cpuname.Text = GetProcessorName();
            gpudrvver.Text = GetGPUDriverVersion();
            footerMain.Text = CommonData.FooterTx;

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

            bool isValidCmd = Enum.TryParse(args[1], out AdminCMD cmd);
            if (!isValidCmd) {
                MessageBox.Show(this, "Invalid command!", "ERROR", MessageBoxButtons.OK);
                Application.Exit();
            }

            switch (cmd) {
                case AdminCMD.TDRSettings:
                    ShowTdrDelayForm();
                    break;
                case AdminCMD.CrashDumpsSetup:
                    SetupCrashDumps();
                    break;
                case AdminCMD.FixWatchdogGen:
                    ResetWatchdogDump();
                    break;
                case AdminCMD.RestoreDumpDefaults:
                    RestoreCrashDumpsDefaults();
                    break;
                case AdminCMD.ExtractDumps:
                    ExtractDumpFiles();
                    break;
                case AdminCMD.ClearDumpFolds:
                    ClearDumpFolders();
                    break;
                default:
                    break;
            }
        }

        private string GetWindowsBuildVersion() {
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

        private string GetProcessorName() {
            object cpu = null;

            try {
                cpu = _localMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0").GetValue("ProcessorNameString");

            } catch (Exception e) {
                MessageBox.Show(this, "Unable to get Processor name.\n\n" + e.Message, "GetProcessorName() Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return cpu == null ? "Unknown" : cpu.ToString();
        }

        private string GetGPUDriverVersion() {
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

                    regGPUPath = _util.GetRegistryPath(gpuPath.ToString()).Replace("\\", "\\");

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

        private void SetupCrashDumps() {
            try {
                RegistryKey lmErrorReport = _localMachine.OpenSubKey(@"Software\Microsoft\Windows\Windows Error Reporting", true);
                RegistryKey usErrorReport = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Windows Error Reporting", true);
                RegistryKey localDumps = _localMachine64.OpenSubKey(@"Software\Microsoft\Windows\Windows Error Reporting\LocalDumps", true);
                RegistryKey liveKrnReports = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl\LiveKernelReports", true);
                RegistryKey crashControl = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl", true);
                RegistryKey memManagment = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", true);
                object pageFile = memManagment.GetValue("PagingFiles");
                object dumpFold = localDumps.GetValue("DumpFolder");
                (RegistryKey, string, int)[] regKeys = {
                    (lmErrorReport, "Disabled", 1),
                    (usErrorReport, "Disabled", 1),
                    (localDumps, "DumpType", 2),
                    (liveKrnReports, "DeleteLiveMiniDumps", 0),
                    (crashControl, "CrashDumpEnabled", 1),
                    (crashControl, "FilterPages", 1)
                };
                bool setPagefile = true;
                bool reqReboot = false;

                if (pageFile != null) {
                    string[] pageFileAr = (string[])pageFile;
                    bool skip = false;
                    int min = 0;
                    int max = 0;

                    if (pageFileAr.Length > 1) {
                        DialogResult ret = MessageBox.Show(this, "Advanced PageFile configuration detected!\n\n" +
                                                    "IGCIT Helper has no support for this!\n" +
                                                    "Intel recommends a pagefile size of 17400.\n\n" +
                                                    "Do you want to skip pagefile setting?\n\n" +
                                                    "Press Cancel to abort.\n" +
                                                    "Press No to let IGCIT Helper overwrite your pagefile settings.\n" +
                                                    "Press Yes to skip pagefile setting and continue.",
                                                    "Crash dumps setup", MessageBoxButtons.YesNoCancel);

                        if (ret == DialogResult.Cancel)
                            return;
                        else if (ret == DialogResult.Yes)
                            skip = true;

                    } else if (pageFileAr.Length == 1 && pageFileAr[0] != "") {
                        string[] pageFileData = pageFileAr[0].Split(' ');

                        if (pageFileData.Length == 3) {
                            Int32.TryParse(pageFileData[1], out min);
                            Int32.TryParse(pageFileData[2], out max);
                        }
                    }

                    setPagefile = !skip && (min < 17400 || max < 17400);
                }

                if (setPagefile) {
                    memManagment.SetValue("PagingFiles", new string[] { @"C:\pagefile.sys 17400 17400" }, RegistryValueKind.MultiString);
                    reqReboot = true;
                }

                foreach ((RegistryKey regK, string key, int expected) in regKeys) {
                    object val = regK.GetValue(key);

                    if (val != null && (int)val == expected)
                        continue;

                    regK.SetValue(key, expected, RegistryValueKind.DWord);
                    reqReboot = true;
                }

                if (dumpFold == null || (string)dumpFold != @"C:\AppCrashDumps") {
                    localDumps.SetValue("DumpFolder", @"C:\AppCrashDumps", RegistryValueKind.ExpandString);
                    reqReboot = true;
                }

                if (!reqReboot)
                    MessageBox.Show(this, "Everything is already set!", "Crash dumps setup", MessageBoxButtons.OK);
                else
                    _util.AskReboot(this, "Crash dumps setup");

            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void ResetWatchdogDump() {
            try {
                RegistryKey liveKrnReports = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl\LiveKernelReports", true);
                RegistryKey watchod = liveKrnReports?.OpenSubKey("WATCHDOG");

                if (watchod != null)
                    liveKrnReports.DeleteSubKeyTree("WATCHDOG");

                MessageBox.Show(this, "Done!", "Reset watchdog dumps", MessageBoxButtons.OK);

            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void RestoreCrashDumpsDefaults() {
            try {
                RegistryKey lmErrorReport = _localMachine.OpenSubKey(@"Software\Microsoft\Windows\Windows Error Reporting", true);
                RegistryKey usErrorReport = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Windows Error Reporting", true);
                RegistryKey localDumps = _localMachine64.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", true);
                RegistryKey liveKrnReports = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl\LiveKernelReports", true);
                RegistryKey crashControl = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl", true);
                RegistryKey memManagment = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", true);
                object pageFile = memManagment.GetValue("PagingFiles");

                lmErrorReport.DeleteValue("Disabled");
                usErrorReport.DeleteValue("Disabled");
                localDumps.DeleteValue("DumpFolder");
                localDumps.DeleteValue("DumpType");
                liveKrnReports.DeleteValue("DeleteLiveMiniDumps");
                crashControl.DeleteValue("FilterPages");
                crashControl.SetValue("CrashDumpEnabled", 3, RegistryValueKind.DWord);

                if (pageFile != null) {
                    string[] pageFileAr = (string[])pageFile;

                    if (pageFileAr.Length == 1) {
                        DialogResult ret = MessageBox.Show(this, "Do you want to restore pagefile to Windows default?",
                                                            "Restore crash dumps settings", MessageBoxButtons.YesNo);

                        if (ret == DialogResult.Yes)
                            memManagment.SetValue("PagingFiles", new string[] { @"?:\pagefile.sys" }, RegistryValueKind.MultiString);
                    }
                }

                _util.AskReboot(this, "Restore crash dumps settings");

            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private async Task<string> CompressAndEncryptDumps() {
            if (_cancTokSources["comprdumps"] == null || _cancTokSources["comprdumps"].Token.IsCancellationRequested)
                _cancTokSources["comprdumps"] = new CancellationTokenSource();

            CancellationToken ctok = _cancTokSources["comprdumps"].Token;
            string outDir = "dumps";
            string outputZipName = "";

            try {
                string sysRoot = Environment.ExpandEnvironmentVariables("%systemroot%");
                DirectoryInfo[] dumpDirs = {
                    new DirectoryInfo(@"C:\AppCrashDumps"),
                    new DirectoryInfo($@"{sysRoot}\Minidump"),
                    new DirectoryInfo($@"{sysRoot}\LiveKernelReports\WATCHDOG")
                };
                DirectoryInfo outDInfo;
                string ret;

                if (Directory.Exists(outDir))
                    Directory.Delete(outDir, true);

                foreach (DirectoryInfo dinfo in dumpDirs) {
                    if (dinfo.Exists && dinfo.GetFiles().Length > 0)
                        _util.DirectoryCopy(dinfo.FullName, $@"{outDir}\{dinfo.Name}", false);
                }

                outDInfo = new DirectoryInfo(outDir);
                if (outDInfo.GetFiles().Length == 0 && outDInfo.GetDirectories().Length == 0)
                    return "No crash dumps have been found on this system.";

                if (ctok.IsCancellationRequested)
                    ctok.ThrowIfCancellationRequested();
                
                string dumpDataJson = await IGCITHttpClient.Instance.GetStringAsync("api/getid");
                Dictionary<string, string> dumpData = JsonConvert.DeserializeObject<Dictionary<string, string>>(dumpDataJson);
                outputZipName = $"igcit-dumps_{dumpData["dumpID"]}.7z";

                if (ctok.IsCancellationRequested)
                    ctok.ThrowIfCancellationRequested();

                _activeProc = _util.CreateProcess(new ProcessStartInfo() {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    FileName = @"7z\7za.exe",
                    Arguments = $@"-mhc=on -mhe=on -p{dumpData["dumpKey"]} a {outputZipName} {outDir}"
                });

                ret = await _util.StartProcessAndGetOutput(_activeProc);

                outDInfo.Delete(true);

                if (ctok.IsCancellationRequested)
                    ctok.ThrowIfCancellationRequested();

                if (!File.Exists(outputZipName))
                    throw new Exception("Unknown compression error!");

                File.WriteAllText($"{outputZipName}-decKey.txt", dumpData["dumpKey"]);

                return ret == "" ?
                    "7z error!\n\nUnable to create dumps archive." :
                    $"{ret}\n\nOutput file: {outputZipName}\n\n" +
                    "Do NOT upload your key to IGCIT and do NOT share it with anyone!";

            } catch (OperationCanceledException oex) {
                if (outputZipName != "" && File.Exists(outputZipName))
                    File.Delete(outputZipName);

                if (Directory.Exists(outDir))
                    Directory.Delete(outDir, true);

                return "Canceled";

            } catch (HttpRequestException hex) {
                if (Directory.Exists(outDir))
                    Directory.Delete(outDir, true);

                return $"{hex.Message}\n\nUnable to contact the server, please check your internet connection.";

            } catch (Exception ex) {
                if (Directory.Exists(outDir))
                    Directory.Delete(outDir, true);

                return ex.Message;
            }
        }

        private async void ExtractDumpFiles() {
            try {
                ModalBox modal = new ModalBox() { Owner = this };
                string compressRetStr;

                menuStrip1.Enabled = false;
                modal.SetText("Please wait..");
                modal.SetTokenName("comprdumps");
                _util.CenterFormToParent(modal, this);
                modal.Show();

                compressRetStr = await CompressAndEncryptDumps();

                modal.Close();
                menuStrip1.Enabled = true;

                MessageBox.Show(this, compressRetStr, "Extract crash dumps", MessageBoxButtons.OK);

            } catch (Exception ex) {
                menuStrip1.Enabled = true;

                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void ClearDumpFolders() {
            try {
                string sysRoot = Environment.ExpandEnvironmentVariables("%systemroot%");
                DirectoryInfo[] dumpDirs = {
                    new DirectoryInfo(@"C:\AppCrashDumps"),
                    new DirectoryInfo($@"{sysRoot}\Minidump"),
                    new DirectoryInfo($@"{sysRoot}\LiveKernelReports\WATCHDOG")
                };

                foreach (DirectoryInfo dinfo in dumpDirs) {
                    FileInfo[] finfoAr = dinfo.GetFiles();
                    DirectoryInfo[] dinfoAr = dinfo.GetDirectories();

                    foreach (FileInfo finfo in finfoAr)
                        finfo.Delete();

                    foreach (DirectoryInfo ddinfo in dinfoAr)
                        ddinfo.Delete(true);
                }
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
            } 
        }

        private void AnonymizeSSU() {
            OpenFileDialog odiag = new OpenFileDialog() {
                Filter = "txt files (*.txt)|*.txt",
                CheckFileExists = true,
                CheckPathExists = true,
                AddExtension = true,
                DefaultExt = "txt",
                Multiselect = false,
                Title = "Open SSU report"
            };
            string outF = "igcit_ssu.txt";

            try {
                if (odiag.ShowDialog() != DialogResult.OK)
                    return;

                FileInfo ssuFinfo = new FileInfo(odiag.FileName);
                StreamReader ssur = new StreamReader(odiag.OpenFile());
                StreamWriter nssu = new StreamWriter($@"{ssuFinfo.DirectoryName}\{outF}");
                string line = ssur.ReadLine();
                Regex rex = new Regex(@"[A-Za-z]:[\\\/]Users[\\\/](?<usrname>[^\\\/]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                while (line != null) {
                    MatchCollection match = rex.Matches(line);
                    string wstr = line;

                    if (line.Contains("IP Address:") || line.Contains("MAC Address:") || line.Contains("Default IP Gateway:") ||
                            line.Contains("Machine name:") || line.Contains("Machine Id:"))
                        wstr = "";
                    else if (match.Count > 0)
                        wstr = wstr.Replace(match[0].Groups["usrname"].Value, "usrname");

                    if (wstr != "")
                        nssu.WriteLine(wstr);

                    line = ssur.ReadLine();
                }

                nssu.Close();
                ssur.Close();

                MessageBox.Show(this, $"Done!\n\nOutput file: {outF}", "Anonymize SSU report", MessageBoxButtons.OK);
                
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void ShowTdrDelayForm() {
            Form tdrDl = new tdrdelayForm() {
                StartPosition = FormStartPosition.CenterParent
            };

            tdrDl.ShowDialog(this);
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

        private void editTDRDelayValuesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!_isAdmin)
                _util.RunAsAdmin(AdminCMD.TDRSettings.ToString());
            else
                ShowTdrDelayForm();
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult ret = MessageBox.Show(this, "IGCIT Helper will now apply Intel recommended crash dumps settings.\n\n" +
                                                "Press Yes to continue, press No to cancel",
                                                "Enable crash dumps", MessageBoxButtons.YesNo);

            if (ret == DialogResult.No)
                return;

            if (!_isAdmin)
                _util.RunAsAdmin(AdminCMD.CrashDumpsSetup.ToString());
            else
                SetupCrashDumps();
        }

        private void fixWATCHDOGDumpsGenerationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!_isAdmin)
                _util.RunAsAdmin(AdminCMD.FixWatchdogGen.ToString());
            else
                ResetWatchdogDump();
        }

        private void restoreWindowsDefaultsToolStripMenuItem_Click(object sender, EventArgs e) {
           DialogResult ret = MessageBox.Show(this,
                                "IGCIT Helper will now restore default Windows settings for dumps\n\n" +
                                "Press Yes to continue, press No to cancel",
                                "Restore default dump settings",
                                MessageBoxButtons.YesNo);

            if (ret == DialogResult.No)
                return;

            if (!_isAdmin)
                _util.RunAsAdmin(AdminCMD.RestoreDumpDefaults.ToString());
            else
                RestoreCrashDumpsDefaults();
        }

        private void extractDumpFilesToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult ret = MessageBox.Show(this,
                                "An internet connection is required!\n\n" +
                                "This will extract all the dumps in your system, except memory dump.\n" +
                                "The result is an encrypted compressed 7z archive.\n\n" +
                                "Press Yes to continue, press No to cancel",
                                "Extract crash dumps",
                                MessageBoxButtons.YesNo);

            if (ret == DialogResult.No)
                return;

            if (!_isAdmin)
                _util.RunAsAdmin(AdminCMD.ExtractDumps.ToString());
            else
                ExtractDumpFiles();
        }

        private void clearWindowsDumpFoldersToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult ret = MessageBox.Show(this, "Do you want to delete all dump files?", "Clear Windows dump folders", MessageBoxButtons.YesNo);

            if (ret == DialogResult.No)
                return;

            if (!_isAdmin)
                _util.RunAsAdmin(AdminCMD.ClearDumpFolds.ToString());
            else
                ClearDumpFolders();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            foreach (KeyValuePair<string, CancellationTokenSource> kvp in _cancTokSources) {
                if (kvp.Value != null)
                    kvp.Value.Cancel();
            }

            if (_activeProc != null && !_activeProc.HasExited) {
                _activeProc.Kill();
                _activeProc.WaitForExit();
            }
        }

        private void anonymizeSSUReportToolStripMenuItem_Click(object sender, EventArgs e) {
            AnonymizeSSU();
        }
    }
}
