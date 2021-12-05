
namespace IGCIT_Helper
{
    partial class tdrdelayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tdrdelayForm));
            this.tdrApplBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tdrInpt = new System.Windows.Forms.NumericUpDown();
            this.tdrDdiInpt = new System.Windows.Forms.NumericUpDown();
            this.tdrDefBtn = new System.Windows.Forms.Button();
            this.footerTdrSet = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.iGCITWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.tdrInpt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tdrDdiInpt)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tdrApplBtn
            // 
            this.tdrApplBtn.Location = new System.Drawing.Point(227, 159);
            this.tdrApplBtn.Name = "tdrApplBtn";
            this.tdrApplBtn.Size = new System.Drawing.Size(96, 23);
            this.tdrApplBtn.TabIndex = 0;
            this.tdrApplBtn.Text = "Apply changes";
            this.tdrApplBtn.UseVisualStyleBackColor = true;
            this.tdrApplBtn.Click += new System.EventHandler(this.tdrApplBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "TdrDelay";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "TdrDdiDelay";
            // 
            // tdrInpt
            // 
            this.tdrInpt.Location = new System.Drawing.Point(147, 48);
            this.tdrInpt.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.tdrInpt.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.tdrInpt.Name = "tdrInpt";
            this.tdrInpt.Size = new System.Drawing.Size(120, 20);
            this.tdrInpt.TabIndex = 3;
            this.tdrInpt.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // tdrDdiInpt
            // 
            this.tdrDdiInpt.Location = new System.Drawing.Point(147, 94);
            this.tdrDdiInpt.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.tdrDdiInpt.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.tdrDdiInpt.Name = "tdrDdiInpt";
            this.tdrDdiInpt.Size = new System.Drawing.Size(120, 20);
            this.tdrDdiInpt.TabIndex = 4;
            this.tdrDdiInpt.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // tdrDefBtn
            // 
            this.tdrDefBtn.Location = new System.Drawing.Point(120, 159);
            this.tdrDefBtn.Name = "tdrDefBtn";
            this.tdrDefBtn.Size = new System.Drawing.Size(101, 23);
            this.tdrDefBtn.TabIndex = 5;
            this.tdrDefBtn.Text = "Reset to default";
            this.tdrDefBtn.UseVisualStyleBackColor = true;
            this.tdrDefBtn.Click += new System.EventHandler(this.tdrDefBtn_Click);
            // 
            // footerTdrSet
            // 
            this.footerTdrSet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerTdrSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.footerTdrSet.Location = new System.Drawing.Point(0, 201);
            this.footerTdrSet.Name = "footerTdrSet";
            this.footerTdrSet.Size = new System.Drawing.Size(335, 21);
            this.footerTdrSet.TabIndex = 8;
            this.footerTdrSet.Text = "IGCIT Helper ver 1.4, Author: IGCIT";
            this.footerTdrSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iGCITWikiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(335, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // iGCITWikiToolStripMenuItem
            // 
            this.iGCITWikiToolStripMenuItem.Name = "iGCITWikiToolStripMenuItem";
            this.iGCITWikiToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.iGCITWikiToolStripMenuItem.Text = "IGCIT Wiki";
            this.iGCITWikiToolStripMenuItem.Click += new System.EventHandler(this.iGCITWikiToolStripMenuItem_Click);
            // 
            // tdrdelayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 222);
            this.Controls.Add(this.footerTdrSet);
            this.Controls.Add(this.tdrDefBtn);
            this.Controls.Add(this.tdrDdiInpt);
            this.Controls.Add(this.tdrInpt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tdrApplBtn);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "tdrdelayForm";
            this.Text = "IGCIT Helper - TDR delay";
            this.Load += new System.EventHandler(this.tdrdelayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tdrInpt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tdrDdiInpt)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button tdrApplBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown tdrInpt;
        private System.Windows.Forms.NumericUpDown tdrDdiInpt;
        private System.Windows.Forms.Button tdrDefBtn;
        private System.Windows.Forms.Label footerTdrSet;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem iGCITWikiToolStripMenuItem;
    }
}