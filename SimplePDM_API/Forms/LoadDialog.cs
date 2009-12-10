using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SimplePDM;
using System.Threading;
using System.IO;

namespace SimplePDM
{
    public partial class LoadContentDialog : BaseForm
    {
        SimplePDMContent _content;
        SimplePDMUser _user;
        Enums.ContentType _contentType = Enums.ContentType.Undefined;
        AboutBoxSimplePDM aboutBox = new AboutBoxSimplePDM();
        string ContentString
        {
            get
            {
                if (_contentType == Enums.ContentType.Undefined)
                    return "";
                else
                    return _contentType.ToString();
            }
        }
        string actionString = "";
        string _errorMsg = "";
        bool _PDSplugin = false;
        string _localFolder = Enums.DOCUMENTFOLDER;
        bool _v10_2 = false;
        string _autoCompleteFile = Enums.DOCUMENTFOLDER + "autocomplete.txt";
        public SimplePDMContent ContentRef
        {
            get { return _content; }
        }
        /// <summary>
        /// Display a dialog with save option for the content
        /// </summary>
        /// <param name="content">Content type to save/load, will return the save/loaded as object too</param>
        /// <param name="saveForm">define if the dialog act as save or load</param>
        public LoadContentDialog(bool V10_2,bool pdsPlugin)
        {
            InitializeComponent();
            
            actionString = "Loading";

            SimplePDM.BaseSimplePDM.OnUpdateLog += new SimplePDM.BaseSimplePDM.UpdateLog(writeMessage);
           // txbLoadNodeID.AutoCompleteCustomSource.AddRange(BaseSimplePDM.ReadAutoCompleteFile(_autoCompleteFile));
            _v10_2 = V10_2;
            _PDSplugin = pdsPlugin;
            BaseSimplePDM.OnSubTaskDone += new BaseSimplePDM.SubTaskDoneDel(BaseSimplePDM_OnSubTaskDone);
            if (!pdsPlugin)
                this.ShowInTaskbar = true;
            else
                this.ShowInTaskbar = false;


        }
        public bool DisplayLoadDialog(SimplePDMUser user, Enums.ContentType contentType, string folderPath)
        {
            if (!_PDSplugin)
                this.StartPosition = FormStartPosition.CenterScreen;
            _user = user;
            _contentType = contentType;
            _localFolder = folderPath;
            return (this.ShowDialog() == DialogResult.OK);
        }
        #region user interface methods
        private void SaveLoadContentDialog_Shown(object sender, EventArgs e)
        {
            Settings.Reload();
            txbLoadNodeID.AutoCompleteCustomSource.Clear();
            txbLoadNodeID.AutoCompleteCustomSource.AddRange(BaseSimplePDM.ReadAutoCompleteFile(_autoCompleteFile));
            openStyleInBrowserAfterSaveToolStripMenuItem.Checked = Settings.OpenContInBrowser;
            promptOverwriteToolStripMenuItem.Checked = Settings.PromptOverWrite;
            useNodeIDToolStripMenuItem.Checked = Settings.UseNodeD;
            useStyleTitleToolStripMenuItem.Checked = Settings.UseContTitle;

            buttonSaveStyle.Text = "Load " + ContentString;//move load button up
            updateLabel1(false);

            txbMessages.Text = "";
            this.txbLoadNodeID.Text = "";
            writeLocalLog("");
            //display group
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(OrganicGroup.AdminOrganicGroupsArray(_user.OrganicGroups));
            comboBox1.DisplayMember = "Name";
            if ((comboBox1.SelectedItem == null) && (comboBox1.Items.Count > 0))
            {
                comboBox1.SelectedIndex = 0;
                //_user.OrganicGroups[0].LoadVocabInListView(listViewTags);//moved to group selection event changed 
            }
            this.buttonSaveStyle.Enabled = true;
            this.txbLoadNodeID.Focus();

        }
        private void updateLabel1(bool switchState)
        {
            if (switchState)
            {
                useNodeIDToolStripMenuItem.Checked = !useNodeIDToolStripMenuItem.Checked;
                useStyleTitleToolStripMenuItem.Checked = !useStyleTitleToolStripMenuItem.Checked;
            }
            if (useStyleTitleToolStripMenuItem.Checked)
            {
                label1.Text = ContentString + " Title";
                comboBox1.Enabled = true;
            }
            else
            {
                label1.Text = "Node ID";
                comboBox1.Enabled = false;
            }
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateLabel1(true);
        }
        delegate void writeLogCallback(string text);
        void writeLocalLog(string msg)
        {
            //Dealing with threads 
            if (this.InvokeRequired)
            {
                writeLogCallback d = new writeLogCallback(writeLocalLog);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                txbMessages.Text = msg;// +"\n";
            }
        }
        void writeMessage(string str, Enum mSender, Enums.MessageType mType)
        {
            if ((mType == Enums.MessageType.Error))
            {
                _errorMsg = str;
                writeLocalLog(str);
            }
        }
        #endregion
        void BaseSimplePDM_OnSubTaskDone(object task, Enums.SubTaskName subTask)
        {
            if (subTask == Enums.SubTaskName.LoadSaveDialogSaveDone)
                if ((bool) task)
                     this.DialogResult = DialogResult.OK;//closes the dialog too
            buttonSaveStyle.Enabled = true;

        }
        private void buttonLoadTags_Click(object sender, EventArgs e)
        {
            //load of current status of existing selected tags
            if (validateTitleNid(txbLoadNodeID.Text))
                _content = loadContentInfo(_contentType, _user, (OrganicGroup)comboBox1.SelectedItem, txbLoadNodeID.Text);
        }
        /// <summary>
        /// Check if title or id is not empty and display an error message when needed
        /// </summary>
        /// <returns></returns>
        private bool validateTitleNid(string txbContent)
        {
            if (txbContent == "") //nothing in textbox
            {
                if (useStyleTitleToolStripMenuItem.Checked)
                    _errorMsg = "Please type a " + ContentString + " title ";
                else
                    _errorMsg = "Please type a node ID";
                return false;
            }
            else
            {
                if (useNodeIDToolStripMenuItem.Checked)
                {//check numbers were entered
                    try
                    {
                        Convert.ToInt32(txbContent);
                    }
                    catch
                    {
                        _errorMsg = "Please type a correct node ID";
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// load the content matching the title/nid
        /// </summary>
        private SimplePDMContent loadContentInfo(Enums.ContentType cType, SimplePDMUser user, OrganicGroup group, string txbContent)
        {
            // ****** use node ID Load/Save *********
            SimplePDMContent content = null;
            if (useNodeIDToolStripMenuItem.Checked)
            {
                content = new SimplePDMContent(cType, user);
                content.NodeID = Convert.ToInt32(txbContent);
            }
            // ****** use  title Load/Save *********
            else
            {
                //seach for title name
                List<SimplePDMContent> searchResult = SimplePDMContent.SearchContent(txbContent, group, cType, user);
                if (searchResult.Count == 0)//no result found
                    _errorMsg = "This " + ContentString + " Title doesn't exist. Try with a different Title or Group.";
                else
                    //TODO: handle multiple search result
                    content = searchResult[0];//take first choise
            }
            return content;
        }
        private void runSaveThread(string txbContent, OrganicGroup group)
        {
            writeLocalLog("");
            bool actionRes = false;
            if (validateTitleNid(txbContent))
            {
                writeLocalLog(actionString + "...");
                try
                {
                    _content = loadContentInfo(_contentType, _user, group, txbContent);
                    //********************* update/load actual actions **********************
                    if (_content != null)
                    {

                        actionRes = _content.Load(_localFolder, true, true, promptOverwriteToolStripMenuItem.Checked, this);
                        if (!((_contentType == Enums.ContentType.Undefined) || (_contentType == _content.ContentType)))
                        {
                            actionRes = false;
                            _errorMsg = "Content type requested is not a " + ContentString;
                        }

                    }
                }
                catch (Exception ex)
                {
                    writeLocalLog(ex.Message);
                }
            } // of  load
            if ((actionRes)&&(_content!=null))
            {
                if (openStyleInBrowserAfterSaveToolStripMenuItem.Checked)
                    _content.OpenContentWebPage(Enums.OpenWebPageMode.View);
            }
            else
            {
              //  if (txbMessages.Text == actionString + "...")
                
                if (_errorMsg != "")
                    writeLocalLog(_errorMsg);
                else
                    writeLocalLog("Error " + this.actionString);

            }

            BaseSimplePDM.SubTaskDone(actionRes, Enums.SubTaskName.LoadSaveDialogSaveDone);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            txbMessages.Text = "";
            buttonSaveStyle.Enabled = false;
            if (!txbLoadNodeID.AutoCompleteCustomSource.Contains(txbLoadNodeID.Text))//add new value to autocomplete list
            {
                BaseSimplePDM.WriteAutoCompleteFile(txbLoadNodeID.Text.Trim(), _autoCompleteFile);
                txbLoadNodeID.AutoCompleteCustomSource.Add(txbLoadNodeID.Text.Trim());
            }
            if (!_v10_2)
            {
                Thread saveThread = new Thread(delegate()
                { runSaveThread(txbLoadNodeID.Text.Trim(), (OrganicGroup)comboBox1.SelectedItem); });
                saveThread.Start();
            }
            else
                runSaveThread(txbLoadNodeID.Text.Trim(), (OrganicGroup)comboBox1.SelectedItem);
        }

        private void helpPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this._PDSplugin)
                 BaseSimplePDM.OpenHelpPage("SimplePDM_Bridge_Plugin_For_OptiTex_PDS");
            else
                BaseSimplePDM.OpenHelpPage("SimplePDM_Bridge_Plugin_For_OptiTex_PDS");

        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutBox.ShowDialog(this);
        }

        private void LoadContentDialog_FormClosing(object sender, FormClosingEventArgs e)
        {

            base.SaveOpenSaveOptions(
                  promptOverwriteToolStripMenuItem.Checked,
                  openStyleInBrowserAfterSaveToolStripMenuItem.Checked,
                  useStyleTitleToolStripMenuItem.Checked,
                  useNodeIDToolStripMenuItem.Checked);
        }
        
    }
}
