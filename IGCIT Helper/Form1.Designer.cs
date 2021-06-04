
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.goToRepositoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDeviceInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMiniDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ramT = new System.Windows.Forms.TextBox();
            this.ramL = new System.Windows.Forms.Label();
            this.dmanufL = new System.Windows.Forms.Label();
            this.dmanufT = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dmodelT = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cpmanuf = new System.Windows.Forms.Button();
            this.cpmodel = new System.Windows.Forms.Button();
            this.cpmem = new System.Windows.Forms.Button();
            this.cpgpud = new System.Windows.Forms.Button();
            this.cpproc = new System.Windows.Forms.Button();
            this.cpwinb = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(0, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(414, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Intel GPU Community Issue Tracker (IGCIT) Helper";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Windows Build:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Processor:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "GPU Driver:";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(414, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "IGCIT Helper ver 1.3, Author: IGCIT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // winbuild
            // 
            this.winbuild.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.winbuild.Location = new System.Drawing.Point(102, 93);
            this.winbuild.Name = "winbuild";
            this.winbuild.ReadOnly = true;
            this.winbuild.Size = new System.Drawing.Size(250, 13);
            this.winbuild.TabIndex = 8;
            this.winbuild.Text = "Unknown";
            // 
            // cpuname
            // 
            this.cpuname.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cpuname.Location = new System.Drawing.Point(102, 122);
            this.cpuname.Name = "cpuname";
            this.cpuname.ReadOnly = true;
            this.cpuname.Size = new System.Drawing.Size(250, 13);
            this.cpuname.TabIndex = 9;
            this.cpuname.Text = "Unknown";
            // 
            // gpudrvver
            // 
            this.gpudrvver.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gpudrvver.Location = new System.Drawing.Point(102, 151);
            this.gpudrvver.Name = "gpudrvver";
            this.gpudrvver.ReadOnly = true;
            this.gpudrvver.Size = new System.Drawing.Size(250, 13);
            this.gpudrvver.TabIndex = 10;
            this.gpudrvver.Text = "Unknown";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exportDeviceInfoToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(414, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToRepositoryToolStripMenuItem,
            this.goToWikiToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(47, 20);
            this.toolStripMenuItem1.Text = "IGCIT";
            // 
            // goToRepositoryToolStripMenuItem
            // 
            this.goToRepositoryToolStripMenuItem.Name = "goToRepositoryToolStripMenuItem";
            this.goToRepositoryToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.goToRepositoryToolStripMenuItem.Text = "Go to repository";
            this.goToRepositoryToolStripMenuItem.Click += new System.EventHandler(this.goToRepositoryToolStripMenuItem_Click);
            // 
            // goToWikiToolStripMenuItem
            // 
            this.goToWikiToolStripMenuItem.Name = "goToWikiToolStripMenuItem";
            this.goToWikiToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.goToWikiToolStripMenuItem.Text = "Go to wiki";
            this.goToWikiToolStripMenuItem.Click += new System.EventHandler(this.goToWikiToolStripMenuItem_Click);
            // 
            // exportDeviceInfoToolStripMenuItem
            // 
            this.exportDeviceInfoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asPNGToolStripMenuItem});
            this.exportDeviceInfoToolStripMenuItem.Name = "exportDeviceInfoToolStripMenuItem";
            this.exportDeviceInfoToolStripMenuItem.Size = new System.Drawing.Size(119, 20);
            this.exportDeviceInfoToolStripMenuItem.Text = "Export information";
            // 
            // asPNGToolStripMenuItem
            // 
            this.asPNGToolStripMenuItem.Name = "asPNGToolStripMenuItem";
            this.asPNGToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.asPNGToolStripMenuItem.Text = "Take screenshot";
            this.asPNGToolStripMenuItem.Click += new System.EventHandler(this.asPNGToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsMiniDToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // windowsMiniDToolStripMenuItem
            // 
            this.windowsMiniDToolStripMenuItem.Name = "windowsMiniDToolStripMenuItem";
            this.windowsMiniDToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.windowsMiniDToolStripMenuItem.Text = "Enable Small Memory Dumps";
            this.windowsMiniDToolStripMenuItem.Click += new System.EventHandler(this.windowsMiniDToolStripMenuItem_Click);
            // 
            // ramT
            // 
            this.ramT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ramT.Location = new System.Drawing.Point(102, 180);
            this.ramT.Name = "ramT";
            this.ramT.ReadOnly = true;
            this.ramT.Size = new System.Drawing.Size(250, 13);
            this.ramT.TabIndex = 16;
            this.ramT.Text = "Unknown";
            // 
            // ramL
            // 
            this.ramL.AutoSize = true;
            this.ramL.Location = new System.Drawing.Point(13, 180);
            this.ramL.Name = "ramL";
            this.ramL.Size = new System.Drawing.Size(47, 13);
            this.ramL.TabIndex = 15;
            this.ramL.Text = "Memory:";
            // 
            // dmanufL
            // 
            this.dmanufL.AutoSize = true;
            this.dmanufL.Location = new System.Drawing.Point(13, 238);
            this.dmanufL.Name = "dmanufL";
            this.dmanufL.Size = new System.Drawing.Size(73, 13);
            this.dmanufL.TabIndex = 17;
            this.dmanufL.Text = "Manufacturer:";
            // 
            // dmanufT
            // 
            this.dmanufT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dmanufT.Location = new System.Drawing.Point(102, 238);
            this.dmanufT.Name = "dmanufT";
            this.dmanufT.ReadOnly = true;
            this.dmanufT.Size = new System.Drawing.Size(250, 13);
            this.dmanufT.TabIndex = 18;
            this.dmanufT.Text = "Unknown";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Firebrick;
            this.label5.Location = new System.Drawing.Point(0, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(414, 29);
            this.label5.TabIndex = 19;
            this.label5.Text = "Device Information";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dmodelT
            // 
            this.dmodelT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dmodelT.Location = new System.Drawing.Point(102, 209);
            this.dmodelT.Name = "dmodelT";
            this.dmodelT.ReadOnly = true;
            this.dmodelT.Size = new System.Drawing.Size(250, 13);
            this.dmodelT.TabIndex = 21;
            this.dmodelT.Text = "Unknown";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Model:";
            // 
            // cpmanuf
            // 
            this.cpmanuf.BackColor = System.Drawing.Color.SteelBlue;
            this.cpmanuf.FlatAppearance.BorderSize = 0;
            this.cpmanuf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cpmanuf.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cpmanuf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cpmanuf.Location = new System.Drawing.Point(358, 233);
            this.cpmanuf.Name = "cpmanuf";
            this.cpmanuf.Size = new System.Drawing.Size(44, 23);
            this.cpmanuf.TabIndex = 22;
            this.cpmanuf.Text = "Copy";
            this.cpmanuf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cpmanuf.UseVisualStyleBackColor = false;
            this.cpmanuf.Click += new System.EventHandler(this.cpmanuf_Click);
            // 
            // cpmodel
            // 
            this.cpmodel.BackColor = System.Drawing.Color.SteelBlue;
            this.cpmodel.FlatAppearance.BorderSize = 0;
            this.cpmodel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cpmodel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cpmodel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cpmodel.Location = new System.Drawing.Point(358, 204);
            this.cpmodel.Name = "cpmodel";
            this.cpmodel.Size = new System.Drawing.Size(44, 23);
            this.cpmodel.TabIndex = 23;
            this.cpmodel.Text = "Copy";
            this.cpmodel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cpmodel.UseVisualStyleBackColor = false;
            this.cpmodel.Click += new System.EventHandler(this.cpmodel_Click);
            // 
            // cpmem
            // 
            this.cpmem.BackColor = System.Drawing.Color.SteelBlue;
            this.cpmem.FlatAppearance.BorderSize = 0;
            this.cpmem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cpmem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cpmem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cpmem.Location = new System.Drawing.Point(358, 175);
            this.cpmem.Name = "cpmem";
            this.cpmem.Size = new System.Drawing.Size(44, 23);
            this.cpmem.TabIndex = 24;
            this.cpmem.Text = "Copy";
            this.cpmem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cpmem.UseVisualStyleBackColor = false;
            this.cpmem.Click += new System.EventHandler(this.cpmem_Click);
            // 
            // cpgpud
            // 
            this.cpgpud.BackColor = System.Drawing.Color.SteelBlue;
            this.cpgpud.FlatAppearance.BorderSize = 0;
            this.cpgpud.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cpgpud.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cpgpud.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cpgpud.Location = new System.Drawing.Point(358, 146);
            this.cpgpud.Name = "cpgpud";
            this.cpgpud.Size = new System.Drawing.Size(44, 23);
            this.cpgpud.TabIndex = 25;
            this.cpgpud.Text = "Copy";
            this.cpgpud.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cpgpud.UseVisualStyleBackColor = false;
            this.cpgpud.Click += new System.EventHandler(this.cpgpud_Click);
            // 
            // cpproc
            // 
            this.cpproc.BackColor = System.Drawing.Color.SteelBlue;
            this.cpproc.FlatAppearance.BorderSize = 0;
            this.cpproc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cpproc.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cpproc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cpproc.Location = new System.Drawing.Point(358, 117);
            this.cpproc.Name = "cpproc";
            this.cpproc.Size = new System.Drawing.Size(44, 23);
            this.cpproc.TabIndex = 26;
            this.cpproc.Text = "Copy";
            this.cpproc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cpproc.UseVisualStyleBackColor = false;
            this.cpproc.Click += new System.EventHandler(this.cpproc_Click);
            // 
            // cpwinb
            // 
            this.cpwinb.BackColor = System.Drawing.Color.SteelBlue;
            this.cpwinb.FlatAppearance.BorderSize = 0;
            this.cpwinb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cpwinb.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cpwinb.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cpwinb.Location = new System.Drawing.Point(358, 88);
            this.cpwinb.Name = "cpwinb";
            this.cpwinb.Size = new System.Drawing.Size(44, 23);
            this.cpwinb.TabIndex = 27;
            this.cpwinb.Text = "Copy";
            this.cpwinb.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cpwinb.UseVisualStyleBackColor = false;
            this.cpwinb.Click += new System.EventHandler(this.cpwinb_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 292);
            this.Controls.Add(this.cpwinb);
            this.Controls.Add(this.cpproc);
            this.Controls.Add(this.cpgpud);
            this.Controls.Add(this.cpmem);
            this.Controls.Add(this.cpmodel);
            this.Controls.Add(this.cpmanuf);
            this.Controls.Add(this.dmodelT);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dmanufT);
            this.Controls.Add(this.dmanufL);
            this.Controls.Add(this.ramT);
            this.Controls.Add(this.ramL);
            this.Controls.Add(this.gpudrvver);
            this.Controls.Add(this.cpuname);
            this.Controls.Add(this.winbuild);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "IGCIT Helper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox ramT;
        private System.Windows.Forms.Label ramL;
        private System.Windows.Forms.Label dmanufL;
        private System.Windows.Forms.TextBox dmanufT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox dmodelT;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cpmanuf;
        private System.Windows.Forms.Button cpmodel;
        private System.Windows.Forms.Button cpmem;
        private System.Windows.Forms.Button cpgpud;
        private System.Windows.Forms.Button cpproc;
        private System.Windows.Forms.Button cpwinb;
        private System.Windows.Forms.ToolStripMenuItem exportDeviceInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem goToRepositoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToWikiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asPNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsMiniDToolStripMenuItem;
    }
}

