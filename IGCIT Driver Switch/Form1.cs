using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Management;

namespace IGCIT_Driver_Switch {
    public partial class appform : Form {
        UInt32 previousRestorePFreq = 1440; // default windows
        const string driversPath = "intel_drivers";
        const string tmpFolder = "igcit_drv_tmp";
        int previousComboboxIdx = -1;
        List<string> driversPaths;
        string deviceID;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        public appform() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            string curDriver = GetGPUDriverVersion();

            driversPaths = new List<string>();
            curDverL.Text = curDriver;
            deviceID = getDeviceID();

            if (deviceID == "") {
                MessageBox.Show(this, "Unable to get DeviceID!\nUnsupported hardware?\n\nApplication will now close", "IGCIT Driver Switch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            WriteLog("I: Found device:\n" + deviceID + "\n", Color.DarkViolet);
            LoadDriversList();

            MessageBox.Show(this, "PLEASE MAKE SURE NO UNDERVOLT, OR SIMILAR CHANGES, ARE CURRENTLY APPLIED TO THE SYSTEM!\n\nUSE THIS AT YOUR OWN RISK!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private string getDeviceID() {
            try {
                string cmd = @"gwmi Win32_PnPSignedDriver | ? DeviceClass -eq ""display"" | ? DeviceName -match  ""Intel"";exit";
                ProcessStartInfo pinfo = new ProcessStartInfo();
                StringReader sr;
                Process proc;
                string outp;
                string line;

                pinfo.UseShellExecute = false;
                pinfo.CreateNoWindow = true;
                pinfo.RedirectStandardOutput = true;
                pinfo.FileName = "powershell";
                pinfo.Arguments = cmd;

                proc = Process.Start(pinfo);
                outp = proc.StandardOutput.ReadToEnd();

                if (proc == null)
                    return "";

                while (!proc.WaitForExit(65000)) {
                    proc.Kill();
                    break;
                }

                if (proc.ExitCode != 0)
                    return "";

                sr = new StringReader(outp);
                line = "";

                while (line != null) {
                    line = sr.ReadLine();

                    if (!line.Contains("DeviceID"))
                        continue;

                    return line.Split(':')[1].Trim();
                }
            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return "";
        }

        private void LoadDriversList() {
            try {
                if (!Directory.Exists(driversPath))
                    Directory.CreateDirectory(driversPath);

                string[] drivers = Directory.GetFiles(driversPath);

                foreach (string d in drivers) {
                    if (Path.GetExtension(d) != ".zip")
                        continue;

                    ZipArchive z = ZipFile.OpenRead(d);

                    foreach (ZipArchiveEntry ze in z.Entries) {
                        if (!ze.FullName.Contains("igcc_dch"))
                            continue;

                        StreamReader sr = new StreamReader(ze.Open());
                        
                        while (!sr.EndOfStream) {
                            string l = sr.ReadLine();

                            if (!l.Contains("DriverVer="))
                                continue;

                            string dver = l.Split(',')[1];

                            WriteLog("I: Found driver version " + dver, Color.Green);
                            driversList.Items.Add(dver);
                            driversPaths.Add(d);
                        }

                        break;
                    }
                }

                if (driversList.Items.Count == 0)
                    WriteLog("I: No drivers found!", Color.DarkGoldenrod);

            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                WriteLog("E: Unable to load drivers from folder!", Color.Red);
            }
        }

        // stolen :P
        private String GetGPUDriverVersion() {
            string ret = "Unknown";

            try {
                RegistryKey regGpus = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\VIDEO");

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

                    gpu = Registry.GetValue(regGPUPath, "DriverDesc", RegistryValueKind.String);
                    if (gpu == null || gpu.ToString().IndexOf("intel", StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    drvVer = Registry.GetValue(regGPUPath, "DriverVersion", RegistryValueKind.String);
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

        private bool ExtractSelectedDriver(int idx) {
            try {
                string zipPath = Path.GetFullPath(driversPaths[idx]);
                string dest = Path.GetFullPath(tmpFolder);

                WriteLog("Extracting driver..", Color.Blue);

                if (Directory.Exists(dest))
                    Directory.Delete(dest, true);

                Directory.CreateDirectory(dest);
                ZipFile.ExtractToDirectory(zipPath, dest);

                return true;

            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void WriteLog(string msg, Color c) {
            logBox.SelectionStart = logBox.TextLength;
            logBox.SelectionLength = 0;
            logBox.SelectionColor = c;
            logBox.SelectedText = msg;

            logBox.AppendText("\n");
            logBox.Update();
        }

        private void driversList_SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox combo = sender as ComboBox;

            if (combo.SelectedIndex != previousComboboxIdx)
                loadBtn.Enabled = true;

            previousComboboxIdx = combo.SelectedIndex;
        }

        private void restoreBtn_Click(object sender, EventArgs e) {
            DialogResult dr = MessageBox.Show(this, "Do you want to restore your driver?", "Restore driver", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            IntPtr wow64Value = IntPtr.Zero;

            if (dr == DialogResult.No)
                return;

            Wow64DisableWow64FsRedirection(ref wow64Value);

            string cmd = @"/c pnputil /remove-device """ + deviceID + @""" && timeout 2 && pnputil /scan-devices";
            ProcessStartInfo pinfo = new ProcessStartInfo();
            bool loadBtnState = loadBtn.Enabled;
            Process proc;

            driversList.Enabled = false;
            loadBtn.Enabled = false;
            restoreBtn.Enabled = false;

            WriteLog("Restoring driver..", Color.Blue);

            pinfo.UseShellExecute = true;
            pinfo.FileName = "cmd.exe";
            pinfo.Arguments = cmd;

            proc = Process.Start(pinfo);
            if (proc == null)
                MessageBox.Show(this, "Failed to execute command!", "Restore driver error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            while (!proc.WaitForExit(65000)) {
                proc.Kill();
                break;
            }
            
            if (proc.ExitCode == 0) {
                System.Threading.Thread.Sleep(7 * 1000); // sleep a few seconds till the gpu is up again
                WriteLog("Success!", Color.Green);
                curDverL.Text = GetGPUDriverVersion();

            } else {
                WriteLog("E: Unable to restore driver!", Color.Red);
            }

            loadBtn.Enabled = loadBtnState;
            restoreBtn.Enabled = true;
            driversList.Enabled = true;

            Wow64RevertWow64FsRedirection(wow64Value);
        }

        private void loadBtn_Click(object sender, EventArgs e) {
            DialogResult dr = MessageBox.Show(this, "Do you want to load the selected driver?", "Load driver", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            ProcessStartInfo pinfo = new ProcessStartInfo();
            Process proc;
            bool ret;

            driversList.Enabled = false;
            loadBtn.Enabled = false;
            restoreBtn.Enabled = false;

            ret = ExtractSelectedDriver(driversList.SelectedIndex);
            if (!ret) {
                WriteLog("E: Unable to load driver!", Color.Red);
                loadBtn.Enabled = true;
                restoreBtn.Enabled = true;
                driversList.Enabled = true;
                return;
            }

            dr = MessageBox.Show(this, "Do you want to create a System Restore Point?", "Load driver", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes) {
                bool limitRet;

                WriteLog("creating restore point..", Color.Blue);

                ret = false;
                limitRet = DisableRestorePointCreationLimit();

                if (limitRet) {
                    ret = CreateRestorePoint();

                    RevertDisableRestorePointCreationLimit();
                }

                if (!ret) {
                    WriteLog("E: Unable to create restore point!", Color.Red);
                    loadBtn.Enabled = true;
                    restoreBtn.Enabled = true;
                    driversList.Enabled = true;
                    return;
                }
            }

            WriteLog("loading driver..", Color.Blue);

            pinfo.UseShellExecute = true;
            pinfo.FileName = "igxpin";
            pinfo.WorkingDirectory = Path.GetFullPath(tmpFolder);
            pinfo.Arguments = "-s -overwrite";

            proc = Process.Start(pinfo);
            if (proc == null)
                MessageBox.Show(this, "Failed to execute command!", "Load driver error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            while (!proc.WaitForExit(200000)) {
                proc.Kill();
                break;
            }

            if (proc.ExitCode == 0) {
                System.Threading.Thread.Sleep(2 * 1000);
                WriteLog("Success!", Color.Green);
                curDverL.Text = GetGPUDriverVersion();

            } else {
                WriteLog("E: Unable to load driver!", Color.Red);
            }

            restoreBtn.Enabled = true;
            driversList.Enabled = true;
        }

        private bool DisableRestorePointCreationLimit() {
            try {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", true);
                object sysRestoreFreq;

                if (key == null) {
                    WriteLog("E: DisLimit: No SystemRestore key!", Color.Red);
                    return false;
                }

                sysRestoreFreq = key.GetValue("SystemRestorePointCreationFrequency");

                if (sysRestoreFreq != null)
                    previousRestorePFreq = UInt32.Parse(sysRestoreFreq.ToString());

                key.SetValue("SystemRestorePointCreationFrequency", 0, RegistryValueKind.DWord);
                key.Close();
                return true;

            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void RevertDisableRestorePointCreationLimit() {
            try {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", true);

                if (key == null) {
                    WriteLog("E: RevDisLimit: No SystemRestore key!", Color.Red);
                    return;
                }

                if (previousRestorePFreq == 1440) // default means we can delete the key
                    key.DeleteValue("SystemRestorePointCreationFrequency");
                else // or restore the user value if custom
                    key.SetValue("SystemRestorePointCreationFrequency", previousRestorePFreq, RegistryValueKind.DWord);

            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CreateRestorePoint() {
            try {
                ManagementClass sysRestore = new ManagementClass(@"\\localhost\root\default", "SystemRestore", new ObjectGetOptions());
                ManagementBaseObject restoreArgs = sysRestore.GetMethodParameters("CreateRestorePoint");
                ManagementBaseObject outObj;

                restoreArgs["Description"] = "IGCIT Driver Switch";
                restoreArgs["EventType"] = 100;
                restoreArgs["RestorePointType"] = 10;

                outObj = sysRestore.InvokeMethod("CreateRestorePoint", restoreArgs, null);

                return Int32.Parse(outObj["ReturnValue"].ToString()) == 0;

            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void openIGCITRepoToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT");
        }

        private void downloadIntelDriversToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("https://downloadcenter.intel.com/product/80939/Graphics");
        }
    }
}
