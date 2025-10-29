namespace SCNET_Restart_Tool.Form.Help
{
    partial class HelpForm
    {
        /// <summary>
        /// 必需的设计器变量
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，则为 true；否则为 false</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.btnFunctionDescription = new System.Windows.Forms.Button();
            this.btnProjectInfo = new System.Windows.Forms.Button();
            this.btnVersionInfo = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.pictureBoxLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 60);
            this.panel1.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(64, 18);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(129, 26);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "帮助与支持";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(12, 6);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // btnFunctionDescription
            // 
            this.btnFunctionDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFunctionDescription.BackColor = System.Drawing.Color.White;
            this.btnFunctionDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunctionDescription.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFunctionDescription.Location = new System.Drawing.Point(12, 78);
            this.btnFunctionDescription.Name = "btnFunctionDescription";
            this.btnFunctionDescription.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.btnFunctionDescription.Size = new System.Drawing.Size(376, 45);
            this.btnFunctionDescription.TabIndex = 1;
            this.btnFunctionDescription.Text = "功能说明";
            this.btnFunctionDescription.UseVisualStyleBackColor = false;
            this.btnFunctionDescription.Click += new System.EventHandler(this.btnFunctionDescription_Click);
            // 
            // btnProjectInfo
            // 
            this.btnProjectInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProjectInfo.BackColor = System.Drawing.Color.White;
            this.btnProjectInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProjectInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProjectInfo.Location = new System.Drawing.Point(12, 129);
            this.btnProjectInfo.Name = "btnProjectInfo";
            this.btnProjectInfo.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.btnProjectInfo.Size = new System.Drawing.Size(376, 45);
            this.btnProjectInfo.TabIndex = 2;
            this.btnProjectInfo.Text = "项目信息";
            this.btnProjectInfo.UseVisualStyleBackColor = false;
            this.btnProjectInfo.Click += new System.EventHandler(this.btnProjectInfo_Click);
            // 
            // btnVersionInfo
            // 
            this.btnVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVersionInfo.BackColor = System.Drawing.Color.White;
            this.btnVersionInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVersionInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnVersionInfo.Location = new System.Drawing.Point(12, 180);
            this.btnVersionInfo.Name = "btnVersionInfo";
            this.btnVersionInfo.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.btnVersionInfo.Size = new System.Drawing.Size(376, 45);
            this.btnVersionInfo.TabIndex = 3;
            this.btnVersionInfo.Text = "版本信息";
            this.btnVersionInfo.UseVisualStyleBackColor = false;
            this.btnVersionInfo.Click += new System.EventHandler(this.btnVersionInfo_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(313, 250);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 292);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnVersionInfo);
            this.Controls.Add(this.btnProjectInfo);
            this.Controls.Add(this.btnFunctionDescription);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "帮助与支持";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Button btnFunctionDescription;
        private System.Windows.Forms.Button btnProjectInfo;
        private System.Windows.Forms.Button btnVersionInfo;
        private System.Windows.Forms.Button btnClose;
    }
}