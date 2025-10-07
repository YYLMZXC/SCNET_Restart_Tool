namespace SCNET_Restart_Tool
{
    partial class FolderManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderManagerForm));
            this.tabControlFolders = new System.Windows.Forms.TabControl();
            this.btnBatchBackupAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabControlFolders
            // 
            this.tabControlFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlFolders.Location = new System.Drawing.Point(0, 30);
            this.tabControlFolders.Name = "tabControlFolders";
            this.tabControlFolders.SelectedIndex = 0;
            this.tabControlFolders.Size = new System.Drawing.Size(900, 570);
            this.tabControlFolders.TabIndex = 0;
            // 
            // btnBatchBackupAll
            // 
            this.btnBatchBackupAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBatchBackupAll.Location = new System.Drawing.Point(0, 0);
            this.btnBatchBackupAll.Name = "btnBatchBackupAll";
            this.btnBatchBackupAll.Size = new System.Drawing.Size(900, 30);
            this.btnBatchBackupAll.TabIndex = 1;
            this.btnBatchBackupAll.Text = "批量备份所有目录";
            this.btnBatchBackupAll.UseVisualStyleBackColor = true;
            this.btnBatchBackupAll.Click += new System.EventHandler(this.btnBatchBackupAll_Click);
            // 
            // FolderManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tabControlFolders);
            this.Controls.Add(this.btnBatchBackupAll);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FolderManagerForm";
            this.Text = "文件夹管理";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControlFolders;
        private System.Windows.Forms.Button btnBatchBackupAll;
        #endregion
    }
}