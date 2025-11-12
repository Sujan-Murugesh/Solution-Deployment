using Sujan_Solution_Deployer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sujan_Solution_Deployer.Services
{
    public class EmailNotificationService
    {
        // SMTP Configuration
        private static class SmtpConfig
        {
            public static string SmtpHost { get; set; } = "smtp.gmail.com";
            public static int SmtpPort { get; set; } = 587;
            public static bool EnableSsl { get; set; } = true;
            public static string SenderEmail { get; set; } = "";
            public static string SenderPassword { get; set; } = "";
            public static string SenderName { get; set; } = "Sujan Solution Deployer";
        }

        /// <summary>
        /// Configure SMTP settings for sending emails
        /// </summary>
        public static void ConfigureSmtp(string smtpHost, int smtpPort, bool enableSsl,
            string senderEmail, string senderPassword, string senderName = "Sujan Solution Deployer")
        {
            SmtpConfig.SmtpHost = smtpHost;
            SmtpConfig.SmtpPort = smtpPort;
            SmtpConfig.EnableSsl = enableSsl;
            SmtpConfig.SenderEmail = senderEmail;
            SmtpConfig.SenderPassword = senderPassword;
            SmtpConfig.SenderName = senderName;
        }

        /// <summary>
        /// Check if SMTP is configured
        /// </summary>
        public static bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(SmtpConfig.SenderEmail) &&
                   !string.IsNullOrWhiteSpace(SmtpConfig.SenderPassword);
        }

        /// <summary>
        /// Send deployment notification email directly via SMTP
        /// </summary>
        public static void SendDeploymentNotification(
            string recipientEmail,
            string environmentName,
            DeploymentHistory deployment,
            bool success)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                return;

            var subject = success
                ? $"✅ Deployment Successful - {deployment.SolutionFriendlyName} to {environmentName}"
                : $"❌ Deployment Failed - {deployment.SolutionFriendlyName} to {environmentName}";

            var body = BuildEmailBody(deployment, success);

            try
            {
                SendEmailSync(recipientEmail, subject, body);  // ✅ Use synchronous method
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email notification error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Send batch deployment summary email directly via SMTP
        /// </summary>
        public static void SendBatchDeploymentSummary(
            string recipientEmail,
            int totalDeployments,
            int successCount,
            int failureCount,
            DateTime startTime,
            DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                return;

            var subject = $"📊 Deployment Summary - {successCount}/{totalDeployments} Successful";
            var duration = endTime - startTime;

            var body = BuildBatchSummaryBody(totalDeployments, successCount, failureCount,
                                            startTime, endTime, duration);

            try
            {
                SendEmailSync(recipientEmail, subject, body);  // ✅ Use synchronous method
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email notification error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Send email synchronously using SMTP - DOES NOT BLOCK UI
        /// </summary>
        private static void SendEmailSync(string toEmail, string subject, string body)
        {
            // Validate SMTP configuration
            if (string.IsNullOrWhiteSpace(SmtpConfig.SenderEmail))
            {
                throw new InvalidOperationException("SMTP sender email is not configured. Please configure SMTP settings first.");
            }

            if (string.IsNullOrWhiteSpace(SmtpConfig.SenderPassword))
            {
                throw new InvalidOperationException("SMTP sender password is not configured. Please configure SMTP settings first.");
            }

            using (var client = new SmtpClient(SmtpConfig.SmtpHost, SmtpConfig.SmtpPort))
            {
                client.EnableSsl = SmtpConfig.EnableSsl;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpConfig.SenderEmail, SmtpConfig.SenderPassword);
                client.Timeout = 30000; // 30 seconds

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(SmtpConfig.SenderEmail, SmtpConfig.SenderName);
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;
                    message.Priority = MailPriority.Normal;

                    try
                    {
                        client.Send(message);  // ✅ Synchronous send
                    }
                    catch (SmtpException smtpEx)
                    {
                        var errorMessage = GetDetailedSmtpError(smtpEx);
                        throw new Exception(errorMessage, smtpEx);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to send email: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Get detailed SMTP error message
        /// </summary>
        private static string GetDetailedSmtpError(SmtpException ex)
        {
            var message = "Failed to send email.\n\n";

            switch (ex.StatusCode)
            {
                case SmtpStatusCode.GeneralFailure:
                    message += "General failure. Check your internet connection and SMTP server.";
                    break;

                case SmtpStatusCode.SyntaxError:
                    message += "Syntax error in email address or SMTP command.";
                    break;

                case SmtpStatusCode.ServiceNotAvailable:
                    message += "SMTP service not available. The server may be down.";
                    break;

                case SmtpStatusCode.MailboxUnavailable:
                    message += "Recipient mailbox unavailable. Check the recipient email address.";
                    break;

                case SmtpStatusCode.ClientNotPermitted:
                    message += "Authentication failed.\n\n" +
                              "Common causes:\n" +
                              "• Gmail/Outlook require App Password, not regular password\n" +
                              "• 2-Factor Authentication must be enabled\n" +
                              "• 'Less secure apps' is disabled (Gmail)\n" +
                              "• Account credentials are incorrect";
                    break;

                case SmtpStatusCode.MustIssueStartTlsFirst:
                    message += "SSL/TLS is required but not enabled. Enable SSL in settings.";
                    break;

                default:
                    message += $"SMTP Error: {ex.StatusCode}\n" +
                              $"Details: {ex.Message}\n\n" +
                              "Troubleshooting:\n" +
                              "• Verify SMTP host and port are correct\n" +
                              "• Check if SSL/TLS is required\n" +
                              "• Use App Password for Gmail/Outlook\n" +
                              "• Ensure firewall allows SMTP";
                    break;
            }

            if (ex.InnerException != null)
            {
                message += $"\n\nInner Error: {ex.InnerException.Message}";
            }

            return message;
        }

        /// <summary>
        /// Build email body for individual deployment
        /// </summary>
        private static string BuildEmailBody(DeploymentHistory deployment, bool success)
        {
            var sb = new StringBuilder();

            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine("      DEPLOYMENT NOTIFICATION");
            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine();
            sb.AppendLine($"Status: {(success ? "✅ SUCCESS" : "❌ FAILED")}");
            sb.AppendLine();
            sb.AppendLine("SOLUTION DETAILS:");
            sb.AppendLine($"  • Name: {deployment.SolutionFriendlyName}");
            sb.AppendLine($"  • Version: {deployment.SourceVersion} → {deployment.TargetVersion}");
            sb.AppendLine($"  • Type: {(deployment.DeployedAsManaged ? "Managed" : "Unmanaged")}");
            sb.AppendLine();
            sb.AppendLine("ENVIRONMENT DETAILS:");
            sb.AppendLine($"  • Source: {deployment.SourceEnvironment}");
            sb.AppendLine($"  • Target: {deployment.TargetEnvironment}");
            sb.AppendLine();
            sb.AppendLine("DEPLOYMENT DETAILS:");
            sb.AppendLine($"  • Date: {deployment.DeploymentDate:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"  • Duration: {deployment.DurationDisplay}");
            sb.AppendLine($"  • Deployed By: {deployment.DeployedBy}");
            sb.AppendLine();

            if (!success && !string.IsNullOrEmpty(deployment.ErrorMessage))
            {
                sb.AppendLine("ERROR DETAILS:");
                sb.AppendLine($"  {deployment.ErrorMessage}");
                sb.AppendLine();
            }

            if (deployment.BackupCreated && !string.IsNullOrEmpty(deployment.BackupPath))
            {
                sb.AppendLine("BACKUP INFORMATION:");
                sb.AppendLine($"  • Backup Created: Yes");
                sb.AppendLine($"  • Backup Location: {deployment.BackupPath}");
                sb.AppendLine();
            }

            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine("This is an automated notification from:");
            sb.AppendLine("Sujan Solution Deployer for Dynamics 365");
            sb.AppendLine("═══════════════════════════════════════════════════");

            return sb.ToString();
        }

        /// <summary>
        /// Build email body for batch deployment summary
        /// </summary>
        private static string BuildBatchSummaryBody(
            int totalDeployments,
            int successCount,
            int failureCount,
            DateTime startTime,
            DateTime endTime,
            TimeSpan duration)
        {
            var sb = new StringBuilder();

            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine("      BATCH DEPLOYMENT SUMMARY");
            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine();
            sb.AppendLine("DEPLOYMENT STATISTICS:");
            sb.AppendLine($"  • Total Deployments: {totalDeployments}");
            sb.AppendLine($"  • ✅ Successful: {successCount}");
            sb.AppendLine($"  • ❌ Failed: {failureCount}");
            sb.AppendLine($"  • Success Rate: {(totalDeployments > 0 ? (successCount * 100.0 / totalDeployments).ToString("F1") : "0")}%");
            sb.AppendLine();
            sb.AppendLine("TIME INFORMATION:");
            sb.AppendLine($"  • Start Time: {startTime:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"  • End Time: {endTime:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"  • Total Duration: {duration.Hours}h {duration.Minutes}m {duration.Seconds}s");
            sb.AppendLine();

            if (failureCount > 0)
            {
                sb.AppendLine("⚠️ ATTENTION REQUIRED:");
                sb.AppendLine($"  Some deployments failed. Please review the deployment");
                sb.AppendLine($"  logs and history for detailed error information.");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine("✅ ALL DEPLOYMENTS COMPLETED SUCCESSFULLY!");
                sb.AppendLine();
            }

            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine("For detailed deployment information, please check:");
            sb.AppendLine("  • Deployment Logs in the application");
            sb.AppendLine("  • Deployment History for individual results");
            sb.AppendLine();
            sb.AppendLine("This is an automated notification from:");
            sb.AppendLine("Sujan Solution Deployer for Dynamics 365");
            sb.AppendLine("═══════════════════════════════════════════════════");

            return sb.ToString();
        }

        /// <summary>
        /// Test SMTP configuration by sending a test email
        /// </summary>
        public static void SendTestEmail(string recipientEmail)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                throw new ArgumentException("Recipient email is required");

            var subject = "Test Email - Sujan Solution Deployer";
            var body = @"
                        ═══════════════════════════════════════════════════
                              SMTP CONFIGURATION TEST
                        ═══════════════════════════════════════════════════

                        This is a test email from Sujan Solution Deployer.

                        If you received this email, your SMTP configuration 
                        is working correctly!

                        SMTP Settings:
                          • Host: " + SmtpConfig.SmtpHost + @"
                          • Port: " + SmtpConfig.SmtpPort + @"
                          • SSL: " + (SmtpConfig.EnableSsl ? "Enabled" : "Disabled") + @"
                          • Sender: " + SmtpConfig.SenderEmail + @"

                        Test Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"

                        ═══════════════════════════════════════════════════
                        Sujan Solution Deployer for Dynamics 365
                        ═══════════════════════════════════════════════════";

            try
            {
                SendEmailSync(recipientEmail, subject, body);  // ✅ Use synchronous method
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send test email: {ex.Message}", ex);
            }
        }
    }
}
