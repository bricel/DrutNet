using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrutNET;
using CookComputing.XmlRpc;
namespace DrutNETSample
{
    public partial class Form1 : Form
    {
        XmlRpcStruct _node;
        Services _serviceCon;
        Curl _curlCon;
        public Form1()
        {
            InitializeComponent();
            // This handle all message throw by the system.
            DrutNETBase.OnUpdateLog += new DrutNETBase.UpdateLog(DrutNETBase_OnUpdateLog);
            
            // Create a settings object to define connection settings.
            ServicesSettings settings = new ServicesSettings();
            settings.DrupalURL = "http://localhost/DrutNet/drupal-sqlite/";
            settings.CleanURL = true;
            settings.UseSessionID = false;
           
            /*settings.UseKeys = true;//Not Implemented yet
            //settings.Key = "03cfd62180a67dcbcb1be9a7f78dc726";
            settings.DomainName = "localhost";*/
            
            // Create a connection object
            _serviceCon = new Services(settings);
                       
            // Login to drupal
            _serviceCon.Login("demo", "1234");

            _curlCon = new Curl(settings.DrupalURL);
            _curlCon.Login("demo","1234");

        }
        void DrutNETBase_OnUpdateLog(string str, string mSender, Enums.MessageType mType, bool verbose)
        {
            // Write log messages on error window.
            textBox_message.Text +=mType.ToString() + " - " + mSender + ": " + str + "\n";
        }
        /// <summary>
        /// Fill a listView with the groups tags
        /// </summary>
        /// <param name="listViewTags"></param>
        public void LoadVocabInListView(ListView listViewTags, string contentType,List<TaxonomyVocabulary> VocabularyLists )
        {
            listViewTags.Items.Clear();
            listViewTags.Groups.Clear();
            if (VocabularyLists != null)
            {
                //add an event to handle multiple/single selection
                listViewTags.ItemChecked -= new ItemCheckedEventHandler(listViewTags_ItemChecked);

                foreach (TaxonomyVocabulary tagList in VocabularyLists)
                {
                    if ((tagList.Terms.Count > 0))// && (tagList.HasContentType(contentType)))
                    {
                        string displayName = tagList.Name;
                        if (tagList.Required)
                            displayName += " *"; //add a star next to group name when is a required group
                        ListViewGroup g = listViewTags.Groups.Add(tagList.Name, displayName);
                        g.Tag = tagList;
                        foreach (TaxonomyTerm tag in tagList.Terms)
                        {
                            ListViewItem lvItem = new ListViewItem(tag.Name, g);
                            lvItem.Tag = tag;//save the reated object
                            tag.LvItem = lvItem; //used to auto update the check box when select property changes
                            listViewTags.Items.Add(lvItem);
                            lvItem.Checked = tag.IsSelected;
                        }
                        if (tagList.FreeTags)
                        {
                            addNewFreeTerm(listViewTags, g);
                            /*  ListViewItem lvItem = new ListViewItem("**Click to add term", g);
                              lvItem.Tag = "free";
                              listViewTags.Items.Add(lvItem);*/
                            // freeTagsLVIs.Add(lvItem);
                        }
                    }
                }
                listViewTags.ItemChecked += new ItemCheckedEventHandler(listViewTags_ItemChecked);
                listViewTags.AfterLabelEdit += new LabelEditEventHandler(listViewTags_AfterLabelEdit);
                listViewTags.Click += new EventHandler(listViewTags_Click);

            }
        }
        /// <summary>
        /// free tag was click - start edit
        /// </summary>
        void listViewTags_Click(object sender, EventArgs e)
        {
            if ((sender as ListView).FocusedItem.Tag.ToString() == "free")
            {
                (sender as ListView).LabelEdit = true;
                (sender as ListView).FocusedItem.BeginEdit();
            }
        }
        void addNewFreeTerm(ListView lv, ListViewGroup group)
        {
            //add new free tag 
            ListViewItem lvItem = new ListViewItem("**Click to add term", group);
            lvItem.Tag = "free";
            lv.Items.Add(lvItem);
        }
        void listViewTags_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            (sender as ListView).LabelEdit = false;

            //add new term to taxonomy
            //  (sender as ListView).FocusedItem.
            TaxonomyTerm tempTerm = new TaxonomyTerm(e.Label);
            tempTerm.LvItem = (sender as ListView).FocusedItem;
            tempTerm.IsSelected = true;
            ((sender as ListView).FocusedItem.Group.Tag as TaxonomyVocabulary).Terms.Add(tempTerm);
            (sender as ListView).FocusedItem.Tag = tempTerm;

            //add new free tag 
            addNewFreeTerm((sender as ListView), (sender as ListView).FocusedItem.Group);

            /* ListViewItem lvItem = new ListViewItem("**Click to add term", (sender as ListView).FocusedItem.Group);
             lvItem.Tag = "free";
              (sender as ListView).Items.Add(lvItem);*/
        }
        /// <summary>
        /// handle single selection property for terms in a groups, and update the property of checked items
        /// </summary>
        void listViewTags_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item != null)
            {
                if (e.Item.Tag.ToString() == "free")
                {
                    return;
                }
                else if (e.Item.Checked)
                {
                    (e.Item.Tag as TaxonomyTerm).IsSelected = true;
                    if (!((e.Item.Group.Tag as TaxonomyVocabulary).Multiple))
                        foreach (ListViewItem lvi in e.Item.Group.Items)//if not multiple select, deselect all others
                            if ((lvi != e.Item) && (lvi.Index != -1) && (lvi.Tag.ToString() != "free"))
                            {
                                //lvi.Checked = false;
                                (lvi.Tag as TaxonomyTerm).IsSelected = false;
                            }

                }
                else //e.Item.Checked == false
                {
                    (e.Item.Tag as TaxonomyTerm).IsSelected = false;
                }
            }
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            textBox_message.Text = "";
            // Node to load
            _node = _serviceCon.NodeGet(Convert.ToInt32(textBox_nodeID.Text));
            if (_node != null)
                richTextBox1.Text = _node["body"].ToString();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            textBox_message.Text = "";
            if (_node != null)
            {   
                // Reload node to prevent access restriction, by other user
                _node = _serviceCon.NodeGet(Convert.ToInt32(textBox_nodeID.Text));
                //update node
                _node["body"] = richTextBox1.Text;
                _serviceCon.NodeSave(_node);
            }
            else
                DrutNETBase.sendLogEvent("No node was loaded, load a node first", "My Sample", Enums.MessageType.Error);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = richTextBox1.Text;
           // webBrowser1.Refresh();
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
            _curlCon.UploadFile(textBox1.Text);
           
           
        }

        private void button_browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }
    }
}
