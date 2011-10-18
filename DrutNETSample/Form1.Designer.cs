namespace DrutNETSample
{
    partial class Form1
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
            this.button_save = new System.Windows.Forms.Button();
            this.button_load = new System.Windows.Forms.Button();
            this.label_nid = new System.Windows.Forms.Label();
            this.textBox_nodeID = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox_fieldName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_fileNode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_upload = new System.Windows.Forms.Button();
            this.button_browse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_logout_curl = new System.Windows.Forms.Button();
            this.button_login_curl = new System.Windows.Forms.Button();
            this.richTextBox_messages = new System.Windows.Forms.RichTextBox();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_url = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox_sessionID = new System.Windows.Forms.CheckBox();
            this.button_login_services = new System.Windows.Forms.Button();
            this.button_logout_services = new System.Windows.Forms.Button();
            this.textBox_endpoint = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(230, 20);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 1;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(149, 20);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(75, 23);
            this.button_load.TabIndex = 1;
            this.button_load.Text = "Load node";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // label_nid
            // 
            this.label_nid.AutoSize = true;
            this.label_nid.Location = new System.Drawing.Point(28, 26);
            this.label_nid.Name = "label_nid";
            this.label_nid.Size = new System.Drawing.Size(26, 13);
            this.label_nid.TabIndex = 3;
            this.label_nid.Text = "NID";
            // 
            // textBox_nodeID
            // 
            this.textBox_nodeID.Location = new System.Drawing.Point(60, 23);
            this.textBox_nodeID.Name = "textBox_nodeID";
            this.textBox_nodeID.Size = new System.Drawing.Size(83, 20);
            this.textBox_nodeID.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(5, 102);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(703, 332);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.webBrowser1);
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Controls.Add(this.textBox_nodeID);
            this.tabPage1.Controls.Add(this.button_save);
            this.tabPage1.Controls.Add(this.label_nid);
            this.tabPage1.Controls.Add(this.button_load);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(695, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Node load/save";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Preview";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(6, 176);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(681, 119);
            this.webBrowser1.TabIndex = 5;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(6, 53);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(682, 104);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.progressBar1);
            this.tabPage2.Controls.Add(this.textBox_fieldName);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.textBox_fileNode);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.button_upload);
            this.tabPage2.Controls.Add(this.button_browse);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(695, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "File upload";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 121);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(635, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // textBox_fieldName
            // 
            this.textBox_fieldName.Location = new System.Drawing.Point(256, 75);
            this.textBox_fieldName.Name = "textBox_fieldName";
            this.textBox_fieldName.Size = new System.Drawing.Size(125, 20);
            this.textBox_fieldName.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(166, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "CCK Field Name";
            // 
            // textBox_fileNode
            // 
            this.textBox_fileNode.Location = new System.Drawing.Point(96, 75);
            this.textBox_fileNode.Name = "textBox_fileNode";
            this.textBox_fileNode.Size = new System.Drawing.Size(55, 20);
            this.textBox_fileNode.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Attache to node";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(67, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(568, 20);
            this.textBox1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "File Name";
            // 
            // button_upload
            // 
            this.button_upload.Location = new System.Drawing.Point(519, 78);
            this.button_upload.Name = "button_upload";
            this.button_upload.Size = new System.Drawing.Size(126, 23);
            this.button_upload.TabIndex = 0;
            this.button_upload.Text = "Upload";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.button_upload_Click);
            // 
            // button_browse
            // 
            this.button_browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_browse.Location = new System.Drawing.Point(640, 36);
            this.button_browse.Name = "button_browse";
            this.button_browse.Size = new System.Drawing.Size(37, 23);
            this.button_browse.TabIndex = 0;
            this.button_browse.Text = "...";
            this.button_browse.UseVisualStyleBackColor = true;
            this.button_browse.Click += new System.EventHandler(this.button_browse_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 437);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Log messages";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_logout_curl
            // 
            this.button_logout_curl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_logout_curl.Location = new System.Drawing.Point(605, 58);
            this.button_logout_curl.Name = "button_logout_curl";
            this.button_logout_curl.Size = new System.Drawing.Size(92, 22);
            this.button_logout_curl.TabIndex = 8;
            this.button_logout_curl.Text = "Logout CURL";
            this.button_logout_curl.UseVisualStyleBackColor = true;
            this.button_logout_curl.Click += new System.EventHandler(this.button_logout_curl_Click);
            // 
            // button_login_curl
            // 
            this.button_login_curl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_login_curl.Location = new System.Drawing.Point(605, 30);
            this.button_login_curl.Name = "button_login_curl";
            this.button_login_curl.Size = new System.Drawing.Size(92, 22);
            this.button_login_curl.TabIndex = 6;
            this.button_login_curl.Text = "Login to CURL";
            this.button_login_curl.UseVisualStyleBackColor = true;
            this.button_login_curl.Click += new System.EventHandler(this.button_login_curl_Click);
            // 
            // richTextBox_messages
            // 
            this.richTextBox_messages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox_messages.Location = new System.Drawing.Point(0, 453);
            this.richTextBox_messages.Name = "richTextBox_messages";
            this.richTextBox_messages.Size = new System.Drawing.Size(709, 143);
            this.richTextBox_messages.TabIndex = 8;
            this.richTextBox_messages.Text = "";
            // 
            // textBox_userName
            // 
            this.textBox_userName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox_userName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.textBox_userName.Location = new System.Drawing.Point(78, 32);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(99, 20);
            this.textBox_userName.TabIndex = 1;
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(78, 61);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(99, 20);
            this.textBox_password.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "User Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Password";
            // 
            // textBox_url
            // 
            this.textBox_url.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_url.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_url.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.textBox_url.Location = new System.Drawing.Point(78, 6);
            this.textBox_url.Name = "textBox_url";
            this.textBox_url.Size = new System.Drawing.Size(619, 20);
            this.textBox_url.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Drupal URL";
            // 
            // checkBox_sessionID
            // 
            this.checkBox_sessionID.AutoSize = true;
            this.checkBox_sessionID.Location = new System.Drawing.Point(500, 86);
            this.checkBox_sessionID.Name = "checkBox_sessionID";
            this.checkBox_sessionID.Size = new System.Drawing.Size(99, 17);
            this.checkBox_sessionID.TabIndex = 4;
            this.checkBox_sessionID.Text = "Use Session ID";
            this.checkBox_sessionID.UseVisualStyleBackColor = true;
            // 
            // button_login_services
            // 
            this.button_login_services.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_login_services.Location = new System.Drawing.Point(499, 58);
            this.button_login_services.Name = "button_login_services";
            this.button_login_services.Size = new System.Drawing.Size(100, 22);
            this.button_login_services.TabIndex = 5;
            this.button_login_services.Text = "Login to Services";
            this.button_login_services.UseVisualStyleBackColor = true;
            this.button_login_services.Click += new System.EventHandler(this.button_login_Click);
            // 
            // button_logout_services
            // 
            this.button_logout_services.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_logout_services.Location = new System.Drawing.Point(499, 30);
            this.button_logout_services.Name = "button_logout_services";
            this.button_logout_services.Size = new System.Drawing.Size(100, 22);
            this.button_logout_services.TabIndex = 7;
            this.button_logout_services.Text = "Logout Services";
            this.button_logout_services.UseVisualStyleBackColor = true;
            this.button_logout_services.Click += new System.EventHandler(this.button_logout_Click);
            // 
            // textBox_endpoint
            // 
            this.textBox_endpoint.Location = new System.Drawing.Point(253, 32);
            this.textBox_endpoint.Name = "textBox_endpoint";
            this.textBox_endpoint.Size = new System.Drawing.Size(100, 20);
            this.textBox_endpoint.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(195, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "End point";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 596);
            this.Controls.Add(this.textBox_endpoint);
            this.Controls.Add(this.button_login_services);
            this.Controls.Add(this.button_login_curl);
            this.Controls.Add(this.button_logout_services);
            this.Controls.Add(this.button_logout_curl);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.checkBox_sessionID);
            this.Controls.Add(this.richTextBox_messages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_url);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_userName);
            this.MinimumSize = new System.Drawing.Size(725, 583);
            this.Name = "Form1";
            this.Text = "DrutNet API sample for drupal7";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Label label_nid;
        private System.Windows.Forms.TextBox textBox_nodeID;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_browse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_upload;
        private System.Windows.Forms.Button button_logout_curl;
        private System.Windows.Forms.Button button_login_curl;
        private System.Windows.Forms.RichTextBox richTextBox_messages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBox_fileNode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_url;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox_sessionID;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox_fieldName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_login_services;
        private System.Windows.Forms.Button button_logout_services;
        private System.Windows.Forms.TextBox textBox_endpoint;
        private System.Windows.Forms.Label label9;
    }
}

