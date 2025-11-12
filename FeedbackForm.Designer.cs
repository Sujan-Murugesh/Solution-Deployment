namespace Sujan_Solution_Deployer
{
    partial class FeedbackForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedbackForm));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblFeedbackType = new System.Windows.Forms.Label();
            this.cmbFeedbackType = new System.Windows.Forms.ComboBox();
            this.grpRating = new System.Windows.Forms.GroupBox();
            this.rbExcellent = new System.Windows.Forms.RadioButton();
            this.rbGood = new System.Windows.Forms.RadioButton();
            this.rbAverage = new System.Windows.Forms.RadioButton();
            this.rbPoor = new System.Windows.Forms.RadioButton();
            this.rbVeryPoor = new System.Windows.Forms.RadioButton();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.linkViewGitHub = new System.Windows.Forms.LinkLabel();
            this.grpRating.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(263, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "💬 We Value Your Feedback!";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(25, 70);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(69, 15);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Your Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(120, 67);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(350, 23);
            this.txtName.TabIndex = 2;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(25, 105);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(66, 15);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "Your Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(120, 102);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(350, 23);
            this.txtEmail.TabIndex = 4;
            // 
            // lblFeedbackType
            // 
            this.lblFeedbackType.AutoSize = true;
            this.lblFeedbackType.Location = new System.Drawing.Point(25, 140);
            this.lblFeedbackType.Name = "lblFeedbackType";
            this.lblFeedbackType.Size = new System.Drawing.Size(88, 15);
            this.lblFeedbackType.TabIndex = 5;
            this.lblFeedbackType.Text = "Feedback Type:";
            // 
            // cmbFeedbackType
            // 
            this.cmbFeedbackType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFeedbackType.FormattingEnabled = true;
            this.cmbFeedbackType.Items.AddRange(new object[] {
            "💡 Feature Request",
            "🐛 Bug Report",
            "💬 General Feedback",
            "⭐ Compliment",
            "❓ Question",
            "📚 Documentation",
            "🚀 Performance",
            "💼 Other"});
            this.cmbFeedbackType.Location = new System.Drawing.Point(120, 137);
            this.cmbFeedbackType.Name = "cmbFeedbackType";
            this.cmbFeedbackType.Size = new System.Drawing.Size(350, 23);
            this.cmbFeedbackType.TabIndex = 6;
            // 
            // grpRating
            // 
            this.grpRating.Controls.Add(this.rbExcellent);
            this.grpRating.Controls.Add(this.rbGood);
            this.grpRating.Controls.Add(this.rbAverage);
            this.grpRating.Controls.Add(this.rbPoor);
            this.grpRating.Controls.Add(this.rbVeryPoor);
            this.grpRating.Location = new System.Drawing.Point(25, 175);
            this.grpRating.Name = "grpRating";
            this.grpRating.Size = new System.Drawing.Size(445, 60);
            this.grpRating.TabIndex = 7;
            this.grpRating.TabStop = false;
            this.grpRating.Text = "⭐ Rate Your Experience (Optional)";
            // 
            // rbExcellent
            // 
            this.rbExcellent.AutoSize = true;
            this.rbExcellent.Checked = true;
            this.rbExcellent.Location = new System.Drawing.Point(15, 25);
            this.rbExcellent.Name = "rbExcellent";
            this.rbExcellent.Size = new System.Drawing.Size(71, 19);
            this.rbExcellent.TabIndex = 0;
            this.rbExcellent.TabStop = true;
            this.rbExcellent.Text = "Excellent";
            this.rbExcellent.UseVisualStyleBackColor = true;
            // 
            // rbGood
            // 
            this.rbGood.AutoSize = true;
            this.rbGood.Location = new System.Drawing.Point(100, 25);
            this.rbGood.Name = "rbGood";
            this.rbGood.Size = new System.Drawing.Size(54, 19);
            this.rbGood.TabIndex = 1;
            this.rbGood.Text = "Good";
            this.rbGood.UseVisualStyleBackColor = true;
            // 
            // rbAverage
            // 
            this.rbAverage.AutoSize = true;
            this.rbAverage.Location = new System.Drawing.Point(170, 25);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new System.Drawing.Size(68, 19);
            this.rbAverage.TabIndex = 2;
            this.rbAverage.Text = "Average";
            this.rbAverage.UseVisualStyleBackColor = true;
            // 
            // rbPoor
            // 
            this.rbPoor.AutoSize = true;
            this.rbPoor.Location = new System.Drawing.Point(255, 25);
            this.rbPoor.Name = "rbPoor";
            this.rbPoor.Size = new System.Drawing.Size(50, 19);
            this.rbPoor.TabIndex = 3;
            this.rbPoor.Text = "Poor";
            this.rbPoor.UseVisualStyleBackColor = true;
            // 
            // rbVeryPoor
            // 
            this.rbVeryPoor.AutoSize = true;
            this.rbVeryPoor.Location = new System.Drawing.Point(320, 25);
            this.rbVeryPoor.Name = "rbVeryPoor";
            this.rbVeryPoor.Size = new System.Drawing.Size(75, 19);
            this.rbVeryPoor.TabIndex = 4;
            this.rbVeryPoor.Text = "Very Poor";
            this.rbVeryPoor.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(25, 250);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(91, 15);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "Your Message: *";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(25, 270);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(445, 120);
            this.txtMessage.TabIndex = 9;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(164)))), ((int)(((byte)(0)))));
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(260, 465);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 35);
            this.btnSend.TabIndex = 12;
            this.btnSend.Text = "📧 Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(370, 465);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "❌ Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblInfo.Location = new System.Drawing.Point(25, 400);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(445, 30);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Your feedback helps us improve! This will open your email client with a pre-fille" +
    "d message.";
            // 
            // linkViewGitHub
            // 
            this.linkViewGitHub.AutoSize = true;
            this.linkViewGitHub.Location = new System.Drawing.Point(25, 435);
            this.linkViewGitHub.Name = "linkViewGitHub";
            this.linkViewGitHub.Size = new System.Drawing.Size(149, 15);
            this.linkViewGitHub.TabIndex = 11;
            this.linkViewGitHub.TabStop = true;
            this.linkViewGitHub.Text = "🔗 Report issues on GitHub";
            this.linkViewGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkViewGitHub_LinkClicked);
            // 
            // FeedbackForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(494, 520);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.linkViewGitHub);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.grpRating);
            this.Controls.Add(this.cmbFeedbackType);
            this.Controls.Add(this.lblFeedbackType);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeedbackForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send Feedback - Sujan Solution Deployer";
            this.grpRating.ResumeLayout(false);
            this.grpRating.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblFeedbackType;
        private System.Windows.Forms.ComboBox cmbFeedbackType;
        private System.Windows.Forms.GroupBox grpRating;
        private System.Windows.Forms.RadioButton rbExcellent;
        private System.Windows.Forms.RadioButton rbGood;
        private System.Windows.Forms.RadioButton rbAverage;
        private System.Windows.Forms.RadioButton rbPoor;
        private System.Windows.Forms.RadioButton rbVeryPoor;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.LinkLabel linkViewGitHub;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnCancel;
    }
}
