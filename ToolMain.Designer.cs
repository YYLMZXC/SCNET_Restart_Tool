namespace SCNET_Restart_Tool
{
    partial class ToolMain
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvServers;
        private System.Windows.Forms.Button btnAddServer;
        private System.Windows.Forms.Button btnDeleteServer;
        private System.Windows.Forms.Button btnBatchRestart;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.TextBox txtExePath;
        private System.Windows.Forms.Button btnSelectExe;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtScheduleTime;
        private System.Windows.Forms.TextBox txtIntervalHours;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnStartMonitor;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnStopAllServers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCommandStatus;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Button btnToggleCommands;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvServers = new System.Windows.Forms.DataGridView();
            this.btnAddServer = new System.Windows.Forms.Button();
            this.btnDeleteServer = new System.Windows.Forms.Button();
            this.btnBatchRestart = new System.Windows.Forms.Button();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtExePath = new System.Windows.Forms.TextBox();
            this.btnSelectExe = new System.Windows.Forms.Button();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtScheduleTime = new System.Windows.Forms.TextBox();
            this.txtIntervalHours = new System.Windows.Forms.TextBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnStartMonitor = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnStopAllServers = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCommandStatus = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnToggleCommands = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();

            // dgvServers
            this.dgvServers.AllowUserToAddRows = false;
            this.dgvServers.AllowUserToDeleteRows = false;
            this.dgvServers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvServers.Location = new System.Drawing.Point(0, 100);
            this.dgvServers.MultiSelect = false;
            this.dgvServers.Name = "dgvServers";
            this.dgvServers.ReadOnly = true;
            this.dgvServers.RowHeadersVisible = false;
            this.dgvServers.RowTemplate.Height = 23;
            this.dgvServers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServers.Size = new System.Drawing.Size(300, 500);
            this.dgvServers.TabIndex = 0;
            this.dgvServers.SelectionChanged += new System.EventHandler(this.DgvServers_SelectionChanged);
            this.dgvServers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvServers_CellFormatting);

            // btnAddServer
            this.btnAddServer.Location = new System.Drawing.Point(12, 12);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(75, 23);
            this.btnAddServer.TabIndex = 1;
            this.btnAddServer.Text = "添加服务端";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler(this.BtnAddServer_Click);

            // btnDeleteServer
            this.btnDeleteServer.Location = new System.Drawing.Point(93, 12);
            this.btnDeleteServer.Name = "btnDeleteServer";
            this.btnDeleteServer.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteServer.TabIndex = 2;
            this.btnDeleteServer.Text = "删除选中";
            this.btnDeleteServer.UseVisualStyleBackColor = true;
            this.btnDeleteServer.Click += new System.EventHandler(this.BtnDeleteServer_Click);

            // btnBatchRestart
            this.btnBatchRestart.Location = new System.Drawing.Point(174, 12);
            this.btnBatchRestart.Name = "btnBatchRestart";
            this.btnBatchRestart.Size = new System.Drawing.Size(75, 23);
            this.btnBatchRestart.TabIndex = 3;
            this.btnBatchRestart.Text = "批量重启";
            this.btnBatchRestart.UseVisualStyleBackColor = true;
            this.btnBatchRestart.Click += new System.EventHandler(this.BtnBatchRestart_Click);

            // txtServerName
            this.txtServerName.Location = new System.Drawing.Point(120, 12);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(200, 21);
            this.txtServerName.TabIndex = 4;

            // txtExePath
            this.txtExePath.Location = new System.Drawing.Point(120, 45);
            this.txtExePath.Name = "txtExePath";
            this.txtExePath.ReadOnly = true;
            this.txtExePath.Size = new System.Drawing.Size(300, 21);
            this.txtExePath.TabIndex = 5;

            // btnSelectExe
            this.btnSelectExe.Location = new System.Drawing.Point(430, 43);
            this.btnSelectExe.Name = "btnSelectExe";
            this.btnSelectExe.Size = new System.Drawing.Size(75, 23);
            this.btnSelectExe.TabIndex = 6;
            this.btnSelectExe.Text = "选择程序";
            this.btnSelectExe.UseVisualStyleBackColor = true;
            this.btnSelectExe.Click += new System.EventHandler(this.BtnSelectExe_Click);

            // txtIp
            this.txtIp.Location = new System.Drawing.Point(120, 78);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(150, 21);
            this.txtIp.TabIndex = 7;

            // txtPort
            this.txtPort.Location = new System.Drawing.Point(330, 78);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(80, 21);
            this.txtPort.TabIndex = 8;

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(120, 111);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 21);
            this.txtPassword.TabIndex = 9;

            // txtScheduleTime
            this.txtScheduleTime.Location = new System.Drawing.Point(120, 144);
            this.txtScheduleTime.Name = "txtScheduleTime";
            this.txtScheduleTime.Size = new System.Drawing.Size(100, 21);
            this.txtScheduleTime.TabIndex = 10;

            // txtIntervalHours
            this.txtIntervalHours.Location = new System.Drawing.Point(120, 177);
            this.txtIntervalHours.Name = "txtIntervalHours";
            this.txtIntervalHours.Size = new System.Drawing.Size(100, 21);
            this.txtIntervalHours.TabIndex = 11;

            // btnSaveSettings
            this.btnSaveSettings.Location = new System.Drawing.Point(120, 210);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSettings.TabIndex = 12;
            this.btnSaveSettings.Text = "保存设置";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);

            // btnStartMonitor
            this.btnStartMonitor.BackColor = System.Drawing.Color.LimeGreen;
            this.btnStartMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartMonitor.ForeColor = System.Drawing.Color.White;
            this.btnStartMonitor.Location = new System.Drawing.Point(220, 210);
            this.btnStartMonitor.Name = "btnStartMonitor";
            this.btnStartMonitor.Size = new System.Drawing.Size(100, 23);
            this.btnStartMonitor.TabIndex = 13;
            this.btnStartMonitor.Text = "开启监控";
            this.btnStartMonitor.UseVisualStyleBackColor = false;
            this.btnStartMonitor.Click += new System.EventHandler(this.BtnStartMonitor_Click);

            // btnStopServer
            this.btnStopServer.BackColor = System.Drawing.Color.Red;
            this.btnStopServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopServer.ForeColor = System.Drawing.Color.White;
            this.btnStopServer.Location = new System.Drawing.Point(340, 210);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(100, 23);
            this.btnStopServer.TabIndex = 14;
            this.btnStopServer.Text = "手动关闭";
            this.btnStopServer.UseVisualStyleBackColor = false;
            this.btnStopServer.Click += new System.EventHandler(this.BtnStopServer_Click);

            // btnStartServer
            this.btnStartServer.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnStartServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartServer.ForeColor = System.Drawing.Color.White;
            this.btnStartServer.Location = new System.Drawing.Point(460, 210);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(100, 23);
            this.btnStartServer.TabIndex = 31;
            this.btnStartServer.Text = "手动启动";
            this.btnStartServer.UseVisualStyleBackColor = false;
            this.btnStartServer.Click += new System.EventHandler(this.BtnStartServer_Click);

            // btnStopAllServers
            this.btnStopAllServers.BackColor = System.Drawing.Color.MediumPurple;
            this.btnStopAllServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopAllServers.ForeColor = System.Drawing.Color.White;
            this.btnStopAllServers.Location = new System.Drawing.Point(580, 210);
            this.btnStopAllServers.Name = "btnStopAllServers";
            this.btnStopAllServers.Size = new System.Drawing.Size(100, 23);
            this.btnStopAllServers.TabIndex = 32;
            this.btnStopAllServers.Text = "关闭所有";
            this.btnStopAllServers.UseVisualStyleBackColor = false;
            this.btnStopAllServers.Click += new System.EventHandler(this.BtnStopAllServers_Click);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "服务端名称：";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "程序路径：";

            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "IP：";

            // label4
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(280, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "端口：";

            // label5
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "密码：";

            // label6
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "定时关闭时间：";

            // label7
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "间隔小时数：";

            // splitContainer1
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // splitContainer1.Panel1
            this.splitContainer1.Panel1.Controls.Add(this.dgvServers);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddServer);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeleteServer);
            this.splitContainer1.Panel1.Controls.Add(this.btnBatchRestart);
            // splitContainer1.Panel2
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 600);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 22;

            // txtCommand
            this.txtCommand.Location = new System.Drawing.Point(120, 260);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(200, 21);
            this.txtCommand.TabIndex = 25;

            // btnSendCommand
            this.btnSendCommand.Location = new System.Drawing.Point(340, 260);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(75, 23);
            this.btnSendCommand.TabIndex = 24;
            this.btnSendCommand.Text = "发送指令";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.BtnSendCommand_Click);

            // label8
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 263);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "指令：";

            // lblCommandStatus
            this.lblCommandStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCommandStatus.Location = new System.Drawing.Point(120, 290);
            this.lblCommandStatus.Name = "lblCommandStatus";
            this.lblCommandStatus.Size = new System.Drawing.Size(560, 30);
            this.lblCommandStatus.TabIndex = 26;
            this.lblCommandStatus.Text = "指令状态";

            // splitContainer2
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // splitContainer2.Panel1
            this.splitContainer2.Panel1.Controls.Add(this.btnToggleCommands);
            this.splitContainer2.Panel1.Controls.Add(this.lblCommandStatus);
            this.splitContainer2.Panel1.Controls.Add(this.txtCommand);
            this.splitContainer2.Panel1.Controls.Add(this.btnSendCommand);
            this.splitContainer2.Panel1.Controls.Add(this.label8);
            this.splitContainer2.Panel1.Controls.Add(this.label7);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.btnStopServer);
            this.splitContainer2.Panel1.Controls.Add(this.btnStartServer);
            this.splitContainer2.Panel1.Controls.Add(this.btnStopAllServers);
            this.splitContainer2.Panel1.Controls.Add(this.btnStartMonitor);
            this.splitContainer2.Panel1.Controls.Add(this.btnSaveSettings);
            this.splitContainer2.Panel1.Controls.Add(this.txtIntervalHours);
            this.splitContainer2.Panel1.Controls.Add(this.txtScheduleTime);
            this.splitContainer2.Panel1.Controls.Add(this.txtPassword);
            this.splitContainer2.Panel1.Controls.Add(this.txtPort);
            this.splitContainer2.Panel1.Controls.Add(this.txtIp);
            this.splitContainer2.Panel1.Controls.Add(this.btnSelectExe);
            this.splitContainer2.Panel1.Controls.Add(this.txtExePath);
            this.splitContainer2.Panel1.Controls.Add(this.txtServerName);
            // splitContainer2.Panel2
            this.splitContainer2.Panel2.Controls.Add(this.dgvLogs);
            this.splitContainer2.Panel2.Controls.Add(this.label9);
            this.splitContainer2.Panel2.Controls.Add(this.btnClearLogs);
            this.splitContainer2.Size = new System.Drawing.Size(700, 600);
            this.splitContainer2.SplitterDistance = 350;
            this.splitContainer2.TabIndex = 27;

            // dgvLogs
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvLogs.Location = new System.Drawing.Point(0, 40);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowTemplate.Height = 23;
            this.dgvLogs.Size = new System.Drawing.Size(700, 210);
            this.dgvLogs.TabIndex = 28;

            // label9
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(10, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 29;
            this.label9.Text = "操作日志";

            // btnClearLogs
            this.btnClearLogs.Location = new System.Drawing.Point(600, 5);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(75, 23);
            this.btnClearLogs.TabIndex = 30;
            this.btnClearLogs.Text = "清空日志";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);

            // btnToggleCommands
            this.btnToggleCommands.Location = new System.Drawing.Point(430, 260);
            this.btnToggleCommands.Name = "btnToggleCommands";
            this.btnToggleCommands.Size = new System.Drawing.Size(100, 23);
            this.btnToggleCommands.TabIndex = 33;
            this.btnToggleCommands.Text = "启用指令";
            this.btnToggleCommands.UseVisualStyleBackColor = true;
            this.btnToggleCommands.Click += new System.EventHandler(this.BtnToggleCommands_Click);

            // ToolMain
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ToolMain";
            this.Text = "生存战争服务端管理工具 v6.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}