using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace IGCIT_Helper {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
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
            Object buildN = null;
            Object version = null;

            try {
                String regWinBuildPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";
                
                buildN = Registry.GetValue(regWinBuildPath, "CurrentBuildNumber", RegistryValueKind.String);
                version = Registry.GetValue(regWinBuildPath, "DisplayVersion", RegistryValueKind.String);

            } catch (Exception e) {
                MessageBox.Show(this, "Unable to get Windows build number.\n\n" + e.Message, "GetWindowsBuildVersion() Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (buildN ?? "Unknown") + " (" + (version ?? "Unknown") + ")";
        }

        private String GetProcessorName() {
            Object cpu = null;

            try {
                cpu = Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "ProcessorNameString", RegistryValueKind.String);

            } catch (Exception e) {
                MessageBox.Show(this, "Unable to get Processor name.\n\n" + e.Message, "GetProcessorName() Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return cpu == null ? "Unknown" : cpu.ToString();
        }

        private String GetGPUDriverVersion() {
            String regVideoPath = "HKEY_LOCAL_MACHINE\\HARDWARE\\DEVICEMAP\\VIDEO";
            String regDeviceKeyBase = "\\Device\\Video";
            String ret = "Unknown";
            int i = 0;

            try {
                while (true) {
                    Object rkey = Registry.GetValue(regVideoPath, regDeviceKeyBase + (i++).ToString(), RegistryValueKind.String);
                    Object gpu, drvVer;
                    String regGPUPath;

                    if (rkey == null)
                        break;

                    regGPUPath = GetRegistryPath(rkey.ToString()).Replace("\\", "\\");

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
                MessageBox.Show(this, "Unabel to get GPU driver version.\n\n" + e.Message, "GetGPUDriverVersion() Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ret;
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.ActiveControl = label2; // unfocus the textbox!
            winbuild.Text = GetWindowsBuildVersion();
            cpuname.Text = GetProcessorName();
            gpudrvver.Text = GetGPUDriverVersion();
        }
    }
}
