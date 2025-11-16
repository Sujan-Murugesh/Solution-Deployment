namespace Sujan_Solution_Deployer
{
    partial class VersionIncrementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionIncrementForm));
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvSolutions = new System.Windows.Forms.DataGridView();
            this.colSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAutoIncrement = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.grpAutoIncrement = new System.Windows.Forms.GroupBox();
            this.btnApplyToAll = new System.Windows.Forms.Button();
            this.cmbIncrementType = new System.Windows.Forms.ComboBox();
            this.lblIncrementType = new System.Windows.Forms.Label();
            this.grpManualEntry = new System.Windows.Forms.GroupBox();
            this.lblManualInstruction = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkUpdateInSource = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSolutions)).BeginInit();
            this.grpAutoIncrement.SuspendLayout();
            this.grpManualEntry.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(272, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🔢 Solution Version Manager";
            // 
            // dgvSolutions
            // 
            this.dgvSolutions.AllowUserToAddRows = false;
            this.dgvSolutions.AllowUserToDeleteRows = false;
            this.dgvSolutions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSolutions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSolutions.BackgroundColor = System.Drawing.Color.White;
            this.dgvSolutions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSolutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSolutions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSolution,
            this.colCurrentVersion,
            this.colNewVersion,
            this.colAutoIncrement});
            this.dgvSolutions.Location = new System.Drawing.Point(25, 60);
            this.dgvSolutions.Name = "dgvSolutions";
            this.dgvSolutions.RowHeadersVisible = false;
            this.dgvSolutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSolutions.Size = new System.Drawing.Size(750, 250);
            this.dgvSolutions.TabIndex = 1;
            this.dgvSolutions.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSolutions_CellValueChanged);
            this.dgvSolutions.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSolutions_DataError);
            // 
            // colSolution
            // 
            this.colSolution.FillWeight = 40F;
            this.colSolution.HeaderText = "Solution Name";
            this.colSolution.Name = "colSolution";
            this.colSolution.ReadOnly = true;
            // 
            // colCurrentVersion
            // 
            this.colCurrentVersion.FillWeight = 20F;
            this.colCurrentVersion.HeaderText = "Current Version";
            this.colCurrentVersion.Name = "colCurrentVersion";
            this.colCurrentVersion.ReadOnly = true;
            // 
            // colNewVersion
            // 
            this.colNewVersion.FillWeight = 20F;
            this.colNewVersion.HeaderText = "New Version";
            this.colNewVersion.Name = "colNewVersion";
            // 
            // colAutoIncrement
            // 
            this.colAutoIncrement.FillWeight = 20F;
            this.colAutoIncrement.HeaderText = "Auto Increment";
            this.colAutoIncrement.Items.AddRange(new object[] {
            "None",
            "Major (x.0.0.0)",
            "Minor (0.x.0.0)",
            "Build (0.0.x.0)",
            "Revision (0.0.0.x)"});
            this.colAutoIncrement.Name = "colAutoIncrement";
            this.colAutoIncrement.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAutoIncrement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // grpAutoIncrement
            // 
            this.grpAutoIncrement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAutoIncrement.Controls.Add(this.btnApplyToAll);
            this.grpAutoIncrement.Controls.Add(this.cmbIncrementType);
            this.grpAutoIncrement.Controls.Add(this.lblIncrementType);
            this.grpAutoIncrement.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAutoIncrement.Location = new System.Drawing.Point(25, 320);
            this.grpAutoIncrement.Name = "grpAutoIncrement";
            this.grpAutoIncrement.Size = new System.Drawing.Size(750, 80);
            this.grpAutoIncrement.TabIndex = 2;
            this.grpAutoIncrement.TabStop = false;
            this.grpAutoIncrement.Text = "⚡ Quick Auto Increment";
            // 
            // btnApplyToAll
            // 
            this.btnApplyToAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnApplyToAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyToAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnApplyToAll.ForeColor = System.Drawing.Color.White;
            this.btnApplyToAll.Location = new System.Drawing.Point(400, 25);
            this.btnApplyToAll.Name = "btnApplyToAll";
            this.btnApplyToAll.Size = new System.Drawing.Size(120, 28);
            this.btnApplyToAll.TabIndex = 2;
            this.btnApplyToAll.Text = "Apply to All";
            this.btnApplyToAll.UseVisualStyleBackColor = false;
            this.btnApplyToAll.Click += new System.EventHandler(this.btnApplyToAll_Click);
            // 
            // cmbIncrementType
            // 
            this.cmbIncrementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIncrementType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbIncrementType.FormattingEnabled = true;
            this.cmbIncrementType.Items.AddRange(new object[] {
            "None",
            "Major (x.0.0.0)",
            "Minor (0.x.0.0)",
            "Build (0.0.x.0)",
            "Revision (0.0.0.x)"});
            this.cmbIncrementType.Location = new System.Drawing.Point(180, 27);
            this.cmbIncrementType.Name = "cmbIncrementType";
            this.cmbIncrementType.Size = new System.Drawing.Size(200, 23);
            this.cmbIncrementType.TabIndex = 1;
            // 
            // lblIncrementType
            // 
            this.lblIncrementType.AutoSize = true;
            this.lblIncrementType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblIncrementType.Location = new System.Drawing.Point(20, 30);
            this.lblIncrementType.Name = "lblIncrementType";
            this.lblIncrementType.Size = new System.Drawing.Size(127, 15);
            this.lblIncrementType.TabIndex = 0;
            this.lblIncrementType.Text = "Apply increment to all:";
            // 
            // grpManualEntry
            // 
            this.grpManualEntry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpManualEntry.Controls.Add(this.lblManualInstruction);
            this.grpManualEntry.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpManualEntry.Location = new System.Drawing.Point(25, 410);
            this.grpManualEntry.Name = "grpManualEntry";
            this.grpManualEntry.Size = new System.Drawing.Size(750, 60);
            this.grpManualEntry.TabIndex = 3;
            this.grpManualEntry.TabStop = false;
            this.grpManualEntry.Text = "✏️ Manual Entry";
            // 
            // lblManualInstruction
            // 
            this.lblManualInstruction.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblManualInstruction.Location = new System.Drawing.Point(20, 25);
            this.lblManualInstruction.Name = "lblManualInstruction";
            this.lblManualInstruction.Size = new System.Drawing.Size(460, 15);
            this.lblManualInstruction.TabIndex = 0;
            this.lblManualInstruction.Text = "💡 Tip: Double-click on \'New Version\' column cells to manually enter version numb" +
    "ers";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(164)))), ((int)(((byte)(0)))));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(555, 520);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 35);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "✅ OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(675, 520);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "❌ Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // chkUpdateInSource
            // 
            this.chkUpdateInSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkUpdateInSource.AutoSize = true;
            this.chkUpdateInSource.Checked = true;
            this.chkUpdateInSource.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateInSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chkUpdateInSource.Location = new System.Drawing.Point(25, 485);
            this.chkUpdateInSource.Name = "chkUpdateInSource";
            this.chkUpdateInSource.Size = new System.Drawing.Size(367, 19);
            this.chkUpdateInSource.TabIndex = 4;
            this.chkUpdateInSource.Text = "✅ Update version in source environment before deployment";
            this.chkUpdateInSource.UseVisualStyleBackColor = true;
            // 
            // VersionIncrementForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 570);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkUpdateInSource);
            this.Controls.Add(this.grpManualEntry);
            this.Controls.Add(this.grpAutoIncrement);
            this.Controls.Add(this.dgvSolutions);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionIncrementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Solution Version Manager - Sujan Solution Deployer";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSolutions)).EndInit();
            this.grpAutoIncrement.ResumeLayout(false);
            this.grpAutoIncrement.PerformLayout();
            this.grpManualEntry.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvSolutions;
        private System.Windows.Forms.GroupBox grpAutoIncrement;
        private System.Windows.Forms.Label lblIncrementType;
        private System.Windows.Forms.ComboBox cmbIncrementType;
        private System.Windows.Forms.Button btnApplyToAll;
        private System.Windows.Forms.GroupBox grpManualEntry;
        private System.Windows.Forms.Label lblManualInstruction;
        private System.Windows.Forms.CheckBox chkUpdateInSource;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewVersion;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAutoIncrement;
    }
}
