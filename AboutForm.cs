using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sujan_Solution_Deployer
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            LoadAboutInfo();
        }

        private void LoadAboutInfo()
        {
            // Set version info
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Version {version.Major}.{version.Minor}.{version.Build}";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("mailto:murugeshsujan22@gmail.com?subject=Sujan Solution Deployer - Inquiry");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open email client: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void linkLinkedIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://www.linkedin.com/in/sujan-murugesh/");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open browser: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuyMeCoffee_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://www.buymeacoffee.com/murugeshsujan");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open browser: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://msujan.netlify.app/");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open browser: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
