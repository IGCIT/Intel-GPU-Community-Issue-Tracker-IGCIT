
namespace IGCIT_Helper {
    partial class Form1 {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.winbuild = new System.Windows.Forms.TextBox();
            this.cpuname = new System.Windows.Forms.TextBox();
            this.gpudrvver = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(414, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Intel GPU Community Issue Tracker (IGCIT) Helper";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Windows Build:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Processor:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "GPU Driver:";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(414, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "IGCIT Helper ver 1.0, Author: IGCIT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // winbuild
            // 
            this.winbuild.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.winbuild.Location = new System.Drawing.Point(102, 61);
            this.winbuild.Name = "winbuild";
            this.winbuild.ReadOnly = true;
            this.winbuild.Size = new System.Drawing.Size(300, 13);
            this.winbuild.TabIndex = 8;
            this.winbuild.Text = "Unknown";
            // 
            // cpuname
            // 
            this.cpuname.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cpuname.Location = new System.Drawing.Point(102, 91);
            this.cpuname.Name = "cpuname";
            this.cpuname.ReadOnly = true;
            this.cpuname.Size = new System.Drawing.Size(300, 13);
            this.cpuname.TabIndex = 9;
            this.cpuname.Text = "Unknown";
            // 
            // gpudrvver
            // 
            this.gpudrvver.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gpudrvver.Location = new System.Drawing.Point(102, 122);
            this.gpudrvver.Name = "gpudrvver";
            this.gpudrvver.ReadOnly = true;
            this.gpudrvver.Size = new System.Drawing.Size(300, 13);
            this.gpudrvver.TabIndex = 10;
            this.gpudrvver.Text = "Unknown";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 182);
            this.Controls.Add(this.gpudrvver);
            this.Controls.Add(this.cpuname);
            this.Controls.Add(this.winbuild);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "IGCIT Helper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox winbuild;
        private System.Windows.Forms.TextBox cpuname;
        private System.Windows.Forms.TextBox gpudrvver;
    }
}

