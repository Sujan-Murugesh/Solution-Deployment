using McTools.Xrm.Connection;
using McTools.Xrm.Connection.WinForms;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using ConnectionManager = Sujan_Solution_Deployer.Services.ConnectionManager;
using SolutionInfo = Sujan_Solution_Deployer.Models.SolutionInfo;

namespace Sujan_Solution_Deployer
{
    public partial class SolutionDeployerControl : PluginControlBase
    {
        private Settings mySettings;
        private SolutionService solutionService;
        private List<DeploymentTarget> targetEnvironments = new List<DeploymentTarget>();
        private List<SolutionInfo> unmanagedSolutions = new List<SolutionInfo>();
        private List<SolutionInfo> managedSolutions = new List<SolutionInfo>();

        public SolutionDeployerControl()
        {
            InitializeComponent();
        }

        #region ==>Methods
        private void SolutionDeployerControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("Welcome to Sujan Solution Deployer! Connect to your DEV environment to begin.", null);
            InitializeDefaultSettings();
        }

        private void InitializeDefaultSettings()
        {
            // Set default backup path
            var defaultBackupPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "SolutionBackups");

            if (!Directory.Exists(defaultBackupPath))
            {
                try
                {
                    Directory.CreateDirectory(defaultBackupPath);
                }
                catch (Exception ex)
                {
                    LogWarning($"Could not create default backup folder: {ex.Message}");
                }
            }

            txtBackupPath.Text = defaultBackupPath;
        }

        private void btnLoadSolutions_Click(object sender, EventArgs e)
        {
            if (Service == null)
            {
                MessageBox.Show("Please connect to an environment first using the connection button in the toolbar.",
                    "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ExecuteMethod(LoadSolutions);
        }

        private void LoadSolutions()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading solutions from connected environment...",
                Work = (worker, args) =>
                {
                solutionService = new SolutionService(Service);
                // Load unmanaged solutions
                var unmanaged = solutionService.GetAllSolutions(false);

                // Load managed solutions
                var managed = solutionService.GetAllSolutions(true);
                args.Result = new { Unmanaged = unmanaged, Managed = managed };
                
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show($"Error loading solutions: {args.Error.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogError($"Failed to load solutions: {args.Error.Message}");
                        return;
                    }

                    var result = (dynamic)args.Result;

                    // Store solutions
                    unmanagedSolutions = (List<SolutionInfo>)result.Unmanaged;
                    managedSolutions = (List<SolutionInfo>)result.Managed;

                    // Populate unmanaged solutions list
                    chkUnmanagedSolutions.Items.Clear();
                    foreach (var solution in unmanagedSolutions)
                    {
                        chkUnmanagedSolutions.Items.Add(
                            $"{solution.FriendlyName} (v{solution.Version})", false);
                    }

                    // Populate managed solutions list
                    chkManagedSolutions.Items.Clear();
                    foreach (var solution in managedSolutions)
                    {
                        chkManagedSolutions.Items.Add(
                            $"{solution.FriendlyName} (v{solution.Version})", false);
                    }

                    LogInfo($"✅ Successfully loaded {unmanagedSolutions.Count} unmanaged and {managedSolutions.Count} managed solutions");
                    ShowInfoNotification($"Loaded {unmanagedSolutions.Count + managedSolutions.Count} solutions successfully!", null);
                }
            });
        }

        private void btnAddTarget_Click(object sender, EventArgs e)
        {
            // Show connection selector to add target environments
            var selector = new ConnectionSelector(true)
            {
                StartPosition = FormStartPosition.CenterParent
            };

            if (selector.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var connection in selector.SelectedConnections)
                {
                    // Check if already added
                    if (targetEnvironments.Any(t => t.ConnectionDetail.ConnectionId == connection.ConnectionId))
                    {
                        LogWarning($"Environment '{connection.ConnectionName}' is already in the target list");
                        continue;
                    }

                    try
                    {
                        // Test connection before adding
                        LogInfo($"🔌 Testing connection to '{connection.ConnectionName}'...");

                        var serviceClient = ConnectionManager.CreateServiceClient(connection);

                        if (!serviceClient.IsReady)
                        {
                            LogError($"Failed to connect to '{connection.ConnectionName}': {serviceClient.LastCrmError}");
                            MessageBox.Show($"Failed to connect to '{connection.ConnectionName}'.\n\nError: {serviceClient.LastCrmError}",
                                "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        var target = new DeploymentTarget
                        {
                            Name = connection.ConnectionName,
                            ConnectionDetail = connection,
                            ServiceClient = serviceClient
                        };

                        targetEnvironments.Add(target);
                        chkTargetEnvironments.Items.Add(target.Name, false);
                        LogInfo($"➕ Added target environment: {target.Name}");
                    }
                    catch (Exception ex)
                    {
                        LogError($"Error adding target '{connection.ConnectionName}': {ex.Message}");
                        MessageBox.Show($"Error adding target '{connection.ConnectionName}':\n\n{ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (selector.SelectedConnections.Count > 0)
                {
                    ShowInfoNotification($"Added target environment(s)", null);
                }
            }
        }

        private void btnRemoveTarget_Click(object sender, EventArgs e)
        {
            if (chkTargetEnvironments.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one environment to remove.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var itemsToRemove = chkTargetEnvironments.SelectedItems.Cast<string>().ToList();

            foreach (var item in itemsToRemove)
            {
                var target = targetEnvironments.FirstOrDefault(t => t.Name == item);
                if (target != null)
                {
                    // Dispose service client
                    if (target.ServiceClient != null)
                    {
                        try
                        {
                            target.ServiceClient.Dispose();
                        }
                        catch { }
                    }

                    targetEnvironments.Remove(target);
                    chkTargetEnvironments.Items.Remove(item);
                    LogInfo($"➖ Removed target environment: {item}");
                }
            }
        }

        private void btnBrowseBackup_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select backup location for solutions";
                dialog.ShowNewFolderButton = true;
                dialog.SelectedPath = txtBackupPath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtBackupPath.Text = dialog.SelectedPath;
                    LogInfo($"💾 Backup path set to: {dialog.SelectedPath}");
                }
            }
        }

        private void chkEnableBackup_CheckedChanged(object sender, EventArgs e)
        {
            txtBackupPath.Enabled = chkEnableBackup.Checked;
            btnBrowseBackup.Enabled = chkEnableBackup.Checked;
        }

        private void btnDeploy_Click(object sender, EventArgs e)
        {
            // Validate selections
            var selectedSolutions = GetSelectedSolutions();
            if (selectedSolutions.Count == 0)
            {
                MessageBox.Show("Please select at least one solution to deploy.",
                    "No Solutions Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedTargets = GetSelectedTargets();
            if (selectedTargets.Count == 0)
            {
                MessageBox.Show("Please select at least one target environment.",
                    "No Target Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (chkEnableBackup.Checked && string.IsNullOrWhiteSpace(txtBackupPath.Text))
            {
                MessageBox.Show("Please select a backup location or disable backup.",
                    "No Backup Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm deployment
            var confirmMessage = $"🚀 DEPLOYMENT CONFIRMATION\n\n" +
                               $"You are about to deploy {selectedSolutions.Count} solution(s) to {selectedTargets.Count} environment(s).\n\n" +
                               $"📦 Solutions to Deploy:\n";

            foreach (var sol in selectedSolutions)
            {
                confirmMessage += $"   • {sol.FriendlyName} (v{sol.Version}) - {(sol.IsManaged ? "Managed" : "Unmanaged")}\n";
            }

            confirmMessage += $"\n🎯 Target Environments:\n";
            foreach (var target in selectedTargets)
            {
                confirmMessage += $"   • {target.Name}\n";
            }

            confirmMessage += $"\n⚙️ Settings:\n";
            confirmMessage += $"   • Deployment Type: {(rbUpgrade.Checked ? "Upgrade" : "Update")}\n";
            confirmMessage += $"   • Backup Enabled: {(chkEnableBackup.Checked ? "Yes" : "No")}\n";
            confirmMessage += $"   • Publish Workflows: {(chkPublishWorkflows.Checked ? "Yes" : "No")}\n";
            confirmMessage += $"   • Overwrite Customizations: {(chkOverwriteCustomizations.Checked ? "Yes" : "No")}\n";
            confirmMessage += $"\n⚠️ This operation cannot be undone!\n\nDo you want to proceed?";

            if (MessageBox.Show(confirmMessage, "Confirm Deployment",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StartDeployment(selectedSolutions, selectedTargets);
            }
        }

        private List<SolutionInfo> GetSelectedSolutions()
        {
            var selected = new List<SolutionInfo>();

            // Get selected unmanaged solutions
            for (int i = 0; i < chkUnmanagedSolutions.CheckedIndices.Count; i++)
            {
                int index = chkUnmanagedSolutions.CheckedIndices[i];
                if (index < unmanagedSolutions.Count)
                {
                    selected.Add(unmanagedSolutions[index]);
                }
            }

            // Get selected managed solutions
            for (int i = 0; i < chkManagedSolutions.CheckedIndices.Count; i++)
            {
                int index = chkManagedSolutions.CheckedIndices[i];
                if (index < managedSolutions.Count)
                {
                    selected.Add(managedSolutions[index]);
                }
            }

            return selected;
        }

        private List<DeploymentTarget> GetSelectedTargets()
        {
            var selected = new List<DeploymentTarget>();

            for (int i = 0; i < chkTargetEnvironments.CheckedIndices.Count; i++)
            {
                int index = chkTargetEnvironments.CheckedIndices[i];
                if (index < targetEnvironments.Count)
                {
                    selected.Add(targetEnvironments[index]);
                }
            }

            return selected;
        }

        private void StartDeployment(List<SolutionInfo> solutions, List<DeploymentTarget> targets)
        {
            // Disable UI during deployment
            btnDeploy.Enabled = false;
            btnLoadSolutions.Enabled = false;
            btnAddTarget.Enabled = false;
            btnRemoveTarget.Enabled = false;
            progressBar.Value = 0;
            progressBar.Maximum = 100;

            LogInfo("==========================================");
            LogInfo("🚀 DEPLOYMENT STARTED");
            LogInfo("==========================================");
            LogInfo($"📊 Total Solutions: {solutions.Count}");
            LogInfo($"🎯 Total Targets: {targets.Count}");
            LogInfo($"⚙️ Deployment Mode: {(rbUpgrade.Checked ? "Upgrade" : "Update")}");
            LogInfo($"💾 Backup: {(chkEnableBackup.Checked ? "Enabled" : "Disabled")}");
            LogInfo("==========================================\n");

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Deploying solutions...",
                Work = (worker, args) =>
                {
                    var totalOperations = solutions.Count * (targets.Count + 1); // +1 for export/backup
                    var completedOperations = 0;

                    foreach (var solution in solutions)
                    {
                        try
                        {
                            worker.ReportProgress(0, $"\n📦 Processing: {solution.FriendlyName}");
                            worker.ReportProgress(0, new string('-', 50));

                            // Step 1: Export solution
                            worker.ReportProgress(
                                (int)((double)completedOperations / totalOperations * 100),
                                $"🔄 Exporting solution: {solution.FriendlyName}");

                            bool exportAsManaged = rbUpgrade.Checked ? true : solution.IsManaged;
                            byte[] solutionFile = solutionService.ExportSolution(
                                solution.UniqueName,
                                exportAsManaged,
                                (msg) => worker.ReportProgress(0, $"   {msg}"));

                            worker.ReportProgress(0, $"✅ Export completed: {solution.FriendlyName}");

                            // Step 2: Backup (if enabled)
                            if (chkEnableBackup.Checked)
                            {
                                worker.ReportProgress(0, $"💾 Creating backup: {solution.FriendlyName}");

                                var backupFileName = solutionService.BackupSolution(
                                    solution.UniqueName,
                                    txtBackupPath.Text,
                                    exportAsManaged,
                                    (msg) => worker.ReportProgress(0, $"   {msg}"));

                                worker.ReportProgress(0, $"✅ Backup saved: {Path.GetFileName(backupFileName)}");
                            }

                            completedOperations++;

                            // Step 3: Deploy to each target
                            foreach (var target in targets)
                            {
                                try
                                {
                                    worker.ReportProgress(
                                        (int)((double)completedOperations / totalOperations * 100),
                                        $"\n🎯 Deploying to: {target.Name}");

                                    // Ensure we have a valid connection
                                    worker.ReportProgress(0, $"   🔌 Verifying connection to {target.Name}...");

                                    IOrganizationService targetService = null;

                                    // Check if ServiceClient is valid
                                    if (target.ServiceClient == null || !target.ServiceClient.IsReady)
                                    {
                                        worker.ReportProgress(0, $"   🔄 Reconnecting to {target.Name}...");
                                        target.ServiceClient = ConnectionManager.CreateServiceClient(target.ConnectionDetail);
                                    }

                                    // ✅ Extend timeout to 20 minutes
                                    if (target.ServiceClient.OrganizationWebProxyClient != null)
                                    {
                                        targetService = target.ServiceClient.OrganizationWebProxyClient;
                                        target.ServiceClient.OrganizationWebProxyClient.InnerChannel.OperationTimeout = new TimeSpan(0, 20, 0);
                                    }
                                    else if (target.ServiceClient.OrganizationServiceProxy != null)
                                    {
                                        targetService = target.ServiceClient.OrganizationServiceProxy;
                                        target.ServiceClient.OrganizationServiceProxy.Timeout = new TimeSpan(0, 20, 0);
                                    }
                                    else
                                    {
                                        throw new Exception("Unable to get organization service from connection");
                                    }

                                    worker.ReportProgress(0, $"   ✅ Connection verified to {target.Name}");

                                    var targetSolutionService = new SolutionService(targetService);

                                    // Import solution
                                    worker.ReportProgress(0, $"   📥 Importing {solution.FriendlyName}...");
                                    var importJobId = targetSolutionService.ImportSolution(
                                        solutionFile,
                                        chkPublishWorkflows.Checked,
                                        chkOverwriteCustomizations.Checked,
                                        (msg) => worker.ReportProgress(0, $"      {msg}"));

                                    // Monitor import progress
                                    worker.ReportProgress(0, $"   ⏳ Monitoring import progress...");
                                    bool importCompleted = false;
                                    int maxRetries = 120; // 10 minutes (5 seconds * 120)
                                    int retryCount = 0;
                                    double lastProgress = 0;

                                    while (!importCompleted && retryCount < maxRetries)
                                    {
                                        Thread.Sleep(5000); // Wait 5 seconds

                                        var (status, progress, isCompleted) = targetSolutionService.GetImportJobStatus(importJobId);

                                        // Only log if progress changed
                                        if (progress != lastProgress || isCompleted)
                                        {
                                            worker.ReportProgress(0, $"   📊 Import Progress: {progress:F0}% - {status}");
                                            lastProgress = progress;
                                        }

                                        importCompleted = isCompleted;
                                        retryCount++;
                                    }

                                    if (importCompleted)
                                    {
                                        worker.ReportProgress(0, $"   ✅ Successfully deployed {solution.FriendlyName} to {target.Name}");
                                    }
                                    else
                                    {
                                        worker.ReportProgress(0, $"   ⚠️ Import timeout for {target.Name}. The import may still be processing. Please check the target environment.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    worker.ReportProgress(0, $"   ❌ Error deploying to {target.Name}: {ex.Message}");

                                    // Log full exception for debugging
                                    if (ex.InnerException != null)
                                    {
                                        worker.ReportProgress(0, $"      Inner Exception: {ex.InnerException.Message}");
                                    }
                                }

                                completedOperations++;
                            }
                        }
                        catch (Exception ex)
                        {
                            worker.ReportProgress(0, $"❌ Error processing {solution.FriendlyName}: {ex.Message}");
                            completedOperations += targets.Count;
                        }
                    }

                    worker.ReportProgress(100, "\n==========================================");
                    worker.ReportProgress(100, "✅ DEPLOYMENT COMPLETED");
                    worker.ReportProgress(100, "==========================================");
                },
                ProgressChanged = (args) =>
                {
                    if (args.ProgressPercentage > 0 && args.ProgressPercentage <= 100)
                    {
                        progressBar.Value = args.ProgressPercentage;
                        lblProgress.Text = $"📊 Deployment Progress: {args.ProgressPercentage}%";
                    }

                    if (args.UserState != null)
                    {
                        var message = args.UserState.ToString();
                        LogInfo(message);
                    }
                },
                PostWorkCallBack = (args) =>
                {
                    // Re-enable UI
                    btnDeploy.Enabled = true;
                    btnLoadSolutions.Enabled = true;
                    btnAddTarget.Enabled = true;
                    btnRemoveTarget.Enabled = true;
                    progressBar.Value = 100;

                    if (args.Error != null)
                    {
                        LogError($"\n❌ Deployment error: {args.Error.Message}");
                        MessageBox.Show($"Deployment encountered an error:\n\n{args.Error.Message}",
                            "Deployment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        LogInfo("\n🎉 All deployment operations completed!");
                        LogInfo($"📝 Check the log above for detailed results.");

                        MessageBox.Show("Deployment process completed!\n\nPlease review the log for detailed results of each deployment.",
                            "Deployment Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ShowInfoNotification("Deployment completed successfully!", null);
                    }
                }
            });
        }

        private void LogInfo(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogInfo(message)));
                return;
            }

            txtLog.SelectionColor = Color.Lime;
            txtLog.AppendText($"{message}\n");
            txtLog.ScrollToCaret();
        }

        private void LogWarning(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogWarning(message)));
                return;
            }

            txtLog.SelectionColor = Color.Yellow;
            txtLog.AppendText($"⚠️ {message}\n");
            txtLog.SelectionColor = Color.Lime;
            txtLog.ScrollToCaret();
        }

        private void LogError(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogError(message)));
                return;
            }

            txtLog.SelectionColor = Color.Red;
            txtLog.AppendText($"❌ {message}\n");
            txtLog.SelectionColor = Color.Lime;
            txtLog.ScrollToCaret();
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }
        private void tsbClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            LogInfo("📝 Deployment Log:");
            LogInfo("==================");
            LogInfo("Ready to deploy solutions...\n");
        }

        public override void ClosingPlugin(PluginCloseInfo info)
        {
            base.ClosingPlugin(info);

            if (info.FormReason != CloseReason.None ||
                info.ToolBoxReason == ToolBoxCloseReason.CloseAll ||
                info.ToolBoxReason == ToolBoxCloseReason.CloseAllExceptActive)
            {
                // Clean up service clients
                foreach (var target in targetEnvironments)
                {
                    try
                    {
                        target.ServiceClient?.Dispose();
                    }
                    catch { }
                }
                return;
            }

            // Confirm close if deployment is in progress
            if (!btnDeploy.Enabled)
            {
                info.Cancel = MessageBox.Show(
                    "⚠️ Deployment is in progress!\n\nAre you sure you want to close and cancel the deployment?",
                    "Deployment In Progress",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes;
            }
            else
            {
                // Clean up service clients
                foreach (var target in targetEnvironments)
                {
                    try
                    {
                        target.ServiceClient?.Dispose();
                    }
                    catch { }
                }
            }
        }
        #endregion

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }
        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(GetAccounts);
        }
        private void GetAccounts()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting accounts",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(new QueryExpression("account")
                    {
                        TopCount = 50
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;
                    if (result != null)
                    {
                        MessageBox.Show($"Found {result.Entities.Count} accounts");
                    }
                }
            });
        }
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }
    }
}