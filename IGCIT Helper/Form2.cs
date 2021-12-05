using System.Windows.Forms;
using IGCIT_Helper.Events;

namespace IGCIT_Helper {
    public partial class ModalBox : Form {
        private string _token = "";

        public ModalBox() {
            InitializeComponent();
        }

        public void SetText(string text) {
            ModalText.Text = text;
        }

        public void SetTokenName(string tok) {
            _token = tok;
        }

        private void button1_Click(object sender, System.EventArgs e) {
            ModalText.Text = "Stopping process..";
            button1.Enabled = false;
            DialogCancelEvent.DialogCancelClicked(new DialogCancelEventArgs(DialogType.WaitForProcess, _token));
        }

        private void ModalBox_Shown(object sender, System.EventArgs e) {
            button1.Enabled = true;
        }
    }
}
