namespace DrutNET
{
    partial class LoginForm
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
            this.txbUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txbMessage = new System.Windows.Forms.TextBox();
            this.checkBoxSavePass = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txbUserName
            // 
            this.txbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbUserName.Location = new System.Drawing.Point(68, 34);
            this.txbUserName.MinimumSize = new System.Drawing.Size(123, 20);
            this.txbUserName.Name = "txbUserName";
            this.txbUserName.Size = new System.Drawing.Size(133, 20);
            this.txbUserName.TabIndex = 0;
            this.txbUserName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txbUserName_PreviewKeyDown);
            this.txbUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // txbPassword
            // 
            this.txbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbPassword.Location = new System.Drawing.Point(68, 58);
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.Size = new System.Drawing.Size(133, 20);
            this.txbPassword.TabIndex = 1;
            this.txbPassword.UseSystemPasswordChar = true;
            this.txbPassword.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txbUserName_PreviewKeyDown);
            this.txbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 84);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 27);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // txbMessage
            // 
            this.txbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMessage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txbMessage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.txbMessage.ForeColor = System.Drawing.Color.DarkRed;
            this.txbMessage.Location = new System.Drawing.Point(12, 117);
            this.txbMessage.Multiline = true;
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.ReadOnly = true;
            this.txbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txbMessage.Size = new System.Drawing.Size(189, 63);
            this.txbMessage.TabIndex = 3;
            this.txbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // checkBoxSavePass
            // 
            this.checkBoxSavePass.AutoSize = true;
            this.checkBoxSavePass.Checked = true;
            this.checkBoxSavePass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSavePass.Location = new System.Drawing.Point(93, 90);
            this.checkBoxSavePass.Name = "checkBoxSavePass";
            this.checkBoxSavePass.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSavePass.TabIndex = 6;
            this.checkBoxSavePass.Text = "Save password";
            this.checkBoxSavePass.UseVisualStyleBackColor = true;
            this.checkBoxSavePass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "URL";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.textBoxURL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.textBoxURL.Location = new System.Drawing.Point(69, 8);
            this.textBoxURL.MinimumSize = new System.Drawing.Size(123, 20);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(133, 20);
            this.textBoxURL.TabIndex = 0;
            this.textBoxURL.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txbUserName_PreviewKeyDown);
            this.textBoxURL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 198);
            this.Controls.Add(this.checkBoxSavePass);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txbMessage);
            this.Controls.Add(this.textBoxURL);
            this.Controls.Add(this.txbUserName);
            this.Controls.Add(this.txbPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(216, 208);
            this.Name = "LoginForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txbUserName;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.TextBox txbMessage;
        private System.Windows.Forms.CheckBox checkBoxSavePass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxURL;
    }
}