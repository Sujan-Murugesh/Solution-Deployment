using McTools.Xrm.Connection;
using McTools.Xrm.Connection.WinForms;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using ConnectionManager = Sujan_Solution_Deployer.Services.ConnectionManager;
using Label = System.Windows.Forms.Label;
using SolutionInfo = Sujan_Solution_Deployer.Models.SolutionInfo;
using Sujan_Solution_Deployer.Helpers;

namespace Sujan_Solution_Deployer
{
    public partial class SolutionDeployerControl : PluginControlBase
    {
        private Settings mySettings;
        private SolutionService solutionService;
        private List<DeploymentTarget> targetEnvironments = new List<DeploymentTarget>();
        private List<SolutionInfo> unmanagedSolutions = new List<SolutionInfo>();
        private List<SolutionInfo> managedSolutions = new List<SolutionInfo>();
        private DeploymentLogForm logForm = null;
        private DeploymentHistoryService deploymentHistoryService;
        private EmailNotificationService emailNotificationService;
        private EmailSettings emailSettings;
        private string lastSavedEmail;
        public SolutionDeployerControl()
        {
            InitializeComponent();
            deploymentHistoryService = new DeploymentHistoryService();
            emailNotificationService = new EmailNotificationService();
            SetPlaceholder(txtNotificationEmail, "your.email@example.com");
        }

        #region ==>Methods
        private void SolutionDeployerControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("Welcome to Sujan Solution Deployer! Connect to your DEV environment to begin.", null);
            InitializeDefaultSettings();
            InitializeLogForm();
            LoadSmtpSettings();
            LoadEmailNotificationSettings();
        }

        private void LoadEmailNotificationSettings()
        {
            try
            {
                emailSettings = EmailSettingsManager.LoadSettings();

                if (!string.IsNullOrWhiteSpace(emailSettings.NotificationEmail))
                {
                    txtNotificationEmail.Text = emailSettings.NotificationEmail;
                    txtNotificationEmail.ForeColor = Color.Black;
                    chkEmailNotification.Checked = emailSettings.EmailNotificationEnabled;
                    lastSavedEmail = emailSettings.NotificationEmail;

                    LogInfo($"📧 Loaded saved notification email: {emailSettings.NotificationEmail}");
                }
            }
            catch (Exception ex)
            {
                LogWarning($"⚠️ Could not load email notification settings: {ex.Message}");
            }
        }

        /// Load saved SMTP settings and configure email service
        private void LoadSmtpSettings()
        {
            try
            {
                if (SmtpSettingsManager.SettingsExist())
                {
                    var settings = SmtpSettingsManager.LoadSettings();

                    // Only configure if we have valid email and password
                    if (!string.IsNullOrWhiteSpace(settings.SenderEmail) &&
                        !string.IsNullOrWhiteSpace(settings.Password))
                    {
                        EmailNotificationService.ConfigureSmtp(
                            settings.SmtpHost,
                            settings.SmtpPort,
                            settings.EnableSsl,
                            settings.SenderEmail,
                            settings.Password,
                            settings.SenderName
                        );

                        LogInfo($"✅ SMTP settings loaded - Email notifications are enabled");
                        LogInfo($"   📧 Sender: {settings.SenderEmail}");
                        LogInfo($"   🖥️ Server: {settings.SmtpHost}:{settings.SmtpPort}");
                    }
                    else
                    {
                        LogWarning("⚠️ SMTP settings found but incomplete - Configure via 📧 SMTP Setup");
                    }
                }
                else
                {
                    LogInfo("ℹ️ No SMTP settings found - Configure email notifications via 📧 SMTP Setup");
                }
            }
            catch (Exception ex)
            {
                LogWarning($"⚠️ Could not load SMTP settings: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"SMTP load error: {ex.Message}");
            }
        }

        private void InitializeLogForm()
        {
            if (logForm == null || logForm.IsDisposed)
            {
                logForm = new DeploymentLogForm();
                logForm.FormHidden += (s, e) =>
                {
                    tsbDeploymentLogs.Text = "📋 Deployment Logs";
                };
            }
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
            // Step 1: Validate selections
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

            // Step 2: Validate email if notification is enabled
            string notificationEmail = null;
            if (chkEmailNotification.Checked)
            {
                notificationEmail = GetValidatedEmailAddress();
                if (notificationEmail == null)
                {
                    return;
                }
            }

            // Step 3: Show Version Increment Dialog
            var versionForm = new VersionIncrementForm(selectedSolutions, solutionService);
            if (versionForm.ShowDialog(this) != DialogResult.OK)
            {
                return; // User cancelled
            }

            var updatedVersions = versionForm.UpdatedVersions;
            var updateInSource = versionForm.UpdateInSource;

            // Step 4: Open log window automatically
            if (logForm == null || logForm.IsDisposed)
            {
                InitializeLogForm();
            }

            if (!logForm.Visible)
            {
                logForm.Show(this);
                tsbDeploymentLogs.Text = "❌ Close Logs";
            }

            // Step 5: Build confirmation message with version information
            var confirmMessage = BuildConfirmationMessage(selectedSolutions, selectedTargets, updatedVersions, updateInSource);

            // Step 6: Confirm and start deployment
            if (MessageBox.Show(confirmMessage, "Confirm Deployment",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StartDeployment(selectedSolutions, selectedTargets, updatedVersions, updateInSource, notificationEmail);
            }
        }

        private string BuildConfirmationMessage(
            List<SolutionInfo> solutions,
            List<DeploymentTarget> targets,
            Dictionary<string, string> updatedVersions,
            bool updateInSource)
        {
            var confirmMessage = "🚀 DEPLOYMENT CONFIRMATION\n\n";
            confirmMessage += $"You are about to deploy {solutions.Count} solution(s) to {targets.Count} environment(s).\n\n";
            confirmMessage += "📦 Solutions to Deploy:\n";

            foreach (var sol in solutions)
            {
                var currentVersion = sol.Version;
                var newVersion = updatedVersions.ContainsKey(sol.UniqueName) ? updatedVersions[sol.UniqueName] : currentVersion;
                var versionChange = currentVersion != newVersion ? $" → v{newVersion}" : "";
                var managedLabel = sol.IsManaged ? "Managed" : "Unmanaged";

                confirmMessage += $"   • {sol.FriendlyName} (v{currentVersion}{versionChange}) - {managedLabel}\n";
            }

            confirmMessage += "\n🎯 Target Environments:\n";
            foreach (var target in targets)
            {
                confirmMessage += $"   • {target.Name}\n";
            }

            confirmMessage += "\n⚙️ Settings:\n";
            confirmMessage += $"   • Deployment Type: {(rbUpgrade.Checked ? "Upgrade" : "Update")}\n";
            confirmMessage += $"   • Update Source Versions: {(updateInSource ? "Yes" : "No")}\n";
            confirmMessage += $"   • Backup Enabled: {(chkEnableBackup.Checked ? "Yes" : "No")}\n";
            confirmMessage += $"   • Publish Workflows: {(chkPublishWorkflows.Checked ? "Yes" : "No")}\n";
            confirmMessage += $"   • Overwrite Customizations: {(chkOverwriteCustomizations.Checked ? "Yes" : "No")}\n";

            if (chkDeployAsManaged.Checked)
            {
                var unmanagedCount = solutions.Count(s => !s.IsManaged);
                if (unmanagedCount > 0)
                {
                    confirmMessage += $"   • Deploy as Managed: Yes ({unmanagedCount} unmanaged solution(s) will be converted)\n";
                }
            }

            confirmMessage += "\n⚠️ This operation cannot be undone!\n\n";
            confirmMessage += "Do you want to proceed?";

            return confirmMessage;
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

        #region ==>Start Deployment Logic
        private void StartDeployment(
            List<SolutionInfo> solutions,
            List<DeploymentTarget> targets,
            Dictionary<string, string> updatedVersions,
            bool updateInSource,
            string notificationEmail)
        {
            // Mark deployment as in progress
            if (logForm != null && !logForm.IsDisposed)
            {
                logForm.SetDeploymentInProgress(true);
            }

            // Disable UI during deployment
            btnDeploy.Enabled = false;
            btnLoadSolutions.Enabled = false;
            btnAddTarget.Enabled = false;
            btnRemoveTarget.Enabled = false;
            progressBar.Value = 0;
            progressBar.Maximum = 100;

            // Log deployment start
            LogInfo("\n\n" + new string('=', 60));
            LogInfo($"🆕 NEW DEPLOYMENT SESSION - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            LogInfo(new string('=', 60));
            LogInfo("==========================================");
            LogInfo("🚀 DEPLOYMENT STARTED");
            LogInfo("==========================================");
            LogInfo($"📊 Total Solutions: {solutions.Count}");
            LogInfo($"🎯 Total Targets: {targets.Count}");
            LogInfo($"⚙️ Deployment Mode: {(rbUpgrade.Checked ? "Upgrade" : "Update")}");
            LogInfo($"💾 Backup: {(chkEnableBackup.Checked ? "Enabled" : "Disabled")}");
            LogInfo($"🔒 Deploy as Managed: {(chkDeployAsManaged.Checked ? "Yes" : "No")}");
            LogInfo($"🔢 Update Source Versions: {(updateInSource ? "Yes" : "No")}");
            if (!string.IsNullOrWhiteSpace(notificationEmail))
            {
                LogInfo($"📧 Email Notification: {notificationEmail}");
            }
            LogInfo("==========================================\n");

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Deploying solutions...",
                Work = (worker, args) =>
                {
                    var totalOperations = solutions.Count * (targets.Count + 1);
                    var completedOperations = 0;
                    var deploymentStartTime = DateTime.Now;

                    // ✅ Track deployment results for email summary
                    var deploymentResults = new List<DeploymentHistory>();
                    int successCount = 0;
                    int failureCount = 0;

                    foreach (var solution in solutions)
                    {
                        var solutionStartTime = DateTime.Now;
                        string deploymentVersion = solution.Version;

                        try
                        {
                            worker.ReportProgress(0, $"\n📦 Processing: {solution.FriendlyName}");
                            worker.ReportProgress(0, new string('-', 50));

                            // Step 0: Update version in source if requested
                            if (updateInSource && updatedVersions.ContainsKey(solution.UniqueName))
                            {
                                var newVersion = updatedVersions[solution.UniqueName];

                                if (newVersion != solution.Version)
                                {
                                    worker.ReportProgress(0, $"🔢 Updating version in source: {solution.Version} → {newVersion}");

                                    try
                                    {
                                        if (!solution.IsManaged)
                                        {
                                            solutionService.UpdateSolutionVersion(
                                                solution.UniqueName,
                                                newVersion,
                                                (msg) => worker.ReportProgress(0, $"   {msg}"));

                                            deploymentVersion = newVersion;
                                            solution.Version = newVersion;

                                            worker.ReportProgress(0, $"✅ Version updated in source environment");
                                        }
                                        else
                                        {
                                            worker.ReportProgress(0, $"⚠️ Cannot update version of managed solution in source");
                                            deploymentVersion = solution.Version;
                                        }
                                    }
                                    catch (Exception versionEx)
                                    {
                                        worker.ReportProgress(0, $"⚠️ Warning: Could not update version in source: {versionEx.Message}");
                                        worker.ReportProgress(0, $"   Continuing with current version: {solution.Version}");
                                        deploymentVersion = solution.Version;
                                    }
                                }
                            }
                            else if (updatedVersions.ContainsKey(solution.UniqueName))
                            {
                                deploymentVersion = updatedVersions[solution.UniqueName];
                                worker.ReportProgress(0, $"📋 Using version for deployment: {deploymentVersion}");
                            }

                            // Determine export type
                            bool exportAsManaged = rbUpgrade.Checked ? true : solution.IsManaged;
                            bool deployingAsManaged = chkDeployAsManaged.Checked && !solution.IsManaged;

                            if (deployingAsManaged)
                            {
                                exportAsManaged = true;
                                worker.ReportProgress(0, $"🔒 Converting to managed solution for deployment");
                            }

                            // Step 1: Export solution
                            worker.ReportProgress(
                                (int)((double)completedOperations / totalOperations * 100),
                                $"🔄 Exporting solution: {solution.FriendlyName}");

                            byte[] solutionFile = solutionService.ExportSolution(
                                solution.UniqueName,
                                exportAsManaged,
                                (msg) => worker.ReportProgress(0, $"   {msg}"));

                            worker.ReportProgress(0, $"✅ Export completed: {solution.FriendlyName}");

                            // Step 2: Backup (if enabled)
                            string backupPath = null;
                            if (chkEnableBackup.Checked)
                            {
                                worker.ReportProgress(0, $"💾 Creating backup: {solution.FriendlyName}");

                                if (deployingAsManaged)
                                {
                                    // Backup both versions
                                    worker.ReportProgress(0, $"   📦 Backing up unmanaged version...");
                                    var unmanagedBackup = solutionService.BackupSolution(
                                        solution.UniqueName,
                                        txtBackupPath.Text,
                                        false,
                                        (msg) => worker.ReportProgress(0, $"   {msg}"));
                                    worker.ReportProgress(0, $"   ✅ Unmanaged backup: {Path.GetFileName(unmanagedBackup)}");

                                    worker.ReportProgress(0, $"   📦 Backing up managed version...");
                                    backupPath = solutionService.BackupSolution(
                                        solution.UniqueName,
                                        txtBackupPath.Text,
                                        true,
                                        (msg) => worker.ReportProgress(0, $"   {msg}"));
                                    worker.ReportProgress(0, $"   ✅ Managed backup: {Path.GetFileName(backupPath)}");
                                }
                                else
                                {
                                    backupPath = solutionService.BackupSolution(
                                        solution.UniqueName,
                                        txtBackupPath.Text,
                                        exportAsManaged,
                                        (msg) => worker.ReportProgress(0, $"   {msg}"));
                                    worker.ReportProgress(0, $"✅ Backup saved: {Path.GetFileName(backupPath)}");
                                }
                            }

                            completedOperations++;

                            // Step 3: Deploy to each target
                            foreach (var target in targets)
                            {
                                // ✅ Collect deployment result
                                var result = DeployToTarget(worker, solution, target, solutionFile, deploymentVersion,
                                             exportAsManaged, deployingAsManaged, backupPath,
                                             ref completedOperations, totalOperations);

                                deploymentResults.Add(result);

                                // ✅ Track success/failure
                                if (result.Status == DeploymentStatus.Completed)
                                    successCount++;
                                else
                                    failureCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            worker.ReportProgress(0, $"❌ Error processing {solution.FriendlyName}: {ex.Message}");
                            completedOperations += targets.Count;

                            // Save failed history
                            var failedHistories = SaveFailedDeploymentHistory(solution, targets, deploymentVersion,
                                                       ex.Message, solutionStartTime);

                            // ✅ Add failed results
                            deploymentResults.AddRange(failedHistories);
                            failureCount += targets.Count;
                        }
                    }

                    // Log completion
                    var deploymentEndTime = DateTime.Now;
                    var totalDuration = deploymentEndTime - deploymentStartTime;

                    worker.ReportProgress(100, "\n==========================================");
                    worker.ReportProgress(100, "✅ DEPLOYMENT COMPLETED");
                    worker.ReportProgress(100, $"⏱️ Total Duration: {totalDuration.Hours}h {totalDuration.Minutes}m {totalDuration.Seconds}s");
                    worker.ReportProgress(100, "==========================================");

                    // ✅ Pass results for email notification
                    args.Result = new
                    {
                        Results = deploymentResults,
                        SuccessCount = successCount,
                        FailureCount = failureCount,
                        StartTime = deploymentStartTime,
                        EndTime = deploymentEndTime
                    };
                },
                ProgressChanged = (args) =>
                {
                    if (args.ProgressPercentage > 0 && args.ProgressPercentage <= 100)
                    {
                        progressBar.Value = args.ProgressPercentage;
                        lblProgress.Text = $"📊 Deployment Progress: {args.ProgressPercentage}%";
                        lblProgress.ForeColor = Color.Black;
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

                    if (logForm != null && !logForm.IsDisposed)
                    {
                        logForm.SetDeploymentInProgress(false);
                    }

                    if (args.Error != null)
                    {
                        LogError($"\n❌ Deployment error: {args.Error.Message}");
                        MessageBox.Show($"Deployment encountered an error:\n\n{args.Error.Message}",
                            "Deployment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // ✅ Send email notification if configured
                        if (!string.IsNullOrWhiteSpace(notificationEmail) && args.Result != null)
                        {
                            try
                            {
                                var result = (dynamic)args.Result;

                                LogInfo($"\n📧 Sending email notification to {notificationEmail}...");

                                // Pass the deployment list for detailed email
                                EmailNotificationService.SendBatchDeploymentSummary(
                                    notificationEmail,
                                    result.Results.Count,
                                    result.SuccessCount,
                                    result.FailureCount,
                                    result.StartTime,
                                    result.EndTime,
                                    result.Results);

                                LogInfo("✅ Email notification sent successfully");
                            }
                            catch (Exception emailEx)
                            {
                                LogWarning($"⚠️ Could not send email notification: {emailEx.Message}");
                            }
                        }

                        LogInfo("\n🎉 All deployment operations completed!");
                        LogInfo($"📝 Review the detailed log above.");
                        LogInfo($"📊 View deployment history for detailed results.");

                        MessageBox.Show("Deployment process completed!\n\nPlease review the deployment log and history for detailed results.",
                            "Deployment Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ShowInfoNotification("Deployment completed successfully!", null);
                    }
                }
            });
        }

        private DeploymentHistory DeployToTarget(
            BackgroundWorker worker,
            SolutionInfo solution,
            DeploymentTarget target,
            byte[] solutionFile,
            string deploymentVersion,
            bool exportAsManaged,
            bool deployingAsManaged,
            string backupPath,
            ref int completedOperations,
            int totalOperations)
        {
            var targetDeploymentStart = DateTime.Now;
            string errorMessage = null;
            DeploymentStatus status = DeploymentStatus.Queued;
            string targetVersion = null;

            try
            {
                worker.ReportProgress(
                    (int)((double)completedOperations / totalOperations * 100),
                    $"\n🎯 Deploying to: {target.Name}");

                worker.ReportProgress(0, $"   🔍 Checking target environment...");

                // Get target service
                IOrganizationService targetService = GetTargetService(target, worker);
                var targetSolutionService = new SolutionService(targetService);

                // Check existing solution
                var existingSolution = targetSolutionService.GetSolutionByUniqueName(solution.UniqueName);
                CheckVersionComparison(worker, solution, existingSolution, deploymentVersion);

                // Import solution
                worker.ReportProgress(0, $"   📥 Importing {solution.FriendlyName} v{deploymentVersion}...");
                var importJobId = targetSolutionService.ImportSolution(
                    solutionFile,
                    chkPublishWorkflows.Checked,
                    chkOverwriteCustomizations.Checked,
                    (msg) => worker.ReportProgress(0, $"      {msg}"));

                // Monitor import
                bool importCompleted = MonitorImport(worker, targetSolutionService, importJobId);

                if (importCompleted)
                {
                    var updatedSolution = targetSolutionService.GetSolutionByUniqueName(solution.UniqueName);
                    targetVersion = updatedSolution?.Version ?? deploymentVersion;

                    worker.ReportProgress(0, $"   ✅ Successfully deployed {solution.FriendlyName} v{targetVersion} to {target.Name}");
                    status = DeploymentStatus.Completed;
                }
                else
                {
                    worker.ReportProgress(0, $"   ⚠️ Import timeout for {target.Name}");
                    status = DeploymentStatus.Failed;
                    errorMessage = "Import timeout - may still be processing";
                    targetVersion = deploymentVersion;
                }
            }
            catch (Exception ex)
            {
                worker.ReportProgress(0, $"   ❌ Error deploying to {target.Name}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    worker.ReportProgress(0, $"      Inner Exception: {ex.InnerException.Message}");
                }

                status = DeploymentStatus.Failed;
                errorMessage = ex.Message;
                targetVersion = deploymentVersion;
            }

            // Save and return history
            var history = SaveDeploymentHistory(solution, target, deploymentVersion, targetVersion, status,
                                 errorMessage, targetDeploymentStart, backupPath, exportAsManaged, deployingAsManaged);

            completedOperations++;

            return history;  // ✅ Return the deployment history
        }

        private IOrganizationService GetTargetService(
            DeploymentTarget target, 
            BackgroundWorker worker)
        {
            if (target.ServiceClient == null || !target.ServiceClient.IsReady)
            {
                worker.ReportProgress(0, $"   🔄 Reconnecting to {target.Name}...");
                target.ServiceClient = ConnectionManager.CreateServiceClient(target.ConnectionDetail);
            }

            IOrganizationService targetService = null;

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
            return targetService;
        }

        private void CheckVersionComparison(
            BackgroundWorker worker, 
            SolutionInfo solution,
            SolutionInfo existingSolution, 
            string deploymentVersion)
        {
            if (existingSolution != null)
            {
                worker.ReportProgress(0, $"   📋 Existing solution found: v{existingSolution.Version}");

                int versionComparison = solutionService.CompareVersions(deploymentVersion, existingSolution.Version);

                if (versionComparison < 0)
                {
                    worker.ReportProgress(0, $"   ⚠️ WARNING: Downgrade detected!");
                    worker.ReportProgress(0, $"   📉 Target v{existingSolution.Version} > Deploying v{deploymentVersion}");
                }
                else if (versionComparison == 0)
                {
                    worker.ReportProgress(0, $"   ℹ️ Same version detected: v{deploymentVersion}");
                }
                else
                {
                    worker.ReportProgress(0, $"   ⬆️ Upgrade: v{existingSolution.Version} → v{deploymentVersion}");
                }
            }
            else
            {
                worker.ReportProgress(0, $"   🆕 New solution - will be installed as v{deploymentVersion}");
            }
        }

        private bool MonitorImport(
            BackgroundWorker worker, 
            SolutionService targetSolutionService, 
            Guid importJobId)
        {
            worker.ReportProgress(0, $"   ⏳ Monitoring import progress...");

            bool importCompleted = false;
            int maxRetries = 240;
            int retryCount = 0;
            double lastProgress = 0;

            while (!importCompleted && retryCount < maxRetries)
            {
                Thread.Sleep(5000);

                var importStatus = targetSolutionService.GetImportJobStatus(importJobId);
                var status = importStatus.Item1;
                var progress = importStatus.Item2;
                var isCompleted = importStatus.Item3;

                if (progress != lastProgress || isCompleted)
                {
                    worker.ReportProgress(0, $"   📊 Import Progress: {progress:F0}% - {status}");
                    lastProgress = progress;
                }

                importCompleted = isCompleted;
                retryCount++;
            }

            return importCompleted;
        }

        private DeploymentHistory SaveDeploymentHistory(
            SolutionInfo solution,
            DeploymentTarget target,
            string sourceVersion,
            string targetVersion,
            DeploymentStatus status,
            string errorMessage,
            DateTime startTime,
            string backupPath,
            bool exportAsManaged,
            bool deployingAsManaged)
        {
            var endTime = DateTime.Now;
            var duration = (int)(endTime - startTime).TotalSeconds;
            string notes = GenerateDeploymentNotes(status, errorMessage, sourceVersion, targetVersion, exportAsManaged, deployingAsManaged);

            var history = new DeploymentHistory
            {
                DeploymentDate = startTime,
                SolutionUniqueName = solution.UniqueName,
                SolutionFriendlyName = solution.FriendlyName,
                SourceVersion = sourceVersion,
                TargetVersion = targetVersion,
                SourceEnvironment = ConnectionDetail?.ConnectionName ?? "Unknown",
                TargetEnvironment = target.Name,
                IsManaged = solution.IsManaged,
                DeployedAsManaged = deployingAsManaged || exportAsManaged,
                Status = status,
                DeployedBy = Environment.UserName,
                ErrorMessage = errorMessage,
                DurationSeconds = duration,
                BackupCreated = chkEnableBackup.Checked,
                BackupPath = backupPath,
                Notes = notes
            };

            try
            {
                deploymentHistoryService.AddDeployment(history);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Could not save history: {ex.Message}");
            }

            return history;
        }

        private List<DeploymentHistory> SaveFailedDeploymentHistory(
            SolutionInfo solution,
            List<DeploymentTarget> targets,
            string version,
            string errorMessage,
            DateTime startTime)
        {
            var endTime = DateTime.Now;
            var duration = (int)(endTime - startTime).TotalSeconds;
            var histories = new List<DeploymentHistory>();

            string notes = $"Pre-deployment failure. {ExtractFailureReason(errorMessage)}. " +
                  $"Error occurred before solution export or import could begin.";

            foreach (var target in targets)
            {
                var history = new DeploymentHistory
                {
                    DeploymentDate = startTime,
                    SolutionUniqueName = solution.UniqueName,
                    SolutionFriendlyName = solution.FriendlyName,
                    SourceVersion = solution.Version,
                    TargetVersion = version,
                    SourceEnvironment = ConnectionDetail?.ConnectionName ?? "Unknown",
                    TargetEnvironment = target.Name,
                    IsManaged = solution.IsManaged,
                    DeployedAsManaged = chkDeployAsManaged.Checked && !solution.IsManaged,
                    Status = DeploymentStatus.Failed,
                    DeployedBy = Environment.UserName,
                    ErrorMessage = errorMessage,
                    DurationSeconds = duration,
                    BackupCreated = false,
                    BackupPath = null,
                    Notes = notes
                };

                try
                {
                    deploymentHistoryService.AddDeployment(history);
                    histories.Add(history);
                }
                catch
                {
                    histories.Add(history); // Add even if save fails
                }
            }

            return histories;  // ✅ Return the list of histories
        }

        private string GenerateDeploymentNotes(
            DeploymentStatus status,
            string errorMessage,
            string sourceVersion,
            string targetVersion,
            bool exportAsManaged,
            bool deployingAsManaged)
        {
            var notes = new StringBuilder();

            // Add deployment type info
            if (deployingAsManaged)
            {
                notes.Append("Converted from unmanaged to managed. ");
            }
            else if (exportAsManaged)
            {
                notes.Append("Deployed as managed solution. ");
            }
            else
            {
                notes.Append("Deployed as unmanaged solution. ");
            }

            // Add version info
            notes.Append($"Version: {sourceVersion} → {targetVersion}. ");

            // Add status-specific notes
            switch (status)
            {
                case DeploymentStatus.Completed:
                    notes.Append("Deployment completed successfully.");
                    break;

                case DeploymentStatus.Failed:
                    notes.Append("Deployment failed. ");

                    // Extract concise error reason
                    string reason = ExtractFailureReason(errorMessage);
                    notes.Append($"Reason: {reason}");
                    break;

                case DeploymentStatus.Cancelled:
                    notes.Append("Deployment was cancelled by user.");
                    break;

                default:
                    notes.Append("Deployment status unknown.");
                    break;
            }

            return notes.ToString();
        }

        private string ExtractFailureReason(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                return "Unknown error";
            }

            // Extract first line or first 150 characters
            var lines = errorMessage.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string firstLine = lines.Length > 0 ? lines[0].Trim() : errorMessage;

            // Common error patterns to simplify (using extension method)
            if (firstLine.ContainsIgnoreCase("connection"))
            {
                return "Connection error";
            }
            if (firstLine.ContainsIgnoreCase("timeout"))
            {
                return "Operation timeout";
            }
            if (firstLine.ContainsIgnoreCase("permission") ||
                firstLine.ContainsIgnoreCase("access denied"))
            {
                return "Permission denied";
            }
            if (firstLine.ContainsIgnoreCase("already exists"))
            {
                return "Component already exists";
            }
            if (firstLine.ContainsIgnoreCase("missing") ||
                firstLine.ContainsIgnoreCase("not found"))
            {
                return "Required component missing";
            }
            if (firstLine.ContainsIgnoreCase("dependencies"))
            {
                return "Missing dependencies";
            }
            if (firstLine.ContainsIgnoreCase("version") &&
                firstLine.ContainsIgnoreCase("lower"))
            {
                return "Solution version conflict";
            }
            if (firstLine.ContainsIgnoreCase("underlying connection"))
            {
                return "Connection closed by server";
            }
            if (firstLine.ContainsIgnoreCase("authentication") ||
                firstLine.ContainsIgnoreCase("unauthorized"))
            {
                return "Authentication failed";
            }
            if (firstLine.ContainsIgnoreCase("maximum") &&
                firstLine.ContainsIgnoreCase("exceeded"))
            {
                return "Resource limit exceeded";
            }

            // Return truncated error message
            if (firstLine.Length > 150)
            {
                return firstLine.Substring(0, 147) + "...";
            }

            return firstLine;
        }

        #endregion

        #region==>Email Notification
        private string GetValidatedEmailAddress()
        {
            if (!chkEmailNotification.Checked)
            {
                return null;
            }

            string email = txtNotificationEmail.Text?.Trim();

            // Check if email is empty or placeholder
            if (string.IsNullOrWhiteSpace(email) ||
                email == "your.email@example.com" ||
                txtNotificationEmail.ForeColor == Color.Gray)
            {
                // Prompt for email
                var inputForm = new Form
                {
                    Text = "Email Notification",
                    Size = new Size(450, 220),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MinimizeBox = false,
                    MaximizeBox = false
                };

                var lblPrompt = new Label
                {
                    Text = "📧 Please enter your email address for deployment notifications:",
                    Location = new Point(20, 20),
                    Size = new Size(400, 40),
                    Font = new Font("Segoe UI", 9F)
                };

                var txtEmail = new TextBox
                {
                    Location = new Point(20, 65),
                    Size = new Size(400, 25),
                    Font = new Font("Segoe UI", 9F)
                };

                var chkRemember = new CheckBox
                {
                    Text = "Remember this email for future deployments",
                    Location = new Point(20, 100),
                    Size = new Size(400, 20),
                    Font = new Font("Segoe UI", 9F),
                    Checked = true
                };

                var btnOk = new Button
                {
                    Text = "✅ OK",
                    Location = new Point(245, 140),
                    Size = new Size(85, 30),
                    DialogResult = DialogResult.OK,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var btnCancel = new Button
                {
                    Text = "❌ Cancel",
                    Location = new Point(335, 140),
                    Size = new Size(85, 30),
                    DialogResult = DialogResult.Cancel,
                    Font = new Font("Segoe UI", 9F)
                };

                inputForm.Controls.AddRange(new Control[] { lblPrompt, txtEmail, chkRemember, btnOk, btnCancel });
                inputForm.AcceptButton = btnOk;
                inputForm.CancelButton = btnCancel;

                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    email = txtEmail.Text?.Trim();

                    if (!string.IsNullOrWhiteSpace(email) && IsValidEmail(email))
                    {
                        // Save the email back to the textbox
                        txtNotificationEmail.Text = email;
                        txtNotificationEmail.ForeColor = Color.Black;

                        // Save if user wants to remember
                        if (chkRemember.Checked)
                        {
                            SaveEmailSettings(email, true);
                            lastSavedEmail = email;
                        }

                        return email;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid email address.",
                            "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }

                return null;
            }

            // Validate existing email
            if (IsValidEmail(email))
            {
                // Check if email changed from last saved
                if (!string.IsNullOrWhiteSpace(lastSavedEmail) &&
                    !email.Equals(lastSavedEmail, StringComparison.OrdinalIgnoreCase))
                {
                    var result = MessageBox.Show(
                        $"Email address has changed from:\n{lastSavedEmail}\n\nto:\n{email}\n\n" +
                        "Do you want to save this new email for future deployments?",
                        "Save Email Address?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SaveEmailSettings(email, chkEmailNotification.Checked);
                        lastSavedEmail = email;
                    }
                }
                else if (string.IsNullOrWhiteSpace(lastSavedEmail))
                {
                    // First time using this email - ask to save
                    var result = MessageBox.Show(
                        $"Do you want to save this email address ({email}) for future deployments?",
                        "Save Email Address?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SaveEmailSettings(email, chkEmailNotification.Checked);
                        lastSavedEmail = email;
                    }
                }

                return email;
            }
            else
            {
                MessageBox.Show("The email address in the notification field is invalid. Please correct it.",
                    "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNotificationEmail.Focus();
                return null;
            }
        }

        private void SaveEmailSettings(string email, bool enabled)
        {
            try
            {
                var settings = new EmailSettings
                {
                    NotificationEmail = email,
                    EmailNotificationEnabled = enabled
                };

                EmailSettingsManager.SaveSettings(settings);
                LogInfo($"✅ Saved email notification settings: {email}");
            }
            catch (Exception ex)
            {
                LogWarning($"⚠️ Could not save email settings: {ex.Message}");
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private void LogInfo(string message)
        {   // Log to log form if it exists
            if (logForm != null && !logForm.IsDisposed)
            {
                logForm.AppendLog(message, Color.Lime);
            }

            // Also keep minimal info in main window
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogInfo(message)));
                return;
            }

            //txtLog.SelectionColor = Color.Lime;
            //txtLog.AppendText($"{message}\n");
            //txtLog.ScrollToCaret();
        }

        private void LogWarning(string message)
        {
            if (logForm != null && !logForm.IsDisposed)
            {
                logForm.AppendLog($"⚠️ {message}", Color.Yellow);
            }

            if (InvokeRequired)
            {
                Invoke(new Action(() => LogWarning(message)));
                return;
            }

            //txtLog.SelectionColor = Color.Yellow;
            //txtLog.AppendText($"⚠️ {message}\n");
            //txtLog.SelectionColor = Color.Lime;
            //txtLog.ScrollToCaret();
        }

        private void LogError(string message)
        {
            if (logForm != null && !logForm.IsDisposed)
            {
                logForm.AppendLog($"❌ {message}", Color.Red);
            }

            if (InvokeRequired)
            {
                Invoke(new Action(() => LogError(message)));
                return;
            }

            //txtLog.SelectionColor = Color.Red;
            //txtLog.AppendText($"❌ {message}\n");
            //txtLog.SelectionColor = Color.Lime;
            //txtLog.ScrollToCaret();
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
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

                // ✅ Properly dispose log form on plugin close
                if (logForm != null && !logForm.IsDisposed)
                {
                    logForm.Close();
                    logForm.Dispose();
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

                // ✅ Properly dispose log form on plugin close
                if (logForm != null && !logForm.IsDisposed)
                {
                    logForm.Close();
                    logForm.Dispose();
                }
            }
        }

        private void tsbDeploymentLogs_Click(object sender, EventArgs e)
        {
            // Ensure log form exists
            if (logForm == null || logForm.IsDisposed)
            {
                InitializeLogForm();
            }

            if (logForm.Visible)
            {
                // Hide the form (logs are preserved)
                logForm.Hide();
                tsbDeploymentLogs.Text = "📋 Deployment Logs";
            }
            else
            {
                // Show the form with preserved logs
                logForm.Show(this);
                tsbDeploymentLogs.Text = "❌ Close Logs";
            }
        }

        private void chkDeployAsManaged_CheckedChanged(object sender, EventArgs e)
        {
            // Only enable for unmanaged solutions
            if (chkDeployAsManaged.Checked)
            {
                // Show warning
                var selectedUnmanaged = GetSelectedSolutions().Where(s => !s.IsManaged).ToList();
                if (selectedUnmanaged.Count > 0)
                {
                    lblProgress.Text = "⚠️ Unmanaged solutions will be converted to managed during deployment";
                    lblProgress.ForeColor = Color.Orange;
                }
            }
            else
            {
                lblProgress.Text = "📊 Deployment Progress: 0%";
                lblProgress.ForeColor = Color.Black;
            }
        }

        private void tsbHistory_Click(object sender, EventArgs e)
        {
            try
            {
                var historyForm = new DeploymentHistoryForm();
                historyForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening deployment history:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "This will clear all loaded solutions and target environments.\n\n" +
                "Are you sure you want to refresh?",
                "Confirm Refresh",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                RefreshAll();
            }
        }

        private void RefreshAll()
        {
            try
            {
                // Clear solutions
                chkUnmanagedSolutions.Items.Clear();
                chkManagedSolutions.Items.Clear();
                unmanagedSolutions.Clear();
                managedSolutions.Clear();

                // Clear target environments
                chkTargetEnvironments.Items.Clear();
                foreach (var target in targetEnvironments)
                {
                    try
                    {
                        target.ServiceClient?.Dispose();
                    }
                    catch { }
                }
                targetEnvironments.Clear();

                // Reset progress
                progressBar.Value = 0;
                lblProgress.Text = "📊 Deployment Progress: 0%";
                lblProgress.ForeColor = Color.Black;

                // Enable buttons
                btnLoadSolutions.Enabled = true;
                btnDeploy.Enabled = true;

                ShowInfoNotification("✅ Refreshed successfully! Ready for new deployment.", null);

                MessageBox.Show("All data cleared. Ready for new deployment setup.",
                    "Refresh Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during refresh: {ex.Message}",
                    "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ==>Advanced Features
        private void tsbAbout_Click(object sender, EventArgs e)
        {
            try
            {
                var aboutForm = new AboutForm();
                aboutForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening About dialog: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbFeedback_Click(object sender, EventArgs e)
        {
            try
            {
                var feedbackForm = new FeedbackForm();
                feedbackForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Feedback form: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbHelp_Click(object sender, EventArgs e)
        {
            try
            {
                ShowHelp();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening help: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowHelp()
        {
            var helpText = @"
                ╔══════════════════════════════════════════════════════════╗
                ║     SUJAN SOLUTION DEPLOYER - QUICK START GUIDE         ║
                ╚══════════════════════════════════════════════════════════╝

                📋 OVERVIEW
                This tool automates solution deployment across Dynamics 365 / 
                Power Platform environments with version management, automated 
                backups, and deployment history tracking.

                🚀 GETTING STARTED

                1️⃣ LOAD SOLUTIONS
                   • Connect to your DEV environment
                   • Click '🔄 Load Solutions'
                   • Select solutions to deploy

                2️⃣ ADD TARGET ENVIRONMENTS
                   • Click '➕ Add Environment'
                   • Select UAT/PROD environments
                   • Multiple targets supported

                3️⃣ CONFIGURE OPTIONS
                   • Set backup location
                   • Choose deployment type (Update/Upgrade)
                   • Enable/disable workflow publishing
                   • Select 'Deploy as Managed' if needed

                4️⃣ VERSION MANAGEMENT
                   • Set version increments (Major/Minor/Build/Revision)
                   • Manual version entry supported
                   • Auto-update source versions

                5️⃣ DEPLOY
                   • Click '🚀 START DEPLOYMENT'
                   • Monitor progress in log window
                   • View detailed results

                📊 FEATURES

                ✅ Auto Backup - Solutions backed up before deployment
                ✅ Version Control - Increment versions automatically
                ✅ Multi-Target - Deploy to multiple environments
                ✅ History Tracking - Full deployment audit trail
                ✅ Managed Conversion - Convert unmanaged to managed
                ✅ Progress Monitoring - Real-time deployment status

                🔧 DEPLOYMENT OPTIONS

                - Update: Upgrades existing or installs if new
                - Upgrade: Forces new version, stages for upgrade
                - Publish Workflows: Auto-publish after import
                - Overwrite Customizations: Replaces unmanaged changes
                - Deploy as Managed: Converts unmanaged to managed

                📜 DEPLOYMENT HISTORY

                - View all past deployments
                - Filter by environment or solution
                - Export to CSV for reporting
                - Track success/failure rates

                💡 TIPS

                - Always backup before deployment
                - Test in UAT before PROD
                - Review version changes carefully
                - Monitor import progress
                - Check deployment history for issues

                ⚠️ IMPORTANT NOTES

                - Managed solutions cannot be uninstalled easily
                - Version downgrades may cause issues
                - Always test in non-production first
                - Keep backups of critical solutions
                - Review overwrite options carefully

                📞 SUPPORT

                - Feedback: Use '💬 Feedback' button
                - Issues: Report via GitHub
                - Updates: Check for latest version

                ═══════════════════════════════════════════════════════════

                For detailed documentation, visit the GitHub repository.
                ";

            var helpForm = new Form
            {
                Text = "Help - Sujan Solution Deployer",
                Size = new Size(700, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MinimizeBox = false,
                MaximizeBox = true
            };

            var txtHelp = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BackColor = Color.White,
                Text = helpText,
                WordWrap = true
            };

            var btnClose = new Button
            {
                Text = "❌ Close",
                Dock = DockStyle.Bottom,
                Height = 40,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, args) => helpForm.Close();

            helpForm.Controls.Add(txtHelp);
            helpForm.Controls.Add(btnClose);
            helpForm.ShowDialog(this);
        }

        private void chkEmailNotification_CheckedChanged(object sender, EventArgs e)
        {
            txtNotificationEmail.Enabled = chkEmailNotification.Checked;

            if (chkEmailNotification.Checked)
            {
                txtNotificationEmail.Focus();
            }
            else
            {
                // If user unchecks, update saved settings
                if (!string.IsNullOrWhiteSpace(lastSavedEmail))
                {
                    SaveEmailSettings(lastSavedEmail, false);
                }
            }
        }

        void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.ForeColor = Color.Gray;
            textBox.Text = placeholder;

            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.ForeColor = Color.Gray;
                    textBox.Text = placeholder;
                }
            };
        }

        private void tsbSmtpConfig_Click(object sender, EventArgs e)
        {
            try
            {
                var smtpForm = new SmtpConfigurationForm();
                var result = smtpForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    LoadSmtpSettings();

                    ShowInfoNotification("SMTP settings configured successfully! Email notifications are now enabled.", null);
                    LogInfo("✅ SMTP configuration updated successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening SMTP configuration:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogError($"Failed to open SMTP configuration: {ex.Message}");
            }
        }

        private void tsbRollbackSolution_Click(object sender, EventArgs e)
        {
            try
            {
                // Get available connections
                var connections = GetAvailableConnections();

                // Also get their active services
                var activeServices = GetActiveServices();

                // Validate connections exist
                if (connections == null || connections.Count == 0)
                {
                    MessageBox.Show(
                        "No connections available for rollback.\n\n" +
                        "Please:\n" +
                        "1. Connect to your DEV environment first, OR\n" +
                        "2. Add target environments, OR\n" +
                        "3. Ensure XrmToolBox has saved connections",
                        "No Connections",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    LogWarning("⚠️ Rollback cancelled - no connections available");
                    return;
                }

                LogInfo($"🔄 Opening Rollback Manager with {connections.Count} available connection(s)...");

                // Open rollback form with both connections and services
                using (var rollbackForm = new RollbackForm(connections, activeServices))
                {
                    var result = rollbackForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        LogInfo("✅ Rollback operation completed successfully!");

                        MessageBox.Show(
                            "Rollback operation completed successfully!\n\n" +
                            "The solution has been restored to the previous version.",
                            "Rollback Complete",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        ShowInfoNotification("Rollback completed successfully!", null);
                    }
                    else
                    {
                        LogInfo("ℹ️ Rollback operation cancelled by user");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"❌ Error opening rollback manager: {ex.Message}");

                MessageBox.Show(
                    $"Error opening Rollback Manager:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ==>Helper Methods
        private Dictionary<Guid, IOrganizationService> GetActiveServices()
        {
            var services = new Dictionary<Guid, IOrganizationService>();

            try
            {
                // Get services from target environments
                foreach (var target in targetEnvironments)
                {
                    if (target.ConnectionDetail != null &&
                        target.ConnectionDetail.ConnectionId.HasValue &&
                        target.ServiceClient != null &&
                        target.ServiceClient.IsReady)
                    {
                        IOrganizationService service = null;

                        if (target.ServiceClient.OrganizationWebProxyClient != null)
                        {
                            service = target.ServiceClient.OrganizationWebProxyClient;
                            LogInfo($"✅ Captured active service for: {target.ConnectionDetail.ConnectionName} (WebProxy)");
                        }
                        else if (target.ServiceClient.OrganizationServiceProxy != null)
                        {
                            service = target.ServiceClient.OrganizationServiceProxy;
                            LogInfo($"✅ Captured active service for: {target.ConnectionDetail.ConnectionName} (ServiceProxy)");
                        }

                        if (service != null)
                        {
                            services[target.ConnectionDetail.ConnectionId.Value] = service;
                        }
                    }
                }

                // Get service from current connection
                if (ConnectionDetail != null &&
                    ConnectionDetail.ConnectionId.HasValue &&
                    Service != null)
                {
                    if (!services.ContainsKey(ConnectionDetail.ConnectionId.Value))
                    {
                        services[ConnectionDetail.ConnectionId.Value] = Service;
                        LogInfo($"✅ Captured current connection service: {ConnectionDetail.ConnectionName}");
                    }
                }

                LogInfo($"📋 Total active services captured: {services.Count}");
            }
            catch (Exception ex)
            {
                LogError($"❌ Error capturing active services: {ex.Message}");
            }

            return services;
        }
        private List<ConnectionDetail> GetAvailableConnections()
        {
            var connections = new List<ConnectionDetail>();

            try
            {
                // Priority 1: Add target environment connections (these already have active ServiceClients)
                foreach (var target in targetEnvironments)
                {
                    if (target.ConnectionDetail != null)
                    {
                        // Ensure ServiceClient is attached
                        if (target.ServiceClient != null && target.ServiceClient.IsReady)
                        {
                            target.ConnectionDetail.ServiceClient = target.ServiceClient;
                        }

                        LogInfo($"✅ Adding target connection: {target.ConnectionDetail.ConnectionName}");
                        connections.Add(target.ConnectionDetail);
                    }
                }

                // Priority 2: Add current source connection (only if not already added)
                if (ConnectionDetail != null)
                {
                    if (!connections.Any(c => c.ConnectionId == ConnectionDetail.ConnectionId))
                    {
                        LogInfo($"✅ Adding current connection: {ConnectionDetail.ConnectionName}");
                        connections.Add(ConnectionDetail);
                    }
                }

                // Priority 3: Load from XrmToolBox only if we have no connections
                // Filter out SDK Login Control connections
                if (connections.Count == 0)
                {
                    LogWarning("⚠️ No active connections found, loading from XrmToolBox...");

                    var connectionService = new ConnectionService();
                    var allConnections = connectionService.GetAllConnections();

                    if (allConnections != null && allConnections.Count > 0)
                    {
                        // Filter out SDK Login Control connections
                        var usableConnections = allConnections.Where(c =>
                        {
                            try
                            {
                                var useSdkLoginControl = c.GetType()
                                    .GetProperty("UseSdkLoginControl")?
                                    .GetValue(c);

                                return useSdkLoginControl == null || !(bool)useSdkLoginControl;
                            }
                            catch
                            {
                                return true; // Include if we can't determine
                            }
                        }).ToList();

                        connections.AddRange(usableConnections);
                        LogInfo($"✅ Loaded {usableConnections.Count} connections from XrmToolBox");

                        if (usableConnections.Count < allConnections.Count)
                        {
                            LogWarning($"⚠️ Filtered out {allConnections.Count - usableConnections.Count} SDK Login Control connection(s)");
                        }
                    }
                    else
                    {
                        LogWarning("⚠️ No connections available in XrmToolBox");
                    }
                }

                if (connections.Count > 0)
                {
                    LogInfo($"📋 Total available connections for rollback: {connections.Count}");
                }
                else
                {
                    LogError("❌ No connections available! Please connect to environments first.");
                }
            }
            catch (Exception ex)
            {
                LogError($"❌ Error getting available connections: {ex.Message}");

                MessageBox.Show(
                    $"Error loading connections for rollback:\n\n{ex.Message}\n\n" +
                    "Please ensure you have at least one active connection.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                connections = new List<ConnectionDetail>();
            }

            return connections;
        }

        #endregion

        #region ==>System methods
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
        #endregion
    }
}