using System;
using System.Drawing;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;
using Panel = System.Windows.Forms.Panel;

namespace Sujan_Solution_Deployer.Forms
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            PopulateTabs();
        }

        private void PopulateTabs()
        {
            tabControl.TabPages.Clear();

            tabControl.TabPages.Add(CreateGettingStartedTab());
            tabControl.TabPages.Add(CreateFeaturesTab());
            tabControl.TabPages.Add(CreateEmailNotificationsTab());
            tabControl.TabPages.Add(CreateRollbackTab());
            tabControl.TabPages.Add(CreateTipsTab());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region Tabs creation

        private TabPage CreateGettingStartedTab()
        {
            var tab = new TabPage("🚀 Getting Started");
            var panel = CreateScrollablePanel();

            AddSectionHeader(panel, "📋 QUICK START GUIDE", Color.FromArgb(41, 128, 185));

            AddInfoBox(panel, "Welcome!",
                "Sujan Solution Deployer automates Dynamics 365 / Power Platform solution deployment with " +
                "version control, automated backups, email notifications, and complete deployment history tracking.",
                Color.FromArgb(52, 152, 219));

            AddStepSection(panel, "🔗", "CONNECT & LOAD SOLUTIONS", new[]
            {
                "• Connect to your SOURCE (DEV) environment",
                "• Click '🔄 Load Solutions' button",
                "• Select one or more solutions from the list",
                "• Solutions will appear in the main grid"
            });

            AddStepSection(panel, "🌍", "ADD TARGET ENVIRONMENTS", new[]
            {
                "• Click '➕ Add Environment' button",
                "• Select UAT, PROD, or other target environments",
                "• Add multiple targets for simultaneous deployment",
                "• Each target appears in the environment list"
            });

            AddStepSection(panel, "📋", "CONFIGURE DEPLOYMENT OPTIONS", new[]
            {
                "📁 Backup Location: Set where backups are saved",
                "🔄 Deployment Type: Choose Update or Upgrade",
                "⚙️ Publish Workflows: Auto-publish after import",
                "🔒 Deploy as Managed: Convert to managed solution",
                "📝 Overwrite Customizations: Replace existing changes"
            });

            AddStepSection(panel, "🔢", "VERSION MANAGEMENT", new[]
            {
                "• Click version numbers to increment (Major.Minor.Build.Revision)",
                "• Manual version entry supported",
                "• Enable 'Update Source Version' to save changes",
                "• Version history tracked automatically"
            });

            AddStepSection(panel, "📊", "DEPLOY & MONITOR", new[]
            {
                "• Click '🚀 START DEPLOYMENT' button",
                "• Monitor real-time progress in log window",
                "• View deployment status for each environment",
                "• Receive email notifications (if configured)",
                "• Check deployment history for complete audit trail"
            });

            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateFeaturesTab()
        {
            var tab = new TabPage("✨ Features");
            var panel = CreateScrollablePanel();

            AddSectionHeader(panel, "🎯 CORE FEATURES", Color.FromArgb(155, 89, 182));

            AddFeatureItem(panel, "🔄", "AUTOMATED DEPLOYMENT",
                "Deploy solutions to multiple environments simultaneously with real-time progress tracking " +
                "and detailed logging of every operation.");

            AddFeatureItem(panel, "💾", "AUTO BACKUP & VERSIONING",
                "Automatic solution backups before deployment with version increment support. " +
                "All backups are timestamped and stored safely.");

            AddFeatureItem(panel, "📧", "EMAIL NOTIFICATIONS",
                "Receive instant email alerts for deployment success, failures, and progress updates. " +
                "Configure SMTP settings for your organization.");

            AddFeatureItem(panel, "↩️", "ROLLBACK CAPABILITY",
                "Quick rollback to previous solution versions using automated backups. " +
                "One-click restore for failed deployments.");

            AddFeatureItem(panel, "📊", "DEPLOYMENT HISTORY",
                "Complete audit trail of all deployments with filtering, searching, and CSV export. " +
                "Track success rates and identify patterns.");

            AddFeatureItem(panel, "🔒", "MANAGED CONVERSION",
                "Convert unmanaged solutions to managed during deployment. " +
                "Protect customizations in production environments.");

            AddFeatureItem(panel, "🎯", "MULTI-TARGET DEPLOYMENT",
                "Deploy to UAT, PROD, and other environments in parallel. " +
                "Individual status tracking for each target.");

            AddFeatureItem(panel, "⚙️", "FLEXIBLE OPTIONS",
                "Choose between Update/Upgrade deployment modes. " +
                "Control workflow publishing and customization overwrites.");

            AddSectionHeader(panel, "🔧 DEPLOYMENT OPTIONS EXPLAINED", Color.FromArgb(230, 126, 34));

            AddOptionExplanation(panel, "Update Mode",
                "Upgrades existing solutions or installs if new. Recommended for most deployments.");

            AddOptionExplanation(panel, "Upgrade Mode",
                "Forces installation of new version and stages for upgrade. Use for major version changes.");

            AddOptionExplanation(panel, "Publish Workflows",
                "Automatically publishes all workflows, flows, and customizations after import.");

            AddOptionExplanation(panel, "Overwrite Customizations",
                "Replaces existing unmanaged customizations with imported ones. Use with caution!");

            AddOptionExplanation(panel, "Deploy as Managed",
                "Converts unmanaged solutions to managed during deployment. Cannot be easily removed.");

            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateEmailNotificationsTab()
        {
            var tab = new TabPage("📧 Email Notifications");
            var panel = CreateScrollablePanel();

            AddSectionHeader(panel, "📬 EMAIL NOTIFICATION SETUP", Color.FromArgb(46, 204, 113));

            AddInfoBox(panel, "Stay Informed!",
                "Configure email notifications to receive instant alerts about deployment status. " +
                "Get notified of successes, failures, and important events.",
                Color.FromArgb(39, 174, 96));

            AddStepSection(panel, "⚙️", "CONFIGURE SMTP SETTINGS", new[]
            {
                "1. Click '📧 Configure Email' button in the toolbar",
                "2. Enter your SMTP server details:",
                "\n",
                "   • SMTP Server: smtp.gmail.com (or your mail server)",
                "   • Port: 587 (TLS) or 465 (SSL)",
                "   • Username: your-email@domain.com",
                "   • Password: your app password or SMTP password \n",
                "3. Select encryption type (TLS recommended)",
                "4. Click 'Test Connection' to verify settings",
                "5. Save configuration"
            });

            AddSectionHeader(panel, "📮 NOTIFICATION TYPES", Color.FromArgb(52, 152, 219));

            AddNotificationItem(panel, "✅", "Deployment Success",
                "Sent when all solutions deploy successfully to all target environments. " +
                "Includes deployment summary and statistics.");

            AddNotificationItem(panel, "❌", "Deployment Failure",
                "Immediate alert when deployment fails. Includes error details and " +
                "affected environments for quick troubleshooting.");

            AddNotificationItem(panel, "⚠️", "Partial Success",
                "Alert when some environments succeed but others fail. " +
                "Detailed breakdown of results per environment.");

            AddNotificationItem(panel, "📊", "Deployment Summary",
                "Comprehensive report after deployment completion. " +
                "Lists all solutions, targets, durations, and outcomes.");

            AddSectionHeader(panel, "🔐 GMAIL USERS - IMPORTANT!", Color.FromArgb(231, 76, 60));

            AddWarningBox(panel, "Gmail Security Settings",
                "Gmail requires an App Password (not your regular password):\n\n" +
                "1. Enable 2-Factor Authentication on your Google account\n" +
                "2. Go to: Google Account → Security → App passwords\n" +
                "3. Generate a new app password for 'Mail'\n" +
                "4. Use this 16-character password in SMTP settings\n\n" +
                "Alternative: Use 'Less secure app access' (not recommended)");

            AddSectionHeader(panel, "🏢 OFFICE 365 / OUTLOOK", Color.FromArgb(41, 128, 185));

            AddInfoBox(panel, "Office 365 Settings",
                "SMTP Server: smtp.office365.com\n" +
                "Port: 587\n" +
                "Encryption: TLS\n" +
                "Authentication: Use your Office 365 credentials",
                Color.FromArgb(52, 152, 219));

            AddStepSection(panel, "🧪", "TEST YOUR CONFIGURATION", new[]
            {
                "• Always use 'Test Connection' before saving",
                "• Check spam/junk folders for test emails",
                "• Verify firewall allows SMTP connections",
                "• Ensure antivirus doesn't block email",
                "• Check SMTP server allows external connections"
            });

            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateRollbackTab()
        {
            var tab = new TabPage("↩️ Rollback & Recovery");
            var panel = CreateScrollablePanel();

            AddSectionHeader(panel, "🔄 ROLLBACK FEATURES", Color.FromArgb(230, 126, 34));

            AddInfoBox(panel, "Safe Deployments!",
                "Every deployment automatically creates backups. If something goes wrong, " +
                "quickly rollback to a previous working version with just a few clicks.",
                Color.FromArgb(230, 126, 34));

            AddStepSection(panel, "💾", "AUTOMATIC BACKUPS", new[]
            {
                "• Solutions backed up BEFORE every deployment",
                "• Backups stored in configured backup location",
                "• Timestamped folders for easy identification",
                "• Original version numbers preserved",
                "• Both managed and unmanaged solutions supported"
            });

            AddSectionHeader(panel, "⏮️ HOW TO ROLLBACK", Color.FromArgb(231, 76, 60));

            AddStepSection(panel, "🛠️", "ACCESS ROLLBACK FEATURE", new[]
            {
                "• Click '↩️ Rollback' button in toolbar",
                "• Or navigate to Tools → Rollback Solution",
                "• Rollback window opens with available backups"
            });

            AddStepSection(panel, "📅", "SELECT BACKUP VERSION", new[]
            {
                "• View list of all solution backups",
                "• Backups sorted by date (newest first)",
                "• See solution name, version, and backup date",
                "• Preview backup details before rollback"
            });

            AddStepSection(panel, "🎯", "CHOOSE TARGET ENVIRONMENT", new[]
            {
                "• Connect to target environment for rollback",
                "• Select environment from connection picker",
                "• Verify you're rolling back to correct environment",
                "• UAT and PROD environments supported"
            });

            AddStepSection(panel, "⚡", "EXECUTE ROLLBACK", new[]
            {
                "• Click 'Start Rollback' button",
                "• Backup solution imports to target environment",
                "• Real-time progress shown in log window",
                "• Success/failure notification displayed",
                "• Email notification sent (if configured)"
            });

            AddSectionHeader(panel, "⚠️ ROLLBACK CONSIDERATIONS", Color.FromArgb(192, 57, 43));

            AddWarningBox(panel, "Important Notes",
                "• Rollback overwrites current solution version\n" +
                "• Data loss may occur if schema changed\n" +
                "• Test rollback in UAT before PROD\n" +
                "• Managed solutions: consider dependencies\n" +
                "• Backup current version before rollback\n" +
                "• Review customizations after rollback\n" +
                "• Workflows may need republishing");

            AddSectionHeader(panel, "🛡️ BACKUP BEST PRACTICES", Color.FromArgb(52, 73, 94));

            AddStepSection(panel, "✅", "RECOMMENDED PRACTICES", new[]
            {
                "• Keep backups for at least 30 days",
                "• Store backups on separate drive/network location",
                "• Document reason for each deployment",
                "• Test backup restore periodically",
                "• Maintain backup location with adequate space",
                "• Label critical backups clearly",
                "• Create manual backups before major changes"
            });

            AddFeatureItem(panel, "📂", "BACKUP LOCATION MANAGEMENT",
                "Configure backup path in settings. Default: Documents\\SolutionBackups. " +
                "Use network drives for team access and better security.");

            AddFeatureItem(panel, "🔍", "BACKUP VERIFICATION",
                "Always verify backup integrity after creation. " +
                "Check file size and ensure .zip files are not corrupted.");

            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateTipsTab()
        {
            var tab = new TabPage("💡 Tips & Best Practices");
            var panel = CreateScrollablePanel();

            AddSectionHeader(panel, "🎯 DEPLOYMENT BEST PRACTICES", Color.FromArgb(142, 68, 173));

            AddTipItem(panel, "🧪", "Always Test First",
                "Deploy to UAT/Test environment before PROD. Validate functionality, " +
                "test workflows, and verify data integrity.");

            AddTipItem(panel, "📝", "Document Changes",
                "Maintain deployment notes. Document what changed, why it changed, " +
                "and any special considerations.");

            AddTipItem(panel, "🕐", "Deploy Off-Hours",
                "Schedule PROD deployments during maintenance windows or off-peak hours " +
                "to minimize user impact.");

            AddTipItem(panel, "📊", "Review History",
                "Check deployment history before new deployments. " +
                "Identify patterns in failures and successes.");

            AddTipItem(panel, "🔐", "Use Managed Solutions",
                "Deploy managed solutions to PROD to protect customizations " +
                "and prevent accidental modifications.");

            AddTipItem(panel, "💾", "Keep Backups",
                "Maintain backups for at least 30 days. Store in secure location " +
                "with proper access controls.");

            AddTipItem(panel, "📧", "Enable Notifications",
                "Configure email notifications to stay informed of deployment status, " +
                "especially for unattended deployments.");

            AddTipItem(panel, "🔄", "Version Consistently",
                "Follow semantic versioning (Major.Minor.Build.Revision). " +
                "Document version changes in release notes.");

            AddSectionHeader(panel, "⚠️ COMMON PITFALLS TO AVOID", Color.FromArgb(231, 76, 60));

            AddWarningItem(panel, "❌ Version Downgrades",
                "Don't deploy older versions over newer ones. Can cause data loss and errors.");

            AddWarningItem(panel, "❌ Skip Testing",
                "Never deploy directly to PROD without UAT testing. Recipe for disaster!");

            AddWarningItem(panel, "❌ Ignore Dependencies",
                "Always deploy dependent solutions first. Check solution dependencies.");

            AddWarningItem(panel, "❌ Overwrite Without Backup",
                "Never overwrite customizations without backup. Always create restore point.");

            AddWarningItem(panel, "❌ Deploy During Peak Hours",
                "Avoid deployments when users are active. Schedule appropriately.");

            AddSectionHeader(panel, "🚀 PRO TIPS", Color.FromArgb(52, 152, 219));

            AddProTipItem(panel, "Use Deployment History",
                "Export history to CSV for reporting to management and stakeholders.");

            AddProTipItem(panel, "Automate Notifications",
                "Set up email notifications to inform team members automatically.");

            AddProTipItem(panel, "Create Deployment Checklist",
                "Maintain a checklist for pre/post deployment tasks and validations.");

            AddProTipItem(panel, "Monitor Import Progress",
                "Watch the log window during deployment for early warning signs.");

            AddProTipItem(panel, "Keep Tool Updated",
                "Check for updates regularly to get latest features and bug fixes.");

            AddSectionHeader(panel, "🆘 TROUBLESHOOTING", Color.FromArgb(44, 62, 80));

            AddTroubleshootItem(panel, "Deployment Fails",
                "• Check connection to target environment\n" +
                "• Verify solution dependencies are met\n" +
                "• Review error message in logs\n" +
                "• Ensure sufficient permissions");

            AddTroubleshootItem(panel, "Email Not Received",
                "• Test SMTP configuration\n" +
                "• Check spam/junk folders\n" +
                "• Verify firewall settings\n" +
                "• Confirm SMTP credentials");

            AddTroubleshootItem(panel, "Rollback Issues",
                "• Ensure backup file exists\n" +
                "• Check backup file integrity\n" +
                "• Verify environment connection\n" +
                "• Review solution dependencies");

            AddSectionHeader(panel, "📞 SUPPORT & FEEDBACK", Color.FromArgb(39, 174, 96));

            AddInfoBox(panel, "Need Help?",
                "💬 Feedback Button: Click toolbar feedback button to send suggestions\n" +
                "🐛 Report Issues: Submit bugs via GitHub repository\n" +
                "📖 Documentation: Visit GitHub for detailed documentation\n" +
                "⭐ Rate & Review: Share your experience with the community",
                Color.FromArgb(46, 204, 113));

            tab.Controls.Add(panel);
            return tab;
        }

        #endregion

        #region Helper UI builders

        private Panel CreateScrollablePanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.White,
                Width = 760
            };
            return panel;
        }

        private void AddSectionHeader(Panel panel, string title, Color color)
        {
            var header = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = color,
                AutoSize = true,
                Padding = new Padding(0, 10, 0, 10),
                Location = new Point(10, GetNextY(panel))
            };
            panel.Controls.Add(header);
        }

        private void AddInfoBox(Panel panel, string title, string message, Color color)
        {
            var box = new Panel
            {
                Width = Math.Max(750, panel.Width - 60),
                AutoSize = true,
                BackColor = Color.FromArgb(240, 248, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(10, GetNextY(panel)),
                Padding = new Padding(15)
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = color,
                AutoSize = true,
                Location = new Point(10, 10)
            };

            var lblMessage = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                MaximumSize = new Size(box.Width - 40, 0),
                Location = new Point(10, 35)
            };

            box.Controls.Add(lblTitle);
            box.Controls.Add(lblMessage);
            box.Height = lblMessage.Bottom + 15;
            panel.Controls.Add(box);
        }

        private void AddWarningBox(Panel panel, string title, string message)
        {
            var box = new Panel
            {
                Width = Math.Max(750, panel.Width - 60),
                AutoSize = true,
                BackColor = Color.FromArgb(255, 243, 224),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(10, GetNextY(panel)),
                Padding = new Padding(15)
            };

            var lblTitle = new Label
            {
                Text = "⚠️ " + title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(230, 126, 34),
                AutoSize = true,
                Location = new Point(10, 10)
            };

            var lblMessage = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                MaximumSize = new Size(box.Width - 40, 0),
                Location = new Point(10, 35)
            };

            box.Controls.Add(lblTitle);
            box.Controls.Add(lblMessage);
            box.Height = lblMessage.Bottom + 15;
            panel.Controls.Add(box);
        }

        private void AddStepSection(Panel panel, string icon, string title, string[] steps)
        {
            var lblHeader = new Label
            {
                Text = $"{icon}   {title}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(10, GetNextY(panel))
            };
            panel.Controls.Add(lblHeader);

            var lblSteps = new Label
            {
                Text = string.Join("\n", steps),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 60, 0),
                Padding = new Padding(20, 5, 0, 10),
                Location = new Point(10, GetNextY(panel))
            };
            panel.Controls.Add(lblSteps);
        }

        private void AddFeatureItem(Panel panel, string icon, string title, string description)
        {
            var lblTitle = new Label
            {
                Text = $"{icon} {title}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(10, GetNextY(panel))
            };
            panel.Controls.Add(lblTitle);

            var lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 60, 0),
                Padding = new Padding(30, 2, 0, 10),
                Location = new Point(10, GetNextY(panel))
            };
            panel.Controls.Add(lblDesc);
        }

        private void AddOptionExplanation(Panel panel, string option, string explanation)
        {
            var lblOption = new Label
            {
                Text = $"▪ {option}:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = true,
                Location = new Point(20, GetNextY(panel))
            };
            panel.Controls.Add(lblOption);

            var lblExplanation = new Label
            {
                Text = explanation,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 80, 0),
                Padding = new Padding(0, 2, 0, 10),
                Location = new Point(40, GetNextY(panel))
            };
            panel.Controls.Add(lblExplanation);
        }

        private void AddNotificationItem(Panel panel, string icon, string title, string description)
        {
            AddFeatureItem(panel, icon, title, description);
        }

        private void AddTipItem(Panel panel, string icon, string title, string description)
        {
            AddFeatureItem(panel, icon, title, description);
        }

        private void AddWarningItem(Panel panel, string title, string description)
        {
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 76, 60),
                AutoSize = true,
                Location = new Point(20, GetNextY(panel))
            };
            panel.Controls.Add(lblTitle);

            var lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 80, 0),
                Padding = new Padding(0, 2, 0, 10),
                Location = new Point(40, GetNextY(panel))
            };
            panel.Controls.Add(lblDesc);
        }

        private void AddProTipItem(Panel panel, string tip, string description)
        {
            var lblTip = new Label
            {
                Text = $"💡 {tip}",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(52, 152, 219),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 60, 0),
                Padding = new Padding(20, 5, 0, 5),
                Location = new Point(10, GetNextY(panel))
            };
            panel.Controls.Add(lblTip);

            var lblTipDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 80, 0),
                Padding = new Padding(0, 2, 0, 10),
                Location = new Point(40, GetNextY(panel))
            };
            panel.Controls.Add(lblTipDesc);
        }

        private void AddTroubleshootItem(Panel panel, string problem, string solution)
        {
            var lblProblem = new Label
            {
                Text = $"❓ {problem}",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, GetNextY(panel))
            };
            panel.Controls.Add(lblProblem);

            var lblSolution = new Label
            {
                Text = solution,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 80, 0),
                Padding = new Padding(0, 2, 0, 10),
                Location = new Point(40, GetNextY(panel))
            };
            panel.Controls.Add(lblSolution);
        }

        private int GetNextY(Panel panel)
        {
            if (panel.Controls.Count == 0)
                return 10;

            var lastControl = panel.Controls[panel.Controls.Count - 1];
            return lastControl.Bottom + 5;
        }

        #endregion
    }
}
