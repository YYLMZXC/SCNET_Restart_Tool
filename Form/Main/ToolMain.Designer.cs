using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    partial class ToolMain
    {
        private System.ComponentModel.IContainer components = null;
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
            aboutBtn = new Button();
            mainSplitContainer = new SplitContainer();
            serverListSplit = new SplitContainer();
            serversGridView = new DataGridView();
            operationPanel = new Panel();
            monitorGroup = new GroupBox();
            stopMonitorBtn = new Button();
            startMonitorBtn = new Button();
            controlGroup = new GroupBox();
            stopAllBtn = new Button();
            restartBtn = new Button();
            stopBtn = new Button();
            startBtn = new Button();
            manageGroup = new GroupBox();
            folderManagerBtn = new Button();
            deleteBtn = new Button();
            editBtn = new Button();
            addBtn = new Button();
            editGroup = new GroupBox();
            cancelEditBtn = new Button();
            saveEditBtn = new Button();
            serverEditPanel = new Panel();
            commandPanel = new Panel();
            sendCommandBtn = new Button();
            commandInput = new TextBox();
            commandLabel = new Label();
            advancedOptionsLabel = new Label();
            enableCommandsChk = new CheckBox();
            intervalLabel = new Label();
            intervalHoursNum = new NumericUpDown();
            scheduleLabel = new Label();
            scheduleTimeInput = new TextBox();
            passwordLabel = new Label();
            passwordInput = new TextBox();
            portLabel = new Label();
            portNum = new NumericUpDown();
            ipLabel = new Label();
            ipInput = new TextBox();
            browseExeBtn = new Button();
            exePathLabel = new Label();
            exePathInput = new TextBox();
            serverNameLabel = new Label();
            serverNameInput = new TextBox();
            clearLogBtn = new Button();
            logSplit = new SplitContainer();
            logTextBox = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)serverListSplit).BeginInit();
            serverListSplit.Panel1.SuspendLayout();
            serverListSplit.Panel2.SuspendLayout();
            serverListSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)serversGridView).BeginInit();
            operationPanel.SuspendLayout();
            monitorGroup.SuspendLayout();
            controlGroup.SuspendLayout();
            manageGroup.SuspendLayout();
            editGroup.SuspendLayout();
            serverEditPanel.SuspendLayout();
            commandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)intervalHoursNum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)portNum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)logSplit).BeginInit();
            logSplit.SuspendLayout();
            SuspendLayout();
            // 
            // aboutBtn
            // 
            aboutBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            aboutBtn.Cursor = Cursors.Hand;
            aboutBtn.FlatAppearance.BorderSize = 0;
            aboutBtn.FlatStyle = FlatStyle.Flat;
            aboutBtn.Font = new Font("微软雅黑", 9F);
            aboutBtn.ForeColor = Color.White;
            aboutBtn.Location = new Point(160, 12);
            aboutBtn.Name = "aboutBtn";
            aboutBtn.Padding = new Padding(4, 2, 4, 2);
            aboutBtn.Size = new Size(70, 34);
            aboutBtn.TabIndex = 0;
            aboutBtn.Text = "帮助";
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.Location = new Point(0, 0);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(logTextBox);
            mainSplitContainer.Panel1.Controls.Add(clearLogBtn);
            mainSplitContainer.Panel1.Controls.Add(serverListSplit);
            mainSplitContainer.Panel1.Controls.Add(serverEditPanel);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(aboutBtn);
            mainSplitContainer.Panel2.Controls.Add(logSplit);
            mainSplitContainer.Size = new Size(1006, 849);
            mainSplitContainer.SplitterDistance = 760;
            mainSplitContainer.TabIndex = 0;
            // 
            // serverListSplit
            // 
            serverListSplit.Dock = DockStyle.Top;
            serverListSplit.Location = new Point(0, 0);
            serverListSplit.Name = "serverListSplit";
            serverListSplit.Orientation = Orientation.Horizontal;
            // 
            // serverListSplit.Panel1
            // 
            serverListSplit.Panel1.Controls.Add(serversGridView);
            // 
            // serverListSplit.Panel2
            // 
            serverListSplit.Panel2.Controls.Add(operationPanel);
            serverListSplit.Size = new Size(760, 431);
            serverListSplit.SplitterDistance = 218;
            serverListSplit.SplitterWidth = 5;
            serverListSplit.TabIndex = 0;
            // 
            // serversGridView
            // 
            serversGridView.AllowUserToResizeRows = false;
            serversGridView.BackgroundColor = Color.White;
            serversGridView.BorderStyle = BorderStyle.Fixed3D;
            serversGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            serversGridView.ColumnHeadersHeight = 29;
            serversGridView.Location = new Point(0, 0);
            serversGridView.Name = "serversGridView";
            serversGridView.RowHeadersVisible = false;
            serversGridView.RowHeadersWidth = 51;
            serversGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            serversGridView.Size = new Size(760, 218);
            serversGridView.TabIndex = 0;
            // 
            // operationPanel
            // 
            operationPanel.Controls.Add(monitorGroup);
            operationPanel.Controls.Add(controlGroup);
            operationPanel.Controls.Add(manageGroup);
            operationPanel.Controls.Add(editGroup);
            operationPanel.Dock = DockStyle.Fill;
            operationPanel.Location = new Point(0, 0);
            operationPanel.Name = "operationPanel";
            operationPanel.Size = new Size(760, 208);
            operationPanel.TabIndex = 0;
            // 
            // monitorGroup
            // 
            monitorGroup.Controls.Add(stopMonitorBtn);
            monitorGroup.Controls.Add(startMonitorBtn);
            monitorGroup.Dock = DockStyle.Left;
            monitorGroup.Location = new Point(579, 0);
            monitorGroup.Name = "monitorGroup";
            monitorGroup.Size = new Size(190, 208);
            monitorGroup.TabIndex = 3;
            monitorGroup.TabStop = false;
            monitorGroup.Text = "监控控制";
            // 
            // stopMonitorBtn
            // 
            stopMonitorBtn.Enabled = false;
            stopMonitorBtn.Location = new Point(13, 91);
            stopMonitorBtn.Name = "stopMonitorBtn";
            stopMonitorBtn.Size = new Size(100, 56);
            stopMonitorBtn.TabIndex = 1;
            stopMonitorBtn.Text = "关闭监控";
            stopMonitorBtn.UseVisualStyleBackColor = true;
            // 
            // startMonitorBtn
            // 
            startMonitorBtn.Enabled = false;
            startMonitorBtn.Location = new Point(13, 27);
            startMonitorBtn.Name = "startMonitorBtn";
            startMonitorBtn.Size = new Size(100, 56);
            startMonitorBtn.TabIndex = 0;
            startMonitorBtn.Text = "开启监控";
            startMonitorBtn.UseVisualStyleBackColor = true;
            // 
            // controlGroup
            // 
            controlGroup.Controls.Add(stopAllBtn);
            controlGroup.Controls.Add(restartBtn);
            controlGroup.Controls.Add(stopBtn);
            controlGroup.Controls.Add(startBtn);
            controlGroup.Dock = DockStyle.Left;
            controlGroup.Location = new Point(346, 0);
            controlGroup.Name = "controlGroup";
            controlGroup.Size = new Size(233, 208);
            controlGroup.TabIndex = 2;
            controlGroup.TabStop = false;
            controlGroup.Text = "运行控制";
            // 
            // stopAllBtn
            // 
            stopAllBtn.Enabled = false;
            stopAllBtn.Location = new Point(120, 91);
            stopAllBtn.Name = "stopAllBtn";
            stopAllBtn.Size = new Size(108, 56);
            stopAllBtn.TabIndex = 3;
            stopAllBtn.Text = "停止全部";
            stopAllBtn.UseVisualStyleBackColor = true;
            // 
            // restartBtn
            // 
            restartBtn.Enabled = false;
            restartBtn.Location = new Point(5, 90);
            restartBtn.Name = "restartBtn";
            restartBtn.Size = new Size(94, 57);
            restartBtn.TabIndex = 2;
            restartBtn.Text = "重启";
            restartBtn.UseVisualStyleBackColor = true;
            // 
            // stopBtn
            // 
            stopBtn.Enabled = false;
            stopBtn.Location = new Point(120, 28);
            stopBtn.Name = "stopBtn";
            stopBtn.Size = new Size(108, 56);
            stopBtn.TabIndex = 1;
            stopBtn.Text = "停止";
            stopBtn.UseVisualStyleBackColor = true;
            // 
            // startBtn
            // 
            startBtn.Enabled = false;
            startBtn.Location = new Point(5, 28);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(94, 56);
            startBtn.TabIndex = 0;
            startBtn.Text = "启动";
            startBtn.UseVisualStyleBackColor = true;
            // 
            // manageGroup
            // 
            manageGroup.Controls.Add(folderManagerBtn);
            manageGroup.Controls.Add(deleteBtn);
            manageGroup.Controls.Add(editBtn);
            manageGroup.Controls.Add(addBtn);
            manageGroup.Dock = DockStyle.Left;
            manageGroup.Location = new Point(153, 0);
            manageGroup.Name = "manageGroup";
            manageGroup.Size = new Size(193, 208);
            manageGroup.TabIndex = 1;
            manageGroup.TabStop = false;
            manageGroup.Text = "服务器管理";
            // 
            // folderManagerBtn
            // 
            folderManagerBtn.Enabled = false;
            folderManagerBtn.Location = new Point(94, 92);
            folderManagerBtn.Name = "folderManagerBtn";
            folderManagerBtn.Size = new Size(88, 56);
            folderManagerBtn.TabIndex = 3;
            folderManagerBtn.Text = "文件夹";
            folderManagerBtn.UseVisualStyleBackColor = true;
            // 
            // deleteBtn
            // 
            deleteBtn.Enabled = false;
            deleteBtn.Location = new Point(5, 91);
            deleteBtn.Name = "deleteBtn";
            deleteBtn.Size = new Size(84, 57);
            deleteBtn.TabIndex = 2;
            deleteBtn.Text = "删除";
            deleteBtn.UseVisualStyleBackColor = true;
            // 
            // editBtn
            // 
            editBtn.Enabled = false;
            editBtn.Location = new Point(94, 28);
            editBtn.Name = "editBtn";
            editBtn.Size = new Size(88, 57);
            editBtn.TabIndex = 1;
            editBtn.Text = "修改";
            editBtn.UseVisualStyleBackColor = true;
            // 
            // addBtn
            // 
            addBtn.Location = new Point(5, 27);
            addBtn.Name = "addBtn";
            addBtn.Size = new Size(84, 57);
            addBtn.TabIndex = 0;
            addBtn.Text = "增加";
            addBtn.UseVisualStyleBackColor = true;
            // 
            // editGroup
            // 
            editGroup.Controls.Add(cancelEditBtn);
            editGroup.Controls.Add(saveEditBtn);
            editGroup.Dock = DockStyle.Left;
            editGroup.Location = new Point(0, 0);
            editGroup.Name = "editGroup";
            editGroup.Size = new Size(153, 208);
            editGroup.TabIndex = 0;
            editGroup.TabStop = false;
            editGroup.Text = "编辑操作";
            // 
            // cancelEditBtn
            // 
            cancelEditBtn.Enabled = false;
            cancelEditBtn.Location = new Point(13, 92);
            cancelEditBtn.Name = "cancelEditBtn";
            cancelEditBtn.Size = new Size(116, 56);
            cancelEditBtn.TabIndex = 1;
            cancelEditBtn.Text = "取消";
            cancelEditBtn.UseVisualStyleBackColor = true;
            // 
            // saveEditBtn
            // 
            saveEditBtn.Enabled = false;
            saveEditBtn.Location = new Point(13, 28);
            saveEditBtn.Name = "saveEditBtn";
            saveEditBtn.Size = new Size(116, 56);
            saveEditBtn.TabIndex = 0;
            saveEditBtn.Text = "保存";
            saveEditBtn.UseVisualStyleBackColor = true;
            // 
            // serverEditPanel
            // 
            serverEditPanel.Controls.Add(commandPanel);
            serverEditPanel.Controls.Add(advancedOptionsLabel);
            serverEditPanel.Controls.Add(enableCommandsChk);
            serverEditPanel.Controls.Add(intervalLabel);
            serverEditPanel.Controls.Add(intervalHoursNum);
            serverEditPanel.Controls.Add(scheduleLabel);
            serverEditPanel.Controls.Add(scheduleTimeInput);
            serverEditPanel.Controls.Add(passwordLabel);
            serverEditPanel.Controls.Add(passwordInput);
            serverEditPanel.Controls.Add(portLabel);
            serverEditPanel.Controls.Add(portNum);
            serverEditPanel.Controls.Add(ipLabel);
            serverEditPanel.Controls.Add(ipInput);
            serverEditPanel.Controls.Add(browseExeBtn);
            serverEditPanel.Controls.Add(exePathLabel);
            serverEditPanel.Controls.Add(exePathInput);
            serverEditPanel.Controls.Add(serverNameLabel);
            serverEditPanel.Controls.Add(serverNameInput);
            serverEditPanel.Location = new Point(3, 488);
            serverEditPanel.Name = "serverEditPanel";
            serverEditPanel.Padding = new Padding(13, 17, 13, 17);
            serverEditPanel.Size = new Size(533, 361);
            serverEditPanel.TabIndex = 1;
            // 
            // commandPanel
            // 
            commandPanel.Controls.Add(sendCommandBtn);
            commandPanel.Controls.Add(commandInput);
            commandPanel.Controls.Add(commandLabel);
            commandPanel.Dock = DockStyle.Bottom;
            commandPanel.Location = new Point(13, 276);
            commandPanel.Name = "commandPanel";
            commandPanel.Size = new Size(507, 68);
            commandPanel.TabIndex = 18;
            // 
            // sendCommandBtn
            // 
            sendCommandBtn.Location = new Point(386, 14);
            sendCommandBtn.Name = "sendCommandBtn";
            sendCommandBtn.Size = new Size(104, 48);
            sendCommandBtn.TabIndex = 2;
            sendCommandBtn.Text = "发送";
            sendCommandBtn.UseVisualStyleBackColor = true;
            // 
            // commandInput
            // 
            commandInput.Location = new Point(66, 14);
            commandInput.Name = "commandInput";
            commandInput.Size = new Size(302, 23);
            commandInput.TabIndex = 1;
            commandInput.Text = "say 服务器即将重启";
            // 
            // commandLabel
            // 
            commandLabel.AutoSize = true;
            commandLabel.Location = new Point(0, 17);
            commandLabel.Name = "commandLabel";
            commandLabel.Size = new Size(68, 17);
            commandLabel.TabIndex = 0;
            commandLabel.Text = "发送指令：";
            // 
            // advancedOptionsLabel
            // 
            advancedOptionsLabel.AutoSize = true;
            advancedOptionsLabel.Location = new Point(0, 249);
            advancedOptionsLabel.Name = "advancedOptionsLabel";
            advancedOptionsLabel.Size = new Size(68, 17);
            advancedOptionsLabel.TabIndex = 17;
            advancedOptionsLabel.Text = "高级选项：";
            // 
            // enableCommandsChk
            // 
            enableCommandsChk.AutoSize = true;
            enableCommandsChk.Location = new Point(82, 248);
            enableCommandsChk.Name = "enableCommandsChk";
            enableCommandsChk.Size = new Size(99, 21);
            enableCommandsChk.TabIndex = 16;
            enableCommandsChk.Text = "启用指令功能";
            enableCommandsChk.UseVisualStyleBackColor = true;
            // 
            // intervalLabel
            // 
            intervalLabel.AutoSize = true;
            intervalLabel.Location = new Point(201, 204);
            intervalLabel.Name = "intervalLabel";
            intervalLabel.Size = new Size(68, 17);
            intervalLabel.TabIndex = 15;
            intervalLabel.Text = "间隔小时：";
            // 
            // intervalHoursNum
            // 
            intervalHoursNum.Location = new Point(276, 201);
            intervalHoursNum.Name = "intervalHoursNum";
            intervalHoursNum.Size = new Size(88, 23);
            intervalHoursNum.TabIndex = 14;
            // 
            // scheduleLabel
            // 
            scheduleLabel.AutoSize = true;
            scheduleLabel.Location = new Point(0, 204);
            scheduleLabel.Name = "scheduleLabel";
            scheduleLabel.Size = new Size(68, 17);
            scheduleLabel.TabIndex = 13;
            scheduleLabel.Text = "定时重启：";
            // 
            // scheduleTimeInput
            // 
            scheduleTimeInput.Location = new Point(82, 201);
            scheduleTimeInput.Name = "scheduleTimeInput";
            scheduleTimeInput.Size = new Size(88, 23);
            scheduleTimeInput.TabIndex = 12;
            scheduleTimeInput.Text = "01:00";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(0, 159);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(44, 17);
            passwordLabel.TabIndex = 11;
            passwordLabel.Text = "密码：";
            // 
            // passwordInput
            // 
            passwordInput.Location = new Point(79, 159);
            passwordInput.Name = "passwordInput";
            passwordInput.Size = new Size(219, 23);
            passwordInput.TabIndex = 10;
            // 
            // portLabel
            // 
            portLabel.AutoSize = true;
            portLabel.Location = new Point(245, 113);
            portLabel.Name = "portLabel";
            portLabel.Size = new Size(44, 17);
            portLabel.TabIndex = 9;
            portLabel.Text = "端口：";
            // 
            // portNum
            // 
            portNum.Location = new Point(293, 110);
            portNum.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            portNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            portNum.Name = "portNum";
            portNum.Size = new Size(88, 23);
            portNum.TabIndex = 8;
            portNum.Value = new decimal(new int[] { 5612, 0, 0, 0 });
            // 
            // ipLabel
            // 
            ipLabel.AutoSize = true;
            ipLabel.Location = new Point(0, 113);
            ipLabel.Name = "ipLabel";
            ipLabel.Size = new Size(31, 17);
            ipLabel.TabIndex = 7;
            ipLabel.Text = "IP：";
            // 
            // ipInput
            // 
            ipInput.Location = new Point(79, 109);
            ipInput.Name = "ipInput";
            ipInput.Size = new Size(132, 23);
            ipInput.TabIndex = 6;
            ipInput.Text = "127.0.0.1";
            // 
            // browseExeBtn
            // 
            browseExeBtn.Location = new Point(399, 51);
            browseExeBtn.Name = "browseExeBtn";
            browseExeBtn.Size = new Size(94, 51);
            browseExeBtn.TabIndex = 5;
            browseExeBtn.Text = "浏览";
            browseExeBtn.UseVisualStyleBackColor = true;
            // 
            // exePathLabel
            // 
            exePathLabel.AutoSize = true;
            exePathLabel.Location = new Point(0, 68);
            exePathLabel.Name = "exePathLabel";
            exePathLabel.Size = new Size(68, 17);
            exePathLabel.TabIndex = 4;
            exePathLabel.Text = "程序路径：";
            // 
            // exePathInput
            // 
            exePathInput.Location = new Point(79, 65);
            exePathInput.Name = "exePathInput";
            exePathInput.Size = new Size(316, 23);
            exePathInput.TabIndex = 3;
            // 
            // serverNameLabel
            // 
            serverNameLabel.AutoSize = true;
            serverNameLabel.Location = new Point(0, 23);
            serverNameLabel.Name = "serverNameLabel";
            serverNameLabel.Size = new Size(68, 17);
            serverNameLabel.TabIndex = 2;
            serverNameLabel.Text = "服务器名：";
            // 
            // serverNameInput
            // 
            serverNameInput.Location = new Point(79, 19);
            serverNameInput.Name = "serverNameInput";
            serverNameInput.Size = new Size(219, 23);
            serverNameInput.TabIndex = 1;
            // 
            // clearLogBtn
            // 
            clearLogBtn.Location = new Point(537, 432);
            clearLogBtn.Name = "clearLogBtn";
            clearLogBtn.Size = new Size(221, 50);
            clearLogBtn.TabIndex = 0;
            clearLogBtn.Text = "清空日志";
            clearLogBtn.UseVisualStyleBackColor = true;
            // 
            // logSplit
            // 
            logSplit.Dock = DockStyle.Bottom;
            logSplit.Location = new Point(0, 421);
            logSplit.Name = "logSplit";
            logSplit.Orientation = Orientation.Horizontal;
            logSplit.Size = new Size(242, 428);
            logSplit.SplitterDistance = 388;
            logSplit.SplitterWidth = 5;
            logSplit.TabIndex = 1;
            // 
            // logTextBox
            // 
            logTextBox.BackColor = Color.FromArgb(245, 245, 245);
            logTextBox.Location = new Point(0, 410);
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.Size = new Size(536, 72);
            logTextBox.TabIndex = 0;
            logTextBox.Text = "";
            // 
            // ToolMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1006, 849);
            Controls.Add(mainSplitContainer);
            Name = "ToolMain";
            Text = "SCNET服务端管理工具";
            mainSplitContainer.Panel1.ResumeLayout(false);
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            serverListSplit.Panel1.ResumeLayout(false);
            serverListSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)serverListSplit).EndInit();
            serverListSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)serversGridView).EndInit();
            operationPanel.ResumeLayout(false);
            monitorGroup.ResumeLayout(false);
            controlGroup.ResumeLayout(false);
            manageGroup.ResumeLayout(false);
            editGroup.ResumeLayout(false);
            serverEditPanel.ResumeLayout(false);
            serverEditPanel.PerformLayout();
            commandPanel.ResumeLayout(false);
            commandPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)intervalHoursNum).EndInit();
            ((System.ComponentModel.ISupportInitialize)portNum).EndInit();
            ((System.ComponentModel.ISupportInitialize)logSplit).EndInit();
            logSplit.ResumeLayout(false);
            ResumeLayout(false);

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
    }
}