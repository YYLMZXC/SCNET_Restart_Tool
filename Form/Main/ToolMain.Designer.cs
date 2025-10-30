using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    partial class ToolMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolTip buttonToolTip;
        private System.Windows.Forms.Button aboutBtn;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMain));
            this.buttonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.aboutBtn = new System.Windows.Forms.Button();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.serverListSplit = new System.Windows.Forms.SplitContainer();
            this.serversGridView = new System.Windows.Forms.DataGridView();
            this.operationPanel = new System.Windows.Forms.Panel();
            this.monitorGroup = new System.Windows.Forms.GroupBox();
            this.stopMonitorBtn = new System.Windows.Forms.Button();
            this.startMonitorBtn = new System.Windows.Forms.Button();
            this.controlGroup = new System.Windows.Forms.GroupBox();
            this.stopAllBtn = new System.Windows.Forms.Button();
            this.restartBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.manageGroup = new System.Windows.Forms.GroupBox();
            this.folderManagerBtn = new System.Windows.Forms.Button();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.editBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.editGroup = new System.Windows.Forms.GroupBox();
            this.cancelEditBtn = new System.Windows.Forms.Button();
            this.saveEditBtn = new System.Windows.Forms.Button();
            this.serverEditPanel = new System.Windows.Forms.Panel();
            this.commandPanel = new System.Windows.Forms.Panel();
            this.sendCommandBtn = new System.Windows.Forms.Button();
            this.commandInput = new System.Windows.Forms.TextBox();
            this.commandLabel = new System.Windows.Forms.Label();
            this.advancedOptionsLabel = new System.Windows.Forms.Label();
            this.enableCommandsChk = new System.Windows.Forms.CheckBox();
            this.intervalLabel = new System.Windows.Forms.Label();
            this.intervalHoursNum = new System.Windows.Forms.NumericUpDown();
            this.scheduleLabel = new System.Windows.Forms.Label();
            this.scheduleTimeInput = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portNum = new System.Windows.Forms.NumericUpDown();
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipInput = new System.Windows.Forms.TextBox();
            this.browseExeBtn = new System.Windows.Forms.Button();
            this.exePathLabel = new System.Windows.Forms.Label();
            this.exePathInput = new System.Windows.Forms.TextBox();
            this.serverNameLabel = new System.Windows.Forms.Label();
            this.serverNameInput = new System.Windows.Forms.TextBox();
            this.clearLogBtn = new System.Windows.Forms.Button();
            this.logSplit = new System.Windows.Forms.SplitContainer();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.logControlPanel = new System.Windows.Forms.Panel();
            this.infoLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverListSplit)).BeginInit();
            this.serverListSplit.Panel1.SuspendLayout();
            this.serverListSplit.Panel2.SuspendLayout();
            this.serverListSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serversGridView)).BeginInit();
            this.operationPanel.SuspendLayout();
            this.monitorGroup.SuspendLayout();
            this.controlGroup.SuspendLayout();
            this.manageGroup.SuspendLayout();
            this.editGroup.SuspendLayout();
            this.serverEditPanel.SuspendLayout();
            this.commandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intervalHoursNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logSplit)).BeginInit();
            this.logSplit.Panel1.SuspendLayout();
            this.logSplit.Panel2.SuspendLayout();
            this.logSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // aboutBtn
            // 
            this.aboutBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.aboutBtn.FlatAppearance.BorderSize = 0;
            this.aboutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutBtn.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.aboutBtn.ForeColor = System.Drawing.Color.White;
            this.aboutBtn.Location = new System.Drawing.Point(185, 174);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.aboutBtn.Size = new System.Drawing.Size(80, 30);
            this.aboutBtn.TabIndex = 0;
            this.aboutBtn.Text = "帮助";
            this.buttonToolTip.SetToolTip(this.aboutBtn, "帮助 - SCNET_Restart_Tool");
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.serverListSplit);
            this.mainSplitContainer.Panel1.Controls.Add(this.serverEditPanel);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.aboutBtn);
            this.mainSplitContainer.Panel2.Controls.Add(this.clearLogBtn);
            this.mainSplitContainer.Panel2.Controls.Add(this.logSplit);
            this.mainSplitContainer.Panel2.Controls.Add(this.infoLabel);
            this.mainSplitContainer.Size = new System.Drawing.Size(1150, 749);
            this.mainSplitContainer.SplitterDistance = 869;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // serverListSplit
            // 
            this.serverListSplit.Dock = System.Windows.Forms.DockStyle.Top;
            this.serverListSplit.Location = new System.Drawing.Point(0, 0);
            this.serverListSplit.Name = "serverListSplit";
            this.serverListSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // serverListSplit.Panel1
            // 
            this.serverListSplit.Panel1.Controls.Add(this.serversGridView);
            // 
            // serverListSplit.Panel2
            // 
            this.serverListSplit.Panel2.Controls.Add(this.operationPanel);
            this.serverListSplit.Size = new System.Drawing.Size(869, 380);
            this.serverListSplit.SplitterDistance = 193;
            this.serverListSplit.TabIndex = 0;
            // 
            // serversGridView
            // 
            this.serversGridView.AllowUserToResizeRows = false;
            this.serversGridView.BackgroundColor = System.Drawing.Color.White;
            this.serversGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.serversGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.serversGridView.ColumnHeadersHeight = 29;
            this.serversGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serversGridView.Location = new System.Drawing.Point(0, 0);
            this.serversGridView.Name = "serversGridView";
            this.serversGridView.RowHeadersVisible = false;
            this.serversGridView.RowHeadersWidth = 51;
            this.serversGridView.RowTemplate.Height = 25;
            this.serversGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.serversGridView.Size = new System.Drawing.Size(869, 193);
            this.serversGridView.TabIndex = 0;
            // 
            // operationPanel
            // 
            this.operationPanel.Controls.Add(this.monitorGroup);
            this.operationPanel.Controls.Add(this.controlGroup);
            this.operationPanel.Controls.Add(this.manageGroup);
            this.operationPanel.Controls.Add(this.editGroup);
            this.operationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationPanel.Location = new System.Drawing.Point(0, 0);
            this.operationPanel.Name = "operationPanel";
            this.operationPanel.Size = new System.Drawing.Size(869, 183);
            this.operationPanel.TabIndex = 0;
            // 
            // monitorGroup
            // 
            this.monitorGroup.Controls.Add(this.stopMonitorBtn);
            this.monitorGroup.Controls.Add(this.startMonitorBtn);
            this.monitorGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.monitorGroup.Location = new System.Drawing.Point(662, 0);
            this.monitorGroup.Name = "monitorGroup";
            this.monitorGroup.Size = new System.Drawing.Size(217, 183);
            this.monitorGroup.TabIndex = 3;
            this.monitorGroup.TabStop = false;
            this.monitorGroup.Text = "监控控制";
            // 
            // stopMonitorBtn
            // 
            this.stopMonitorBtn.Enabled = false;
            this.stopMonitorBtn.Location = new System.Drawing.Point(15, 80);
            this.stopMonitorBtn.Name = "stopMonitorBtn";
            this.stopMonitorBtn.Size = new System.Drawing.Size(114, 49);
            this.stopMonitorBtn.TabIndex = 1;
            this.stopMonitorBtn.Text = "关闭监控";
            this.stopMonitorBtn.UseVisualStyleBackColor = true;
            // 
            // startMonitorBtn
            // 
            this.startMonitorBtn.Enabled = false;
            this.startMonitorBtn.Location = new System.Drawing.Point(15, 24);
            this.startMonitorBtn.Name = "startMonitorBtn";
            this.startMonitorBtn.Size = new System.Drawing.Size(114, 49);
            this.startMonitorBtn.TabIndex = 0;
            this.startMonitorBtn.Text = "开启监控";
            this.startMonitorBtn.UseVisualStyleBackColor = true;
            // 
            // controlGroup
            // 
            this.controlGroup.Controls.Add(this.stopAllBtn);
            this.controlGroup.Controls.Add(this.restartBtn);
            this.controlGroup.Controls.Add(this.stopBtn);
            this.controlGroup.Controls.Add(this.startBtn);
            this.controlGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.controlGroup.Location = new System.Drawing.Point(396, 0);
            this.controlGroup.Name = "controlGroup";
            this.controlGroup.Size = new System.Drawing.Size(266, 183);
            this.controlGroup.TabIndex = 2;
            this.controlGroup.TabStop = false;
            this.controlGroup.Text = "运行控制";
            // 
            // stopAllBtn
            // 
            this.stopAllBtn.Enabled = false;
            this.stopAllBtn.Location = new System.Drawing.Point(137, 80);
            this.stopAllBtn.Name = "stopAllBtn";
            this.stopAllBtn.Size = new System.Drawing.Size(123, 49);
            this.stopAllBtn.TabIndex = 3;
            this.stopAllBtn.Text = "停止全部";
            this.stopAllBtn.UseVisualStyleBackColor = true;
            // 
            // restartBtn
            // 
            this.restartBtn.Enabled = false;
            this.restartBtn.Location = new System.Drawing.Point(6, 79);
            this.restartBtn.Name = "restartBtn";
            this.restartBtn.Size = new System.Drawing.Size(107, 50);
            this.restartBtn.TabIndex = 2;
            this.restartBtn.Text = "重启";
            this.restartBtn.UseVisualStyleBackColor = true;
          
            // 
            // stopBtn
            // 
            this.stopBtn.Enabled = false;
            this.stopBtn.Location = new System.Drawing.Point(137, 25);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(123, 49);
            this.stopBtn.TabIndex = 1;
            this.stopBtn.Text = "停止";
            this.stopBtn.UseVisualStyleBackColor = true;
            // 
            // startBtn
            // 
            this.startBtn.Enabled = false;
            this.startBtn.Location = new System.Drawing.Point(6, 25);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(107, 49);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "启动";
            this.startBtn.UseVisualStyleBackColor = true;
            // 
            // manageGroup
            // 
            this.manageGroup.Controls.Add(this.folderManagerBtn);
            this.manageGroup.Controls.Add(this.deleteBtn);
            this.manageGroup.Controls.Add(this.editBtn);
            this.manageGroup.Controls.Add(this.addBtn);
            this.manageGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.manageGroup.Location = new System.Drawing.Point(175, 0);
            this.manageGroup.Name = "manageGroup";
            this.manageGroup.Size = new System.Drawing.Size(221, 183);
            this.manageGroup.TabIndex = 1;
            this.manageGroup.TabStop = false;
            this.manageGroup.Text = "服务器管理";
            // 
            // folderManagerBtn
            // 
            this.folderManagerBtn.Enabled = false;
            this.folderManagerBtn.Location = new System.Drawing.Point(108, 81);
            this.folderManagerBtn.Name = "folderManagerBtn";
            this.folderManagerBtn.Size = new System.Drawing.Size(101, 49);
            this.folderManagerBtn.TabIndex = 3;
            this.folderManagerBtn.Text = "文件夹";
            this.folderManagerBtn.UseVisualStyleBackColor = true;
           
            // 
            // deleteBtn
            // 
            this.deleteBtn.Enabled = false;
            this.deleteBtn.Location = new System.Drawing.Point(6, 80);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(96, 50);
            this.deleteBtn.TabIndex = 2;
            this.deleteBtn.Text = "删除";
            this.deleteBtn.UseVisualStyleBackColor = true;
            // 
            // editBtn
            // 
            this.editBtn.Enabled = false;
            this.editBtn.Location = new System.Drawing.Point(108, 25);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(101, 50);
            this.editBtn.TabIndex = 1;
            this.editBtn.Text = "修改";
            this.editBtn.UseVisualStyleBackColor = true;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(6, 24);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(96, 50);
            this.addBtn.TabIndex = 0;
            this.addBtn.Text = "增加";
            this.addBtn.UseVisualStyleBackColor = true;
            // 
            // editGroup
            // 
            this.editGroup.Controls.Add(this.cancelEditBtn);
            this.editGroup.Controls.Add(this.saveEditBtn);
            this.editGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.editGroup.Location = new System.Drawing.Point(0, 0);
            this.editGroup.Name = "editGroup";
            this.editGroup.Size = new System.Drawing.Size(175, 183);
            this.editGroup.TabIndex = 0;
            this.editGroup.TabStop = false;
            this.editGroup.Text = "编辑操作";
            // 
            // cancelEditBtn
            // 
            this.cancelEditBtn.Enabled = false;
            this.cancelEditBtn.Location = new System.Drawing.Point(15, 81);
            this.cancelEditBtn.Name = "cancelEditBtn";
            this.cancelEditBtn.Size = new System.Drawing.Size(132, 49);
            this.cancelEditBtn.TabIndex = 1;
            this.cancelEditBtn.Text = "取消";
            this.cancelEditBtn.UseVisualStyleBackColor = true;
            // 
            // saveEditBtn
            // 
            this.saveEditBtn.Enabled = false;
            this.saveEditBtn.Location = new System.Drawing.Point(15, 25);
            this.saveEditBtn.Name = "saveEditBtn";
            this.saveEditBtn.Size = new System.Drawing.Size(132, 49);
            this.saveEditBtn.TabIndex = 0;
            this.saveEditBtn.Text = "保存";
            this.saveEditBtn.UseVisualStyleBackColor = true;
            // 
            // serverEditPanel
            // 
            this.serverEditPanel.Controls.Add(this.commandPanel);
            this.serverEditPanel.Controls.Add(this.advancedOptionsLabel);
            this.serverEditPanel.Controls.Add(this.enableCommandsChk);
            this.serverEditPanel.Controls.Add(this.intervalLabel);
            this.serverEditPanel.Controls.Add(this.intervalHoursNum);
            this.serverEditPanel.Controls.Add(this.scheduleLabel);
            this.serverEditPanel.Controls.Add(this.scheduleTimeInput);
            this.serverEditPanel.Controls.Add(this.passwordLabel);
            this.serverEditPanel.Controls.Add(this.passwordInput);
            this.serverEditPanel.Controls.Add(this.portLabel);
            this.serverEditPanel.Controls.Add(this.portNum);
            this.serverEditPanel.Controls.Add(this.ipLabel);
            this.serverEditPanel.Controls.Add(this.ipInput);
            this.serverEditPanel.Controls.Add(this.browseExeBtn);
            this.serverEditPanel.Controls.Add(this.exePathLabel);
            this.serverEditPanel.Controls.Add(this.exePathInput);
            this.serverEditPanel.Controls.Add(this.serverNameLabel);
            this.serverEditPanel.Controls.Add(this.serverNameInput);
            this.serverEditPanel.Location = new System.Drawing.Point(0, 413);
            this.serverEditPanel.Name = "serverEditPanel";
            this.serverEditPanel.Padding = new System.Windows.Forms.Padding(15);
            this.serverEditPanel.Size = new System.Drawing.Size(866, 333);
            this.serverEditPanel.TabIndex = 1;
            // 
            // commandPanel
            // 
            this.commandPanel.Controls.Add(this.sendCommandBtn);
            this.commandPanel.Controls.Add(this.commandInput);
            this.commandPanel.Controls.Add(this.commandLabel);
            this.commandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.commandPanel.Location = new System.Drawing.Point(15, 258);
            this.commandPanel.Name = "commandPanel";
            this.commandPanel.Size = new System.Drawing.Size(836, 60);
            this.commandPanel.TabIndex = 18;
            // 
            // sendCommandBtn
            // 
            this.sendCommandBtn.Location = new System.Drawing.Point(441, 12);
            this.sendCommandBtn.Name = "sendCommandBtn";
            this.sendCommandBtn.Size = new System.Drawing.Size(119, 42);
            this.sendCommandBtn.TabIndex = 2;
            this.sendCommandBtn.Text = "发送";
            this.sendCommandBtn.UseVisualStyleBackColor = true;
            // 
            // commandInput
            // 
            this.commandInput.Location = new System.Drawing.Point(75, 12);
            this.commandInput.Name = "commandInput";
            this.commandInput.Size = new System.Drawing.Size(345, 25);
            this.commandInput.TabIndex = 1;
            this.commandInput.Text = "say 服务器即将重启";
            // 
            // commandLabel
            // 
            this.commandLabel.AutoSize = true;
            this.commandLabel.Location = new System.Drawing.Point(0, 15);
            this.commandLabel.Name = "commandLabel";
            this.commandLabel.Size = new System.Drawing.Size(82, 15);
            this.commandLabel.TabIndex = 0;
            this.commandLabel.Text = "发送指令：";
            // 
            // advancedOptionsLabel
            // 
            this.advancedOptionsLabel.AutoSize = true;
            this.advancedOptionsLabel.Location = new System.Drawing.Point(0, 220);
            this.advancedOptionsLabel.Name = "advancedOptionsLabel";
            this.advancedOptionsLabel.Size = new System.Drawing.Size(82, 15);
            this.advancedOptionsLabel.TabIndex = 17;
            this.advancedOptionsLabel.Text = "高级选项：";
            // 
            // enableCommandsChk
            // 
            this.enableCommandsChk.AutoSize = true;
            this.enableCommandsChk.Location = new System.Drawing.Point(94, 219);
            this.enableCommandsChk.Name = "enableCommandsChk";
            this.enableCommandsChk.Size = new System.Drawing.Size(119, 19);
            this.enableCommandsChk.TabIndex = 16;
            this.enableCommandsChk.Text = "启用指令功能";
            this.enableCommandsChk.UseVisualStyleBackColor = true;
            // 
            // intervalLabel
            // 
            this.intervalLabel.AutoSize = true;
            this.intervalLabel.Location = new System.Drawing.Point(230, 180);
            this.intervalLabel.Name = "intervalLabel";
            this.intervalLabel.Size = new System.Drawing.Size(82, 15);
            this.intervalLabel.TabIndex = 15;
            this.intervalLabel.Text = "间隔小时：";
            // 
            // intervalHoursNum
            // 
            this.intervalHoursNum.Location = new System.Drawing.Point(315, 177);
            this.intervalHoursNum.Name = "intervalHoursNum";
            this.intervalHoursNum.Size = new System.Drawing.Size(100, 25);
            this.intervalHoursNum.TabIndex = 14;
            // 
            // scheduleLabel
            // 
            this.scheduleLabel.AutoSize = true;
            this.scheduleLabel.Location = new System.Drawing.Point(0, 180);
            this.scheduleLabel.Name = "scheduleLabel";
            this.scheduleLabel.Size = new System.Drawing.Size(82, 15);
            this.scheduleLabel.TabIndex = 13;
            this.scheduleLabel.Text = "定时重启：";
            // 
            // scheduleTimeInput
            // 
            this.scheduleTimeInput.Location = new System.Drawing.Point(94, 177);
            this.scheduleTimeInput.Name = "scheduleTimeInput";
            this.scheduleTimeInput.Size = new System.Drawing.Size(100, 25);
            this.scheduleTimeInput.TabIndex = 12;
            this.scheduleTimeInput.Text = "01:00";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(0, 140);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(52, 15);
            this.passwordLabel.TabIndex = 11;
            this.passwordLabel.Text = "密码：";
            // 
            // passwordInput
            // 
            this.passwordInput.Location = new System.Drawing.Point(90, 140);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.Size = new System.Drawing.Size(250, 25);
            this.passwordInput.TabIndex = 10;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(280, 100);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(52, 15);
            this.portLabel.TabIndex = 9;
            this.portLabel.Text = "端口：";
            // 
            // portNum
            // 
            this.portNum.Location = new System.Drawing.Point(335, 97);
            this.portNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.portNum.Name = "portNum";
            this.portNum.Size = new System.Drawing.Size(100, 25);
            this.portNum.TabIndex = 8;
            this.portNum.Value = new decimal(new int[] {
            5612,
            0,
            0,
            0});
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(0, 100);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(38, 15);
            this.ipLabel.TabIndex = 7;
            this.ipLabel.Text = "IP：";
            // 
            // ipInput
            // 
            this.ipInput.Location = new System.Drawing.Point(90, 96);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(150, 25);
            this.ipInput.TabIndex = 6;
            this.ipInput.Text = "127.0.0.1";
            // 
            // browseExeBtn
            // 
            this.browseExeBtn.Location = new System.Drawing.Point(456, 45);
            this.browseExeBtn.Name = "browseExeBtn";
            this.browseExeBtn.Size = new System.Drawing.Size(108, 45);
            this.browseExeBtn.TabIndex = 5;
            this.browseExeBtn.Text = "浏览";
            this.browseExeBtn.UseVisualStyleBackColor = true;
            // 
            // exePathLabel
            // 
            this.exePathLabel.AutoSize = true;
            this.exePathLabel.Location = new System.Drawing.Point(0, 60);
            this.exePathLabel.Name = "exePathLabel";
            this.exePathLabel.Size = new System.Drawing.Size(82, 15);
            this.exePathLabel.TabIndex = 4;
            this.exePathLabel.Text = "程序路径：";
            // 
            // exePathInput
            // 
            this.exePathInput.Location = new System.Drawing.Point(90, 57);
            this.exePathInput.Name = "exePathInput";
            this.exePathInput.Size = new System.Drawing.Size(360, 25);
            this.exePathInput.TabIndex = 3;
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.AutoSize = true;
            this.serverNameLabel.Location = new System.Drawing.Point(0, 20);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(82, 15);
            this.serverNameLabel.TabIndex = 2;
            this.serverNameLabel.Text = "服务器名：";
            // 
            // serverNameInput
            // 
            this.serverNameInput.Location = new System.Drawing.Point(90, 17);
            this.serverNameInput.Name = "serverNameInput";
            this.serverNameInput.Size = new System.Drawing.Size(250, 25);
            this.serverNameInput.TabIndex = 1;
         
            // 
            // clearLogBtn
            // 
            this.clearLogBtn.Location = new System.Drawing.Point(12, 321);
            this.clearLogBtn.Name = "clearLogBtn";
            this.clearLogBtn.Size = new System.Drawing.Size(253, 44);
            this.clearLogBtn.TabIndex = 0;
            this.clearLogBtn.Text = "清空日志";
            this.clearLogBtn.UseVisualStyleBackColor = true;
            // 
            // logSplit
            // 
            this.logSplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logSplit.Location = new System.Drawing.Point(0, 371);
            this.logSplit.Name = "logSplit";
            this.logSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // logSplit.Panel1
            // 
            this.logSplit.Panel1.Controls.Add(this.logTextBox);
            // 
            // logSplit.Panel2
            // 
            this.logSplit.Panel2.Controls.Add(this.logControlPanel);
            this.logSplit.Size = new System.Drawing.Size(277, 378);
            this.logSplit.SplitterDistance = 343;
            this.logSplit.TabIndex = 1;
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(277, 343);
            this.logTextBox.TabIndex = 0;
            this.logTextBox.Text = "";
            // 
            // logControlPanel
            // 
            this.logControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logControlPanel.Location = new System.Drawing.Point(0, 0);
            this.logControlPanel.Name = "logControlPanel";
            this.logControlPanel.Size = new System.Drawing.Size(277, 31);
            this.logControlPanel.TabIndex = 0;
            // 
            // infoLabel
            // 
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.infoLabel.Location = new System.Drawing.Point(0, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(10);
            this.infoLabel.Size = new System.Drawing.Size(277, 332);
            this.infoLabel.TabIndex = 0;
         
            // 
            // ToolMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 749);
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "ToolMain";
            this.Text = "SCNET服务端管理工具";
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.serverListSplit.Panel1.ResumeLayout(false);
            this.serverListSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serverListSplit)).EndInit();
            this.serverListSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serversGridView)).EndInit();
            this.operationPanel.ResumeLayout(false);
            this.monitorGroup.ResumeLayout(false);
            this.controlGroup.ResumeLayout(false);
            this.manageGroup.ResumeLayout(false);
            this.editGroup.ResumeLayout(false);
            this.serverEditPanel.ResumeLayout(false);
            this.serverEditPanel.PerformLayout();
            this.commandPanel.ResumeLayout(false);
            this.commandPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intervalHoursNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNum)).EndInit();
            this.logSplit.Panel1.ResumeLayout(false);
            this.logSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logSplit)).EndInit();
            this.logSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer serverListSplit;
        private System.Windows.Forms.DataGridView serversGridView;
        private System.Windows.Forms.Panel operationPanel;
        private System.Windows.Forms.GroupBox monitorGroup;
        private System.Windows.Forms.Button stopMonitorBtn;
        private System.Windows.Forms.Button startMonitorBtn;
        private System.Windows.Forms.GroupBox controlGroup;
        private System.Windows.Forms.Button stopAllBtn;
        private System.Windows.Forms.Button restartBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.GroupBox manageGroup;
        private System.Windows.Forms.Button folderManagerBtn;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.GroupBox editGroup;
        private System.Windows.Forms.Button cancelEditBtn;
        private System.Windows.Forms.Button saveEditBtn;
        private System.Windows.Forms.Panel serverEditPanel;
        private System.Windows.Forms.Panel commandPanel;
        private System.Windows.Forms.Button sendCommandBtn;
        private System.Windows.Forms.TextBox commandInput;
        private System.Windows.Forms.Label commandLabel;
        private System.Windows.Forms.Label advancedOptionsLabel;
        private System.Windows.Forms.CheckBox enableCommandsChk;
        private System.Windows.Forms.Label intervalLabel;
        private System.Windows.Forms.NumericUpDown intervalHoursNum;
        private System.Windows.Forms.Label scheduleLabel;
        private System.Windows.Forms.TextBox scheduleTimeInput;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.NumericUpDown portNum;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.TextBox ipInput;
        private System.Windows.Forms.Button browseExeBtn;
        private System.Windows.Forms.Label exePathLabel;
        private System.Windows.Forms.TextBox exePathInput;
        private System.Windows.Forms.Label serverNameLabel;
        private System.Windows.Forms.TextBox serverNameInput;
        private System.Windows.Forms.SplitContainer logSplit;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Button clearLogBtn;
        private System.Windows.Forms.Label infoLabel;
        private Panel logControlPanel;
    }
}