namespace SCNET_Restart_Tool
{
    partial class HelpForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtHelpContent = new System.Windows.Forms.RichTextBox();
            this.txtProjectInfo = new System.Windows.Forms.RichTextBox();
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
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 410);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtHelpContent);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 384);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "功能说明";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtProjectInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 384);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "项目信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtVersionInfo);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 384);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "版本信息";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtHelpContent
            // 
            this.txtHelpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelpContent.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtHelpContent.Location = new System.Drawing.Point(3, 3);
            this.txtHelpContent.Name = "txtHelpContent";
            this.txtHelpContent.ReadOnly = true;
            this.txtHelpContent.Size = new System.Drawing.Size(786, 378);
            this.txtHelpContent.TabIndex = 0;
            this.txtHelpContent.Text = "";
            // 
            // txtProjectInfo
            // 
            this.txtProjectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProjectInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtProjectInfo.Location = new System.Drawing.Point(3, 3);
            this.txtProjectInfo.Name = "txtProjectInfo";
            this.txtProjectInfo.ReadOnly = true;
            this.txtProjectInfo.Size = new System.Drawing.Size(786, 378);
            this.txtProjectInfo.TabIndex = 0;
            this.txtProjectInfo.Text = "";
            // 
            // txtVersionInfo
            // 
            this.txtVersionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersionInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVersionInfo.Location = new System.Drawing.Point(3, 3);
            this.txtVersionInfo.Name = "txtVersionInfo";
            this.txtVersionInfo.ReadOnly = true;
            this.txtVersionInfo.Size = new System.Drawing.Size(786, 378);
            this.txtVersionInfo.TabIndex = 0;
            this.txtVersionInfo.Text = "";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(715, 416);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpForm";
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