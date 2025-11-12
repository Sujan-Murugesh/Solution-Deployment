using Sujan_Solution_Deployer.Services;
using System.Drawing;
using System.Windows.Forms;

namespace Sujan_Solution_Deployer
{
    partial class SmtpConfigurationForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmtpConfigurationForm));
            this.grpSmtpSettings = new System.Windows.Forms.GroupBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.linkHelp = new System.Windows.Forms.LinkLabel();
            this.lblSmtpHost = new System.Windows.Forms.Label();
            this.txtSmtpHost = new System.Windows.Forms.TextBox();
            this.lblSmtpPort = new System.Windows.Forms.Label();
            this.numSmtpPort = new System.Windows.Forms.NumericUpDown();
            this.chkEnableSsl = new System.Windows.Forms.CheckBox();
            this.lblSenderEmail = new System.Windows.Forms.Label();
            this.txtSenderEmail = new System.Windows.Forms.TextBox();
            this.lblSenderPassword = new System.Windows.Forms.Label();
            this.txtSenderPassword = new System.Windows.Forms.TextBox();
            this.lblSenderName = new System.Windows.Forms.Label();
            this.txtSenderName = new System.Windows.Forms.TextBox();
            this.lblCommonServers = new System.Windows.Forms.Label();
            this.lblTestEmail = new System.Windows.Forms.Label();
            this.txtTestEmail = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSmtpDiagnostic = new System.Windows.Forms.Button();
            this.btnClearSettings = new System.Windows.Forms.Button();
            this.grpSmtpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSmtpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSmtpSettings
            // 
            this.grpSmtpSettings.Controls.Add(this.lblInfo);
            this.grpSmtpSettings.Controls.Add(this.linkHelp);
            this.grpSmtpSettings.Controls.Add(this.lblSmtpHost);
            this.grpSmtpSettings.Controls.Add(this.txtSmtpHost);
            this.grpSmtpSettings.Controls.Add(this.lblSmtpPort);
            this.grpSmtpSettings.Controls.Add(this.numSmtpPort);
            this.grpSmtpSettings.Controls.Add(this.chkEnableSsl);
            this.grpSmtpSettings.Controls.Add(this.lblSenderEmail);
            this.grpSmtpSettings.Controls.Add(this.txtSenderEmail);
            this.grpSmtpSettings.Controls.Add(this.lblSenderPassword);
            this.grpSmtpSettings.Controls.Add(this.txtSenderPassword);
            this.grpSmtpSettings.Controls.Add(this.lblSenderName);
            this.grpSmtpSettings.Controls.Add(this.txtSenderName);
            this.grpSmtpSettings.Controls.Add(this.lblCommonServers);
            this.grpSmtpSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSmtpSettings.Location = new System.Drawing.Point(20, 20);
            this.grpSmtpSettings.Name = "grpSmtpSettings";
            this.grpSmtpSettings.Size = new System.Drawing.Size(495, 380);
            this.grpSmtpSettings.TabIndex = 0;
            this.grpSmtpSettings.TabStop = false;
            this.grpSmtpSettings.Text = "Email Server Settings";
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            this.lblInfo.Location = new System.Drawing.Point(15, 25);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(460, 40);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "ℹ️ IMPORTANT: Gmail/Outlook require App Password, NOT regular password.\r\nClick th" +
    "e link below for step-by-step instructions.";
            // 
            // linkHelp
            // 
            this.linkHelp.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.linkHelp.Location = new System.Drawing.Point(15, 65);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(460, 20);
            this.linkHelp.TabIndex = 1;
            this.linkHelp.TabStop = true;
            this.linkHelp.Text = "How to get App Password (Gmail/Outlook/Yahoo)";
            // 
            // lblSmtpHost
            // 
            this.lblSmtpHost.Location = new System.Drawing.Point(15, 95);
            this.lblSmtpHost.Name = "lblSmtpHost";
            this.lblSmtpHost.Size = new System.Drawing.Size(120, 20);
            this.lblSmtpHost.TabIndex = 2;
            this.lblSmtpHost.Text = "SMTP Host:";
            // 
            // txtSmtpHost
            // 
            this.txtSmtpHost.Location = new System.Drawing.Point(145, 93);
            this.txtSmtpHost.Name = "txtSmtpHost";
            this.txtSmtpHost.Size = new System.Drawing.Size(330, 23);
            this.txtSmtpHost.TabIndex = 3;
            this.txtSmtpHost.Text = "smtp.gmail.com";
            // 
            // lblSmtpPort
            // 
            this.lblSmtpPort.Location = new System.Drawing.Point(15, 130);
            this.lblSmtpPort.Name = "lblSmtpPort";
            this.lblSmtpPort.Size = new System.Drawing.Size(120, 20);
            this.lblSmtpPort.TabIndex = 4;
            this.lblSmtpPort.Text = "SMTP Port:";
            // 
            // numSmtpPort
            // 
            this.numSmtpPort.Location = new System.Drawing.Point(145, 128);
            this.numSmtpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numSmtpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSmtpPort.Name = "numSmtpPort";
            this.numSmtpPort.Size = new System.Drawing.Size(100, 23);
            this.numSmtpPort.TabIndex = 5;
            this.numSmtpPort.Value = new decimal(new int[] {
            587,
            0,
            0,
            0});
            // 
            // chkEnableSsl
            // 
            this.chkEnableSsl.Checked = true;
            this.chkEnableSsl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableSsl.Location = new System.Drawing.Point(260, 128);
            this.chkEnableSsl.Name = "chkEnableSsl";
            this.chkEnableSsl.Size = new System.Drawing.Size(200, 23);
            this.chkEnableSsl.TabIndex = 6;
            this.chkEnableSsl.Text = "Enable SSL/TLS";
            // 
            // lblSenderEmail
            // 
            this.lblSenderEmail.Location = new System.Drawing.Point(15, 165);
            this.lblSenderEmail.Name = "lblSenderEmail";
            this.lblSenderEmail.Size = new System.Drawing.Size(100, 23);
            this.lblSenderEmail.TabIndex = 7;
            this.lblSenderEmail.Text = "Sender Email:";
            // 
            // txtSenderEmail
            // 
            this.txtSenderEmail.Location = new System.Drawing.Point(145, 163);
            this.txtSenderEmail.Name = "txtSenderEmail";
            this.txtSenderEmail.Size = new System.Drawing.Size(330, 23);
            this.txtSenderEmail.TabIndex = 8;
            // 
            // lblSenderPassword
            // 
            this.lblSenderPassword.Location = new System.Drawing.Point(15, 200);
            this.lblSenderPassword.Name = "lblSenderPassword";
            this.lblSenderPassword.Size = new System.Drawing.Size(100, 23);
            this.lblSenderPassword.TabIndex = 9;
            this.lblSenderPassword.Text = "Password/App Key:";
            // 
            // txtSenderPassword
            // 
            this.txtSenderPassword.Location = new System.Drawing.Point(145, 198);
            this.txtSenderPassword.Name = "txtSenderPassword";
            this.txtSenderPassword.Size = new System.Drawing.Size(330, 23);
            this.txtSenderPassword.TabIndex = 10;
            this.txtSenderPassword.UseSystemPasswordChar = true;
            // 
            // lblSenderName
            // 
            this.lblSenderName.Location = new System.Drawing.Point(15, 235);
            this.lblSenderName.Name = "lblSenderName";
            this.lblSenderName.Size = new System.Drawing.Size(100, 23);
            this.lblSenderName.TabIndex = 11;
            this.lblSenderName.Text = "Sender Name:";
            // 
            // txtSenderName
            // 
            this.txtSenderName.Location = new System.Drawing.Point(145, 233);
            this.txtSenderName.Name = "txtSenderName";
            this.txtSenderName.Size = new System.Drawing.Size(330, 23);
            this.txtSenderName.TabIndex = 12;
            // 
            // lblCommonServers
            // 
            this.lblCommonServers.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCommonServers.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblCommonServers.Location = new System.Drawing.Point(15, 270);
            this.lblCommonServers.Name = "lblCommonServers";
            this.lblCommonServers.Size = new System.Drawing.Size(460, 100);
            this.lblCommonServers.TabIndex = 13;
            this.lblCommonServers.Text = resources.GetString("lblCommonServers.Text");
            // 
            // lblTestEmail
            // 
            this.lblTestEmail.Location = new System.Drawing.Point(20, 415);
            this.lblTestEmail.Name = "lblTestEmail";
            this.lblTestEmail.Size = new System.Drawing.Size(100, 23);
            this.lblTestEmail.TabIndex = 1;
            this.lblTestEmail.Text = "Test Email To:";
            // 
            // txtTestEmail
            // 
            this.txtTestEmail.Location = new System.Drawing.Point(125, 413);
            this.txtTestEmail.Name = "txtTestEmail";
            this.txtTestEmail.Size = new System.Drawing.Size(262, 20);
            this.txtTestEmail.TabIndex = 2;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(404, 410);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(111, 30);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "🧪 Test";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(164)))), ((int)(((byte)(0)))));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(404, 460);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(111, 35);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "💾 Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(309, 460);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 35);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "❌ Cancel";
            // 
            // btnSmtpDiagnostic
            // 
            this.btnSmtpDiagnostic.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSmtpDiagnostic.Location = new System.Drawing.Point(168, 460);
            this.btnSmtpDiagnostic.Name = "btnSmtpDiagnostic";
            this.btnSmtpDiagnostic.Size = new System.Drawing.Size(131, 35);
            this.btnSmtpDiagnostic.TabIndex = 6;
            this.btnSmtpDiagnostic.Text = "🔍 Run Diagnostic";
            this.btnSmtpDiagnostic.Click += new System.EventHandler(this.btnSmtpDiagnostic_Click);
            // 
            // btnClearSettings
            // 
            this.btnClearSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnClearSettings.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearSettings.ForeColor = System.Drawing.Color.White;
            this.btnClearSettings.Location = new System.Drawing.Point(20, 460);
            this.btnClearSettings.Name = "btnClearSettings";
            this.btnClearSettings.Size = new System.Drawing.Size(142, 35);
            this.btnClearSettings.TabIndex = 7;
            this.btnClearSettings.Text = "🗑️ Clear Saved Settings";
            this.btnClearSettings.UseVisualStyleBackColor = false;
            this.btnClearSettings.Visible = false;
            // 
            // SmtpConfigurationForm
            // 
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.Controls.Add(this.btnClearSettings);
            this.Controls.Add(this.btnSmtpDiagnostic);
            this.Controls.Add(this.grpSmtpSettings);
            this.Controls.Add(this.lblTestEmail);
            this.Controls.Add(this.txtTestEmail);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SmtpConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sujan Solution Deployer - SMTP Configuration";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.controlHelpPdfOpen);
            this.grpSmtpSettings.ResumeLayout(false);
            this.grpSmtpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSmtpPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox grpSmtpSettings;
        private Label lblInfo;
        private LinkLabel linkHelp;
        private Label lblSmtpHost;
        private TextBox txtSmtpHost;
        private Label lblSmtpPort;
        private NumericUpDown numSmtpPort;
        private CheckBox chkEnableSsl;
        private Label lblSenderEmail;
        private TextBox txtSenderEmail;
        private Label lblSenderPassword;
        private TextBox txtSenderPassword;
        private Label lblSenderName;
        private TextBox txtSenderName;
        private Label lblCommonServers;
        private Label lblTestEmail;
        private TextBox txtTestEmail;
        private Button btnTest;
        private Button btnSave;
        private Button btnCancel;
        private Button btnSmtpDiagnostic;
        private Button btnClearSettings;
        //private Button btnClearSettings;
    }
}
