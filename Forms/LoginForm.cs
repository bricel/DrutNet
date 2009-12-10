using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrutNET;
using System.Threading;

namespace DrutNET
{
    public partial class LoginForm : DrutNET.BaseForm
    {
        User _user;
        bool closing = false;
        bool _confirmExit;
        string _httpPackageLink="";
        delegate void writeLogCallback(string text, string mSender, Enums.MessageType mType,bool verbose);
        public LoginForm(bool confirmExit, User user, bool showIcon, string httpPackageLink)
            : this(confirmExit, showIcon, httpPackageLink)
        {
            _user = user;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="confirmExit"></param>
        /// <param name="httpPackageLink">upgrade link</param>
        public LoginForm(bool confirmExit, string httpPackageLink)
            : this(confirmExit, false, httpPackageLink)
        {
        }
        /// <summary>
        /// Open the login form, and check for updates online
        /// </summary>
        /// <param name="confirmExit"></param>
        /// <param name="loadUserVocabs"></param>
        /// <param name="showIcon"></param>
        /// <param name="httpPackageLink"></param>
        public LoginForm(bool confirmExit, bool showIcon, string httpPackageLink)
        {

               _httpPackageLink = httpPackageLink;
                InitializeComponent();
                DrutNET.DrutNETBase.OnUpdateLog += new DrutNET.DrutNETBase.UpdateLog(writeMessage);
                this._confirmExit = confirmExit;
                if (showIcon)
                {
                    this.ShowInTaskbar = true;
                    this.StartPosition = FormStartPosition.CenterScreen;
                }
                else
                {
                    this.ShowInTaskbar = false;
                    this.StartPosition = FormStartPosition.CenterParent;
                }
               
        }
        private void writeMessage(string text, string mSender, Enums.MessageType mType, bool verbose)
        {
            //Dealing with threads 
            if (this.txbMessage.InvokeRequired)
            {
                writeLogCallback d = new writeLogCallback(writeMessage);
                this.Invoke(d, new object[] { text, mSender, mType });
            }
            else
            {
                if ((this.Visible)&&(!verbose)) //don't show verbose
                    txbMessage.Text += text + "\n ";
            }
        }
        private void LoginForm_Shown(object sender, EventArgs e)
        {
            // Save settings in settings file
            txbMessage.Text = "";
            txbUserName.Text = Settings.Username;
            txbPassword.Text = Settings.Password;
            checkBoxSavePass.Checked = Settings.SavePass;
            if (Settings.Username != "")//set password textbox active if name is already filled
                this.ActiveControl = txbPassword;
            closing = false;
        }
        /// <summary>
        /// username used to login last time , is also reset automatically to last saved username
        /// </summary>
        public string UserName { get { return txbUserName.Text; } }
        /// <summary>
        /// User instance of last login, is null when not logged in
        /// </summary>
        public User User { get { return _user; } }
        public string Password { get { return txbPassword.Text; } }
        public bool LoggedIn
        {
            get
            {
                if (_user == null) //first try to login
                    return false;
                else
                    return _user.IsLoggedIn;

            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }
        private void Login()
        {
            txbMessage.Text = "";
            try
            {
                
                bool loginRes = false;
                this.Cursor = Cursors.WaitCursor;
                writeMessage("Connecting...\n", "Login Form", Enums.MessageType.Error,false);
                if (_user == null) //first try to login
                {
                    //TODO: add server UTL box to form
                    _user = new User("");
                    loginRes = _user.Login(UserName, Password);//login with new user
                }
                else //second try or more or login as differnent user
                {
                    loginRes = _user.Login(UserName, Password);//login with new user
                }
                if (loginRes)
                {
                    if (!_user.LoadUser())
                        writeMessage(" Cannot load user", "Login Form", Enums.MessageType.Error, false);
                    else
                    {
                        SaveUserSettings(UserName, Password, checkBoxSavePass.Checked);
                        this.DialogResult = DialogResult.OK;
                        closing = true;
                        this.Close();
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                txbMessage.Text += ex.Message + "\n";
                this.Cursor = Cursors.Default;

            }
        }
        private void txbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                Login();
        }
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {

            if ((m.Msg != 0x0010) || (closing) || (!_confirmExit))//case i already ask to close
            {
              //  this.DialogResult = DialogResult.Abort;
                base.WndProc(ref m);
            }
            else
            {
                DialogResult d = DialogResult.Cancel;
                d = MessageBox.Show(this,
                    "Do you want to exit ?", "Warning",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (d == DialogResult.OK)
                {
                    SaveUserSettings(UserName,Password,checkBoxSavePass.Checked);
                    this.DialogResult = DialogResult.Abort;
                    closing = true;
                    this.Close();
                }
            }

        }




        private void txbUserName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control)
                if (e.KeyCode == Keys.Q)
                {
                    this.DialogResult = DialogResult.Ignore;
                    closing = true;
                    this.Close();
                }

        }

    }
    
}
