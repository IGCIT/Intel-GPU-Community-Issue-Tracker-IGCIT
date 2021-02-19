
namespace IGCIT_Driver_Switch {
    partial class appform {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(appform));
            this.label1 = new System.Windows.Forms.Label();
            this.driversList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.curDverL = new System.Windows.Forms.Label();
            this.restoreBtn = new System.Windows.Forms.Button();
            this.loadBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openIGCITRepoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadIntelDriversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Intel driver version:";
            // 
            // driversList
            // 
            this.driversList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driversList.FormattingEnabled = true;
            this.driversList.Location = new System.Drawing.Point(146, 116);
            this.driversList.Name = "driversList";
            this.driversList.Size = new System.Drawing.Size(177, 21);
            this.driversList.TabIndex = 1;
            this.driversList.SelectedIndexChanged += new System.EventHandler(this.driversList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 430);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(335, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Author IGCIT - v1.0 [based on Ciphray switch_driver.bat]";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logBox
            // 
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logBox.Location = new System.Drawing.Point(0, 0);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(306, 179);
            this.logBox.TabIndex = 4;
            this.logBox.Text = "";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.logBox);
            this.panel1.Location = new System.Drawing.Point(15, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(308, 181);
            this.panel1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 336);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Current GPU driver:";
            // 
            // curDverL
            // 
            this.curDverL.AutoSize = true;
            this.curDverL.ForeColor = System.Drawing.Color.MediumBlue;
            this.curDverL.Location = new System.Drawing.Point(121, 338);
            this.curDverL.Name = "curDverL";
            this.curDverL.Size = new System.Drawing.Size(53, 13);
            this.curDverL.TabIndex = 7;
            this.curDverL.Text = "Unknown";
            // 
            // restoreBtn
            // 
            this.restoreBtn.Location = new System.Drawing.Point(16, 393);
            this.restoreBtn.Name = "restoreBtn";
            this.restoreBtn.Size = new System.Drawing.Size(108, 23);
            this.restoreBtn.TabIndex = 8;
            this.restoreBtn.Text = "Restore Driver";
            this.restoreBtn.UseVisualStyleBackColor = true;
            this.restoreBtn.Click += new System.EventHandler(this.restoreBtn_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.Enabled = false;
            this.loadBtn.Location = new System.Drawing.Point(214, 393);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(108, 23);
            this.loadBtn.TabIndex = 9;
            this.loadBtn.Text = "Load Driver";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openIGCITRepoToolStripMenuItem,
            this.downloadIntelDriversToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(335, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openIGCITRepoToolStripMenuItem
            // 
            this.openIGCITRepoToolStripMenuItem.Name = "openIGCITRepoToolStripMenuItem";
            this.openIGCITRepoToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.openIGCITRepoToolStripMenuItem.Text = "Open IGCIT repo";
            this.openIGCITRepoToolStripMenuItem.Click += new System.EventHandler(this.openIGCITRepoToolStripMenuItem_Click);
            // 
            // downloadIntelDriversToolStripMenuItem
            // 
            this.downloadIntelDriversToolStripMenuItem.Name = "downloadIntelDriversToolStripMenuItem";
            this.downloadIntelDriversToolStripMenuItem.Size = new System.Drawing.Size(137, 20);
            this.downloadIntelDriversToolStripMenuItem.Text = "Download Intel drivers";
            this.downloadIntelDriversToolStripMenuItem.Click += new System.EventHandler(this.downloadIntelDriversToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(63, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(206, 83);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // appform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 456);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.loadBtn);
            this.Controls.Add(this.restoreBtn);
            this.Controls.Add(this.curDverL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.driversList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "appform";
            this.Text = "IGCIT Driver Switch";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox driversList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label curDverL;
        private System.Windows.Forms.Button restoreBtn;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openIGCITRepoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadIntelDriversToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

