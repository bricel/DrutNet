using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrutNet;

namespace DrutNet
{
    public partial class BaseForm : Form
    {
         public  Properties.Settings _settings;
         public Properties.Settings Settings
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
        
        protected void SaveOpenSaveOptions(bool promptOverWrite,
            bool openContInBrowser, bool useContTitle, bool useNodeD)
        {
            //save last dir
            _settings.PromptOverWrite = promptOverWrite;
            _settings.OpenContInBrowser = openContInBrowser;
            _settings.UseContTitle = useContTitle;
            _settings.UseNodeD = useNodeD;
            _settings.Save();
        }
    }
}
