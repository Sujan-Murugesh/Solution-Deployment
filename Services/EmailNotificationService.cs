using Sujan_Solution_Deployer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public static bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(SmtpConfig.SenderEmail) &&
                   !string.IsNullOrWhiteSpace(SmtpConfig.SenderPassword);
        }

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
                SendEmailSync(recipientEmail, subject, body, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email notification error: {ex.Message}");
                throw;
            }
        }

        public static void SendBatchDeploymentSummary(
            string recipientEmail,
            int totalDeployments,
            int successCount,
            int failureCount,
            DateTime startTime,
            DateTime endTime,
            List<DeploymentHistory> deployments)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                return;

            var subject = $"📊 Deployment Summary - {successCount}/{totalDeployments} Successful";
            var duration = endTime - startTime;

            var body = BuildBatchSummaryBodyHtml(totalDeployments, successCount, failureCount,
                                                startTime, endTime, duration, deployments);

            try
            {
                SendEmailSync(recipientEmail, subject, body, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email notification error: {ex.Message}");
                throw;
            }
        }

        /// Send email synchronously using SMTP
        private static void SendEmailSync(string toEmail, string subject, string body, bool isHtml = false)
        {
            if (string.IsNullOrWhiteSpace(SmtpConfig.SenderEmail))
            {
                throw new InvalidOperationException("SMTP sender email is not configured.");
            }

            if (string.IsNullOrWhiteSpace(SmtpConfig.SenderPassword))
            {
                throw new InvalidOperationException("SMTP sender password is not configured.");
            }

            using (var client = new SmtpClient(SmtpConfig.SmtpHost, SmtpConfig.SmtpPort))
            {
                client.EnableSsl = SmtpConfig.EnableSsl;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpConfig.SenderEmail, SmtpConfig.SenderPassword);
                client.Timeout = 30000;

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(SmtpConfig.SenderEmail, SmtpConfig.SenderName);
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.IsBodyHtml = isHtml;
                    message.Priority = MailPriority.Normal;

                    if (isHtml)
                    {
                        // Create HTML view with embedded image
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

                        // Add logo as embedded resource
                        try
                        {
                            var logo = global::Sujan_Solution_Deployer.Properties.Resources.logo;
                            if (logo != null)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    ms.Position = 0;

                                    LinkedResource logoResource = new LinkedResource(ms, "image/png");
                                    logoResource.ContentId = "companyLogo"; // This ID is used in the HTML
                                    logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                                    htmlView.LinkedResources.Add(logoResource);
                                    message.AlternateViews.Add(htmlView);

                                    client.Send(message);
                                }
                            }
                            else
                            {
                                // No logo available, send without it
                                message.Body = body;
                                client.Send(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error embedding logo: {ex.Message}");
                            // Send without logo if there's an error
                            message.Body = body;
                            client.Send(message);
                        }
                    }
                    else
                    {
                        // Plain text email
                        message.Body = body;

                        try
                        {
                            client.Send(message);
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
        }

        /// Get detailed SMTP error message
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

        #region ==> HTML Email Body Builders 
        /// Build HTML email body for individual deployment
        private static string BuildEmailBody(DeploymentHistory deployment, bool success)
        {
            var statusColor = success ? "#28a745" : "#dc3545";
            var statusIcon = success ? "✅" : "❌";
            var statusText = success ? "SUCCESS" : "FAILED";

            var html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0;}}
                        .status-badge {{ background-color: {statusColor}; color: white; padding: 10px 20px; display: inline-block; margin: 10px 0; font-weight: bold; }}
                        .content {{ background-color: #f8f9fa; padding: 30px; }}
                        .section {{ background-color: white; margin: 20px 0; padding: 20px;border-radius: 8px; }}
                        .section-title {{ color: #667eea; font-weight: bold; font-size: 16px; margin-bottom: 15px; border-bottom: 2px solid #667eea; padding-bottom: 5px; }}
                        .info-row {{ margin: 10px 0; padding: 8px 0; border-bottom: 1px solid #e9ecef; }}
                        .label {{ color: #6c757d; font-weight: 600; }}
                        .value {{ color: #212529; }}
                        .version-flow {{ background-color: #e7f3ff; padding: 10px; text-align: center; font-weight: bold; color: #0066cc; margin: 10px 0; }}
                        .error-box {{ background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 15px 0; }}
                        .footer {{ text-align: center; padding: 20px; color: #6c757d; font-size: 12px; }}
                        .logo {{ max-width: 200px; max-height: 100px; margin: 20px auto; display: block; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🚀 Deployment Notification</h1>
                            <div class='status-badge'>{statusIcon} {statusText}</div>
                        </div>
                        <div class='content'>
                            <div class='section'>
                                <div class='section-title'>📦 Solution Details</div>
                                <div class='info-row'><span class='label'>Solution Name:</span> <strong>{deployment.SolutionFriendlyName}</strong></div>
                                <div class='info-row'><span class='label'>Unique Name:</span> {deployment.SolutionUniqueName}</div>
                                <div class='version-flow'>Version: {deployment.SourceVersion} ➔ {deployment.TargetVersion}</div>
                                <div class='info-row'><span class='label'>Solution Type:</span> {(deployment.DeployedAsManaged ? "Managed" : "Unmanaged")}</div>
                            </div>
                            <div class='section'>
                                <div class='section-title'>🎯 Environment Details</div>
                                <div class='info-row'><span class='label'>Source:</span> <strong>{deployment.SourceEnvironment}</strong></div>
                                <div class='info-row'><span class='label'>Target:</span> <strong>{deployment.TargetEnvironment}</strong></div>
                            </div>
                            <div class='section'>
                                <div class='section-title'>📊 Deployment Information</div>
                                <div class='info-row'><span class='label'>Date:</span> {deployment.DeploymentDate:yyyy-MM-dd HH:mm:ss}</div>
                                <div class='info-row'><span class='label'>Duration:</span> {deployment.DurationDisplay}</div>
                                <div class='info-row'><span class='label'>Deployed By:</span> {deployment.DeployedBy}</div>
                            </div>
                            {(success ? "" : $"<div class='section'><div class='section-title'>❌ Error Details</div><div class='error-box'>{deployment.ErrorMessage}</div></div>")}
                            {(deployment.BackupCreated ? $"<div class='section'><div class='section-title'>💾 Backup Information</div><div class='info-row'><span class='label'>Backup Created:</span> ✅ Yes</div><div class='info-row'><span class='label'>Location:</span> {deployment.BackupPath}</div></div>" : "")}
                        </div>
                        <div class='footer'>
                            <img src='cid:companyLogo' alt='Sujan Solution Deployer Logo' class='logo' />
                            <p><strong>Sujan Solution Deployer</strong></p>
                            <p>Automated Deployment Notification for Dynamics 365</p>
                        </div>
                    </div>
                </body>
                </html>";

            return html;
        }

        private static string BuildBatchSummaryBodyHtml(
            int totalDeployments,
            int successCount,
            int failureCount,
            DateTime startTime,
            DateTime endTime,
            TimeSpan duration,
            List<DeploymentHistory> deployments)
        {
            var successRate = totalDeployments > 0 ? (successCount * 100.0 / totalDeployments).ToString("F1") : "0";
            var statusColor = failureCount == 0 ? "#28a745" : "#ffc107";
            var statusIcon = failureCount == 0 ? "✅" : "⚠️";
            var statusText = failureCount == 0 ? "ALL SUCCESSFUL" : "COMPLETED WITH ISSUES";

            // Build deployment details table
            var deploymentDetailsHtml = "";
            if (deployments != null && deployments.Count > 0)
            {
                deploymentDetailsHtml = "<div class='section'><div class='section-title'>📋 Deployment Details</div><table style='width: 100%; border-collapse: collapse;'><thead><tr style='background-color: #f8f9fa;'><th style='padding: 10px; text-align: left; border-bottom: 2px solid #dee2e6;'>Solution</th><th style='padding: 10px; text-align: left; border-bottom: 2px solid #dee2e6;'>Version</th><th style='padding: 10px; text-align: left; border-bottom: 2px solid #dee2e6;'>Environment</th><th style='padding: 10px; text-align: center; border-bottom: 2px solid #dee2e6;'>Status</th></tr></thead><tbody>";

                foreach (var dep in deployments)
                {
                    var rowStatus = dep.Status == DeploymentStatus.Completed ? "✅" : "❌";
                    var rowBg = dep.Status == DeploymentStatus.Completed ? "#f0fff4" : "#fff5f5";

                    deploymentDetailsHtml += $"<tr style='background-color: {rowBg};'><td style='padding: 10px; border-bottom: 1px solid #dee2e6;'><strong>{dep.SolutionFriendlyName}</strong></td><td style='padding: 10px; border-bottom: 1px solid #dee2e6;'>{dep.SourceVersion} ➔ {dep.TargetVersion}</td><td style='padding: 10px; border-bottom: 1px solid #dee2e6;'>{dep.SourceEnvironment} ➔ {dep.TargetEnvironment}</td><td style='padding: 10px; border-bottom: 1px solid #dee2e6; text-align: center;'>{rowStatus}</td></tr>";
                }

                deploymentDetailsHtml += "</tbody></table></div>";
            }

            var html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }}
                        .container {{ max-width: 700px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0;}}
                        .status-badge {{ background-color: {statusColor}; color: white; padding: 10px 20px; display: inline-block; margin: 10px 0; font-weight: bold; }}
                        .content {{ background-color: #f8f9fa; padding: 30px; }}
                        .section {{ background-color: white; margin: 20px 0; padding: 20px;border-radius: 8px; }}
                        .section-title {{ color: #667eea; font-weight: bold; font-size: 16px; margin-bottom: 15px; border-bottom: 2px solid #667eea; padding-bottom: 5px; }}
                        .stat-box {{ display: inline-block; width: 48%; background-color: #f8f9fa; padding: 15px; text-align: center; border-left: 4px solid #667eea; margin-bottom: 10px; vertical-align: top; box-sizing: border-box;border-radius:8px;margin-right:2px;margin-left:2px; }}
                        .stat-number {{ font-size: 32px; font-weight: bold; color: #667eea; }}
                        .stat-label {{ color: #6c757d; font-size: 14px; margin-top: 5px; }}
                        .success-box {{ background-color: #d4edda; border-left-color: #28a745; }}
                        .failure-box {{ background-color: #f8d7da; border-left-color: #dc3545; }}
                        .info-row {{ margin: 10px 0; padding: 8px 0; border-bottom: 1px solid #e9ecef; }}
                        .label {{ color: #6c757d; font-weight: 600; }}
                        .value {{ color: #212529; }}
                        .success-message {{ background-color: #d4edda; border: 1px solid #c3e6cb; color: #155724; padding: 15px; text-align: center; font-weight: bold; margin: 20px 0;border-radius: 5px; }}
                        .warning-message {{ background-color: #fff3cd; border: 1px solid #ffeeba; color: #856404; padding: 15px; margin: 20px 0;border-radius: 5px; }}
                        .footer {{ text-align: center; padding: 20px; color: #6c757d; font-size: 12px; }}
                        .logo {{ max-width: 100px; max-height: 120px; margin: 20px auto; display: block; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>📊 Batch Deployment Summary</h1>
                            <div class='status-badge'>{statusIcon} {statusText}</div>
                        </div>
                        <div class='content'>
                            <div class='section'>
                                <div class='section-title'>📈 Deployment Statistics</div>
                                <div class='stat-box'><div class='stat-number'>{totalDeployments}</div><div class='stat-label'>Total Deployments</div></div>
                                <div class='stat-box success-box'><div class='stat-number' style='color: #28a745;'>{successCount}</div><div class='stat-label'>✅ Successful</div></div>
                                <div class='stat-box failure-box'><div class='stat-number' style='color: #dc3545;'>{failureCount}</div><div class='stat-label'>❌ Failed</div></div>
                                <div class='stat-box'><div class='stat-number'>{successRate}%</div><div class='stat-label'>Success Rate</div></div>
                            </div>
                            <div class='section'>
                                <div class='section-title'>⏱️ Time Information</div>
                                <div class='info-row'><span class='label'>Start Time:</span> {startTime:yyyy-MM-dd HH:mm:ss}</div>
                                <div class='info-row'><span class='label'>End Time:</span> {endTime:yyyy-MM-dd HH:mm:ss}</div>
                                <div class='info-row'><span class='label'>Total Duration:</span> <strong>{duration.Hours}h {duration.Minutes}m {duration.Seconds}s</strong></div>
                            </div>
                            {deploymentDetailsHtml}
                            {(failureCount > 0 ? "<div class='warning-message'><strong>⚠️ ATTENTION REQUIRED</strong><br/>Some deployments failed. Please review the deployment logs and history.</div>" : "<div class='success-message'>✅ ALL DEPLOYMENTS COMPLETED SUCCESSFULLY!</div>")}
                            <div class='section' style='background-color: #e7f3ff; border-left: 4px solid #0066cc;'>
                                <div class='section-title' style='color: #0066cc;'>📝 Next Steps</div>
                                <ul style='margin: 10px 0; padding-left: 20px;'>
                                    <li>Review Deployment Logs in the application</li>
                                    <li>Check Deployment History for individual results</li>
                                    <li>Verify solutions in target environments</li>
                                </ul>
                            </div>
                        </div>
                        <div class='footer'>
                            <img src='cid:companyLogo' alt='Sujan Solution Deployer Logo' class='logo' />
                            <p><strong>Sujan Solution Deployer</strong></p>
                            <p>Automated Batch Deployment Notification for Dynamics 365</p>
                        </div>
                    </div>
                </body>
                </html>";

            return html;
        }

        #endregion
        /// Test SMTP configuration by sending a test email
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
