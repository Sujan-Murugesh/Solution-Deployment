using System;

namespace Sujan_Solution_Deployer
{
    partial class SolutionDeployerControl
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

        #region Code généré par le Concepteur de composants

        private void InitializeComponent()
        {
            this.grpSourceEnvironment = new System.Windows.Forms.GroupBox();
            this.btnLoadSolutions = new System.Windows.Forms.Button();
            this.lblManaged = new System.Windows.Forms.Label();
            this.lblUnmanaged = new System.Windows.Forms.Label();
            this.chkManagedSolutions = new System.Windows.Forms.CheckedListBox();
            this.chkUnmanagedSolutions = new System.Windows.Forms.CheckedListBox();
            this.grpTargetEnvironment = new System.Windows.Forms.GroupBox();
            this.btnRemoveTarget = new System.Windows.Forms.Button();
            this.btnAddTarget = new System.Windows.Forms.Button();
            this.chkTargetEnvironments = new System.Windows.Forms.CheckedListBox();
            this.grpBackup = new System.Windows.Forms.GroupBox();
            this.btnBrowseBackup = new System.Windows.Forms.Button();
            this.txtBackupPath = new System.Windows.Forms.TextBox();
            this.chkEnableBackup = new System.Windows.Forms.CheckBox();
            this.chkDeployAsManaged = new System.Windows.Forms.CheckBox();
            this.grpDeploymentOptions = new System.Windows.Forms.GroupBox();
            this.chkOverwriteCustomizations = new System.Windows.Forms.CheckBox();
            this.chkPublishWorkflows = new System.Windows.Forms.CheckBox();
            this.rbUpgrade = new System.Windows.Forms.RadioButton();
            this.rbUpdate = new System.Windows.Forms.RadioButton();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDeploymentLogs = new System.Windows.Forms.ToolStripButton();
            this.grpSourceEnvironment.SuspendLayout();
            this.grpTargetEnvironment.SuspendLayout();
            this.grpBackup.SuspendLayout();
            this.grpDeploymentOptions.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSourceEnvironment
            // 
            this.grpSourceEnvironment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSourceEnvironment.Controls.Add(this.btnLoadSolutions);
            this.grpSourceEnvironment.Controls.Add(this.lblManaged);
            this.grpSourceEnvironment.Controls.Add(this.lblUnmanaged);
            this.grpSourceEnvironment.Controls.Add(this.chkManagedSolutions);
            this.grpSourceEnvironment.Controls.Add(this.chkUnmanagedSolutions);
            this.grpSourceEnvironment.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSourceEnvironment.Location = new System.Drawing.Point(15, 40);
            this.grpSourceEnvironment.Name = "grpSourceEnvironment";
            this.grpSourceEnvironment.Size = new System.Drawing.Size(620, 300);
            this.grpSourceEnvironment.TabIndex = 0;
            this.grpSourceEnvironment.TabStop = false;
            this.grpSourceEnvironment.Text = "📦 Source Environment (Connected)";
            // 
            // btnLoadSolutions
            // 
            this.btnLoadSolutions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnLoadSolutions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadSolutions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoadSolutions.ForeColor = System.Drawing.Color.White;
            this.btnLoadSolutions.Location = new System.Drawing.Point(15, 25);
            this.btnLoadSolutions.Name = "btnLoadSolutions";
            this.btnLoadSolutions.Size = new System.Drawing.Size(150, 35);
            this.btnLoadSolutions.TabIndex = 0;
            this.btnLoadSolutions.Text = "🔄 Load Solutions";
            this.btnLoadSolutions.UseVisualStyleBackColor = false;
            this.btnLoadSolutions.Click += new System.EventHandler(this.btnLoadSolutions_Click);
            // 
            // lblManaged
            // 
            this.lblManaged.AutoSize = true;
            this.lblManaged.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblManaged.Location = new System.Drawing.Point(315, 70);
            this.lblManaged.Name = "lblManaged";
            this.lblManaged.Size = new System.Drawing.Size(128, 15);
            this.lblManaged.TabIndex = 3;
            this.lblManaged.Text = "🔒 Managed Solutions";
            // 
            // lblUnmanaged
            // 
            this.lblUnmanaged.AutoSize = true;
            this.lblUnmanaged.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUnmanaged.Location = new System.Drawing.Point(15, 70);
            this.lblUnmanaged.Name = "lblUnmanaged";
            this.lblUnmanaged.Size = new System.Drawing.Size(144, 15);
            this.lblUnmanaged.TabIndex = 1;
            this.lblUnmanaged.Text = "📄 Unmanaged Solutions";
            // 
            // chkManagedSolutions
            // 
            this.chkManagedSolutions.CheckOnClick = true;
            this.chkManagedSolutions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkManagedSolutions.FormattingEnabled = true;
            this.chkManagedSolutions.Location = new System.Drawing.Point(315, 90);
            this.chkManagedSolutions.Name = "chkManagedSolutions";
            this.chkManagedSolutions.Size = new System.Drawing.Size(280, 184);
            this.chkManagedSolutions.TabIndex = 4;
            // 
            // chkUnmanagedSolutions
            // 
            this.chkUnmanagedSolutions.CheckOnClick = true;
            this.chkUnmanagedSolutions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkUnmanagedSolutions.FormattingEnabled = true;
            this.chkUnmanagedSolutions.Location = new System.Drawing.Point(15, 90);
            this.chkUnmanagedSolutions.Name = "chkUnmanagedSolutions";
            this.chkUnmanagedSolutions.Size = new System.Drawing.Size(280, 184);
            this.chkUnmanagedSolutions.TabIndex = 2;
            // 
            // grpTargetEnvironment
            // 
            this.grpTargetEnvironment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTargetEnvironment.Controls.Add(this.btnRemoveTarget);
            this.grpTargetEnvironment.Controls.Add(this.btnAddTarget);
            this.grpTargetEnvironment.Controls.Add(this.chkTargetEnvironments);
            this.grpTargetEnvironment.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTargetEnvironment.Location = new System.Drawing.Point(650, 40);
            this.grpTargetEnvironment.Name = "grpTargetEnvironment";
            this.grpTargetEnvironment.Size = new System.Drawing.Size(350, 180);
            this.grpTargetEnvironment.TabIndex = 1;
            this.grpTargetEnvironment.TabStop = false;
            this.grpTargetEnvironment.Text = "🎯 Target Environments";
            // 
            // btnRemoveTarget
            // 
            this.btnRemoveTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.btnRemoveTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveTarget.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRemoveTarget.ForeColor = System.Drawing.Color.White;
            this.btnRemoveTarget.Location = new System.Drawing.Point(180, 25);
            this.btnRemoveTarget.Name = "btnRemoveTarget";
            this.btnRemoveTarget.Size = new System.Drawing.Size(150, 35);
            this.btnRemoveTarget.TabIndex = 1;
            this.btnRemoveTarget.Text = "➖ Remove Selected";
            this.btnRemoveTarget.UseVisualStyleBackColor = false;
            this.btnRemoveTarget.Click += new System.EventHandler(this.btnRemoveTarget_Click);
            // 
            // btnAddTarget
            // 
            this.btnAddTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnAddTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTarget.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddTarget.ForeColor = System.Drawing.Color.White;
            this.btnAddTarget.Location = new System.Drawing.Point(15, 25);
            this.btnAddTarget.Name = "btnAddTarget";
            this.btnAddTarget.Size = new System.Drawing.Size(150, 35);
            this.btnAddTarget.TabIndex = 0;
            this.btnAddTarget.Text = "➕ Add Environment";
            this.btnAddTarget.UseVisualStyleBackColor = false;
            this.btnAddTarget.Click += new System.EventHandler(this.btnAddTarget_Click);
            // 
            // chkTargetEnvironments
            // 
            this.chkTargetEnvironments.CheckOnClick = true;
            this.chkTargetEnvironments.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkTargetEnvironments.FormattingEnabled = true;
            this.chkTargetEnvironments.Location = new System.Drawing.Point(15, 70);
            this.chkTargetEnvironments.Name = "chkTargetEnvironments";
            this.chkTargetEnvironments.Size = new System.Drawing.Size(315, 94);
            this.chkTargetEnvironments.TabIndex = 2;
            // 
            // grpBackup
            // 
            this.grpBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBackup.Controls.Add(this.btnBrowseBackup);
            this.grpBackup.Controls.Add(this.txtBackupPath);
            this.grpBackup.Controls.Add(this.chkEnableBackup);
            this.grpBackup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpBackup.Location = new System.Drawing.Point(650, 230);
            this.grpBackup.Name = "grpBackup";
            this.grpBackup.Size = new System.Drawing.Size(350, 110);
            this.grpBackup.TabIndex = 2;
            this.grpBackup.TabStop = false;
            this.grpBackup.Text = "💾 Backup Options";
            // 
            // btnBrowseBackup
            // 
            this.btnBrowseBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBrowseBackup.Location = new System.Drawing.Point(265, 58);
            this.btnBrowseBackup.Name = "btnBrowseBackup";
            this.btnBrowseBackup.Size = new System.Drawing.Size(65, 27);
            this.btnBrowseBackup.TabIndex = 2;
            this.btnBrowseBackup.Text = "Browse...";
            this.btnBrowseBackup.UseVisualStyleBackColor = true;
            this.btnBrowseBackup.Click += new System.EventHandler(this.btnBrowseBackup_Click);
            // 
            // txtBackupPath
            // 
            this.txtBackupPath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBackupPath.Location = new System.Drawing.Point(15, 60);
            this.txtBackupPath.Name = "txtBackupPath";
            this.txtBackupPath.Size = new System.Drawing.Size(240, 23);
            this.txtBackupPath.TabIndex = 1;
            // 
            // chkEnableBackup
            // 
            this.chkEnableBackup.AutoSize = true;
            this.chkEnableBackup.Checked = true;
            this.chkEnableBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkEnableBackup.Location = new System.Drawing.Point(15, 30);
            this.chkEnableBackup.Name = "chkEnableBackup";
            this.chkEnableBackup.Size = new System.Drawing.Size(177, 19);
            this.chkEnableBackup.TabIndex = 0;
            this.chkEnableBackup.Text = "✅ Enable Automatic Backup";
            this.chkEnableBackup.UseVisualStyleBackColor = true;
            this.chkEnableBackup.CheckedChanged += new System.EventHandler(this.chkEnableBackup_CheckedChanged);
            // 
            // chkDeployAsManaged
            // 
            this.chkDeployAsManaged.AutoSize = true;
            this.chkDeployAsManaged.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkDeployAsManaged.Location = new System.Drawing.Point(650, 30);
            this.chkDeployAsManaged.Name = "chkDeployAsManaged";
            this.chkDeployAsManaged.Size = new System.Drawing.Size(260, 19);
            this.chkDeployAsManaged.TabIndex = 4;
            this.chkDeployAsManaged.Text = "🔒 Deploy Unmanaged as Managed Solution";
            this.chkDeployAsManaged.UseVisualStyleBackColor = true;
            this.chkDeployAsManaged.CheckedChanged += new System.EventHandler(this.chkDeployAsManaged_CheckedChanged);
            // 
            // grpDeploymentOptions
            // 
            this.grpDeploymentOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDeploymentOptions.Controls.Add(this.chkOverwriteCustomizations);
            this.grpDeploymentOptions.Controls.Add(this.chkPublishWorkflows);
            this.grpDeploymentOptions.Controls.Add(this.chkDeployAsManaged);
            this.grpDeploymentOptions.Controls.Add(this.rbUpgrade);
            this.grpDeploymentOptions.Controls.Add(this.rbUpdate);
            this.grpDeploymentOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpDeploymentOptions.Location = new System.Drawing.Point(15, 350);
            this.grpDeploymentOptions.Name = "grpDeploymentOptions";
            this.grpDeploymentOptions.Size = new System.Drawing.Size(985, 90);
            this.grpDeploymentOptions.TabIndex = 3;
            this.grpDeploymentOptions.TabStop = false;
            this.grpDeploymentOptions.Text = "⚙️ Deployment Options";
            // 
            // chkOverwriteCustomizations
            // 
            this.chkOverwriteCustomizations.AutoSize = true;
            this.chkOverwriteCustomizations.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkOverwriteCustomizations.Location = new System.Drawing.Point(400, 55);
            this.chkOverwriteCustomizations.Name = "chkOverwriteCustomizations";
            this.chkOverwriteCustomizations.Size = new System.Drawing.Size(209, 19);
            this.chkOverwriteCustomizations.TabIndex = 3;
            this.chkOverwriteCustomizations.Text = "⚠️ Overwrite Unmanaged Changes";
            this.chkOverwriteCustomizations.UseVisualStyleBackColor = true;
            // 
            // chkPublishWorkflows
            // 
            this.chkPublishWorkflows.AutoSize = true;
            this.chkPublishWorkflows.Checked = true;
            this.chkPublishWorkflows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPublishWorkflows.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkPublishWorkflows.Location = new System.Drawing.Point(400, 30);
            this.chkPublishWorkflows.Name = "chkPublishWorkflows";
            this.chkPublishWorkflows.Size = new System.Drawing.Size(195, 19);
            this.chkPublishWorkflows.TabIndex = 2;
            this.chkPublishWorkflows.Text = "✅ Publish Workflows/Processes";
            this.chkPublishWorkflows.UseVisualStyleBackColor = true;
            // 
            // rbUpgrade
            // 
            this.rbUpgrade.AutoSize = true;
            this.rbUpgrade.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rbUpgrade.Location = new System.Drawing.Point(15, 55);
            this.rbUpgrade.Name = "rbUpgrade";
            this.rbUpgrade.Size = new System.Drawing.Size(298, 19);
            this.rbUpgrade.TabIndex = 1;
            this.rbUpgrade.Text = "⬆️ Upgrade (Stage for upgrade, convert to managed)";
            this.rbUpgrade.UseVisualStyleBackColor = true;
            // 
            // rbUpdate
            // 
            this.rbUpdate.AutoSize = true;
            this.rbUpdate.Checked = true;
            this.rbUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rbUpdate.Location = new System.Drawing.Point(15, 30);
            this.rbUpdate.Name = "rbUpdate";
            this.rbUpdate.Size = new System.Drawing.Size(260, 19);
            this.rbUpdate.TabIndex = 0;
            this.rbUpdate.TabStop = true;
            this.rbUpdate.Text = "🔄 Update (Upgrade existing or install if new)";
            this.rbUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDeploy
            // 
            this.btnDeploy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeploy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(164)))), ((int)(((byte)(0)))));
            this.btnDeploy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeploy.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeploy.ForeColor = System.Drawing.Color.White;
            this.btnDeploy.Location = new System.Drawing.Point(15, 450);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(985, 61);
            this.btnDeploy.TabIndex = 4;
            this.btnDeploy.Text = "🚀 START DEPLOYMENT";
            this.btnDeploy.UseVisualStyleBackColor = false;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(15, 551);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(985, 30);
            this.progressBar.TabIndex = 6;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProgress.Location = new System.Drawing.Point(15, 529);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(166, 15);
            this.lblProgress.TabIndex = 5;
            this.lblProgress.Text = "📊 Deployment Progress: 0%";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.toolStripSeparator1,
            this.tsbDeploymentLogs,
            this.toolStripSeparator2,
            this.tsbHistory,
            });
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1020, 25);
            this.toolStrip.TabIndex = 8;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::Sujan_Solution_Deployer.Properties.Resources.close_16px;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(56, 22);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            //
            // tsbHistory
            // 
            this.tsbHistory.Image = null;
            this.tsbHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHistory.Name = "tsbHistory";
            this.tsbHistory.Size = new System.Drawing.Size(85, 22);
            this.tsbHistory.Text = "📜 History";
            this.tsbHistory.Click += new System.EventHandler(this.tsbHistory_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDeploymentLogs
            // 
            this.tsbDeploymentLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeploymentLogs.Name = "tsbDeploymentLogs";
            this.tsbDeploymentLogs.Size = new System.Drawing.Size(119, 22);
            this.tsbDeploymentLogs.Text = "📋 Deployment Logs";
            this.tsbDeploymentLogs.Click += new System.EventHandler(this.tsbDeploymentLogs_Click);
            // 
            // SolutionDeployerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnDeploy);
            this.Controls.Add(this.grpDeploymentOptions);
            this.Controls.Add(this.grpBackup);
            this.Controls.Add(this.grpTargetEnvironment);
            this.Controls.Add(this.grpSourceEnvironment);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "SolutionDeployerControl";
            this.Size = new System.Drawing.Size(1020, 605);
            this.Load += new System.EventHandler(this.SolutionDeployerControl_Load);
            this.grpSourceEnvironment.ResumeLayout(false);
            this.grpSourceEnvironment.PerformLayout();
            this.grpTargetEnvironment.ResumeLayout(false);
            this.grpBackup.ResumeLayout(false);
            this.grpBackup.PerformLayout();
            this.grpDeploymentOptions.ResumeLayout(false);
            this.grpDeploymentOptions.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpSourceEnvironment;
        private System.Windows.Forms.Button btnLoadSolutions;
        private System.Windows.Forms.Label lblUnmanaged;
        private System.Windows.Forms.Label lblManaged;
        private System.Windows.Forms.CheckedListBox chkUnmanagedSolutions;
        private System.Windows.Forms.CheckedListBox chkManagedSolutions;
        private System.Windows.Forms.GroupBox grpTargetEnvironment;
        private System.Windows.Forms.Button btnAddTarget;
        private System.Windows.Forms.Button btnRemoveTarget;
        private System.Windows.Forms.CheckedListBox chkTargetEnvironments;
        private System.Windows.Forms.GroupBox grpBackup;
        private System.Windows.Forms.CheckBox chkEnableBackup;
        private System.Windows.Forms.TextBox txtBackupPath;
        private System.Windows.Forms.Button btnBrowseBackup;
        private System.Windows.Forms.GroupBox grpDeploymentOptions;
        private System.Windows.Forms.RadioButton rbUpdate;
        private System.Windows.Forms.RadioButton rbUpgrade;
        private System.Windows.Forms.CheckBox chkPublishWorkflows;
        private System.Windows.Forms.CheckBox chkOverwriteCustomizations;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbDeploymentLogs;
        private System.Windows.Forms.CheckBox chkDeployAsManaged;
        private System.Windows.Forms.ToolStripButton tsbHistory;
    }
}
