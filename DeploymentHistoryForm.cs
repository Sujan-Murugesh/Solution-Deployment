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
    public partial class DeploymentHistoryForm : Form
    {
        private DeploymentHistoryService historyService;
        public DeploymentHistoryForm()
        {
            InitializeComponent();
            historyService = new DeploymentHistoryService();
            LoadHistory();
        }

        private void LoadHistory()
        {
            try
            {
                var history = historyService.GetAllHistory(200);

                dgvHistory.Rows.Clear();

                foreach (var item in history)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvHistory);

                    row.Cells[0].Value = item.DeploymentDate.ToString("yyyy-MM-dd HH:mm");
                    row.Cells[1].Value = item.SolutionFriendlyName;
                    row.Cells[2].Value = item.SourceVersion;
                    row.Cells[3].Value = item.TargetVersion;
                    row.Cells[4].Value = item.SourceEnvironment;
                    row.Cells[5].Value = item.TargetEnvironment;
                    row.Cells[6].Value = item.DeployedAsManaged ? "Managed" : "Unmanaged";
                    row.Cells[7].Value = item.StatusDisplay;
                    row.Cells[8].Value = item.DurationDisplay;
                    row.Cells[9].Value = item.Notes;

                    // Color code by status
                    if (item.Status == Models.DeploymentStatus.Completed)
                    {
                        row.Cells[7].Style.BackColor = Color.LightGreen;
                    }
                    else if (item.Status == Models.DeploymentStatus.Failed)
                    {
                        row.Cells[7].Style.BackColor = Color.LightCoral;
                    }

                    row.Tag = item;
                    dgvHistory.Rows.Add(row);
                }

                lblTotalDeployments.Text = $"Total Deployments: {history.Count}";
                lblSuccessful.Text = $"Successful: {history.Count(h => h.Status == Models.DeploymentStatus.Completed)}";
                lblFailed.Text = $"Failed: {history.Count(h => h.Status == Models.DeploymentStatus.Failed)}";

                // ✅ Enable/disable buttons based on history count
                bool hasHistory = history.Count > 0;
                btnExport.Enabled = hasHistory;
                btnClearHistory.Enabled = hasHistory;

                // ✅ Show message if no history
                if (!hasHistory)
                {
                    var emptyRow = new DataGridViewRow();
                    emptyRow.CreateCells(dgvHistory);
                    emptyRow.Cells[0].Value = "";
                    emptyRow.Cells[1].Value = "No deployment history found";
                    emptyRow.Cells[2].Value = "";
                    emptyRow.Cells[3].Value = "";
                    emptyRow.Cells[4].Value = "";
                    emptyRow.Cells[5].Value = "";
                    emptyRow.Cells[6].Value = "";
                    emptyRow.Cells[7].Value = "";
                    emptyRow.Cells[8].Value = "";
                    emptyRow.Cells[9].Value = "";
                    dgvHistory.Rows.Add(emptyRow);
                    dgvHistory.Rows[0].DefaultCellStyle.Font = new Font(dgvHistory.Font, FontStyle.Italic);
                    dgvHistory.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // ✅ Disable buttons on error
                btnExport.Enabled = false;
                btnClearHistory.Enabled = false;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadHistory();
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            // ✅ Check if there's any history
            if (dgvHistory.Rows.Count == 0 || !btnClearHistory.Enabled)
            {
                MessageBox.Show("No deployment history to clear.",
                    "No History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to clear all deployment history?\n\n" +
                "This action cannot be undone!",
                "Clear History",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    historyService.ClearHistory();
                    LoadHistory();
                    MessageBox.Show("Deployment history cleared successfully.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error clearing history: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // ✅ Check if there's any history
            if (dgvHistory.Rows.Count == 0 || !btnExport.Enabled)
            {
                MessageBox.Show("No deployment history to export.",
                    "No History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                    saveDialog.DefaultExt = "csv";
                    saveDialog.FileName = $"DeploymentHistory_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToCsv(saveDialog.FileName);
                        MessageBox.Show($"History exported successfully to:\n{saveDialog.FileName}",
                            "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting history: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                // Write header
                writer.WriteLine("Date,Solution,Source Version,Target Version,Source Environment,Target Environment,Type,Status,Duration");

                // Write rows
                foreach (DataGridViewRow row in dgvHistory.Rows)
                {
                    if (row.IsNewRow) continue;

                    var line = string.Join(",",
                        row.Cells[0].Value,
                        row.Cells[1].Value,
                        row.Cells[2].Value,
                        row.Cells[3].Value,
                        row.Cells[4].Value,
                        row.Cells[5].Value,
                        row.Cells[6].Value,
                        row.Cells[7].Value?.ToString().Replace("✅", "").Replace("❌", "").Replace("⚠️", "").Trim(),
                        row.Cells[8].Value,
                        row.Cells[9].Value);

                    writer.WriteLine(line);
                }
            }
        }

        private void dgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var history = dgvHistory.Rows[e.RowIndex].Tag as Models.DeploymentHistory;
                if (history != null)
                {
                    ShowHistoryDetails(history);
                }
            }
        }

        private void ShowHistoryDetails(Models.DeploymentHistory history)
        {
            var details = $"Deployment Details\n" +
                         $"==================\n\n" +
                         $"Solution: {history.SolutionFriendlyName} ({history.SolutionUniqueName})\n" +
                         $"Date: {history.DeploymentDate:yyyy-MM-dd HH:mm:ss}\n" +
                         $"Source Version: {history.SourceVersion}\n" +
                         $"Target Version: {history.TargetVersion}\n" +
                         $"Source Environment: {history.SourceEnvironment}\n" +
                         $"Target Environment: {history.TargetEnvironment}\n" +
                         $"Type: {(history.DeployedAsManaged ? "Managed" : "Unmanaged")}\n" +
                         $"Status: {history.StatusDisplay}\n" +
                         $"Duration: {history.DurationDisplay}\n" +
                         $"Deployed By: {history.DeployedBy}\n" +
                         $"Notes: {(history.Notes ?? "").Trim()}\n" +
                         $"Backup Created: {(history.BackupCreated ? "Yes" : "No")}\n";

            if (history.BackupCreated && !string.IsNullOrEmpty(history.BackupPath))
            {
                details += $"Backup Path: {history.BackupPath}\n";
            }

            if (!string.IsNullOrEmpty(history.ErrorMessage))
            {
                details += $"\nError Message:\n{history.ErrorMessage}";
            }

            MessageBox.Show(details, "Deployment Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
