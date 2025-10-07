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
        private System.Windows.Forms.Button btnSettings;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMain));
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnToggleCommands = new System.Windows.Forms.Button();
            this.lblCommandStatus = new System.Windows.Forms.Label();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
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
            // 
            // dgvServers
            // 
            this.dgvServers.AllowUserToAddRows = false;
            this.dgvServers.AllowUserToDeleteRows = false;
            this.dgvServers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvServers.Location = new System.Drawing.Point(0, 125);
            this.dgvServers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvServers.MultiSelect = false;
            this.dgvServers.Name = "dgvServers";
            this.dgvServers.ReadOnly = true;
            this.dgvServers.RowHeadersVisible = false;
            this.dgvServers.RowTemplate.Height = 23;
            this.dgvServers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServers.Size = new System.Drawing.Size(399, 625);
            this.dgvServers.TabIndex = 0;
            this.dgvServers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvServers_CellFormatting);
            this.dgvServers.SelectionChanged += new System.EventHandler(this.DgvServers_SelectionChanged);
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point(16, 15);
            this.btnAddServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(100, 29);
            this.btnAddServer.TabIndex = 1;
            this.btnAddServer.Text = "添加服务端";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler(this.BtnAddServer_Click);
            // 
            // btnDeleteServer
            // 
            this.btnDeleteServer.Location = new System.Drawing.Point(124, 15);
            this.btnDeleteServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteServer.Name = "btnDeleteServer";
            this.btnDeleteServer.Size = new System.Drawing.Size(100, 29);
            this.btnDeleteServer.TabIndex = 2;
            this.btnDeleteServer.Text = "删除选中";
            this.btnDeleteServer.UseVisualStyleBackColor = true;
            this.btnDeleteServer.Click += new System.EventHandler(this.BtnDeleteServer_Click);
            // 
            // btnBatchRestart
            // 
            this.btnBatchRestart.Location = new System.Drawing.Point(232, 15);
            this.btnBatchRestart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBatchRestart.Name = "btnBatchRestart";
            this.btnBatchRestart.Size = new System.Drawing.Size(100, 29);
            this.btnBatchRestart.TabIndex = 3;
            this.btnBatchRestart.Text = "批量重启";
            this.btnBatchRestart.UseVisualStyleBackColor = true;
            this.btnBatchRestart.Click += new System.EventHandler(this.BtnBatchRestart_Click);
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(160, 15);
            this.txtServerName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(265, 25);
            this.txtServerName.TabIndex = 4;
            // 
            // txtExePath
            // 
            this.txtExePath.Location = new System.Drawing.Point(160, 56);
            this.txtExePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtExePath.Name = "txtExePath";
            this.txtExePath.ReadOnly = true;
            this.txtExePath.Size = new System.Drawing.Size(399, 25);
            this.txtExePath.TabIndex = 5;
            // 
            // btnSelectExe
            // 
            this.btnSelectExe.Location = new System.Drawing.Point(573, 54);
            this.btnSelectExe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectExe.Name = "btnSelectExe";
            this.btnSelectExe.Size = new System.Drawing.Size(100, 29);
            this.btnSelectExe.TabIndex = 6;
            this.btnSelectExe.Text = "选择程序";
            this.btnSelectExe.UseVisualStyleBackColor = true;
            this.btnSelectExe.Click += new System.EventHandler(this.BtnSelectExe_Click);
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(160, 98);
            this.txtIp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(199, 25);
            this.txtIp.TabIndex = 7;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(440, 98);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(105, 25);
            this.txtPort.TabIndex = 8;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(160, 139);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(265, 25);
            this.txtPassword.TabIndex = 9;
            // 
            // txtScheduleTime
            // 
            this.txtScheduleTime.Location = new System.Drawing.Point(160, 180);
            this.txtScheduleTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtScheduleTime.Name = "txtScheduleTime";
            this.txtScheduleTime.Size = new System.Drawing.Size(132, 25);
            this.txtScheduleTime.TabIndex = 10;
            // 
            // txtIntervalHours
            // 
            this.txtIntervalHours.Location = new System.Drawing.Point(160, 221);
            this.txtIntervalHours.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIntervalHours.Name = "txtIntervalHours";
            this.txtIntervalHours.Size = new System.Drawing.Size(132, 25);
            this.txtIntervalHours.TabIndex = 11;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(160, 262);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(100, 29);
            this.btnSaveSettings.TabIndex = 12;
            this.btnSaveSettings.Text = "保存设置";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);
            // 
            // btnStartMonitor
            // 
            this.btnStartMonitor.BackColor = System.Drawing.Color.LimeGreen;
            this.btnStartMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartMonitor.ForeColor = System.Drawing.Color.White;
            this.btnStartMonitor.Location = new System.Drawing.Point(293, 262);
            this.btnStartMonitor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStartMonitor.Name = "btnStartMonitor";
            this.btnStartMonitor.Size = new System.Drawing.Size(133, 29);
            this.btnStartMonitor.TabIndex = 13;
            this.btnStartMonitor.Text = "开启监控";
            this.btnStartMonitor.UseVisualStyleBackColor = false;
            this.btnStartMonitor.Click += new System.EventHandler(this.BtnStartMonitor_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.BackColor = System.Drawing.Color.Red;
            this.btnStopServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopServer.ForeColor = System.Drawing.Color.White;
            this.btnStopServer.Location = new System.Drawing.Point(453, 262);
            this.btnStopServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(133, 29);
            this.btnStopServer.TabIndex = 14;
            this.btnStopServer.Text = "手动关闭";
            this.btnStopServer.UseVisualStyleBackColor = false;
            this.btnStopServer.Click += new System.EventHandler(this.BtnStopServer_Click);
            // 
            // btnStartServer
            // 
            this.btnStartServer.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnStartServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartServer.ForeColor = System.Drawing.Color.White;
            this.btnStartServer.Location = new System.Drawing.Point(613, 262);
            this.btnStartServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(133, 29);
            this.btnStartServer.TabIndex = 31;
            this.btnStartServer.Text = "手动启动";
            this.btnStartServer.UseVisualStyleBackColor = false;
            this.btnStartServer.Click += new System.EventHandler(this.BtnStartServer_Click);
            // 
            // btnStopAllServers
            // 
            this.btnStopAllServers.BackColor = System.Drawing.Color.MediumPurple;
            this.btnStopAllServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopAllServers.ForeColor = System.Drawing.Color.White;
            this.btnStopAllServers.Location = new System.Drawing.Point(773, 262);
            this.btnStopAllServers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStopAllServers.Name = "btnStopAllServers";
            this.btnStopAllServers.Size = new System.Drawing.Size(133, 29);
            this.btnStopAllServers.TabIndex = 32;
            this.btnStopAllServers.Text = "关闭所有";
            this.btnStopAllServers.UseVisualStyleBackColor = false;
            this.btnStopAllServers.Click += new System.EventHandler(this.BtnStopAllServers_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "服务端名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "程序路径：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 101);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "IP：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(373, 101);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "端口：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 142);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "密码：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 184);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 15);
            this.label6.TabIndex = 20;
            this.label6.Text = "定时关闭时间：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 225);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "间隔小时数：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvServers);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddServer);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeleteServer);
            this.splitContainer1.Panel1.Controls.Add(this.btnBatchRestart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1333, 750);
            this.splitContainer1.SplitterDistance = 399;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 22;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
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
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgvLogs);
            this.splitContainer2.Panel2.Controls.Add(this.label9);
            this.splitContainer2.Panel2.Controls.Add(this.btnClearLogs);
            this.splitContainer2.Size = new System.Drawing.Size(929, 750);
            this.splitContainer2.SplitterDistance = 437;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 27;
            // 
            // btnToggleCommands
            // 
            this.btnToggleCommands.Location = new System.Drawing.Point(573, 325);
            this.btnToggleCommands.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToggleCommands.Name = "btnToggleCommands";
            this.btnToggleCommands.Size = new System.Drawing.Size(133, 29);
            this.btnToggleCommands.TabIndex = 33;
            this.btnToggleCommands.Text = "启用指令";
            this.btnToggleCommands.UseVisualStyleBackColor = true;
            this.btnToggleCommands.Click += new System.EventHandler(this.BtnToggleCommands_Click);
            // 
            // lblCommandStatus
            // 
            this.lblCommandStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCommandStatus.Location = new System.Drawing.Point(160, 362);
            this.lblCommandStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCommandStatus.Name = "lblCommandStatus";
            this.lblCommandStatus.Size = new System.Drawing.Size(746, 37);
            this.lblCommandStatus.TabIndex = 26;
            this.lblCommandStatus.Text = "指令状态";
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(160, 325);
            this.txtCommand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(265, 25);
            this.txtCommand.TabIndex = 25;
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Location = new System.Drawing.Point(453, 325);
            this.btnSendCommand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(100, 29);
            this.btnSendCommand.TabIndex = 24;
            this.btnSendCommand.Text = "发送指令";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.BtnSendCommand_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 329);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 15);
            this.label8.TabIndex = 23;
            this.label8.Text = "指令：";
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvLogs.Location = new System.Drawing.Point(0, 46);
            this.dgvLogs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowTemplate.Height = 23;
            this.dgvLogs.Size = new System.Drawing.Size(929, 262);
            this.dgvLogs.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(13, 12);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 15);
            this.label9.TabIndex = 29;
            this.label9.Text = "操作日志";
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Location = new System.Drawing.Point(800, 6);
            this.btnClearLogs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(100, 29);
            this.btnClearLogs.TabIndex = 30;
            this.btnClearLogs.Text = "清空日志";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(1227, 12);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(80, 29);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "设置";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // ToolMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 750);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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