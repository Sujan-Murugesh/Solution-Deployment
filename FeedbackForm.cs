using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace Sujan_Solution_Deployer
{
    public partial class FeedbackForm : Form
    {
        private const string FEEDBACK_EMAIL = "murugeshsujan22@gmail.com";
        public FeedbackForm()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter your name.",
                    "Name Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter your email address.",
                    "Email Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.",
                    "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (cmbFeedbackType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a feedback type.",
                    "Feedback Type Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbFeedbackType.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Please enter your feedback message.",
                    "Message Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMessage.Focus();
                return;
            }

            // Send feedback
            try
            {
                SendFeedback();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending feedback: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetRating()
        {
            if (rbExcellent.Checked) return "⭐⭐⭐⭐⭐ Excellent";
            if (rbGood.Checked) return "⭐⭐⭐⭐ Good";
            if (rbAverage.Checked) return "⭐⭐⭐ Average";
            if (rbPoor.Checked) return "⭐⭐ Poor";
            if (rbVeryPoor.Checked) return "⭐ Very Poor";
            return "Not Rated";
        }

        private string GetVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void SendFeedback()
        {
            var feedbackType = cmbFeedbackType.SelectedItem.ToString();
            var rating = GetRating();

            // Build email body
            var emailBody = $"Feedback Type: {feedbackType}\n";
            emailBody += $"Rating: {rating}\n";
            emailBody += $"Name: {txtName.Text}\n";
            emailBody += $"Email: {txtEmail.Text}\n";
            emailBody += $"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n";
            emailBody += $"Message:\n{txtMessage.Text}\n\n";
            emailBody += "---\n";
            emailBody += $"Sent from: Sujan Solution Deployer v{GetVersion()}\n";

            // Create mailto link
            var subject = $"Sujan Solution Deployer - {feedbackType}";
            var body = Uri.EscapeDataString(emailBody);
            var mailtoUrl = $"mailto:{FEEDBACK_EMAIL}?subject={Uri.EscapeDataString(subject)}&body={body}";

            try
            {
                Process.Start(mailtoUrl);

                MessageBox.Show(
                    "Your default email client has been opened with the feedback.\n\n" +
                    "Please click Send in your email client to submit your feedback.\n\n" +
                    "Thank you for your feedback!",
                    "Feedback Ready",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not open email client.\n\n" +
                    $"Please send your feedback manually to:\n{FEEDBACK_EMAIL}\n\n" + $"Error: {ex.Message}",
                    "Error Opening Email Client",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void linkViewGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/Sujan-Murugesh/Solution-Deployment/issues");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open browser: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
