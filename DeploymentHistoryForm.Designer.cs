namespace Sujan_Solution_Deployer
{
    partial class DeploymentHistoryForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSourceVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTargetVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSourceEnv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTargetEnv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClearHistory = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblFailed = new System.Windows.Forms.Label();
            this.lblSuccessful = new System.Windows.Forms.Label();
            this.lblTotalDeployments = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dgvHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colSolution,
            this.colSourceVersion,
            this.colTargetVersion,
            this.colSourceEnv,
            this.colTargetEnv,
            this.colType,
            this.colStatus,
            this.colDuration});
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistory.EnableHeadersVisualStyles = false;
            this.dgvHistory.Location = new System.Drawing.Point(0, 80);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersVisible = false;
            this.dgvHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistory.Size = new System.Drawing.Size(1100, 470);
            this.dgvHistory.TabIndex = 0;
            this.dgvHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellDoubleClick);
            // 
            // colDate
            // 
            this.colDate.FillWeight = 12F;
            this.colDate.HeaderText = "📅 Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colSolution
            // 
            this.colSolution.FillWeight = 18F;
            this.colSolution.HeaderText = "📦 Solution";
            this.colSolution.Name = "colSolution";
            this.colSolution.ReadOnly = true;
            // 
            // colSourceVersion
            // 
            this.colSourceVersion.FillWeight = 10F;
            this.colSourceVersion.HeaderText = "🔢 Source Ver";
            this.colSourceVersion.Name = "colSourceVersion";
            this.colSourceVersion.ReadOnly = true;
            // 
            // colTargetVersion
            // 
            this.colTargetVersion.FillWeight = 10F;
            this.colTargetVersion.HeaderText = "🔢 Target Ver";
            this.colTargetVersion.Name = "colTargetVersion";
            this.colTargetVersion.ReadOnly = true;
            // 
            // colSourceEnv
            // 
            this.colSourceEnv.FillWeight = 12F;
            this.colSourceEnv.HeaderText = "📍 Source";
            this.colSourceEnv.Name = "colSourceEnv";
            this.colSourceEnv.ReadOnly = true;
            // 
            // colTargetEnv
            // 
            this.colTargetEnv.FillWeight = 12F;
            this.colTargetEnv.HeaderText = "🎯 Target";
            this.colTargetEnv.Name = "colTargetEnv";
            this.colTargetEnv.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.FillWeight = 10F;
            this.colType.HeaderText = "📋 Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.FillWeight = 10F;
            this.colStatus.HeaderText = "✅ Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colDuration
            // 
            this.colDuration.FillWeight = 8F;
            this.colDuration.HeaderText = "⏱️ Duration";
            this.colDuration.Name = "colDuration";
            this.colDuration.ReadOnly = true;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelTop.Controls.Add(this.btnExport);
            this.panelTop.Controls.Add(this.btnClearHistory);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1100, 80);
            this.panelTop.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(730, 25);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 35);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "📊 Export CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClearHistory
            // 
            this.btnClearHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.btnClearHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearHistory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearHistory.ForeColor = System.Drawing.Color.White;
            this.btnClearHistory.Location = new System.Drawing.Point(970, 25);
            this.btnClearHistory.Name = "btnClearHistory";
            this.btnClearHistory.Size = new System.Drawing.Size(110, 35);
            this.btnClearHistory.TabIndex = 2;
            this.btnClearHistory.Text = "🗑️ Clear All";
            this.btnClearHistory.UseVisualStyleBackColor = false;
            this.btnClearHistory.Click += new System.EventHandler(this.btnClearHistory_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(850, 25);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 35);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "🔄 Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(287, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📜 Deployment History";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Controls.Add(this.lblFailed);
            this.panelBottom.Controls.Add(this.lblSuccessful);
            this.panelBottom.Controls.Add(this.lblTotalDeployments);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 550);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1100, 50);
            this.panelBottom.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(980, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "❌ Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblFailed
            // 
            this.lblFailed.AutoSize = true;
            this.lblFailed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFailed.ForeColor = System.Drawing.Color.Red;
            this.lblFailed.Location = new System.Drawing.Point(380, 17);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(57, 15);
            this.lblFailed.TabIndex = 2;
            this.lblFailed.Text = "Failed: 0";
            // 
            // lblSuccessful
            // 
            this.lblSuccessful.AutoSize = true;
            this.lblSuccessful.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSuccessful.ForeColor = System.Drawing.Color.Green;
            this.lblSuccessful.Location = new System.Drawing.Point(220, 17);
            this.lblSuccessful.Name = "lblSuccessful";
            this.lblSuccessful.Size = new System.Drawing.Size(82, 15);
            this.lblSuccessful.TabIndex = 1;
            this.lblSuccessful.Text = "Successful: 0";
            // 
            // lblTotalDeployments
            // 
            this.lblTotalDeployments.AutoSize = true;
            this.lblTotalDeployments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalDeployments.Location = new System.Drawing.Point(20, 17);
            this.lblTotalDeployments.Name = "lblTotalDeployments";
            this.lblTotalDeployments.Size = new System.Drawing.Size(129, 15);
            this.lblTotalDeployments.TabIndex = 0;
            this.lblTotalDeployments.Text = "Total Deployments: 0";
            // 
            // DeploymentHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 600);
            this.Controls.Add(this.dgvHistory);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimizeBox = false;
            this.Name = "DeploymentHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Deployment History - Sujan Solution Deployer";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClearHistory;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblTotalDeployments;
        private System.Windows.Forms.Label lblSuccessful;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSourceVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTargetVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSourceEnv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTargetEnv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
    }
}
