﻿using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Management;
using System.Threading.Tasks;
using System.Threading;

namespace IGCIT_Driver_Switch {
    public partial class appform : Form {
        private const string _driversPath = "intel_drivers";
        private const string _tmpFolder = "igcit_drv_tmp";
        private Dictionary<string, CancellationTokenSource> _cancTokSources;
        private TaskCompletionSource<bool> _startProcessTask;
        private appform _switchDriverForm;
        private List<string> _driverZipPaths;
        private RegistryKey _localMachine;
        private uint _prevRestorePFreq;
        private int _prevComboboxIdx;
        private string _deviceID;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        public appform() {
            InitializeComponent();

            _switchDriverForm = this;
            _driverZipPaths = new List<string>();
            _localMachine = Registry.LocalMachine;
            _prevRestorePFreq = 1440; // default windows
            _prevComboboxIdx = -1;

            _cancTokSources = new Dictionary<string, CancellationTokenSource>() {
                {"getDvcId",    null },
                {"loadDrv",     null },
                {"getDrvVer",   null },
                {"extrDrv",     null },
                {"mkRestP",     null }
            };
        }

        private void Form1_Load(object sender, EventArgs e) {
            MessageBox.Show(this, "PLEASE MAKE SURE NO UNDERVOLT, OR SIMILAR CHANGES, ARE CURRENTLY APPLIED TO THE SYSTEM!\n\nUSE THIS AT YOUR OWN RISK!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private async void Form1_Shown(object sender, EventArgs e) {
            LockUI();

            _deviceID = await GetDeviceID();
            if (_deviceID == "") {
                spinnerImg.Visible = false;

                MessageBox.Show(this, "Unable to get DeviceID!\nUnsupported hardware?\n\nApplication will now close.", "IGCIT Driver Switch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            WriteLog($"I: Found device:\n{_deviceID}\n", Color.DarkViolet);
            UpdateDriversListUI(await LoadDriversList());

            curDverL.Text = await GetGPUDriverVersion();

            UnlockUI();
        }

        private async Task<string> GetDeviceID() {
            ProcessStartInfo pinfo = new ProcessStartInfo() {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                FileName = "powershell",
                Arguments = @"gwmi Win32_PnPSignedDriver | ? DeviceClass -eq ""display"" | ? DeviceName -match  ""Intel"";exit"
            };
            string line = "";
            string id = "";
            StringReader sr;

            if (_cancTokSources["getDvcId"] == null || _cancTokSources["getDvcId"].Token.IsCancellationRequested)
                _cancTokSources["getDvcId"] = new CancellationTokenSource();

            CancellationToken ctok = _cancTokSources["getDvcId"].Token;

            WriteLog("I: Getting device info..", Color.Blue);

            try {
                string ret = await startProcessAndGetOutput(pinfo);

                if (ret == "")
                    return ret;

                sr = new StringReader(ret);

                while (line != null) {
                    if (ctok.IsCancellationRequested)
                        ctok.ThrowIfCancellationRequested();

                    line = sr.ReadLine();

                    if (!line.Contains("DeviceID"))
                        continue;

                    id = line.Split(':')[1].Trim();
                    break;
                }
            } catch (Exception e) {
                ShowCatchErrorMessage(e);
            }

            return id;
        }

        private Task<List<string>> LoadDriversList() {
            if (_cancTokSources["loadDrv"] == null || _cancTokSources["loadDrv"].Token.IsCancellationRequested)
                _cancTokSources["loadDrv"] = new CancellationTokenSource();

            CancellationToken ctok = _cancTokSources["loadDrv"].Token;

            WriteLog("I: Scanning intel_drivers folder..", Color.Blue);

            return Task<List<string>>.Factory.StartNew(() => {
                List<string> verList = new List<string>();

                ctok.ThrowIfCancellationRequested();
                _driverZipPaths.Clear();

                try {
                    if (!Directory.Exists(_driversPath))
                        Directory.CreateDirectory(_driversPath);

                    string[] drivers = Directory.GetFiles(_driversPath);
                    
                    foreach (string d in drivers) {
                        if (ctok.IsCancellationRequested)
                            ctok.ThrowIfCancellationRequested();

                        if (Path.GetExtension(d) != ".zip")
                            continue;

                        try {
                            ZipArchive z = ZipFile.OpenRead(d);

                            foreach (ZipArchiveEntry ze in z.Entries) {
                                if (ctok.IsCancellationRequested)
                                    ctok.ThrowIfCancellationRequested();

                                if (!ze.FullName.Contains("igcc_dch"))
                                    continue;

                                StreamReader sr = new StreamReader(ze.Open());

                                while (!sr.EndOfStream) {
                                    if (ctok.IsCancellationRequested)
                                        ctok.ThrowIfCancellationRequested();

                                    string l = sr.ReadLine();

                                    if (!l.Contains("DriverVer="))
                                        continue;

                                    verList.Add(l.Split(',')[1]);
                                    _driverZipPaths.Add(d);
                                }

                                break;
                            }
                        } catch (Exception e) {
                            _switchDriverForm.BeginInvoke((MethodInvoker)delegate () {
                                WriteLog("E: unable to open " + Path.GetFileName(d), Color.Red);
                            });
                            
                            continue;
                        }
                    }
                } catch (Exception e) {
                    verList = null;

                    _switchDriverForm.BeginInvoke((MethodInvoker)delegate () {
                        ShowCatchErrorMessage(e);
                        WriteLog("E: Unable to load drivers from folder!", Color.Red);
                    });
                }

                return verList;
            }, _cancTokSources["loadDrv"].Token);
        }

        private Task<string> GetGPUDriverVersion() {
            if (_cancTokSources["getDrvVer"] == null || _cancTokSources["getDrvVer"].Token.IsCancellationRequested)
                _cancTokSources["getDrvVer"] = new CancellationTokenSource();

            CancellationToken ctok = _cancTokSources["getDrvVer"].Token;

            return Task<string>.Factory.StartNew(() => {
                string ret = "Unknown";

                ctok.ThrowIfCancellationRequested();

                try {
                    RegistryKey regGpus = _localMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\VIDEO");

                    if (regGpus == null)
                        return ret;

                    foreach (string k in regGpus.GetValueNames()) {
                        if (ctok.IsCancellationRequested)
                            ctok.ThrowIfCancellationRequested();

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
                    _switchDriverForm.BeginInvoke((MethodInvoker)delegate () {
                        ShowCatchErrorMessage(e);
                        WriteLog("E: Unable to get GPU driver version", Color.Red);
                    });
                }

                return ret;
            }, _cancTokSources["getDrvVer"].Token);
        }

        private Task<bool> ExtractSelectedDriver(int idx) {
            if (_cancTokSources["extrDrv"] == null || _cancTokSources["extrDrv"].Token.IsCancellationRequested)
                _cancTokSources["extrDrv"] = new CancellationTokenSource();

            CancellationToken ctok = _cancTokSources["extrDrv"].Token;
            string selectedV = driversList.SelectedItem as string;

            return Task<bool>.Factory.StartNew(() => {
                ctok.ThrowIfCancellationRequested();

                try {
                    string verStrFile = $"{_tmpFolder}/version";

                    if (File.Exists(verStrFile)) {
                        string ver = File.ReadAllText(verStrFile).Trim();

                        if (ver == selectedV)
                            return true;
                    }

                    string zipPath = Path.GetFullPath(_driverZipPaths[idx]);
                    string dest = Path.GetFullPath(_tmpFolder);

                    _switchDriverForm.BeginInvoke((MethodInvoker)delegate () {
                        WriteLog("I: Extracting driver..", Color.Blue);
                    });

                    if (ctok.IsCancellationRequested)
                        ctok.ThrowIfCancellationRequested();

                    if (Directory.Exists(dest))
                        Directory.Delete(dest, true);

                    Directory.CreateDirectory(dest);
                    ZipFile.ExtractToDirectory(zipPath, dest);
                    File.WriteAllText(verStrFile, selectedV);

                    return true;

                } catch (Exception e) {
                    _switchDriverForm.BeginInvoke((MethodInvoker)delegate () {
                        ShowCatchErrorMessage(e);
                    });
                }

                return false;
            }, _cancTokSources["extrDrv"].Token);
        }

        private void driversList_SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox combo = sender as ComboBox;

            if (combo.SelectedIndex != _prevComboboxIdx && combo.SelectedIndex != 0)
                loadBtn.Enabled = true;

            _prevComboboxIdx = combo.SelectedIndex;
        }

        private async void restoreBtn_Click(object sender, EventArgs e) {
            DialogResult dr = MessageBox.Show(this, "Do you want to restore your driver?", "Restore driver", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            IntPtr wow64Value = IntPtr.Zero;

            if (dr == DialogResult.No)
                return;

            LockUI();

            try {
                Wow64DisableWow64FsRedirection(ref wow64Value);

                string cmd = @"/c pnputil /remove-device """ + _deviceID + @""" && timeout 2 && pnputil /scan-devices";
                ProcessStartInfo pinfo = new ProcessStartInfo() {
                    UseShellExecute = true,
                    FileName = "cmd.exe",
                    Arguments = cmd
                };
                bool ret;

                WriteLog("I: Restoring driver..", Color.Blue);

                ret = await startProcess(pinfo);
                if (!ret) {
                    MessageBox.Show(this, "Failed to execute command!", "Restore driver error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WriteLog("E: Unable to restore driver!", Color.Red);

                } else {
                    await Task.Delay(7 * 1000); // sleep a few seconds till the gpu is up again
                    curDverL.Text = await GetGPUDriverVersion();
                    WriteLog("Success!", Color.Green);
                }

                Wow64RevertWow64FsRedirection(wow64Value);

            } catch(Exception ex) {
                ShowCatchErrorMessage(ex);
            }
            
            UnlockUI();
            driversList.SelectedIndex = 0;
        }

        private async void loadBtn_Click(object sender, EventArgs e) {
            if (driversList.SelectedIndex == 0)
                return;

            DialogResult dr = MessageBox.Show(this, "Do you want to load the selected driver?", "Load driver", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            ProcessStartInfo pinfo = new ProcessStartInfo();
            bool ret;

            LockUI();

            try {
                ret = await ExtractSelectedDriver(driversList.SelectedIndex - 1);

                if (!ret) {
                    WriteLog("E: Unable to load driver!", Color.Red);
                    UnlockUI(true);
                    return;
                }

                dr = MessageBox.Show(this, "Do you want to create a System Restore Point?", "Load driver", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) {
                    bool limitRet;

                    WriteLog("I: creating restore point..", Color.Blue);

                    ret = false;
                    limitRet = DisableRestorePointCreationLimit();

                    if (limitRet) {
                        int res = await CreateRestorePoint();

                        WriteLog($"I: CreateRestorePoint result: {res}", Color.Blue);
                        RevertDisableRestorePointCreationLimit();

                        ret = res == 0;
                    }

                    if (!ret) {
                        WriteLog("E: Unable to create restore point!", Color.Red);
                        UnlockUI(true);
                        return;
                    }
                }

                WriteLog("I: loading driver, this may take a few minutes..", Color.Blue);

                pinfo.UseShellExecute = true;
                pinfo.FileName = "igxpin";
                pinfo.WorkingDirectory = Path.GetFullPath(_tmpFolder);
                pinfo.Arguments = "-s -overwrite";

                ret = await startProcess(pinfo);
                if (!ret) {
                    MessageBox.Show(this, "Failed to execute command!", "Load driver error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WriteLog("E: Unable to load driver!", Color.Red);

                } else {
                    await Task.Delay(3 * 1000);
                    WriteLog("Success!", Color.Green);
                    curDverL.Text = await GetGPUDriverVersion();
                }
            } catch(Exception ex) {
                ShowCatchErrorMessage(ex);
            }
            
            UnlockUI();
            driversList.SelectedIndex = 0;
        }

        private bool DisableRestorePointCreationLimit() {
            try {
                RegistryKey key = _localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", true);
                object sysRestoreFreq;

                if (key == null) {
                    WriteLog("E: DisLimit: No SystemRestore key!", Color.Red);
                    return false;
                }

                sysRestoreFreq = key.GetValue("SystemRestorePointCreationFrequency");

                if (sysRestoreFreq != null)
                    _prevRestorePFreq = UInt32.Parse(sysRestoreFreq.ToString());

                key.SetValue("SystemRestorePointCreationFrequency", 0, RegistryValueKind.DWord);
                key.Close();
                return true;

            } catch (Exception e) {
                ShowCatchErrorMessage(e);
            }

            return false;
        }

        private void RevertDisableRestorePointCreationLimit() {
            try {
                RegistryKey key = _localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", true);

                if (key == null) {
                    WriteLog("E: RevDisLimit: No SystemRestore key!", Color.Red);
                    return;
                }

                if (_prevRestorePFreq == 1440) // default means we can delete the key
                    key.DeleteValue("SystemRestorePointCreationFrequency");
                else // or restore the user value if custom
                    key.SetValue("SystemRestorePointCreationFrequency", _prevRestorePFreq, RegistryValueKind.DWord);

            } catch (Exception e) {
                ShowCatchErrorMessage(e);
            }
        }

        private Task<int> CreateRestorePoint() {
            if (_cancTokSources["mkRestP"] == null || _cancTokSources["mkRestP"].Token.IsCancellationRequested)
                _cancTokSources["mkRestP"] = new CancellationTokenSource();

            CancellationToken ctok = _cancTokSources["mkRestP"].Token;

            return Task<int>.Factory.StartNew(() => {
                ctok.ThrowIfCancellationRequested();

                try {
                    ManagementClass sysRestore = new ManagementClass(@"\\localhost\root\default", "SystemRestore", new ObjectGetOptions());
                    ManagementBaseObject restoreArgs = sysRestore.GetMethodParameters("CreateRestorePoint");
                    ManagementBaseObject outObj;

                    restoreArgs["Description"] = "IGCIT Driver Switch";
                    restoreArgs["EventType"] = 100;
                    restoreArgs["RestorePointType"] = 10;

                    if (ctok.IsCancellationRequested)
                        ctok.ThrowIfCancellationRequested();

                    outObj = sysRestore.InvokeMethod("CreateRestorePoint", restoreArgs, null);

                    return Int32.Parse(outObj["ReturnValue"].ToString());

                } catch (Exception e) {
                    _switchDriverForm.BeginInvoke((MethodInvoker)delegate () {
                        ShowCatchErrorMessage(e);
                    });
                }

                return -1;
            }, _cancTokSources["mkRestP"].Token);
        }

        private async Task<bool> startProcess(ProcessStartInfo pinfo) {
            Process proc = new Process();

            try {
                _startProcessTask = new TaskCompletionSource<bool>();

                proc.StartInfo = pinfo;
                proc.EnableRaisingEvents = true;
                proc.Exited += onProcessExited;

                proc.Start();
                await Task.WhenAny(_startProcessTask.Task, Task.Delay(300 * 1000));

                if (!proc.HasExited) {
                    proc.Kill();
                    return false;
                }
            } catch (Exception e) {
                ShowCatchErrorMessage(e);
            }
            
            return _startProcessTask.Task.Result;
        }

        private async Task<string> startProcessAndGetOutput(ProcessStartInfo pinfo) {
            try {
                Process proc = new Process();
                string outp = "";

                _startProcessTask = new TaskCompletionSource<bool>();

                proc.StartInfo = pinfo;
                proc.EnableRaisingEvents = true;
                proc.Exited += onProcessExited;

                proc.Start();
                await Task.WhenAny(_startProcessTask.Task, Task.Delay(60 * 1000));

                if (!proc.HasExited) {
                    proc.Kill();
                    return "";
                }

                outp = proc.StandardOutput.ReadToEnd();

                return _startProcessTask.Task.Result ? outp : "";

            } catch (Exception e) {
                ShowCatchErrorMessage(e);
            }

            return "";
        }

        private void onProcessExited(object sender, EventArgs args) {
            Process p = sender as Process;

            _startProcessTask.TrySetResult(p.ExitCode == 0);
        }

        private void WriteLog(string msg, Color c) {
            logBox.SelectionStart = logBox.TextLength;
            logBox.SelectionLength = 0;
            logBox.SelectionColor = c;
            logBox.SelectedText = msg;

            logBox.AppendText("\n");
            logBox.Update();
        }

        private void UpdateDriversListUI(List<string> drivers) {
            if (drivers == null) {
                return;

            } else if (drivers.Count == 0) {
                WriteLog("I: No drivers found!", Color.DarkGoldenrod);
                return;
            }

            driversList.Items.Clear();
            driversList.Items.Add("");

            foreach (string versionStr in drivers) {
                WriteLog($"I: Found driver version: {versionStr}", Color.Green);
                driversList.Items.Add(versionStr);
            }
        }

        private void LockUI() {
            loadBtn.Enabled = false;
            driversList.Enabled = false;
            restoreBtn.Enabled = false;
            spinnerImg.Visible = true;
        }

        private void UnlockUI(bool enableLoad = false) {
            restoreBtn.Enabled = true;
            driversList.Enabled = true;
            spinnerImg.Visible = false;

            if (enableLoad)
                loadBtn.Enabled = true;
        }

        private string GetRegistryPath(string path) {
            string[] pathAr = path.Split('\\');
            string ret = "";

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

        private void ShowCatchErrorMessage(Exception ex) {
            MessageBox.Show(this, $"{ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void rescanDriversFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            LockUI();
            UpdateDriversListUI(await LoadDriversList());
            UnlockUI();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki/igcit-driver-switch");
        }

        private void openIGCITRepoToolStripMenuItem1_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT");
        }

        private void downloadIntelDriversToolStripMenuItem1_Click(object sender, EventArgs e) {
            Process.Start("https://downloadcenter.intel.com/product/80939/Graphics");
        }

        private void appform_FormClosing(object sender, FormClosingEventArgs e) {
            foreach(KeyValuePair<string, CancellationTokenSource> kvp in _cancTokSources) {
                if (kvp.Value != null)
                    kvp.Value.Cancel();
            }
        }
    }
}
