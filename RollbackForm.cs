using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectionManager = Sujan_Solution_Deployer.Services.ConnectionManager;
using Label = System.Windows.Forms.Label;

namespace Sujan_Solution_Deployer
{
    public partial class RollbackForm : Form
    {
        private DeploymentHistoryService historyService;
        private ConnectionService connectionService;
        private List<DeploymentHistory> deploymentHistory;
        private DeploymentHistory selectedDeployment;
        private ConnectionDetail targetConnection;
        private IOrganizationService organizationService;
        private List<ConnectionDetail> availableConnections;
        private Dictionary<Guid, IOrganizationService> activeServices;

        private class ConnectionListItem
        {
            public ConnectionDetail Connection { get; set; }
            public string DisplayText { get; set; }
        }

        public RollbackForm()
        {
            InitializeComponent();
            historyService = new DeploymentHistoryService();
            connectionService = new ConnectionService();
            activeServices = new Dictionary<Guid, IOrganizationService>();
            LoadRecentDeployments();
        }

        // NEW: Constructor that accepts both connections and their active services
        public RollbackForm(List<ConnectionDetail> connections, Dictionary<Guid, IOrganizationService> services) : this()
        {
            availableConnections = connections;
            activeServices = services ?? new Dictionary<Guid, IOrganizationService>();

            System.Diagnostics.Debug.WriteLine($"[RollbackForm] Initialized with {connections?.Count ?? 0} connections and {activeServices.Count} active services");
        }

        public RollbackForm(List<ConnectionDetail> connections) : this()
        {
            availableConnections = connections;

            // Store active services from connections
            if (connections != null)
            {
                foreach (var conn in connections)
                {
                    // Check if ConnectionId has a value
                    if (conn.ConnectionId.HasValue && conn.ServiceClient != null && conn.ServiceClient.IsReady)
                    {
                        IOrganizationService service = null;

                        if (conn.ServiceClient.OrganizationWebProxyClient != null)
                        {
                            service = conn.ServiceClient.OrganizationWebProxyClient;
                        }
                        else if (conn.ServiceClient.OrganizationServiceProxy != null)
                        {
                            service = conn.ServiceClient.OrganizationServiceProxy;
                        }

                        if (service != null)
                        {
                            activeServices[conn.ConnectionId.Value] = service;  // Use .Value to get Guid
                            System.Diagnostics.Debug.WriteLine($"[RollbackForm] Stored active service for: {conn.ConnectionName}");
                        }
                    }
                }
            }
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

        private async void btnRollback_Click(object sender, EventArgs e)
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

            // Let user select/confirm the target connection
            if (!SelectTargetConnection())
            {
                return;
            }

            var confirmMessage = $"⚠️ ROLLBACK CONFIRMATION\n\n" +
                               $"You are about to rollback the following deployment:\n\n" +
                               $"Solution: {selectedDeployment.SolutionFriendlyName}\n" +
                               $"Version: {selectedDeployment.TargetVersion}\n" +
                               $"Target Environment: {targetConnection.ConnectionName}\n" +
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
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            // Disable UI during rollback
            SetUIEnabled(false);

            try
            {
                await PerformRollbackAsync();

                MessageBox.Show("Rollback completed successfully!\n\n" +
                              $"Solution '{selectedDeployment.SolutionFriendlyName}' has been restored.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Log the rollback operation
                LogRollbackOperation();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                var errorDetails = GetDetailedErrorMessage(ex);
                MessageBox.Show($"Rollback failed:\n\n{errorDetails}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Re-enable UI
                SetUIEnabled(true);

                // Log the failure
                LogRollbackFailure(ex);
            }
        }

        private bool SelectTargetConnection()
        {
            var connectionsToUse = availableConnections;

            if (connectionsToUse == null || connectionsToUse.Count == 0)
            {
                connectionsToUse = connectionService.GetAllConnections();
            }

            if (connectionsToUse == null || connectionsToUse.Count == 0)
            {
                MessageBox.Show(
                    "No connections available.\n\n" +
                    "Please add target environments in the main form first.",
                    "No Connections",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            // Try to find matching connection by environment name
            targetConnection = connectionsToUse.FirstOrDefault(c =>
                c.ConnectionName.Equals(selectedDeployment.TargetEnvironment, StringComparison.OrdinalIgnoreCase));

            // If we found an exact match and it has an active service, use it
            if (targetConnection != null &&
                targetConnection.ConnectionId.HasValue &&
                activeServices.ContainsKey(targetConnection.ConnectionId.Value))
            {
                System.Diagnostics.Debug.WriteLine($"[RollbackForm] Using exact match with active service: {targetConnection.ConnectionName}");
                return true;
            }

            // Show connection selection dialog
            using (var selectForm = new Form())
            {
                selectForm.Text = "Select Target Environment";
                selectForm.Size = new Size(650, 460);
                selectForm.StartPosition = FormStartPosition.CenterParent;
                selectForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                selectForm.MaximizeBox = false;
                selectForm.MinimizeBox = false;

                var lblInfo = new Label
                {
                    Text = $"Original environment: {selectedDeployment.TargetEnvironment}\n\n" +
                           $"Select the connection to use for rollback:\n" +
                           $"✅ = Active connection available",
                    Location = new Point(20, 20),
                    Size = new Size(600, 70),
                    Font = new Font("Segoe UI", 9.5F)
                };

                var listBox = new ListBox
                {
                    Location = new Point(20, 100),
                    Size = new Size(600, 270),
                    Font = new Font("Segoe UI", 9F)
                };

                // Add connections to listbox with active status indicator
                foreach (var conn in connectionsToUse)
                {
                    var hasActiveService = conn.ConnectionId.HasValue &&
                                          activeServices.ContainsKey(conn.ConnectionId.Value);
                    var statusIcon = hasActiveService ? "✅" : "⚠️";
                    var displayText = $"{statusIcon} {conn.ConnectionName} - {conn.OrganizationFriendlyName}";

                    listBox.Items.Add(new ConnectionListItem
                    {
                        Connection = conn,
                        DisplayText = displayText
                    });
                }

                listBox.DisplayMember = "DisplayText";

                // Pre-select first connection with active service, or partial match
                int selectedIndex = -1;
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    var item = (ConnectionListItem)listBox.Items[i];

                    // Prioritize connections with active services
                    if (item.Connection.ConnectionId.HasValue &&
                        activeServices.ContainsKey(item.Connection.ConnectionId.Value))
                    {
                        if (selectedIndex == -1)
                        {
                            selectedIndex = i;
                        }

                        // Perfect match with active service - select and stop
                        if (item.Connection.ConnectionName.IndexOf(selectedDeployment.TargetEnvironment,
                            StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }
                }

                if (selectedIndex >= 0)
                {
                    listBox.SelectedIndex = selectedIndex;
                }

                var btnOk = new Button
                {
                    Text = "Select",
                    DialogResult = DialogResult.OK,
                    Location = new Point(450, 375),
                    Size = new Size(80, 35),
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(540, 375),
                    Size = new Size(80, 35),
                    BackColor = Color.FromArgb(100, 100, 100),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                selectForm.Controls.AddRange(new Control[] { lblInfo, listBox, btnOk, btnCancel });
                selectForm.AcceptButton = btnOk;
                selectForm.CancelButton = btnCancel;

                btnOk.Enabled = listBox.SelectedItem != null;
                listBox.SelectedIndexChanged += (s, e) =>
                {
                    btnOk.Enabled = listBox.SelectedItem != null;

                    // Show warning if selected connection doesn't have active service
                    if (listBox.SelectedItem != null)
                    {
                        var selectedItem = (ConnectionListItem)listBox.SelectedItem;
                        var hasActiveService = selectedItem.Connection.ConnectionId.HasValue &&
                                              activeServices.ContainsKey(selectedItem.Connection.ConnectionId.Value);

                        if (!hasActiveService)
                        {
                            lblInfo.Text = $"⚠️ WARNING: This connection may not work for rollback.\n" +
                                          $"Please add it as a target environment in the main form first.";
                            lblInfo.ForeColor = Color.Orange;
                        }
                        else
                        {
                            lblInfo.Text = $"Original environment: {selectedDeployment.TargetEnvironment}\n\n" +
                                          $"Select the connection to use for rollback:\n" +
                                          $"✅ = Active connection available";
                            lblInfo.ForeColor = Color.Black;
                        }
                    }
                };

                if (selectForm.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
                {
                    var selectedItem = (ConnectionListItem)listBox.SelectedItem;
                    targetConnection = selectedItem.Connection;

                    // Warn if no active service
                    var hasActiveService = targetConnection.ConnectionId.HasValue &&
                                          activeServices.ContainsKey(targetConnection.ConnectionId.Value);

                    if (!hasActiveService)
                    {
                        var result = MessageBox.Show(
                            "⚠️ This connection doesn't have an active service.\n\n" +
                            "Rollback may fail if this is an SDK Login Control connection.\n\n" +
                            "Do you want to try anyway?",
                            "Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result != DialogResult.Yes)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return targetConnection != null;
        }

        #region ==> Rollback Logic 
        private async Task PerformRollbackAsync()
        {
            UpdateStatus("Connecting to environment...");

            // Get CRM service with fresh connection
            organizationService = await GetFreshCrmServiceAsync();

            UpdateStatus("Verifying connection...");
            await VerifyConnectionAsync();

            UpdateStatus("Reading backup file...");
            byte[] solutionBytes = await Task.Run(() => File.ReadAllBytes(selectedDeployment.BackupPath));

            UpdateStatus($"Importing solution (this may take several minutes)...\nFile size: {FormatFileSize(solutionBytes.Length)}");

            // Import the solution
            var importJobId = Guid.NewGuid();
            var importRequest = new ImportSolutionRequest
            {
                CustomizationFile = solutionBytes,
                OverwriteUnmanagedCustomizations = true,
                PublishWorkflows = true,
                ImportJobId = importJobId
            };

            // Execute import
            try
            {
                await Task.Run(() => organizationService.Execute(importRequest));
                UpdateStatus("✅ Import request submitted successfully");
            }
            catch (Exception ex)
            {
                UpdateStatus($"⚠️ Warning during import submission: {ex.Message}");
                UpdateStatus("ℹ️ Import may still be processing. Checking status...");
            }

            // Wait a moment for import to register
            await Task.Delay(3000);

            UpdateStatus("Monitoring import progress...");

            // Monitor the import job with fresh connections
            await MonitorImportJobWithReconnection(importJobId);

            UpdateStatus("✅ Rollback completed successfully!");
        }

        private async Task<IOrganizationService> GetFreshCrmServiceAsync()
        {
            return await Task.Run(() =>
            {
                if (targetConnection == null)
                {
                    throw new Exception("Target connection not selected.");
                }

                try
                {
                    IOrganizationService service = null;

                    // Try to use stored active service first
                    if (targetConnection.ConnectionId.HasValue &&
                        activeServices.ContainsKey(targetConnection.ConnectionId.Value))
                    {
                        service = activeServices[targetConnection.ConnectionId.Value];
                        System.Diagnostics.Debug.WriteLine($"[RollbackForm] Using stored service for: {targetConnection.ConnectionName}");
                    }
                    // Try existing ServiceClient
                    else if (targetConnection.ServiceClient != null && targetConnection.ServiceClient.IsReady)
                    {
                        if (targetConnection.ServiceClient.OrganizationWebProxyClient != null)
                        {
                            service = targetConnection.ServiceClient.OrganizationWebProxyClient;
                        }
                        else if (targetConnection.ServiceClient.OrganizationServiceProxy != null)
                        {
                            service = targetConnection.ServiceClient.OrganizationServiceProxy;
                        }

                        System.Diagnostics.Debug.WriteLine($"[RollbackForm] Using ServiceClient for: {targetConnection.ConnectionName}");
                    }

                    if (service == null)
                    {
                        throw new Exception("No active service available for rollback");
                    }

                    // Set appropriate timeout
                    SetServiceTimeout(service, TimeSpan.FromMinutes(20));

                    return service;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Connection failed: {ex.Message}", ex);
                }
            });
        }

        private async Task MonitorImportJobWithReconnection(Guid importJobId)
        {
            await Task.Run(() =>
            {
                var maxWaitTime = TimeSpan.FromMinutes(30);
                var startTime = DateTime.Now;
                var checkInterval = TimeSpan.FromSeconds(5);
                double lastProgress = 0;

                while (DateTime.Now - startTime < maxWaitTime)
                {
                    try
                    {
                        // Create fresh query each time
                        var query = new Microsoft.Xrm.Sdk.Query.QueryExpression("importjob")
                        {
                            ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("progress", "completedon", "data"),
                            Criteria = new Microsoft.Xrm.Sdk.Query.FilterExpression
                            {
                                Conditions =
                        {
                            new Microsoft.Xrm.Sdk.Query.ConditionExpression("importjobid",
                                Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, importJobId)
                        }
                            }
                        };

                        var results = organizationService.RetrieveMultiple(query);

                        if (results.Entities.Count > 0)
                        {
                            var job = results.Entities[0];
                            var progress = job.GetAttributeValue<double>("progress");
                            var completedOn = job.GetAttributeValue<DateTime?>("completedon");

                            // Only update if progress changed
                            if (Math.Abs(progress - lastProgress) > 0.01 || completedOn.HasValue)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    UpdateStatus($"Import progress: {progress:F2}%");
                                }));
                                lastProgress = progress;
                            }

                            if (completedOn.HasValue)
                            {
                                // Check for errors
                                var data = job.GetAttributeValue<string>("data");
                                if (!string.IsNullOrEmpty(data) && data.Contains("result=\"failure\""))
                                {
                                    throw new Exception("Solution import completed with errors. Check import job logs for details.");
                                }

                                return; // Success
                            }
                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                UpdateStatus("⏳ Waiting for import job to be created...");
                            }));
                        }

                        System.Threading.Thread.Sleep(checkInterval);
                    }
                    catch (System.ServiceModel.CommunicationException)
                    {
                        // Connection issue - try to get fresh service and continue
                        this.Invoke(new Action(() =>
                        {
                            UpdateStatus("⚠️ Connection reset. Reconnecting...");
                        }));

                        try
                        {
                            // Get fresh service synchronously
                            var freshServiceTask = GetFreshCrmServiceAsync();
                            freshServiceTask.Wait();
                            organizationService = freshServiceTask.Result;

                            this.Invoke(new Action(() =>
                            {
                                UpdateStatus("✅ Reconnected. Continuing...");
                            }));
                        }
                        catch
                        {
                            // If reconnection fails, wait longer and try again
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(15));  // ✅ Fixed
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateStatus($"⚠️ Monitoring error: {ex.Message}");
                        }));

                        System.Threading.Thread.Sleep(checkInterval);
                    }
                }

                throw new TimeoutException("Import job monitoring timed out after 30 minutes.");
            });
        }

        private void SetServiceTimeout(IOrganizationService service, TimeSpan timeout)
        {
            try
            {
                var serviceType = service.GetType();

                // For OrganizationServiceProxy
                if (service is Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy proxy)
                {
                    proxy.Timeout = timeout;
                    System.Diagnostics.Debug.WriteLine($"[RollbackForm] Set proxy timeout to {timeout.TotalMinutes} minutes");
                }
                // For WebServiceClient
                else
                {
                    var channelProperty = serviceType.GetProperty("InnerChannel");
                    if (channelProperty != null)
                    {
                        var channel = channelProperty.GetValue(service);
                        var timeoutProperty = channel?.GetType().GetProperty("OperationTimeout");
                        if (timeoutProperty != null)
                        {
                            timeoutProperty.SetValue(channel, timeout);
                            System.Diagnostics.Debug.WriteLine($"[RollbackForm] Set channel timeout to {timeout.TotalMinutes} minutes");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RollbackForm] Warning: Could not set timeout: {ex.Message}");
            }
        }

        #endregion

        private async Task<IOrganizationService> GetCrmServiceAsync()
        {
            return await Task.Run(() =>
            {
                if (targetConnection == null)
                {
                    throw new Exception("Target connection not selected.");
                }

                try
                {
                    // PRIORITY 1: Use the active service we stored during initialization
                    if (targetConnection.ConnectionId.HasValue &&
                        activeServices.ContainsKey(targetConnection.ConnectionId.Value))
                    {
                        System.Diagnostics.Debug.WriteLine($"[RollbackForm] Using stored active service for: {targetConnection.ConnectionName}");
                        return activeServices[targetConnection.ConnectionId.Value];
                    }

                    // PRIORITY 2: Try to get service from existing ServiceClient
                    if (targetConnection.ServiceClient != null && targetConnection.ServiceClient.IsReady)
                    {
                        System.Diagnostics.Debug.WriteLine($"[RollbackForm] Using ServiceClient for: {targetConnection.ConnectionName}");

                        if (targetConnection.ServiceClient.OrganizationWebProxyClient != null)
                        {
                            return targetConnection.ServiceClient.OrganizationWebProxyClient;
                        }
                        else if (targetConnection.ServiceClient.OrganizationServiceProxy != null)
                        {
                            return targetConnection.ServiceClient.OrganizationServiceProxy;
                        }
                    }

                    // PRIORITY 3: Last resort - try ConnectionManager (this will fail for SDK Login Control)
                    throw new Exception(
                        $"No active connection available for '{targetConnection.ConnectionName}'.\n\n" +
                        "This connection may have been created with SDK Login Control, which cannot be reused.\n\n" +
                        "Please:\n" +
                        "1. Add this environment as a target in the main deployment form first, OR\n" +
                        "2. Connect to this environment before attempting rollback");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Connection failed: {ex.Message}", ex);
                }
            });
        }
        private async Task VerifyConnectionAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    var whoAmI = (WhoAmIResponse)organizationService.Execute(new WhoAmIRequest());
                    if (whoAmI.UserId == Guid.Empty)
                    {
                        throw new Exception("Connection verification failed.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Connection verification failed: {ex.Message}", ex);
                }
            });
        }

        private async Task MonitorImportJobAsync(Guid importJobId)
        {
            await Task.Run(() =>
            {
                var maxWaitTime = TimeSpan.FromMinutes(30); // Maximum wait time
                var startTime = DateTime.Now;
                var checkInterval = TimeSpan.FromSeconds(5);

                while (DateTime.Now - startTime < maxWaitTime)
                {
                    try
                    {
                        var query = new Microsoft.Xrm.Sdk.Query.QueryExpression("importjob")
                        {
                            ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("progress", "completedon", "data"),
                            Criteria = new Microsoft.Xrm.Sdk.Query.FilterExpression
                            {
                                Conditions =
                                {
                                    new Microsoft.Xrm.Sdk.Query.ConditionExpression("importjobid",
                                        Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, importJobId)
                                }
                            }
                        };

                        var results = organizationService.RetrieveMultiple(query);

                        if (results.Entities.Count > 0)
                        {
                            var job = results.Entities[0];
                            var progress = job.GetAttributeValue<double>("progress");
                            var completedOn = job.GetAttributeValue<DateTime?>("completedon");

                            // Update progress on UI thread
                            this.Invoke(new Action(() =>
                            {
                                UpdateStatus($"Import progress: {progress:F2}%");
                            }));

                            if (completedOn.HasValue)
                            {
                                // Check for errors in import data
                                var data = job.GetAttributeValue<string>("data");
                                if (!string.IsNullOrEmpty(data) && data.Contains("result=\"failure\""))
                                {
                                    throw new Exception("Solution import completed with errors. Check import job logs for details.");
                                }

                                return; // Import completed successfully
                            }
                        }

                        System.Threading.Thread.Sleep(checkInterval);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error monitoring import job: {ex.Message}", ex);
                    }
                }

                throw new TimeoutException("Import job monitoring timed out after 30 minutes.");
            });
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), message);
                return;
            }

            txtDetails.AppendText(message + "\r\n");
            txtDetails.SelectionStart = txtDetails.Text.Length;
            txtDetails.ScrollToCaret();
            Application.DoEvents();
        }

        private void SetUIEnabled(bool enabled)
        {
            btnRollback.Enabled = enabled;
            btnCancel.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            btnOpenBackupFolder.Enabled = enabled;
            dgvDeployments.Enabled = enabled;

            if (!enabled)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private string GetDetailedErrorMessage(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);

            if (ex.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendLine("Inner Exception:");
                sb.AppendLine(ex.InnerException.Message);
            }

            return sb.ToString();
        }

        private void LogRollbackOperation()
        {
            try
            {
                // Create a history entry for the rollback
                var rollbackHistory = new DeploymentHistory
                {
                    SolutionUniqueName = selectedDeployment.SolutionUniqueName,
                    SolutionFriendlyName = selectedDeployment.SolutionFriendlyName,
                    SourceVersion = selectedDeployment.TargetVersion, // The version we're rolling back from
                    TargetVersion = selectedDeployment.TargetVersion,  // The version we're restoring
                    SourceEnvironment = selectedDeployment.TargetEnvironment, // Where it was deployed
                    TargetEnvironment = targetConnection.ConnectionName,      // Where we're rolling back to
                    DeploymentDate = DateTime.Now,
                    Status = DeploymentStatus.Completed,
                    DeployedBy = Environment.UserName,
                    IsManaged = selectedDeployment.DeployedAsManaged,
                    DeployedAsManaged = selectedDeployment.DeployedAsManaged,
                    BackupCreated = false,
                    BackupPath = null,
                    DurationSeconds = 0,
                    Notes = $"Rollback from deployment on {selectedDeployment.DeploymentDate:yyyy-MM-dd HH:mm}"
                };

                historyService.AddHistory(rollbackHistory);
            }
            catch (Exception ex)
            {
                // Don't fail the rollback if logging fails
                System.Diagnostics.Debug.WriteLine($"Failed to log rollback: {ex.Message}");
            }
        }

        private void LogRollbackFailure(Exception ex)
        {
            try
            {
                var rollbackHistory = new DeploymentHistory
                {
                    SolutionUniqueName = selectedDeployment.SolutionUniqueName,
                    SolutionFriendlyName = selectedDeployment.SolutionFriendlyName,
                    TargetVersion = selectedDeployment.TargetVersion,
                    TargetEnvironment = targetConnection?.ConnectionName ?? selectedDeployment.TargetEnvironment,
                    DeploymentDate = DateTime.Now,
                    Status = DeploymentStatus.Failed,
                    DeployedBy = Environment.UserName,
                    DeployedAsManaged = selectedDeployment.DeployedAsManaged,
                    BackupCreated = false,
                    BackupPath = null,
                    Notes = $"Rollback FAILED: {ex.Message}"
                };

                historyService.AddHistory(rollbackHistory);
            }
            catch
            {
                // Silently fail if we can't log
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
