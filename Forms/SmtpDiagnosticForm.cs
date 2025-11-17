using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Sujan_Solution_Deployer
{
    public partial class SmtpDiagnosticForm : Form
    {
        public SmtpDiagnosticForm()
        {
            InitializeComponent();
            btnTest.Click += BtnTest_Click;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            txtResults.Clear();
            btnTest.Enabled = false;
            btnTest.Text = "⏳ Testing...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                RunDiagnostic(txtHost.Text, (int)numPort.Value);
            }
            finally
            {
                btnTest.Enabled = true;
                btnTest.Text = "🔍 Run Diagnostic";
                this.Cursor = Cursors.Default;
            }
        }

        private void RunDiagnostic(string host, int port)
        {
            AppendResult("═══════════════════════════════════════════════════", Color.White);
            AppendResult($"SMTP DIAGNOSTIC TEST - {DateTime.Now:yyyy-MM-dd HH:mm:ss}", Color.White);
            AppendResult("═══════════════════════════════════════════════════\n", Color.White);

            AppendResult($"Testing connection to: {host}:{port}\n", Color.Yellow);

            // Test 1: DNS Resolution
            AppendResult("[1/4] Testing DNS resolution...", Color.Cyan);
            try
            {
                var addresses = System.Net.Dns.GetHostAddresses(host);
                AppendResult($"✅ DNS OK - Resolved to {addresses.Length} address(es)", Color.Lime);
                foreach (var addr in addresses)
                {
                    AppendResult($"    → {addr}", Color.Gray);
                }
                AppendResult("", Color.White);
            }
            catch (Exception ex)
            {
                AppendResult($"❌ DNS FAILED - {ex.Message}", Color.Red);
                AppendResult("This means the SMTP host name is invalid or unreachable.\n", Color.Yellow);
                return;
            }

            // Test 2: TCP Connection
            AppendResult("[2/4] Testing TCP connection...", Color.Cyan);
            TcpClient client = null;
            try
            {
                client = new TcpClient();
                var result = client.BeginConnect(host, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10));

                if (success)
                {
                    client.EndConnect(result);
                    AppendResult($"✅ TCP CONNECTION OK - Port {port} is open", Color.Lime);
                    AppendResult("", Color.White);
                }
                else
                {
                    AppendResult($"❌ TCP CONNECTION TIMEOUT", Color.Red);
                    AppendResult($"Port {port} is not responding. Possible causes:", Color.Yellow);
                    AppendResult("  • Firewall is blocking the connection", Color.Gray);
                    AppendResult("  • Wrong port number", Color.Gray);
                    AppendResult("  • SMTP server is down\n", Color.Gray);
                    return;
                }
            }
            catch (SocketException ex)
            {
                AppendResult($"❌ TCP CONNECTION FAILED - {ex.Message}", Color.Red);
                AppendResult("Possible causes:", Color.Yellow);
                AppendResult("  • Port is closed or blocked by firewall", Color.Gray);
                AppendResult("  • SMTP server is not running on this port", Color.Gray);
                AppendResult("  • Network connectivity issues\n", Color.Gray);
                return;
            }
            finally
            {
                client?.Close();
            }

            // Test 3: SMTP Banner
            AppendResult("[3/4] Testing SMTP banner...", Color.Cyan);
            try
            {
                using (var tcpClient = new TcpClient(host, port))
                using (var stream = tcpClient.GetStream())
                using (var reader = new System.IO.StreamReader(stream))
                {
                    tcpClient.ReceiveTimeout = 5000;
                    string banner = reader.ReadLine();

                    if (!string.IsNullOrEmpty(banner))
                    {
                        AppendResult($"✅ SMTP BANNER RECEIVED", Color.Lime);
                        AppendResult($"    {banner}", Color.Gray);
                        AppendResult("", Color.White);

                        if (banner.StartsWith("220"))
                        {
                            AppendResult("✅ SMTP service is ready for connections", Color.Lime);
                        }
                    }
                    else
                    {
                        AppendResult($"⚠️ NO BANNER RECEIVED", Color.Yellow);
                    }
                }
                AppendResult("", Color.White);
            }
            catch (Exception ex)
            {
                AppendResult($"⚠️ BANNER TEST FAILED - {ex.Message}", Color.Yellow);
                AppendResult("This might be normal for some SMTP servers.\n", Color.Gray);
            }

            // Test 4: Recommendations
            AppendResult("[4/4] Analysis & Recommendations:", Color.Cyan);
            AppendResult("", Color.White);

            if (port == 587)
            {
                AppendResult("✅ Using standard SMTP submission port (587) with STARTTLS", Color.Lime);
                AppendResult("   • SSL/TLS should be ENABLED", Color.Gray);
                AppendResult("   • This is the recommended configuration\n", Color.Gray);
            }
            else if (port == 465)
            {
                AppendResult("ℹ️ Using SMTP SSL port (465)", Color.Cyan);
                AppendResult("   • SSL/TLS should be ENABLED", Color.Gray);
                AppendResult("   • This is an alternative secure configuration\n", Color.Gray);
            }
            else if (port == 25)
            {
                AppendResult("⚠️ Using standard SMTP port (25)", Color.Yellow);
                AppendResult("   • Often blocked by ISPs to prevent spam", Color.Gray);
                AppendResult("   • May not support authentication", Color.Gray);
                AppendResult("   • Consider using port 587 instead\n", Color.Gray);
            }

            AppendResult("═══════════════════════════════════════════════════", Color.White);
            AppendResult("NEXT STEPS:", Color.Yellow);
            AppendResult("═══════════════════════════════════════════════════\n", Color.White);

            if (host.Contains("gmail"))
            {
                AppendResult("For Gmail:", Color.Cyan);
                AppendResult("✅ Use App Password (NOT regular password)", Color.Lime);
                AppendResult("✅ Get it from: https://myaccount.google.com/apppasswords", Color.Gray);
                AppendResult("✅ 2FA must be enabled first\n", Color.Gray);
            }
            else if (host.Contains("office365") || host.Contains("outlook"))
            {
                AppendResult("For Outlook/Office365:", Color.Cyan);
                AppendResult("✅ Use App Password if 2FA is enabled", Color.Lime);
                AppendResult("✅ Or try your regular password\n", Color.Gray);
            }

            AppendResult("Connection test completed successfully! ✅", Color.Lime);
            AppendResult("You can now configure SMTP settings with confidence.", Color.White);
        }

        private void AppendResult(string text, Color color)
        {
            txtResults.SelectionStart = txtResults.TextLength;
            txtResults.SelectionLength = 0;
            txtResults.SelectionColor = color;
            txtResults.AppendText(text + "\n");
            txtResults.SelectionColor = txtResults.ForeColor;
            txtResults.ScrollToCaret();
        }
    }
}
