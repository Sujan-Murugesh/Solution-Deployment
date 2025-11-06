using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sujan_Solution_Deployer
{
    public partial class DeploymentLogForm : Form
    {
        private bool isDeploymentInProgress = false;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeploymentLogForm));
        public DeploymentLogForm()
        {
            InitializeComponent();
        }

        public void AppendLog(string message, Color? color = null)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => AppendLog(message, color)));
                return;
            }

            txtLog.SelectionColor = color ?? Color.Lime;
            txtLog.AppendText($"{message}\n");
            txtLog.SelectionColor = Color.Lime;
            txtLog.ScrollToCaret();

            // Update status
            UpdateStatus($"Last update: {DateTime.Now:HH:mm:ss}");
        }

        public void UpdateStatus(string status)
        {
            if (lblStatus.GetCurrentParent().InvokeRequired)
            {
                lblStatus.GetCurrentParent().Invoke(new Action(() => UpdateStatus(status)));
                return;
            }

            lblStatus.Text = status;
        }

        public void SetDeploymentInProgress(bool inProgress)
        {
            isDeploymentInProgress = inProgress;

            if (inProgress)
            {
                UpdateStatus("⏳ Deployment in progress...");
            }
            else
            {
                UpdateStatus("✅ Deployment completed");
            }
        }
        
        public void ClearLog()
        {
            txtLog.Clear();
            txtLog.SelectionColor = Color.Lime;
            txtLog.AppendText(resources.GetString("txtLog.Text"));
            UpdateStatus("Log cleared");
        }

        private void tsbDownloadLog_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Text Files (*.txt)|*.txt|Log Files (*.log)|*.log|All Files (*.*)|*.*";
                    saveDialog.DefaultExt = "txt";
                    saveDialog.FileName = $"DeploymentLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                    saveDialog.Title = "Save Deployment Log";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveDialog.FileName, txtLog.Text);
                        MessageBox.Show($"Log saved successfully to:\n{saveDialog.FileName}",
                            "Log Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UpdateStatus($"Log saved: {Path.GetFileName(saveDialog.FileName)}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving log:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbCopyLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLog.Text))
                {
                    Clipboard.SetText(txtLog.Text);
                    MessageBox.Show("Log copied to clipboard successfully!",
                        "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("Log copied to clipboard");
                }
                else
                {
                    MessageBox.Show("Log is empty. Nothing to copy.",
                        "Empty Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying log:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbClearLog_Click(object sender, EventArgs e)
        {
            if (isDeploymentInProgress)
            {
                MessageBox.Show("Cannot clear log while deployment is in progress.",
                    "Deployment In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Better confirmation message
            var result = MessageBox.Show(
                "Are you sure you want to clear the deployment log?\n\n" +
                "This will permanently delete all log entries from this session.",
                "Clear Log",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearLog();
                MessageBox.Show("Deployment log has been cleared.",
                    "Log Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void DeploymentLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            // Don't allow closing during deployment, just hide
            //if (isDeploymentInProgress)
            //{
            //    e.Cancel = true;
            //    this.Hide();
            //}
            //else
            //{
            //    // Allow closing when deployment is not in progress
            //    e.Cancel = false;
            //}
        }
    }
}