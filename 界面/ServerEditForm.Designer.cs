namespace SCNET_Restart_Tool
{
    partial class ServerEditForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExePath = new System.Windows.Forms.TextBox();
            this.btnBrowseExe = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtScheduleTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nudIntervalHours = new System.Windows.Forms.NumericUpDown();
            this.chkEnableCommands = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntervalHours)).BeginInit();
            this.SuspendLayout();

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务端名称：";

            // txtServerName
            this.txtServerName.Location = new System.Drawing.Point(113, 27);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(300, 25);
            this.txtServerName.TabIndex = 1;

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "程序路径（EXE）：";

            // txtExePath
            this.txtExePath.Location = new System.Drawing.Point(113, 67);
            this.txtExePath.Name = "txtExePath";
            this.txtExePath.Size = new System.Drawing.Size(250, 25);
            this.txtExePath.TabIndex = 3;

            // btnBrowseExe
            this.btnBrowseExe.Location = new System.Drawing.Point(369, 67);
            this.btnBrowseExe.Name = "btnBrowseExe";
            this.btnBrowseExe.Size = new System.Drawing.Size(44, 25);
            this.btnBrowseExe.TabIndex = 4;
            this.btnBrowseExe.Text = "...";
            this.btnBrowseExe.UseVisualStyleBackColor = true;
            this.btnBrowseExe.Click += new System.EventHandler(this.btnBrowseExe_Click);

            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP：";

            // txtIp
            this.txtIp.Location = new System.Drawing.Point(113, 107);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(150, 25);
            this.txtIp.TabIndex = 6;

            // label4
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "端口：";

            // nudPort
            this.nudPort.Location = new System.Drawing.Point(113, 147);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(100, 25);
            this.nudPort.TabIndex = 8;
            this.nudPort.Value = new decimal(new int[] {
            5612,
            0,
            0,
            0});

            // label5
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "密码：";

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(113, 187);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 25);
            this.txtPassword.TabIndex = 10;

            // label6
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 230);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "定时重启时间：";

            // txtScheduleTime
            this.txtScheduleTime.Location = new System.Drawing.Point(113, 227);
            this.txtScheduleTime.Name = "txtScheduleTime";
            this.txtScheduleTime.Size = new System.Drawing.Size(100, 25);
            this.txtScheduleTime.TabIndex = 12;
            this.txtScheduleTime.Text = "01:00";

            // label7
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 270);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "间隔重启（小时）：";

            // nudIntervalHours
            this.nudIntervalHours.DecimalPlaces = 1;
            this.nudIntervalHours.Location = new System.Drawing.Point(113, 267);
            this.nudIntervalHours.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nudIntervalHours.Name = "nudIntervalHours";
            this.nudIntervalHours.Size = new System.Drawing.Size(100, 25);
            this.nudIntervalHours.TabIndex = 14;
            this.nudIntervalHours.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});

            // chkEnableCommands
            this.chkEnableCommands.AutoSize = true;
            this.chkEnableCommands.Location = new System.Drawing.Point(113, 307);
            this.chkEnableCommands.Name = "chkEnableCommands";
            this.chkEnableCommands.Size = new System.Drawing.Size(126, 19);
            this.chkEnableCommands.TabIndex = 15;
            this.chkEnableCommands.Text = "启用命令行控制";
            this.chkEnableCommands.UseVisualStyleBackColor = true;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(113, 350);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(233, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ServerEditForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 400);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkEnableCommands);
            this.Controls.Add(this.nudIntervalHours);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtScheduleTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowseExe);
            this.Controls.Add(this.txtExePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.label1);
            this.Name = "ServerEditForm";
            this.Text = "编辑服务端";
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntervalHours)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtExePath;
        private System.Windows.Forms.Button btnBrowseExe;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtScheduleTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudIntervalHours;
        private System.Windows.Forms.CheckBox chkEnableCommands;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}