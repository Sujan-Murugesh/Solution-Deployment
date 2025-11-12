using Sujan_Solution_Deployer.Models;
using Sujan_Solution_Deployer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sujan_Solution_Deployer
{
    public partial class RollbackForm : Form
    {
        private DeploymentHistoryService historyService;
        private List<DeploymentHistory> deploymentHistory;
        private DeploymentHistory selectedDeployment;
        public RollbackForm()
        {
            InitializeComponent();
            historyService = new DeploymentHistoryService();
            LoadRecentDeployments();
        }

        private void LoadRecentDeployments()
        {
            try
            {
                // Get last 50 successful deployments with backups
                var allHistory = historyService.GetAllHistory(100);
                deploymentHistory = allHistory
                    .Where(h => h.Status == DeploymentStatus.Completed && h.BackupCreated && !string.IsNullOrEmpty(h.BackupPath))
                    .OrderByDescending(h => h.DeploymentDate)
                    .Take(50)
                    .ToList();

                dgvDeployments.Rows.Clear();

                if (deploymentHistory.Count == 0)
                {
                    var emptyRow = dgvDeployments.Rows.Add();
                    dgvDeployments.Rows[emptyRow].Cells[0].Value = "No rollback-eligible deployments found";
                    dgvDeployments.Rows[emptyRow].DefaultCellStyle.Font = new Font(dgvDeployments.Font, FontStyle.Italic);
                    dgvDeployments.Rows[emptyRow].DefaultCellStyle.ForeColor = Color.Gray;
                    btnRollback.Enabled = false;
                    return;
                }

                foreach (var deployment in deploymentHistory)
                {
                    var rowIndex = dgvDeployments.Rows.Add();
                    var row = dgvDeployments.Rows[rowIndex];

                    row.Cells[0].Value = deployment.DeploymentDate.ToString("yyyy-MM-dd HH:mm");
                    row.Cells[1].Value = deployment.SolutionFriendlyName;
                    row.Cells[2].Value = deployment.TargetVersion;
                    row.Cells[3].Value = deployment.TargetEnvironment;
                    row.Cells[4].Value = deployment.DeployedAsManaged ? "Managed" : "Unmanaged";
                    row.Cells[5].Value = Path.GetFileName(deployment.BackupPath);
                    row.Cells[6].Value = File.Exists(deployment.BackupPath) ? "✅ Available" : "❌ Missing";

                    row.Tag = deployment;

                    // Highlight if backup file is missing
                    if (!File.Exists(deployment.BackupPath))
                    {
                        row.Cells[6].Style.BackColor = Color.LightCoral;
                    }
                }

                lblTotalDeployments.Text = $"Rollback-eligible deployments: {deploymentHistory.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading deployments: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDeployments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDeployments.SelectedRows.Count > 0)
            {
                selectedDeployment = dgvDeployments.SelectedRows[0].Tag as DeploymentHistory;

                if (selectedDeployment != null)
                {
                    UpdateDeploymentDetails();
                    btnRollback.Enabled = File.Exists(selectedDeployment.BackupPath);
                }
            }
            else
            {
                selectedDeployment = null;
                btnRollback.Enabled = false;
            }
        }

        private void UpdateDeploymentDetails()
        {
            if (selectedDeployment == null) return;

            var details = $"Deployment Details\n";
            details += $"==================\n\n";
            details += $"Solution: {selectedDeployment.SolutionFriendlyName}\n";
            details += $"Unique Name: {selectedDeployment.SolutionUniqueName}\n";
            details += $"Version: {selectedDeployment.TargetVersion}\n";
            details += $"Target Environment: {selectedDeployment.TargetEnvironment}\n";
            details += $"Deployment Date: {selectedDeployment.DeploymentDate:yyyy-MM-dd HH:mm:ss}\n";
            details += $"Type: {(selectedDeployment.DeployedAsManaged ? "Managed" : "Unmanaged")}\n";
            details += $"Deployed By: {selectedDeployment.DeployedBy}\n\n";
            details += $"Backup Information\n";
            details += $"==================\n";
            details += $"Backup File: {Path.GetFileName(selectedDeployment.BackupPath)}\n";
            details += $"Backup Location: {selectedDeployment.BackupPath}\n";
            details += $"File Exists: {(File.Exists(selectedDeployment.BackupPath) ? "Yes ✅" : "No ❌")}\n";

            if (File.Exists(selectedDeployment.BackupPath))
            {
                var fileInfo = new FileInfo(selectedDeployment.BackupPath);
                details += $"File Size: {FormatFileSize(fileInfo.Length)}\n";
                details += $"File Created: {fileInfo.CreationTime:yyyy-MM-dd HH:mm:ss}\n";
            }

            txtDetails.Text = details;
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {
            if (selectedDeployment == null)
            {
                MessageBox.Show("Please select a deployment to rollback.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(selectedDeployment.BackupPath))
            {
                MessageBox.Show("Backup file not found. Cannot perform rollback.",
                    "Backup Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirmMessage = $"⚠️ ROLLBACK CONFIRMATION\n\n" +
                               $"You are about to rollback the following deployment:\n\n" +
                               $"Solution: {selectedDeployment.SolutionFriendlyName}\n" +
                               $"Version: {selectedDeployment.TargetVersion}\n" +
                               $"Environment: {selectedDeployment.TargetEnvironment}\n" +
                               $"Deployment Date: {selectedDeployment.DeploymentDate:yyyy-MM-dd HH:mm}\n\n" +
                               $"This will:\n" +
                               $"1. Import the backup solution from:\n" +
                               $"   {Path.GetFileName(selectedDeployment.BackupPath)}\n" +
                               $"2. Restore the previous version of the solution\n" +
                               $"3. This action cannot be undone!\n\n" +
                               $"⚠️ WARNING: Rollback may cause data loss if customizations\n" +
                               $"were made after the original deployment.\n\n" +
                               $"Do you want to proceed with the rollback?";

            if (MessageBox.Show(confirmMessage, "Confirm Rollback",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Tag = selectedDeployment; // Pass selected deployment back to caller
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRecentDeployments();
        }

        private void btnOpenBackupFolder_Click(object sender, EventArgs e)
        {
            if (selectedDeployment == null || string.IsNullOrEmpty(selectedDeployment.BackupPath))
            {
                MessageBox.Show("No deployment selected or backup path not available.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var folderPath = Path.GetDirectoryName(selectedDeployment.BackupPath);
                if (Directory.Exists(folderPath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", folderPath);
                }
                else
                {
                    MessageBox.Show("Backup folder not found.",
                        "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening backup folder: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
