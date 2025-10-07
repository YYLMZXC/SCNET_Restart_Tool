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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtHelpContent = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtProjectInfo = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtVersionInfo = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1067, 512);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtHelpContent);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(1059, 483);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "功能说明";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtHelpContent
            // 
            this.txtHelpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelpContent.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtHelpContent.Location = new System.Drawing.Point(4, 4);
            this.txtHelpContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtHelpContent.Name = "txtHelpContent";
            this.txtHelpContent.ReadOnly = true;
            this.txtHelpContent.Size = new System.Drawing.Size(1051, 475);
            this.txtHelpContent.TabIndex = 0;
            this.txtHelpContent.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtProjectInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(1059, 483);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "项目信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtProjectInfo
            // 
            this.txtProjectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProjectInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtProjectInfo.Location = new System.Drawing.Point(4, 4);
            this.txtProjectInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProjectInfo.Name = "txtProjectInfo";
            this.txtProjectInfo.ReadOnly = true;
            this.txtProjectInfo.Size = new System.Drawing.Size(1051, 475);
            this.txtProjectInfo.TabIndex = 0;
            this.txtProjectInfo.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtVersionInfo);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Size = new System.Drawing.Size(1059, 483);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "版本信息";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtVersionInfo
            // 
            this.txtVersionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersionInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVersionInfo.Location = new System.Drawing.Point(4, 4);
            this.txtVersionInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVersionInfo.Name = "txtVersionInfo";
            this.txtVersionInfo.ReadOnly = true;
            this.txtVersionInfo.Size = new System.Drawing.Size(1051, 475);
            this.txtVersionInfo.TabIndex = 0;
            this.txtVersionInfo.Text = "";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(953, 520);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 29);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 562);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "帮助 - SCNET_Restart_Tool";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox txtHelpContent;
        private System.Windows.Forms.RichTextBox txtProjectInfo;
        private System.Windows.Forms.RichTextBox txtVersionInfo;
        private System.Windows.Forms.Button btnClose;

        #endregion
    }
}