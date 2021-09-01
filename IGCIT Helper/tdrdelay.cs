using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace IGCIT_Helper {
    public partial class tdrdelayForm : Form {
        RegistryKey _localMachine;

        public tdrdelayForm() {
            InitializeComponent();

            _localMachine = Registry.LocalMachine;
        }

        private void tdrdelayForm_Load(object sender, EventArgs e) {
            RegistryKey gfxDriverReg = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers");
            object tdrDelayV, tdrDdiDelayV;

            if (gfxDriverReg == null) {
                MessageBox.Show(this, "Unable to open GraphicsDriver registry key", "Error", MessageBoxButtons.OK);
                Close();
            }

            tdrInpt.Value = 2;
            tdrDdiInpt.Value = 5;
            tdrDelayV = gfxDriverReg.GetValue("TdrDelay");
            tdrDdiDelayV = gfxDriverReg.GetValue("TdrDdiDelay");          

            if (tdrDelayV != null && gfxDriverReg.GetValueKind("TdrDelay") == RegistryValueKind.DWord)
                tdrInpt.Value = (int)tdrDelayV;

            if (tdrDdiDelayV != null && gfxDriverReg.GetValueKind("TdrDdiDelay") == RegistryValueKind.DWord)
                tdrDdiInpt.Value = (int)tdrDdiDelayV;

            footerTdrSet.Text = CommonData.FooterTx;
        }

        private void tdrDefBtn_Click(object sender, EventArgs e) {
            try {
                RegistryKey gfxDriverReg = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers", true);

                if (gfxDriverReg == null) {
                    MessageBox.Show(this, "Unable to open GraphicsDriver registry key", "Error", MessageBoxButtons.OK);
                    return;
                }

                gfxDriverReg.DeleteValue("TdrDelay");
                gfxDriverReg.DeleteValue("TdrDdiDelay");

                tdrInpt.Value = 2;
                tdrDdiInpt.Value = 5;

                Utils.AskReboot(this);
                Close();

            } catch (Exception ex) {
                MessageBox.Show(this, "Unable to get TDR values\n" + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void tdrApplBtn_Click(object sender, EventArgs e) {
            try {
                RegistryKey gfxDriverReg = _localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers", true);

                if (gfxDriverReg == null) {
                    MessageBox.Show(this, "Unable to open GraphicsDriver registry key", "Error", MessageBoxButtons.OK);
                    return;
                }

                gfxDriverReg.SetValue("TdrDelay", tdrInpt.Value, RegistryValueKind.DWord);
                gfxDriverReg.SetValue("TdrDdiDelay", tdrDdiInpt.Value, RegistryValueKind.DWord);
                Utils.AskReboot(this);
                Close();

            } catch (Exception ex) {
                MessageBox.Show(this, "Unable to set TDR values\n" + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void iGCITWikiToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki/GPU-drivers-crash-with-long-computations");
        }
    }
}
