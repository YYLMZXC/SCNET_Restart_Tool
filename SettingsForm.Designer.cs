namespace SCNET_Restart_Tool
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtLogPath = new System.Windows.Forms.TextBox();
            this.btnSelectLogPath = new System.Windows.Forms.Button();
            this.chkAutoClearLogs = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numAutoClearDays = new System.Windows.Forms.NumericUpDown();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoClearDays)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "日志保存路径：";
            // 
            // txtLogPath
            // 
            this.txtLogPath.Location = new System.Drawing.Point(167, 21);
            this.txtLogPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.ReadOnly = true;
            this.txtLogPath.Size = new System.Drawing.Size(332, 25);
            this.txtLogPath.TabIndex = 9;
            // 
            // btnSelectLogPath
            // 
            this.btnSelectLogPath.Location = new System.Drawing.Point(507, 19);
            this.btnSelectLogPath.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectLogPath.Name = "btnSelectLogPath";
            this.btnSelectLogPath.Size = new System.Drawing.Size(100, 29);
            this.btnSelectLogPath.TabIndex = 8;
            this.btnSelectLogPath.Text = "选择目录";
            this.btnSelectLogPath.UseVisualStyleBackColor = true;
            this.btnSelectLogPath.Click += new System.EventHandler(this.btnSelectLogPath_Click);
            // 
            // chkAutoClearLogs
            // 
            this.chkAutoClearLogs.AutoSize = true;
            this.chkAutoClearLogs.Location = new System.Drawing.Point(31, 69);
            this.chkAutoClearLogs.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoClearLogs.Name = "chkAutoClearLogs";
            this.chkAutoClearLogs.Size = new System.Drawing.Size(149, 19);
            this.chkAutoClearLogs.TabIndex = 7;
            this.chkAutoClearLogs.Text = "自动清理过期日志";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 71);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "清理超过（天）的日志：";
            // 
            // numAutoClearDays
            // 
            this.numAutoClearDays.Location = new System.Drawing.Point(400, 69);
            this.numAutoClearDays.Margin = new System.Windows.Forms.Padding(4);
            this.numAutoClearDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numAutoClearDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAutoClearDays.Name = "numAutoClearDays";
            this.numAutoClearDays.Size = new System.Drawing.Size(100, 25);
            this.numAutoClearDays.TabIndex = 5;
            this.numAutoClearDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(333, 162);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(100, 29);
            this.btnSaveSettings.TabIndex = 1;
            this.btnSaveSettings.Text = "保存";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(507, 162);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 29);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 212);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.numAutoClearDays);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkAutoClearLogs);
            this.Controls.Add(this.btnSelectLogPath);
            this.Controls.Add(this.txtLogPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "程序设置";
            ((System.ComponentModel.ISupportInitialize)(this.numAutoClearDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLogPath;
        private System.Windows.Forms.Button btnSelectLogPath;
        private System.Windows.Forms.CheckBox chkAutoClearLogs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numAutoClearDays;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancel;

        #endregion
    }
}