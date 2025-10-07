using System.Drawing;
using System;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    partial class ToolMain
    {
        private System.ComponentModel.IContainer components = null;

        private Button defaultButton;
        private TextBox ipInput;
        private TextBox portInput;
        private TextBox commandInput;
        private Button sendButton;
        private Label commandStatusLabel;
        private Label ipLabel;
        private Label portLabel;
        private Label commandLabel;
        private TextBox passwordInput;
        private Label passwordLabel;
        private Button monitorButton; // 新增：监控开关按钮

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
            this.statusLabel = new System.Windows.Forms.Label();
            this.killButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.selectButton = new System.Windows.Forms.Button();
            this.defaultButton = new System.Windows.Forms.Button();
            this.scheduleLabel = new System.Windows.Forms.Label();
            this.timeInput = new System.Windows.Forms.TextBox();
            this.saveTimeButton = new System.Windows.Forms.Button();
            this.intervalLabel = new System.Windows.Forms.Label();
            this.intervalInput = new System.Windows.Forms.TextBox();
            this.saveIntervalButton = new System.Windows.Forms.Button();
            this.ipInput = new System.Windows.Forms.TextBox();
            this.portInput = new System.Windows.Forms.TextBox();
            this.commandInput = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.commandStatusLabel = new System.Windows.Forms.Label();
            this.ipLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.commandLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mainTab = new System.Windows.Forms.TabPage();
            this.statusGroup = new System.Windows.Forms.GroupBox();
            this.processGroup = new System.Windows.Forms.GroupBox();
            this.monitorButton = new System.Windows.Forms.Button();
            this.scheduleGroup = new System.Windows.Forms.GroupBox();
            this.settingsGroup = new System.Windows.Forms.GroupBox();
            this.connectionTab = new System.Windows.Forms.TabPage();
            this.headerPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.mainTab.SuspendLayout();
            this.statusGroup.SuspendLayout();
            this.processGroup.SuspendLayout();
            this.scheduleGroup.SuspendLayout();
            this.settingsGroup.SuspendLayout();
            this.connectionTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.White;
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.statusLabel.ForeColor = System.Drawing.Color.Black;
            this.statusLabel.Location = new System.Drawing.Point(18, 26);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(828, 53);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "初始化中...";
            // 
            // killButton
            // 
            this.killButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.killButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.killButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.killButton.ForeColor = System.Drawing.Color.White;
            this.killButton.Location = new System.Drawing.Point(23, 26);
            this.killButton.Name = "killButton";
            this.killButton.Size = new System.Drawing.Size(400, 41);
            this.killButton.TabIndex = 1;
            this.killButton.Text = "手动关闭生存战争服务端";
            this.killButton.UseVisualStyleBackColor = false;
            this.killButton.Click += new System.EventHandler(this.killButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.exitButton.ForeColor = System.Drawing.Color.White;
            this.exitButton.Location = new System.Drawing.Point(446, 26);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(400, 41);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "退出程序";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.selectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.selectButton.ForeColor = System.Drawing.Color.White;
            this.selectButton.Location = new System.Drawing.Point(23, 26);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(400, 50);
            this.selectButton.TabIndex = 3;
            this.selectButton.Text = "选择程序";
            this.selectButton.UseVisualStyleBackColor = false;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // defaultButton
            // 
            this.defaultButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.defaultButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defaultButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.defaultButton.ForeColor = System.Drawing.Color.White;
            this.defaultButton.Location = new System.Drawing.Point(446, 26);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(400, 50);
            this.defaultButton.TabIndex = 4;
            this.defaultButton.Text = "设为默认程序并保存配置";
            this.defaultButton.UseVisualStyleBackColor = false;
            this.defaultButton.Click += new System.EventHandler(this.defaultButton_Click);
            // 
            // scheduleLabel
            // 
            this.scheduleLabel.AutoSize = true;
            this.scheduleLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.scheduleLabel.ForeColor = System.Drawing.Color.Black;
            this.scheduleLabel.Location = new System.Drawing.Point(23, 26);
            this.scheduleLabel.Name = "scheduleLabel";
            this.scheduleLabel.Size = new System.Drawing.Size(173, 20);
            this.scheduleLabel.TabIndex = 5;
            this.scheduleLabel.Text = "每日关闭时间 (HH:mm):";
            // 
            // timeInput
            // 
            this.timeInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timeInput.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.timeInput.Location = new System.Drawing.Point(223, 24);
            this.timeInput.Name = "timeInput";
            this.timeInput.Size = new System.Drawing.Size(114, 27);
            this.timeInput.TabIndex = 6;
            this.timeInput.Text = "01:00";
            // 
            // saveTimeButton
            // 
            this.saveTimeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.saveTimeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveTimeButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.saveTimeButton.ForeColor = System.Drawing.Color.White;
            this.saveTimeButton.Location = new System.Drawing.Point(343, 18);
            this.saveTimeButton.Name = "saveTimeButton";
            this.saveTimeButton.Size = new System.Drawing.Size(80, 37);
            this.saveTimeButton.TabIndex = 7;
            this.saveTimeButton.Text = "保存";
            this.saveTimeButton.UseVisualStyleBackColor = false;
            this.saveTimeButton.Click += new System.EventHandler(this.saveTimeButton_Click);
            // 
            // intervalLabel
            // 
            this.intervalLabel.AutoSize = true;
            this.intervalLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.intervalLabel.ForeColor = System.Drawing.Color.Black;
            this.intervalLabel.Location = new System.Drawing.Point(23, 62);
            this.intervalLabel.Name = "intervalLabel";
            this.intervalLabel.Size = new System.Drawing.Size(345, 20);
            this.intervalLabel.TabIndex = 8;
            this.intervalLabel.Text = "间隔关闭时间 (小时/分钟, 0=禁用, 如0.6=36分钟):";
            // 
            // intervalInput
            // 
            this.intervalInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intervalInput.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.intervalInput.Location = new System.Drawing.Point(398, 60);
            this.intervalInput.Name = "intervalInput";
            this.intervalInput.Size = new System.Drawing.Size(114, 27);
            this.intervalInput.TabIndex = 9;
            this.intervalInput.Text = "0";
            // 
            // saveIntervalButton
            // 
            this.saveIntervalButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.saveIntervalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveIntervalButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.saveIntervalButton.ForeColor = System.Drawing.Color.White;
            this.saveIntervalButton.Location = new System.Drawing.Point(518, 54);
            this.saveIntervalButton.Name = "saveIntervalButton";
            this.saveIntervalButton.Size = new System.Drawing.Size(80, 36);
            this.saveIntervalButton.TabIndex = 10;
            this.saveIntervalButton.Text = "保存";
            this.saveIntervalButton.UseVisualStyleBackColor = false;
            this.saveIntervalButton.Click += new System.EventHandler(this.saveIntervalButton_Click);
            // 
            // ipInput
            // 
            this.ipInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipInput.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ipInput.Location = new System.Drawing.Point(97, 15);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(171, 27);
            this.ipInput.TabIndex = 11;
            this.ipInput.Text = "127.0.0.1";
            // 
            // portInput
            // 
            this.portInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.portInput.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.portInput.Location = new System.Drawing.Point(354, 15);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(114, 27);
            this.portInput.TabIndex = 12;
            this.portInput.Text = "5612";
            // 
            // commandInput
            // 
            this.commandInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandInput.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.commandInput.Location = new System.Drawing.Point(97, 59);
            this.commandInput.Name = "commandInput";
            this.commandInput.Size = new System.Drawing.Size(554, 27);
            this.commandInput.TabIndex = 13;
            // 
            // sendButton
            // 
            this.sendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.sendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.sendButton.ForeColor = System.Drawing.Color.White;
            this.sendButton.Location = new System.Drawing.Point(657, 50);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(200, 44);
            this.sendButton.TabIndex = 14;
            this.sendButton.Text = "发送指令";
            this.sendButton.UseVisualStyleBackColor = false;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // commandStatusLabel
            // 
            this.commandStatusLabel.BackColor = System.Drawing.Color.White;
            this.commandStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandStatusLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.commandStatusLabel.Location = new System.Drawing.Point(23, 106);
            this.commandStatusLabel.Name = "commandStatusLabel";
            this.commandStatusLabel.Size = new System.Drawing.Size(834, 53);
            this.commandStatusLabel.TabIndex = 15;
            this.commandStatusLabel.Text = "指令状态";
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ipLabel.Location = new System.Drawing.Point(23, 18);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(60, 20);
            this.ipLabel.TabIndex = 16;
            this.ipLabel.Text = "IP 地址:";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.portLabel.Location = new System.Drawing.Point(286, 18);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(58, 20);
            this.portLabel.TabIndex = 17;
            this.portLabel.Text = "端口号:";
            // 
            // commandLabel
            // 
            this.commandLabel.AutoSize = true;
            this.commandLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.commandLabel.Location = new System.Drawing.Point(23, 62);
            this.commandLabel.Name = "commandLabel";
            this.commandLabel.Size = new System.Drawing.Size(43, 20);
            this.commandLabel.TabIndex = 18;
            this.commandLabel.Text = "指令:";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.passwordLabel.Location = new System.Drawing.Point(486, 18);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(43, 20);
            this.passwordLabel.TabIndex = 19;
            this.passwordLabel.Text = "密码:";
            // 
            // passwordInput
            // 
            this.passwordInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordInput.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.passwordInput.Location = new System.Drawing.Point(549, 15);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.Size = new System.Drawing.Size(308, 27);
            this.passwordInput.TabIndex = 20;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1077, 53);
            this.headerPanel.TabIndex = 21;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(23, 13);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(384, 31);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "生存战争服务端自动重启工具 v5.0";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.mainTab);
            this.tabControl1.Controls.Add(this.connectionTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tabControl1.Location = new System.Drawing.Point(0, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1077, 580);
            this.tabControl1.TabIndex = 22;
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.statusGroup);
            this.mainTab.Controls.Add(this.processGroup);
            this.mainTab.Controls.Add(this.scheduleGroup);
            this.mainTab.Controls.Add(this.settingsGroup);
            this.mainTab.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.mainTab.Location = new System.Drawing.Point(4, 29);
            this.mainTab.Name = "mainTab";
            this.mainTab.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.mainTab.Size = new System.Drawing.Size(1069, 547);
            this.mainTab.TabIndex = 0;
            this.mainTab.Text = "主控制";
            this.mainTab.UseVisualStyleBackColor = true;
            // 
            // statusGroup
            // 
            this.statusGroup.Controls.Add(this.statusLabel);
            this.statusGroup.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.statusGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.statusGroup.Location = new System.Drawing.Point(25, 366);
            this.statusGroup.Name = "statusGroup";
            this.statusGroup.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.statusGroup.Size = new System.Drawing.Size(869, 112);
            this.statusGroup.TabIndex = 26;
            this.statusGroup.TabStop = false;
            this.statusGroup.Text = "状态信息";
            // 
            // processGroup
            // 
            this.processGroup.Controls.Add(this.killButton);
            this.processGroup.Controls.Add(this.exitButton);
            this.processGroup.Controls.Add(this.monitorButton);
            this.processGroup.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.processGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.processGroup.Location = new System.Drawing.Point(14, 231);
            this.processGroup.Name = "processGroup";
            this.processGroup.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.processGroup.Size = new System.Drawing.Size(869, 129);
            this.processGroup.TabIndex = 25;
            this.processGroup.TabStop = false;
            this.processGroup.Text = "进程控制";
            // 
            // monitorButton
            // 
            this.monitorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.monitorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.monitorButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.monitorButton.ForeColor = System.Drawing.Color.White;
            this.monitorButton.Location = new System.Drawing.Point(23, 73);
            this.monitorButton.Name = "monitorButton";
            this.monitorButton.Size = new System.Drawing.Size(823, 41);
            this.monitorButton.TabIndex = 3;
            this.monitorButton.Text = "开启服务端监控";
            this.monitorButton.UseVisualStyleBackColor = false;
            this.monitorButton.Click += new System.EventHandler(this.MonitorButton_Click);
            // 
            // scheduleGroup
            // 
            this.scheduleGroup.Controls.Add(this.scheduleLabel);
            this.scheduleGroup.Controls.Add(this.timeInput);
            this.scheduleGroup.Controls.Add(this.saveTimeButton);
            this.scheduleGroup.Controls.Add(this.intervalLabel);
            this.scheduleGroup.Controls.Add(this.intervalInput);
            this.scheduleGroup.Controls.Add(this.saveIntervalButton);
            this.scheduleGroup.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.scheduleGroup.Location = new System.Drawing.Point(15, 119);
            this.scheduleGroup.Name = "scheduleGroup";
            this.scheduleGroup.Padding = new System.Windows.Forms.Padding(17, 13, 17, 13);
            this.scheduleGroup.Size = new System.Drawing.Size(869, 106);
            this.scheduleGroup.TabIndex = 24;
            this.scheduleGroup.TabStop = false;
            this.scheduleGroup.Text = "计划任务设置";
            // 
            // settingsGroup
            // 
            this.settingsGroup.Controls.Add(this.selectButton);
            this.settingsGroup.Controls.Add(this.defaultButton);
            this.settingsGroup.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.settingsGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.settingsGroup.Location = new System.Drawing.Point(15, 25);
            this.settingsGroup.Name = "settingsGroup";
            this.settingsGroup.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.settingsGroup.Size = new System.Drawing.Size(869, 88);
            this.settingsGroup.TabIndex = 23;
            this.settingsGroup.TabStop = false;
            this.settingsGroup.Text = "程序设置";
            // 
            // connectionTab
            // 
            this.connectionTab.Controls.Add(this.commandStatusLabel);
            this.connectionTab.Controls.Add(this.sendButton);
            this.connectionTab.Controls.Add(this.commandInput);
            this.connectionTab.Controls.Add(this.commandLabel);
            this.connectionTab.Controls.Add(this.passwordInput);
            this.connectionTab.Controls.Add(this.passwordLabel);
            this.connectionTab.Controls.Add(this.portInput);
            this.connectionTab.Controls.Add(this.portLabel);
            this.connectionTab.Controls.Add(this.ipInput);
            this.connectionTab.Controls.Add(this.ipLabel);
            this.connectionTab.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.connectionTab.Location = new System.Drawing.Point(4, 29);
            this.connectionTab.Name = "connectionTab";
            this.connectionTab.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.connectionTab.Size = new System.Drawing.Size(1029, 433);
            this.connectionTab.TabIndex = 1;
            this.connectionTab.Text = "远程控制";
            this.connectionTab.UseVisualStyleBackColor = true;
            // 
            // ToolMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1077, 633);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ToolMain";
            this.Text = "生存战争服务端自动重启工具 v5.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.mainTab.ResumeLayout(false);
            this.statusGroup.ResumeLayout(false);
            this.processGroup.ResumeLayout(false);
            this.scheduleGroup.ResumeLayout(false);
            this.scheduleGroup.PerformLayout();
            this.settingsGroup.ResumeLayout(false);
            this.connectionTab.ResumeLayout(false);
            this.connectionTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label statusLabel;
        private Button killButton;
        private Button exitButton;
        private Button selectButton;
        private Label scheduleLabel;
        private TextBox timeInput;
        private Button saveTimeButton;
        private Label intervalLabel;
        private TextBox intervalInput;
        private Button saveIntervalButton;
        private Panel headerPanel;
        private Label titleLabel;
        private TabControl tabControl1;
        private TabPage mainTab;
        private TabPage connectionTab;
        private GroupBox settingsGroup;
        private GroupBox scheduleGroup;
        private GroupBox processGroup;
        private GroupBox statusGroup;
    }
}