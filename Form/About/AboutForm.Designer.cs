namespace SCNET_Restart_Tool
{
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            txtHelpContent = new System.Windows.Forms.RichTextBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            txtProjectInfo = new System.Windows.Forms.RichTextBox();
            tabPage3 = new System.Windows.Forms.TabPage();
            txtVersionInfo = new System.Windows.Forms.RichTextBox();
            btnClose = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            labelTitle = new System.Windows.Forms.Label();
            pictureBoxLogo = new System.Windows.Forms.PictureBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            tabControl1.ItemSize = new System.Drawing.Size(100, 30);
            tabControl1.Location = new System.Drawing.Point(10, 66);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(836, 500);
            tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.Color.White;
            tabPage1.Controls.Add(txtHelpContent);
            tabPage1.Location = new System.Drawing.Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            tabPage1.Size = new System.Drawing.Size(828, 462);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "功能说明";
            // 
            // txtHelpContent
            // 
            txtHelpContent.BackColor = System.Drawing.Color.White;
            txtHelpContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtHelpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            txtHelpContent.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            txtHelpContent.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            txtHelpContent.Location = new System.Drawing.Point(9, 10);
            txtHelpContent.Name = "txtHelpContent";
            txtHelpContent.ReadOnly = true;
            txtHelpContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            txtHelpContent.Size = new System.Drawing.Size(810, 442);
            txtHelpContent.TabIndex = 0;
            txtHelpContent.Text = "";
            txtHelpContent.LinkClicked += txtHelpContent_LinkClicked;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = System.Drawing.Color.White;
            tabPage2.Controls.Add(txtProjectInfo);
            tabPage2.Location = new System.Drawing.Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            tabPage2.Size = new System.Drawing.Size(828, 462);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "项目信息";
            // 
            // txtProjectInfo
            // 
            txtProjectInfo.BackColor = System.Drawing.Color.White;
            txtProjectInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtProjectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProjectInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            txtProjectInfo.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            txtProjectInfo.Location = new System.Drawing.Point(9, 10);
            txtProjectInfo.Name = "txtProjectInfo";
            txtProjectInfo.ReadOnly = true;
            txtProjectInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            txtProjectInfo.Size = new System.Drawing.Size(810, 442);
            txtProjectInfo.TabIndex = 0;
            txtProjectInfo.Text = "";
            txtProjectInfo.LinkClicked += txtProjectInfo_LinkClicked;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = System.Drawing.Color.White;
            tabPage3.Controls.Add(txtVersionInfo);
            tabPage3.Location = new System.Drawing.Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            tabPage3.Size = new System.Drawing.Size(828, 462);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "版本信息";
            // 
            // txtVersionInfo
            // 
            txtVersionInfo.BackColor = System.Drawing.Color.White;
            txtVersionInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtVersionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            txtVersionInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            txtVersionInfo.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            txtVersionInfo.Location = new System.Drawing.Point(9, 10);
            txtVersionInfo.Name = "txtVersionInfo";
            txtVersionInfo.ReadOnly = true;
            txtVersionInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            txtVersionInfo.Size = new System.Drawing.Size(810, 442);
            txtVersionInfo.TabIndex = 0;
            txtVersionInfo.Text = "";
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnClose.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(749, 572);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(98, 35);
            btnClose.TabIndex = 1;
            btnClose.Text = "关闭";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            panel1.Controls.Add(labelTitle);
            panel1.Controls.Add(pictureBoxLogo);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(858, 60);
            panel1.TabIndex = 3;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
            labelTitle.ForeColor = System.Drawing.Color.White;
            labelTitle.Location = new System.Drawing.Point(61, 15);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new System.Drawing.Size(299, 26);
            labelTitle.TabIndex = 1;
            labelTitle.Text = "SCNET_Restart_Tool - 帮助文档";
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Location = new System.Drawing.Point(10, 8);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new System.Drawing.Size(42, 48);
            pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(858, 619);
            Controls.Add(btnClose);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "帮助 - SCNET_Restart_Tool";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox txtHelpContent;
        private System.Windows.Forms.RichTextBox txtProjectInfo;
        private System.Windows.Forms.RichTextBox txtVersionInfo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.PictureBox pictureBoxLogo;

        #endregion
    }
}