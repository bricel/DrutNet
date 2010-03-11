using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrutNET;

namespace DrutNET
{
    public partial class BaseForm : Form
    {
         internal Properties.Settings _settings;
         internal Properties.Settings Settings
         {
             get { return _settings; }
             //set { _settings = value; }
         }
        public BaseForm()
        {
            InitializeComponent();
            _settings = new Properties.Settings();

        }
        protected void SaveUserSettings(string userName, string password,bool savePass)
        {
            //save last dir
            _settings.Username = userName;
            if (savePass)  
                _settings.Password = password;
            else
                _settings.Password = "";

            _settings.SavePass = savePass;
            _settings.Save();
        }
    }
}
