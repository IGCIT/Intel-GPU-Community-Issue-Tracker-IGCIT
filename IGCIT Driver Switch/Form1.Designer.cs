
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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanDriversFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.openIGCITRepoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadIntelDriversToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadIntelBetaDCHDriversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.spinnerImg = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.writeDLogChk = new System.Windows.Forms.CheckBox();
            this.rebootChk = new System.Windows.Forms.CheckBox();
            this.skipExtrasChk = new System.Windows.Forms.CheckBox();
            this.forceLegacyChk = new System.Windows.Forms.CheckBox();
            this.forceOneCoreChk = new System.Windows.Forms.CheckBox();
            this.forceUnifiedChk = new System.Windows.Forms.CheckBox();
            this.forceDiscreteChk = new System.Windows.Forms.CheckBox();
            this.forceIntegratedChk = new System.Windows.Forms.CheckBox();
            this.noSignCheckChk = new System.Windows.Forms.CheckBox();
            this.forceCleanChk = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Intel driver version:";
            // 
            // driversList
            // 
            this.driversList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driversList.FormattingEnabled = true;
            this.driversList.Location = new System.Drawing.Point(143, 115);
            this.driversList.Name = "driversList";
            this.driversList.Size = new System.Drawing.Size(180, 21);
            this.driversList.TabIndex = 1;
            this.driversList.SelectedIndexChanged += new System.EventHandler(this.driversList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 430);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(535, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "IGCIT Driver Switch  v3.0";
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
            this.toolStripMenuItem1,
            this.toolStripMenuItem3,
            this.toolStripMenuItem2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(535, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rescanDriversFolderToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(61, 20);
            this.toolStripMenuItem1.Text = "Options";
            // 
            // rescanDriversFolderToolStripMenuItem
            // 
            this.rescanDriversFolderToolStripMenuItem.Name = "rescanDriversFolderToolStripMenuItem";
            this.rescanDriversFolderToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.rescanDriversFolderToolStripMenuItem.Text = "Rescan drivers folder";
            this.rescanDriversFolderToolStripMenuItem.Click += new System.EventHandler(this.rescanDriversFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openIGCITRepoToolStripMenuItem1,
            this.downloadIntelDriversToolStripMenuItem1,
            this.downloadIntelBetaDCHDriversToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(130, 20);
            this.toolStripMenuItem3.Text = "Links and downloads";
            // 
            // openIGCITRepoToolStripMenuItem1
            // 
            this.openIGCITRepoToolStripMenuItem1.Name = "openIGCITRepoToolStripMenuItem1";
            this.openIGCITRepoToolStripMenuItem1.Size = new System.Drawing.Size(254, 22);
            this.openIGCITRepoToolStripMenuItem1.Text = "Open IGCIT repo";
            this.openIGCITRepoToolStripMenuItem1.Click += new System.EventHandler(this.openIGCITRepoToolStripMenuItem1_Click);
            // 
            // downloadIntelDriversToolStripMenuItem1
            // 
            this.downloadIntelDriversToolStripMenuItem1.Name = "downloadIntelDriversToolStripMenuItem1";
            this.downloadIntelDriversToolStripMenuItem1.Size = new System.Drawing.Size(254, 22);
            this.downloadIntelDriversToolStripMenuItem1.Text = "Download Intel stable DCH drivers";
            this.downloadIntelDriversToolStripMenuItem1.Click += new System.EventHandler(this.downloadIntelDriversToolStripMenuItem1_Click);
            // 
            // downloadIntelBetaDCHDriversToolStripMenuItem
            // 
            this.downloadIntelBetaDCHDriversToolStripMenuItem.Name = "downloadIntelBetaDCHDriversToolStripMenuItem";
            this.downloadIntelBetaDCHDriversToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.downloadIntelBetaDCHDriversToolStripMenuItem.Text = "Download Intel beta DCH drivers";
            this.downloadIntelBetaDCHDriversToolStripMenuItem.Click += new System.EventHandler(this.downloadIntelBetaDCHDriversToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(44, 20);
            this.toolStripMenuItem2.Text = "Help";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // spinnerImg
            // 
            this.spinnerImg.Image = ((System.Drawing.Image)(resources.GetObject("spinnerImg.Image")));
            this.spinnerImg.Location = new System.Drawing.Point(151, 381);
            this.spinnerImg.Name = "spinnerImg";
            this.spinnerImg.Size = new System.Drawing.Size(36, 35);
            this.spinnerImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.spinnerImg.TabIndex = 12;
            this.spinnerImg.TabStop = false;
            this.spinnerImg.Visible = false;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.writeDLogChk);
            this.groupBox1.Controls.Add(this.rebootChk);
            this.groupBox1.Controls.Add(this.skipExtrasChk);
            this.groupBox1.Controls.Add(this.forceLegacyChk);
            this.groupBox1.Controls.Add(this.forceOneCoreChk);
            this.groupBox1.Controls.Add(this.forceUnifiedChk);
            this.groupBox1.Controls.Add(this.forceDiscreteChk);
            this.groupBox1.Controls.Add(this.forceIntegratedChk);
            this.groupBox1.Controls.Add(this.noSignCheckChk);
            this.groupBox1.Controls.Add(this.forceCleanChk);
            this.groupBox1.Location = new System.Drawing.Point(344, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 389);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced options";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkRed;
            this.label4.Location = new System.Drawing.Point(6, 333);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 53);
            this.label4.TabIndex = 10;
            this.label4.Text = "Do not enable any of these options unless you know what you are doing!\r\n";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // writeDLogChk
            // 
            this.writeDLogChk.AutoSize = true;
            this.writeDLogChk.Location = new System.Drawing.Point(6, 249);
            this.writeDLogChk.Name = "writeDLogChk";
            this.writeDLogChk.Size = new System.Drawing.Size(149, 30);
            this.writeDLogChk.TabIndex = 9;
            this.writeDLogChk.Text = "Save driver installation log\r\n(drvInstallLog.txt)";
            this.writeDLogChk.UseVisualStyleBackColor = true;
            // 
            // rebootChk
            // 
            this.rebootChk.AutoSize = true;
            this.rebootChk.Location = new System.Drawing.Point(6, 225);
            this.rebootChk.Name = "rebootChk";
            this.rebootChk.Size = new System.Drawing.Size(172, 17);
            this.rebootChk.TabIndex = 8;
            this.rebootChk.Text = "Reboot system after installation";
            this.rebootChk.UseVisualStyleBackColor = true;
            // 
            // skipExtrasChk
            // 
            this.skipExtrasChk.AutoSize = true;
            this.skipExtrasChk.Enabled = false;
            this.skipExtrasChk.Location = new System.Drawing.Point(6, 201);
            this.skipExtrasChk.Name = "skipExtrasChk";
            this.skipExtrasChk.Size = new System.Drawing.Size(156, 17);
            this.skipExtrasChk.TabIndex = 7;
            this.skipExtrasChk.Text = "Skip additional components";
            this.skipExtrasChk.UseVisualStyleBackColor = true;
            // 
            // forceLegacyChk
            // 
            this.forceLegacyChk.AutoSize = true;
            this.forceLegacyChk.Enabled = false;
            this.forceLegacyChk.Location = new System.Drawing.Point(6, 177);
            this.forceLegacyChk.Name = "forceLegacyChk";
            this.forceLegacyChk.Size = new System.Drawing.Size(116, 17);
            this.forceLegacyChk.TabIndex = 6;
            this.forceLegacyChk.Text = "Force legacy driver";
            this.forceLegacyChk.UseVisualStyleBackColor = true;
            // 
            // forceOneCoreChk
            // 
            this.forceOneCoreChk.AutoSize = true;
            this.forceOneCoreChk.Enabled = false;
            this.forceOneCoreChk.Location = new System.Drawing.Point(6, 153);
            this.forceOneCoreChk.Name = "forceOneCoreChk";
            this.forceOneCoreChk.Size = new System.Drawing.Size(133, 17);
            this.forceOneCoreChk.TabIndex = 5;
            this.forceOneCoreChk.Text = "Force OnceCore driver";
            this.forceOneCoreChk.UseVisualStyleBackColor = true;
            // 
            // forceUnifiedChk
            // 
            this.forceUnifiedChk.AutoSize = true;
            this.forceUnifiedChk.Enabled = false;
            this.forceUnifiedChk.Location = new System.Drawing.Point(6, 129);
            this.forceUnifiedChk.Name = "forceUnifiedChk";
            this.forceUnifiedChk.Size = new System.Drawing.Size(116, 17);
            this.forceUnifiedChk.TabIndex = 4;
            this.forceUnifiedChk.Text = "Force unified driver";
            this.forceUnifiedChk.UseVisualStyleBackColor = true;
            // 
            // forceDiscreteChk
            // 
            this.forceDiscreteChk.AutoSize = true;
            this.forceDiscreteChk.Enabled = false;
            this.forceDiscreteChk.Location = new System.Drawing.Point(6, 105);
            this.forceDiscreteChk.Name = "forceDiscreteChk";
            this.forceDiscreteChk.Size = new System.Drawing.Size(122, 17);
            this.forceDiscreteChk.TabIndex = 3;
            this.forceDiscreteChk.Text = "Force discrete driver";
            this.forceDiscreteChk.UseVisualStyleBackColor = true;
            // 
            // forceIntegratedChk
            // 
            this.forceIntegratedChk.AutoSize = true;
            this.forceIntegratedChk.Enabled = false;
            this.forceIntegratedChk.Location = new System.Drawing.Point(6, 81);
            this.forceIntegratedChk.Name = "forceIntegratedChk";
            this.forceIntegratedChk.Size = new System.Drawing.Size(132, 17);
            this.forceIntegratedChk.TabIndex = 2;
            this.forceIntegratedChk.Text = "Force integrated driver";
            this.forceIntegratedChk.UseVisualStyleBackColor = true;
            // 
            // noSignCheckChk
            // 
            this.noSignCheckChk.AutoSize = true;
            this.noSignCheckChk.Enabled = false;
            this.noSignCheckChk.Location = new System.Drawing.Point(6, 57);
            this.noSignCheckChk.Name = "noSignCheckChk";
            this.noSignCheckChk.Size = new System.Drawing.Size(140, 17);
            this.noSignCheckChk.TabIndex = 1;
            this.noSignCheckChk.Text = "No signature verification";
            this.noSignCheckChk.UseVisualStyleBackColor = true;
            // 
            // forceCleanChk
            // 
            this.forceCleanChk.AutoSize = true;
            this.forceCleanChk.Enabled = false;
            this.forceCleanChk.Location = new System.Drawing.Point(6, 34);
            this.forceCleanChk.Name = "forceCleanChk";
            this.forceCleanChk.Size = new System.Drawing.Size(111, 17);
            this.forceCleanChk.TabIndex = 0;
            this.forceCleanChk.Text = "Force clean install";
            this.forceCleanChk.UseVisualStyleBackColor = true;
            // 
            // appform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 456);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.spinnerImg);
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
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(551, 495);
            this.MinimumSize = new System.Drawing.Size(551, 495);
            this.Name = "appform";
            this.Text = "IGCIT Driver Switch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.appform_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox spinnerImg;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem rescanDriversFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem openIGCITRepoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem downloadIntelDriversToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem downloadIntelBetaDCHDriversToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox forceCleanChk;
        private System.Windows.Forms.CheckBox writeDLogChk;
        private System.Windows.Forms.CheckBox rebootChk;
        private System.Windows.Forms.CheckBox skipExtrasChk;
        private System.Windows.Forms.CheckBox forceLegacyChk;
        private System.Windows.Forms.CheckBox forceOneCoreChk;
        private System.Windows.Forms.CheckBox forceUnifiedChk;
        private System.Windows.Forms.CheckBox forceDiscreteChk;
        private System.Windows.Forms.CheckBox forceIntegratedChk;
        private System.Windows.Forms.CheckBox noSignCheckChk;
        private System.Windows.Forms.Label label4;
    }
}

