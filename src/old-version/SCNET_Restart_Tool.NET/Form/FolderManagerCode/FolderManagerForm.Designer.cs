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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderManagerForm));
            this.tabControlFolders = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControlFolders
            // 
            this.tabControlFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlFolders.Location = new System.Drawing.Point(0, 0);
            this.tabControlFolders.Name = "tabControlFolders";
            this.tabControlFolders.SelectedIndex = 0;
            this.tabControlFolders.Size = new System.Drawing.Size(900, 600);
            this.tabControlFolders.TabIndex = 0;
            // 
            // FolderManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tabControlFolders);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FolderManagerForm";
            this.Text = "文件夹管理";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControlFolders;
        #endregion
    }
}