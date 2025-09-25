using System.Drawing;
using System;
using System.Windows.Forms;

namespace Restart
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
       
        private Button defaultButton; // 设置默认程序按钮

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

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        /// 

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.Location = new System.Drawing.Point(50, 47);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(700, 94);
            this.statusLabel.TabIndex = 0;
            // 
            // killButton
            // 
            this.killButton.Location = new System.Drawing.Point(50, 150);
            this.killButton.Name = "killButton";
            this.killButton.Size = new System.Drawing.Size(501, 54);
            this.killButton.TabIndex = 1;
            this.killButton.Text = "手动关闭生存战争服务端";
            this.killButton.Click += new System.EventHandler(this.killButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(53, 241);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(498, 63);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "退出程序";
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(53, 330);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(240, 63);
            this.selectButton.TabIndex = 3;
            this.selectButton.Text = "选择程序";
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // defaultButton
            // 
            this.defaultButton.Location = new System.Drawing.Point(311, 330);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(240, 63);
            this.defaultButton.TabIndex = 4;
            this.defaultButton.Text = "设为默认程序保存配置";
            this.defaultButton.Click += new System.EventHandler(this.defaultButton_Click);
            // 
            // scheduleLabel
            // 
            this.scheduleLabel.Location = new System.Drawing.Point(600, 150);
            this.scheduleLabel.Name = "scheduleLabel";
            this.scheduleLabel.Size = new System.Drawing.Size(250, 30);
            this.scheduleLabel.TabIndex = 5;
            this.scheduleLabel.Text = "每日关闭时间 (HH:mm):";
            // 
            // timeInput
            // 
            this.timeInput.Location = new System.Drawing.Point(600, 180);
            this.timeInput.Name = "timeInput";
            this.timeInput.Size = new System.Drawing.Size(100, 25);
            this.timeInput.TabIndex = 6;
            this.timeInput.Text = "01:00";
            // 
            // saveTimeButton
            // 
            this.saveTimeButton.Location = new System.Drawing.Point(720, 180);
            this.saveTimeButton.Name = "saveTimeButton";
            this.saveTimeButton.Size = new System.Drawing.Size(80, 25);
            this.saveTimeButton.TabIndex = 7;
            this.saveTimeButton.Text = "保存";
            this.saveTimeButton.Click += new System.EventHandler(this.saveTimeButton_Click);
            // 
            // intervalLabel
            // 
            this.intervalLabel.Location = new System.Drawing.Point(600, 220);
            this.intervalLabel.Name = "intervalLabel";
            this.intervalLabel.Size = new System.Drawing.Size(350, 30);
            this.intervalLabel.TabIndex = 8;
            this.intervalLabel.Text = "间隔关闭时间 (小时/分钟, 0=禁用, 如0.6=36分钟):";
            // 
            // intervalInput
            // 
            this.intervalInput.Location = new System.Drawing.Point(600, 250);
            this.intervalInput.Name = "intervalInput";
            this.intervalInput.Size = new System.Drawing.Size(100, 25);
            this.intervalInput.TabIndex = 9;
            this.intervalInput.Text = "0";
            // 
            // saveIntervalButton
            // 
            this.saveIntervalButton.Location = new System.Drawing.Point(720, 250);
            this.saveIntervalButton.Name = "saveIntervalButton";
            this.saveIntervalButton.Size = new System.Drawing.Size(80, 25);
            this.saveIntervalButton.TabIndex = 10;
            this.saveIntervalButton.Text = "保存";
            this.saveIntervalButton.Click += new System.EventHandler(this.saveIntervalButton_Click);
            // 
            // ipInput
            // 
            this.ipInput.Location = new System.Drawing.Point(600, 307);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(150, 25);
            this.ipInput.TabIndex = 11;
            this.ipInput.Text = "127.0.0.1";
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(773, 307);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(100, 25);
            this.portInput.TabIndex = 12;
            this.portInput.Text = "5612";
            // 
            // commandInput
            // 
            this.commandInput.Location = new System.Drawing.Point(600, 368);
            this.commandInput.Name = "commandInput";
            this.commandInput.Size = new System.Drawing.Size(270, 25);
            this.commandInput.TabIndex = 13;
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(897, 368);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(270, 25);
            this.sendButton.TabIndex = 14;
            this.sendButton.Text = "发送";
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // commandStatusLabel
            // 
            this.commandStatusLabel.Location = new System.Drawing.Point(600, 417);
            this.commandStatusLabel.Name = "commandStatusLabel";
            this.commandStatusLabel.Size = new System.Drawing.Size(567, 30);
            this.commandStatusLabel.TabIndex = 15;
            this.commandStatusLabel.Text = "指令状态";
            this.commandStatusLabel.Click += new System.EventHandler(this.commandStatusLabel_Click);
            // 
            // ipLabel
            // 
            this.ipLabel.Location = new System.Drawing.Point(600, 284);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(100, 20);
            this.ipLabel.TabIndex = 16;
            this.ipLabel.Text = "IP 地址:";
            // 
            // portLabel
            // 
            this.portLabel.Location = new System.Drawing.Point(770, 284);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(100, 20);
            this.portLabel.TabIndex = 17;
            this.portLabel.Text = "端口号:";
            this.portLabel.Click += new System.EventHandler(this.portLabel_Click);
            // 
            // commandLabel
            // 
            this.commandLabel.Location = new System.Drawing.Point(600, 335);
            this.commandLabel.Name = "commandLabel";
            this.commandLabel.Size = new System.Drawing.Size(100, 20);
            this.commandLabel.TabIndex = 18;
            this.commandLabel.Text = "指令:";
            // 
            // passwordLabel
            // 
            this.passwordLabel.Location = new System.Drawing.Point(903, 284);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(100, 20);
            this.passwordLabel.TabIndex = 19;
            this.passwordLabel.Text = "密码:";
            // 
            // passwordInput
            // 
            this.passwordInput.Location = new System.Drawing.Point(897, 307);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.Size = new System.Drawing.Size(270, 25);
            this.passwordInput.TabIndex = 20;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1339, 483);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.killButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.defaultButton);
            this.Controls.Add(this.scheduleLabel);
            this.Controls.Add(this.timeInput);
            this.Controls.Add(this.saveTimeButton);
            this.Controls.Add(this.intervalLabel);
            this.Controls.Add(this.intervalInput);
            this.Controls.Add(this.saveIntervalButton);
            this.Controls.Add(this.ipInput);
            this.Controls.Add(this.portInput);
            this.Controls.Add(this.commandInput);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.commandStatusLabel);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.commandLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "生存战争服务端程序自动重启工具v4.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion
    }
}

