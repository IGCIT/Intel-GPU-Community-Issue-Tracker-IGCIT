using System;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace IGCIT_Helper {
    public static class Utils {
        private static void RebootDevice() {
            ManagementClass w32os = new ManagementClass("Win32_OperatingSystem");

            w32os.Scope.Options.EnablePrivileges = true;
            w32os.GetInstances().OfType<ManagementObject>().First().InvokeMethod("Win32Shutdown", new object[] { 0x2, 0 }); // 2 = reboot
            Application.Exit();
        }

        public static void AskReboot(IWin32Window owner, string messageBoxTitle = "Success") {
           DialogResult dret = MessageBox.Show(owner, "Success!\n\nA reboot is required to apply the changes,\nDo you want to reboot now?", messageBoxTitle, MessageBoxButtons.YesNo);

            if (dret == DialogResult.Yes)
                RebootDevice();
        }

        public static string GetRegistryPath(in string path) {
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
    }
}
