using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IGCIT_Helper {
    public class Utils {
        private static Utils _instance = null;
        public static Utils Instance {
            get {
                if (_instance == null)
                    _instance = new Utils();

                return _instance;
            }
            private set { }
        }
        private TaskCompletionSource<bool> _startProcessTask;

        private Utils() { }

        public void CenterFormToParent(Form child, Form parent) {
            int px = parent.Location.X + (parent.Size.Width / 2) - (child.Size.Width / 2);
            int py = parent.Location.Y + (parent.Size.Height / 2) - (child.Size.Height / 2);

            child.StartPosition = FormStartPosition.Manual;
            child.Location = new System.Drawing.Point(px, py);
        }

        public void RunAsAdmin(string args) {
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

        public void RebootDevice() {
            ManagementClass w32os = new ManagementClass("Win32_OperatingSystem");

            w32os.Scope.Options.EnablePrivileges = true;
            w32os.GetInstances().OfType<ManagementObject>().First().InvokeMethod("Win32Shutdown", new object[] { 0x2, 0 }); // 2 = reboot
            Application.Exit();
        }

        public void AskReboot(IWin32Window owner, string messageBoxTitle = "Success") {
           DialogResult dret = MessageBox.Show(owner, "Success!\n\nA reboot is required to apply the changes,\nDo you want to reboot now?", messageBoxTitle, MessageBoxButtons.YesNo);

            if (dret == DialogResult.Yes)
                RebootDevice();
        }

        public string GetRegistryPath(in string path) {
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

        public void DirectoryCopy(string src, string dst, bool copySubDirs) {
            DirectoryInfo dir = new DirectoryInfo(src);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {src}");

            DirectoryInfo[] dirs = dir.GetDirectories();
            FileInfo[] files = dir.GetFiles();

            Directory.CreateDirectory(dst);
            
            foreach (FileInfo file in files)
                file.CopyTo(Path.Combine(dst, file.Name), false);

            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string tempPath = Path.Combine(dst, subdir.Name);

                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        public Process CreateProcess(ProcessStartInfo pinfo) {
            return new Process {
                StartInfo = pinfo
            };
        }

        public async Task<string> StartProcessAndGetOutput(Process proc) {
            try {
                string outp = "";

                _startProcessTask = new TaskCompletionSource<bool>();

                proc.EnableRaisingEvents = true;
                proc.Exited += OnProcessExited;

                proc.Start();
                await _startProcessTask.Task;

                outp = proc.StandardOutput.ReadToEnd();

                return _startProcessTask.Task.Result ? outp : "";

            } catch (Exception ex) {
                return ex.Message;
            }
        }

        private void OnProcessExited(object sender, EventArgs args) {
            Process p = sender as Process;

            _startProcessTask.TrySetResult(p.ExitCode == 0);
        }
    }
}
