using Sujan_Solution_Deployer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sujan_Solution_Deployer
{
    public partial class SmtpConfigurationForm : Form
    {

        public SmtpConfigurationForm()
        {
            InitializeComponent();

            linkHelp.LinkClicked += (s, e) =>
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://support.google.com/accounts/answer/185833",
                        UseShellExecute = true
                    });
                }
                catch { }
            };

            btnTest.Click += async (s, e) => await BtnTest_Click_Async(s, e);  // ✅ Use async handler
            btnSave.Click += BtnSave_Click;
            btnClearSettings.Click += BtnClearSettings_Click;
            LoadCurrentSettings();
        }

        private void BtnClearSettings_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "⚠️ Are you sure you want to delete all saved SMTP settings?\n\n" +
                "This will:\n" +
                "• Delete the saved configuration file\n" +
                "• Clear all form fields\n" +
                "• Disable email notifications\n\n" +
                "This action cannot be undone!",
                "Clear SMTP Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete settings file
                    SmtpSettingsManager.DeleteSettings();

                    // Clear form fields
                    txtSmtpHost.Text = "smtp.gmail.com";
                    numSmtpPort.Value = 587;
                    chkEnableSsl.Checked = true;
                    txtSenderEmail.Text = "";
                    txtSenderPassword.Text = "";
                    txtSenderName.Text = "Sujan Solution Deployer";
                    txtTestEmail.Text = "";

                    // Hide clear button
                    btnClearSettings.Visible = false;

                    MessageBox.Show(
                        "✅ SMTP settings cleared successfully!\n\n" +
                        "Email notifications are now disabled.\n" +
                        "Configure new settings and click 'Save' to re-enable.",
                        "Settings Cleared",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.Text = "SMTP Configuration";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"❌ Error clearing settings:\n\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// Load SMTP settings from storage and populate form
        private void LoadCurrentSettings()
        {
            try
            {
                var settings = SmtpSettingsManager.LoadSettings();

                txtSmtpHost.Text = settings.SmtpHost;
                numSmtpPort.Value = settings.SmtpPort;
                chkEnableSsl.Checked = settings.EnableSsl;
                txtSenderEmail.Text = settings.SenderEmail;
                txtSenderName.Text = settings.SenderName;
                txtSenderPassword.Text = settings.Password;

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
                }

                // ✅ Update UI based on settings existence
                bool settingsExist = SmtpSettingsManager.SettingsExist();

                if (settingsExist)
                {
                    this.Text = "SMTP Configuration (Settings Loaded ✅)";
                    btnClearSettings.Visible = true;  // Show clear button
                }
                else
                {
                    this.Text = "SMTP Configuration";
                    btnClearSettings.Visible = false;  // Hide clear button
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading SMTP settings: {ex.Message}");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtSmtpHost.Text))
            {
                MessageBox.Show("Please enter SMTP host.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSmtpHost.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSenderEmail.Text))
            {
                MessageBox.Show("Please enter sender email.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenderEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSenderPassword.Text))
            {
                MessageBox.Show("Please enter password or app key.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenderPassword.Focus();
                return;
            }

            // Validate email format
            try
            {
                var addr = new System.Net.Mail.MailAddress(txtSenderEmail.Text);
                if (addr.Address != txtSenderEmail.Text)
                {
                    throw new FormatException();
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid sender email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenderEmail.Focus();
                return;
            }

            try
            {
                // Create settings object
                var settings = new SmtpSettingsManager.SmtpSettings
                {
                    SmtpHost = txtSmtpHost.Text.Trim(),
                    SmtpPort = (int)numSmtpPort.Value,
                    EnableSsl = chkEnableSsl.Checked,
                    SenderEmail = txtSenderEmail.Text.Trim(),
                    SenderName = txtSenderName.Text.Trim(),
                    Password = txtSenderPassword.Text  // Will be encrypted automatically
                };

                // ✅ Save settings to file (with encrypted password)
                SmtpSettingsManager.SaveSettings(settings);

                // Configure SMTP in EmailNotificationService
                EmailNotificationService.ConfigureSmtp(
                    settings.SmtpHost,
                    settings.SmtpPort,
                    settings.EnableSsl,
                    settings.SenderEmail,
                    settings.Password,  // Use plain password for service
                    settings.SenderName
                );

                MessageBox.Show(
                    "✅ SMTP settings saved successfully!\n\n" +
                    "• Email notifications are now enabled\n" +
                    "• Settings will be loaded automatically next time\n" +
                    "• Password is stored securely (encrypted)\n\n" +
                    $"Settings file location:\n{SmtpSettingsManager.GetSettingsFilePath()}",
                    "Settings Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Error saving SMTP settings:\n\n{ex.Message}\n\n" +
                    "Your settings were not saved. Please try again.",
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async Task BtnTest_Click_Async(object sender, EventArgs e)
        {
            // Validate test email
            if (string.IsNullOrWhiteSpace(txtTestEmail.Text))
            {
                MessageBox.Show("Please enter a test email address.", "Test Email Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTestEmail.Focus();
                return;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(txtTestEmail.Text);
                if (addr.Address != txtTestEmail.Text)
                {
                    throw new FormatException();
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid test email address.", "Invalid Email",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTestEmail.Focus();
                return;
            }

            // Disable button and show progress
            btnTest.Enabled = false;
            btnTest.Text = "⏳ Sending...";
            this.Cursor = Cursors.WaitCursor;

            Application.DoEvents();

            try
            {
                // Configure SMTP with current form values (don't save yet)
                EmailNotificationService.ConfigureSmtp(
                    txtSmtpHost.Text.Trim(),
                    (int)numSmtpPort.Value,
                    chkEnableSsl.Checked,
                    txtSenderEmail.Text.Trim(),
                    txtSenderPassword.Text,
                    txtSenderName.Text.Trim()
                );

                // Run email sending on background thread
                await Task.Run(() =>
                {
                    EmailNotificationService.SendTestEmail(txtTestEmail.Text.Trim());
                });

                MessageBox.Show(
                    "✅ Test email sent successfully!\n\n" +
                    $"Check the inbox of {txtTestEmail.Text} to confirm receipt.\n\n" +
                    "If you don't receive the email within a few minutes:\n" +
                    "• Check your spam/junk folder\n" +
                    "• Verify SMTP settings are correct\n" +
                    "• Ensure password/app key is valid\n\n" +
                    "💡 Don't forget to click 'Save' to persist these settings!",
                    "Test Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Test email failed!\n\n" +
                    $"Error: {ex.Message}\n\n" +
                    "Common issues:\n" +
                    "• Incorrect SMTP host or port\n" +
                    "• Invalid email or password\n" +
                    "• Gmail/Outlook: Use App Password, not regular password\n" +
                    "• Firewall blocking SMTP connection\n" +
                    "• Less secure apps disabled (Gmail)",
                    "Test Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnTest.Enabled = true;
                btnTest.Text = "🧪 Test";
                this.Cursor = Cursors.Default;
            }
        }

        //private void controlHelpPdfOpen(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    try
        //    {
        //        string pdfPath = Path.Combine(Application.StartupPath, "Assets", "App-Passwordkey-Generation-Guide.pdf");

        //        if (!File.Exists(pdfPath))
        //        {
        //            MessageBox.Show("Help file not found:\n" + pdfPath, "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        Process.Start(new ProcessStartInfo
        //        {
        //            FileName = pdfPath,
        //            UseShellExecute = true
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Unable to open help PDF.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void btnSmtpDiagnostic_Click(object sender, EventArgs e)
        {
            try
            {
                var emailDiagnosticsForm = new SmtpDiagnosticForm();
                emailDiagnosticsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening SmtpDiagnosticForm form: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
