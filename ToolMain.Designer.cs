namespace SCNET_Restart_Tool
{
    partial class ToolMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvServers = new System.Windows.Forms.DataGridView();
            this.panelServerControls = new System.Windows.Forms.Panel();
            this.btnOpenFolderManager = new System.Windows.Forms.Button();
            this.btnStopAllServers = new System.Windows.Forms.Button();
            this.btnRestartServer = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnDeleteServer = new System.Windows.Forms.Button();
            this.btnEditServer = new System.Windows.Forms.Button();
            this.btnAddServer = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.panelLogControls = new System.Windows.Forms.Panel();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).BeginInit();
            this.panelServerControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panelLogControls.SuspendLayout();
            this.SuspendLayout();

            // splitContainer1
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;

            // splitContainer1.Panel1
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);

            // splitContainer1.Panel2
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 761);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 0;

            // splitContainer2
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Vertical;

            // splitContainer2.Panel1
            this.splitContainer2.Panel1.Controls.Add(this.dgvServers);
            this.splitContainer2.Panel1.Controls.Add(this.panelServerControls);

            // splitContainer2.Panel2
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Size = new System.Drawing.Size(1084, 400);
            this.splitContainer2.SplitterDistance = 850;
            this.splitContainer2.TabIndex = 0;

            // dgvServers
            this.dgvServers.AllowUserToResizeRows = false;
            this.dgvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServers.Location = new System.Drawing.Point(0, 100);
            this.dgvServers.Name = "dgvServers";
            this.dgvServers.RowHeadersVisible = false;
            this.dgvServers.Size = new System.Drawing.Size(850, 300);
            this.dgvServers.TabIndex = 1;
            this.dgvServers.SelectionChanged += new System.EventHandler(this.dgvServers_SelectionChanged);

            // panelServerControls
            this.panelServerControls.Controls.Add(this.btnOpenFolderManager);
            this.panelServerControls.Controls.Add(this.btnStopAllServers);
            this.panelServerControls.Controls.Add(this.btnRestartServer);
            this.panelServerControls.Controls.Add(this.btnStopServer);
            this.panelServerControls.Controls.Add(this.btnStartServer);
            this.panelServerControls.Controls.Add(this.btnDeleteServer);
            this.panelServerControls.Controls.Add(this.btnEditServer);
            this.panelServerControls.Controls.Add(this.btnAddServer);
            this.panelServerControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelServerControls.Height = 100;
            this.panelServerControls.Location = new System.Drawing.Point(0, 0);
            this.panelServerControls.Name = "panelServerControls";
            this.panelServerControls.Size = new System.Drawing.Size(850, 100);
            this.panelServerControls.TabIndex = 0;

            // btnOpenFolderManager
            this.btnOpenFolderManager.Location = new System.Drawing.Point(650, 20);
            this.btnOpenFolderManager.Name = "btnOpenFolderManager";
            this.btnOpenFolderManager.Size = new System.Drawing.Size(133, 29);
            this.btnOpenFolderManager.TabIndex = 7;
            this.btnOpenFolderManager.Text = "文件夹管理";
            this.btnOpenFolderManager.UseVisualStyleBackColor = true;
            this.btnOpenFolderManager.Click += new System.EventHandler(this.btnOpenFolderManager_Click);

            // btnStopAllServers
            this.btnStopAllServers.Location = new System.Drawing.Point(650, 60);
            this.btnStopAllServers.Name = "btnStopAllServers";
            this.btnStopAllServers.Size = new System.Drawing.Size(133, 29);
            this.btnStopAllServers.TabIndex = 6;
            this.btnStopAllServers.Text = "停止所有服务端";
            this.btnStopAllServers.UseVisualStyleBackColor = true;
            this.btnStopAllServers.Click += new System.EventHandler(this.btnStopAllServers_Click);

            // btnRestartServer
            this.btnRestartServer.Location = new System.Drawing.Point(180, 60);
            this.btnRestartServer.Name = "btnRestartServer";
            this.btnRestartServer.Size = new System.Drawing.Size(133, 29);
            this.btnRestartServer.TabIndex = 5;
            this.btnRestartServer.Text = "重启服务端";
            this.btnRestartServer.UseVisualStyleBackColor = true;
            this.btnRestartServer.Click += new System.EventHandler(this.btnRestartServer_Click);

            // btnStopServer
            this.btnStopServer.Location = new System.Drawing.Point(180, 20);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(133, 29);
            this.btnStopServer.TabIndex = 4;
            this.btnStopServer.Text = "停止服务端";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);

            // btnStartServer
            this.btnStartServer.Location = new System.Drawing.Point(40, 20);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(133, 29);
            this.btnStartServer.TabIndex = 3;
            this.btnStartServer.Text = "启动服务端";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);

            // btnDeleteServer
            this.btnDeleteServer.Location = new System.Drawing.Point(370, 60);
            this.btnDeleteServer.Name = "btnDeleteServer";
            this.btnDeleteServer.Size = new System.Drawing.Size(133, 29);
            this.btnDeleteServer.TabIndex = 2;
            this.btnDeleteServer.Text = "删除服务端";
            this.btnDeleteServer.UseVisualStyleBackColor = true;
            this.btnDeleteServer.Click += new System.EventHandler(this.btnDeleteServer_Click);

            // btnEditServer
            this.btnEditServer.Location = new System.Drawing.Point(370, 20);
            this.btnEditServer.Name = "btnEditServer";
            this.btnEditServer.Size = new System.Drawing.Size(133, 29);
            this.btnEditServer.TabIndex = 1;
            this.btnEditServer.Text = "编辑服务端";
            this.btnEditServer.UseVisualStyleBackColor = true;
            this.btnEditServer.Click += new System.EventHandler(this.btnEditServer_Click);

            // btnAddServer
            this.btnAddServer.Location = new System.Drawing.Point(40, 60);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(133, 29);
            this.btnAddServer.TabIndex = 0;
            this.btnAddServer.Text = "添加服务端";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler(this.btnAddServer_Click);

            // splitContainer3
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Vertical;

            // splitContainer3.Panel1
            this.splitContainer3.Panel1.Controls.Add(this.rtbLog);
            this.splitContainer3.Panel1.Controls.Add(this.panelLogControls);

            // splitContainer3.Panel2
            this.splitContainer3.Panel2.Size = new System.Drawing.Size(230, 357);
            this.splitContainer3.Panel2.TabIndex = 1;

            this.splitContainer3.Size = new System.Drawing.Size(1084, 357);
            this.splitContainer3.SplitterDistance = 850;
            this.splitContainer3.TabIndex = 0;

            // rtbLog
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 40);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(850, 317);
            this.rtbLog.TabIndex = 1;

            // panelLogControls
            this.panelLogControls.Controls.Add(this.btnClearLog);
            this.panelLogControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogControls.Height = 40;
            this.panelLogControls.Location = new System.Drawing.Point(0, 0);
            this.panelLogControls.Name = "panelLogControls";
            this.panelLogControls.Size = new System.Drawing.Size(850, 40);
            this.panelLogControls.TabIndex = 0;

            // btnClearLog
            this.btnClearLog.Location = new System.Drawing.Point(750, 5);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(80, 30);
            this.btnClearLog.TabIndex = 0;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);

            // label1
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 400);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务端管理工具\r\n\r\n功能说明：\r\n1. 管理多个服务端的启动/停止\r\n2. 支持服务端配置编辑\r\n3. 文件夹管理（日志、皮肤等）\r\n4. 操作日志记录";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            // ToolMain
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 761);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ToolMain";
            this.Text = "SCNET服务端管理工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolMain_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).EndInit();
            this.panelServerControls.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panelLogControls.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvServers;
        private System.Windows.Forms.Panel panelServerControls;
        private System.Windows.Forms.Button btnAddServer;
        private System.Windows.Forms.Button btnEditServer;
        private System.Windows.Forms.Button btnDeleteServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnRestartServer;
        private System.Windows.Forms.Button btnStopAllServers;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Panel panelLogControls;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenFolderManager;
    }
}