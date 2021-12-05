
namespace IGCIT_Helper {
    partial class ModalBox {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ModalText = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ModalText
            // 
            this.ModalText.BackColor = System.Drawing.SystemColors.Window;
            this.ModalText.Dock = System.Windows.Forms.DockStyle.Top;
            this.ModalText.Location = new System.Drawing.Point(0, 0);
            this.ModalText.Name = "ModalText";
            this.ModalText.Size = new System.Drawing.Size(203, 52);
            this.ModalText.TabIndex = 0;
            this.ModalText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Location = new System.Drawing.Point(0, 52);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ModalBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(203, 81);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ModalText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(219, 120);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(219, 120);
            this.Name = "ModalBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dialog";
            this.Shown += new System.EventHandler(this.ModalBox_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ModalText;
        private System.Windows.Forms.Button button1;
    }
}