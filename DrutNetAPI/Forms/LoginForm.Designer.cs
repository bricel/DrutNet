namespace DrutNet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.txbUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txbMessage = new System.Windows.Forms.TextBox();
            this.checkBoxSavePass = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txbUserName
            // 
            this.txbUserName.Location = new System.Drawing.Point(71, 15);
            this.txbUserName.MaximumSize = new System.Drawing.Size(123, 20);
            this.txbUserName.MinimumSize = new System.Drawing.Size(123, 20);
            this.txbUserName.Name = "txbUserName";
            this.txbUserName.Size = new System.Drawing.Size(123, 20);
            this.txbUserName.TabIndex = 0;
            this.txbUserName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txbUserName_PreviewKeyDown);
            this.txbUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // txbPassword
            // 
            this.txbPassword.Location = new System.Drawing.Point(71, 39);
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.Size = new System.Drawing.Size(123, 20);
            this.txbPassword.TabIndex = 1;
            this.txbPassword.UseSystemPasswordChar = true;
            this.txbPassword.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txbUserName_PreviewKeyDown);
            this.txbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(15, 65);
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
            this.txbMessage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txbMessage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.txbMessage.ForeColor = System.Drawing.Color.DarkRed;
            this.txbMessage.Location = new System.Drawing.Point(15, 98);
            this.txbMessage.Multiline = true;
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.ReadOnly = true;
            this.txbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txbMessage.Size = new System.Drawing.Size(179, 63);
            this.txbMessage.TabIndex = 3;
            this.txbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // checkBoxSavePass
            // 
            this.checkBoxSavePass.AutoSize = true;
            this.checkBoxSavePass.Checked = true;
            this.checkBoxSavePass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSavePass.Location = new System.Drawing.Point(96, 71);
            this.checkBoxSavePass.Name = "checkBoxSavePass";
            this.checkBoxSavePass.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSavePass.TabIndex = 6;
            this.checkBoxSavePass.Text = "Save password";
            this.checkBoxSavePass.UseVisualStyleBackColor = true;
            this.checkBoxSavePass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPassword_KeyDown);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 170);
            this.Controls.Add(this.checkBoxSavePass);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txbMessage);
            this.Controls.Add(this.txbUserName);
            this.Controls.Add(this.txbPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(216, 208);
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
    }
}