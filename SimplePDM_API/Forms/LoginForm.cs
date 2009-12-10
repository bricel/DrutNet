﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SimplePDM;
using System.Threading;

namespace SimplePDM
{
    public partial class LoginForm : SimplePDM.BaseForm
    {
        SimplePDMUser _user;
        bool closing = false;
        bool _confirmExit;
        string _httpPackageLink="";
        delegate void writeLogCallback(string text, Enum mSender, Enums.MessageType mType);
        public LoginForm(bool confirmExit, SimplePDMUser user, bool showIcon, string httpPackageLink)
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
                SimplePDM.BaseSimplePDM.OnUpdateLog += new SimplePDM.BaseSimplePDM.UpdateLog(writeMessage);
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
        private void writeMessage(string text, Enum mSender, Enums.MessageType mType)
        {
            //Dealing with threads 
            if (this.txbMessage.InvokeRequired)
            {
                writeLogCallback d = new writeLogCallback(writeMessage);
                this.Invoke(d, new object[] { text, mSender, mType });
            }
            else
            {
                if (this.Visible)
                    txbMessage.Text += text + "\n ";
            }
        }
        private void LoginForm_Shown(object sender, EventArgs e)
        {
            if (BaseSimplePDM.checkVersionCompatibility(_httpPackageLink))
            {
                txbMessage.Text = "";
                txbUserName.Text = Settings.Username;
                txbPassword.Text = Settings.Password;
                checkBoxSavePass.Checked = Settings.SavePass;
                if (Settings.Username != "")//set password textbox active if name is already filled
                    this.ActiveControl = txbPassword;
                closing = false;
            }
            else
            {
                _confirmExit = false;
                this.Close();
            }
        }
        /// <summary>
        /// username used to login last time , is also reset automatically to last saved username
        /// </summary>
        public string UserName { get { return txbUserName.Text; } }
        /// <summary>
        /// User instance of last login, is null when not logged in
        /// </summary>
        public SimplePDMUser User { get { return _user; } }
        public string Password { get { return txbPassword.Text; } }
        public bool LoggedIn
        {
            get
            {
                if (_user == null) //first try to login
                    return false;
                else
                    return _user.LoggedIn;

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
                writeMessage("Connecting...\n", Enums.MessageSender.Other, Enums.MessageType.Error);
                if (_user == null) //first try to login
                {
                    _user = new SimplePDMUser();
                    loginRes = _user.Login(UserName, Password);//login with new user
                }
                else //second try or more or login as differnent user
                {
                    loginRes = _user.Login(UserName, Password);//login with new user
                }
                if (loginRes)
                {
                    if (!_user.LoadUser())
                        writeMessage(" Cannot load user, Contact simplePDM support.", Enums.MessageSender.Other, Enums.MessageType.Error);
                    else
                    {
                        //  loggedIn = true;
                        if (_user.OrganicGroups.Count == 0)
                            writeMessage("You are not a member of any group", Enums.MessageSender.Other, Enums.MessageType.Error);
                        else if (OrganicGroup.AdminOrganicGroupsArray(_user.OrganicGroups).Length == 0)
                            writeMessage("You don't have admin privileges to any group", Enums.MessageSender.Other, Enums.MessageType.Error);
                        else if (!_user.IsRoleMember(Enums.Roles.ContentProvider))
                            writeMessage("You don't have rights to create content", Enums.MessageSender.Other, Enums.MessageType.Error);
                        else //all ok 
                        {
                            SaveUserSettings(UserName, Password,checkBoxSavePass.Checked);
                            this.DialogResult = DialogResult.OK;
                            closing = true;
                            this.Close();
                        }
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