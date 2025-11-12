namespace Sujan_Solution_Deployer
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTagline = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.groupBoxSupport = new System.Windows.Forms.GroupBox();
            this.btnWebsite = new System.Windows.Forms.Button();
            this.btnBuyMeCoffee = new System.Windows.Forms.Button();
            this.lblSupportText = new System.Windows.Forms.Label();
            this.groupBoxContact = new System.Windows.Forms.GroupBox();
            this.linkLinkedIn = new System.Windows.Forms.LinkLabel();
            this.lblLinkedInLabel = new System.Windows.Forms.Label();
            this.linkGitHub = new System.Windows.Forms.LinkLabel();
            this.lblGitHubLabel = new System.Windows.Forms.Label();
            this.linkEmail = new System.Windows.Forms.LinkLabel();
            this.lblEmailLabel = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelMiddle.SuspendLayout();
            this.groupBoxSupport.SuspendLayout();
            this.groupBoxContact.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.panelTop.Controls.Add(this.lblTagline);
            this.panelTop.Controls.Add(this.lblVersion);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.pictureBoxLogo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(600, 150);
            this.panelTop.TabIndex = 0;
            // 
            // lblTagline
            // 
            this.lblTagline.AutoSize = true;
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblTagline.ForeColor = System.Drawing.Color.White;
            this.lblTagline.Location = new System.Drawing.Point(155, 105);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(362, 15);
            this.lblTagline.TabIndex = 3;
            this.lblTagline.Text = "Automated Solution Deployment for Dynamics 365 / Power Platform";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(155, 80);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(88, 19);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version 1.0.0";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(150, 40);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(327, 37);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Sujan Solution Deployer";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Lavender;
            this.pictureBoxLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxLogo.Image = global::Sujan_Solution_Deployer.Properties.Resources.deployer_logo;
            this.pictureBoxLogo.Location = new System.Drawing.Point(30, 30);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(100, 100);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // panelMiddle
            // 
            this.panelMiddle.BackColor = System.Drawing.Color.White;
            this.panelMiddle.Controls.Add(this.groupBoxSupport);
            this.panelMiddle.Controls.Add(this.groupBoxContact);
            this.panelMiddle.Controls.Add(this.lblDescription);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 150);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Padding = new System.Windows.Forms.Padding(20);
            this.panelMiddle.Size = new System.Drawing.Size(600, 400);
            this.panelMiddle.TabIndex = 1;
            // 
            // groupBoxSupport
            // 
            this.groupBoxSupport.Controls.Add(this.btnWebsite);
            this.groupBoxSupport.Controls.Add(this.btnBuyMeCoffee);
            this.groupBoxSupport.Controls.Add(this.lblSupportText);
            this.groupBoxSupport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxSupport.Location = new System.Drawing.Point(30, 240);
            this.groupBoxSupport.Name = "groupBoxSupport";
            this.groupBoxSupport.Size = new System.Drawing.Size(540, 120);
            this.groupBoxSupport.TabIndex = 2;
            this.groupBoxSupport.TabStop = false;
            this.groupBoxSupport.Text = "☕ Support & Resources";
            // 
            // btnWebsite
            // 
            this.btnWebsite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnWebsite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWebsite.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnWebsite.ForeColor = System.Drawing.Color.White;
            this.btnWebsite.Location = new System.Drawing.Point(190, 70);
            this.btnWebsite.Name = "btnWebsite";
            this.btnWebsite.Size = new System.Drawing.Size(150, 35);
            this.btnWebsite.TabIndex = 2;
            this.btnWebsite.Text = "🌐 Visit Website";
            this.btnWebsite.UseVisualStyleBackColor = false;
            this.btnWebsite.Click += new System.EventHandler(this.btnWebsite_Click);
            // 
            // btnBuyMeCoffee
            // 
            this.btnBuyMeCoffee.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(221)))), ((int)(((byte)(0)))));
            this.btnBuyMeCoffee.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuyMeCoffee.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuyMeCoffee.ForeColor = System.Drawing.Color.Black;
            this.btnBuyMeCoffee.Location = new System.Drawing.Point(23, 70);
            this.btnBuyMeCoffee.Name = "btnBuyMeCoffee";
            this.btnBuyMeCoffee.Size = new System.Drawing.Size(150, 35);
            this.btnBuyMeCoffee.TabIndex = 1;
            this.btnBuyMeCoffee.Text = "☕ Buy Me a Coffee";
            this.btnBuyMeCoffee.UseVisualStyleBackColor = false;
            this.btnBuyMeCoffee.Click += new System.EventHandler(this.btnBuyMeCoffee_Click);
            // 
            // lblSupportText
            // 
            this.lblSupportText.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSupportText.Location = new System.Drawing.Point(20, 25);
            this.lblSupportText.Name = "lblSupportText";
            this.lblSupportText.Size = new System.Drawing.Size(500, 40);
            this.lblSupportText.TabIndex = 0;
            this.lblSupportText.Text = "If you find this tool helpful, consider supporting the development! Your support " +
    "helps maintain and improve this tool.";
            // 
            // groupBoxContact
            // 
            this.groupBoxContact.Controls.Add(this.linkLinkedIn);
            this.groupBoxContact.Controls.Add(this.lblLinkedInLabel);
            this.groupBoxContact.Controls.Add(this.linkGitHub);
            this.groupBoxContact.Controls.Add(this.lblGitHubLabel);
            this.groupBoxContact.Controls.Add(this.linkEmail);
            this.groupBoxContact.Controls.Add(this.lblEmailLabel);
            this.groupBoxContact.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxContact.Location = new System.Drawing.Point(30, 110);
            this.groupBoxContact.Name = "groupBoxContact";
            this.groupBoxContact.Size = new System.Drawing.Size(540, 120);
            this.groupBoxContact.TabIndex = 1;
            this.groupBoxContact.TabStop = false;
            this.groupBoxContact.Text = "📧 Contact Information";
            // 
            // linkLinkedIn
            // 
            this.linkLinkedIn.AutoSize = true;
            this.linkLinkedIn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.linkLinkedIn.Location = new System.Drawing.Point(100, 90);
            this.linkLinkedIn.Name = "linkLinkedIn";
            this.linkLinkedIn.Size = new System.Drawing.Size(183, 15);
            this.linkLinkedIn.TabIndex = 5;
            this.linkLinkedIn.TabStop = true;
            this.linkLinkedIn.Text = "linkedin.com/in/sujan-murugesh";
            this.linkLinkedIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLinkedIn_LinkClicked);
            // 
            // lblLinkedInLabel
            // 
            this.lblLinkedInLabel.AutoSize = true;
            this.lblLinkedInLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLinkedInLabel.Location = new System.Drawing.Point(20, 90);
            this.lblLinkedInLabel.Name = "lblLinkedInLabel";
            this.lblLinkedInLabel.Size = new System.Drawing.Size(55, 15);
            this.lblLinkedInLabel.TabIndex = 4;
            this.lblLinkedInLabel.Text = "LinkedIn:";
            // 
            // linkGitHub
            // 
            this.linkGitHub.AutoSize = true;
            this.linkGitHub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.linkGitHub.Location = new System.Drawing.Point(100, 60);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(162, 15);
            this.linkGitHub.TabIndex = 3;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "github.com/Sujan-Murugesh";
            this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
            // 
            // lblGitHubLabel
            // 
            this.lblGitHubLabel.AutoSize = true;
            this.lblGitHubLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGitHubLabel.Location = new System.Drawing.Point(20, 60);
            this.lblGitHubLabel.Name = "lblGitHubLabel";
            this.lblGitHubLabel.Size = new System.Drawing.Size(48, 15);
            this.lblGitHubLabel.TabIndex = 2;
            this.lblGitHubLabel.Text = "GitHub:";
            // 
            // linkEmail
            // 
            this.linkEmail.AutoSize = true;
            this.linkEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.linkEmail.Location = new System.Drawing.Point(100, 30);
            this.linkEmail.Name = "linkEmail";
            this.linkEmail.Size = new System.Drawing.Size(169, 15);
            this.linkEmail.TabIndex = 1;
            this.linkEmail.TabStop = true;
            this.linkEmail.Text = "murugeshsujan22@gmail.com";
            this.linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEmail_LinkClicked);
            // 
            // lblEmailLabel
            // 
            this.lblEmailLabel.AutoSize = true;
            this.lblEmailLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEmailLabel.Location = new System.Drawing.Point(20, 30);
            this.lblEmailLabel.Name = "lblEmailLabel";
            this.lblEmailLabel.Size = new System.Drawing.Size(39, 15);
            this.lblEmailLabel.TabIndex = 0;
            this.lblEmailLabel.Text = "Email:";
            // 
            // lblDescription
            // 
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.Location = new System.Drawing.Point(20, 20);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Padding = new System.Windows.Forms.Padding(10);
            this.lblDescription.Size = new System.Drawing.Size(560, 80);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = resources.GetString("lblDescription.Text");
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelBottom.Controls.Add(this.lblCopyright);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 550);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(600, 50);
            this.panelBottom.TabIndex = 2;
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCopyright.ForeColor = System.Drawing.Color.Gray;
            this.lblCopyright.Location = new System.Drawing.Point(20, 18);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(177, 13);
            this.lblCopyright.TabIndex = 0;
            this.lblCopyright.Text = "© 2025 Sujan. All rights reserved.";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(490, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "❌ Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About - Sujan Solution Deployer";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelMiddle.ResumeLayout(false);
            this.groupBoxSupport.ResumeLayout(false);
            this.groupBoxContact.ResumeLayout(false);
            this.groupBoxContact.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblTagline;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.GroupBox groupBoxContact;
        private System.Windows.Forms.LinkLabel linkEmail;
        private System.Windows.Forms.Label lblEmailLabel;
        private System.Windows.Forms.LinkLabel linkGitHub;
        private System.Windows.Forms.Label lblGitHubLabel;
        private System.Windows.Forms.LinkLabel linkLinkedIn;
        private System.Windows.Forms.Label lblLinkedInLabel;
        private System.Windows.Forms.GroupBox groupBoxSupport;
        private System.Windows.Forms.Button btnBuyMeCoffee;
        private System.Windows.Forms.Button btnWebsite;
        private System.Windows.Forms.Label lblSupportText;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Button btnClose;
    }
}
