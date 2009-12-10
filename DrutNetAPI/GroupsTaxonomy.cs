using System;
using System.Collections.Generic;
using System.Text;
using SeasideResearch.LibCurlNet;
using CookComputing.XmlRpc;
using System.Collections;
using DrutNet;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DrutNet
{
    /// <summary>
    /// represent one term
    /// </summary>
    public class TaxonomyTerm
    {
        string _name;
        int _tid;
        bool _isSelected = false;
        ListViewItem _lvItem; //the item in the list view
        public ListViewItem LvItem { set { _lvItem = value; } }
        public TaxonomyTerm(XmlRpcStruct term)
        {
            _tid = Convert.ToInt32(term["tid"]);
            _name = term["name"].ToString();
        }
        public TaxonomyTerm(string termName)
        {
            _tid = -1; //new tag no id
            _name = termName;
        }
        public string Name { get { return _name; } }
        public int TID { get { return _tid; } }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (_lvItem != null)
                {
                    if (_isSelected)
                        _lvItem.Checked = true;
                    else
                        _lvItem.Checked = false;
                }
            }
        }
    }
    /// <summary>
    /// represent a vocabulary category
    /// </summary>
    public class TaxonomyVocabulary : IConnection
    {
        string _name;
        int _vid;
        bool _required = false;
        bool _multiple = false;
        bool _freeTags = false;
        List<TaxonomyTerm> _terms = new List<TaxonomyTerm>();
        private List<Enums.ContentType> _contentTypes = new List<Enums.ContentType>();
        /* public TaxonomyVocabulary(int getVID, string getName, bool getRequired,
             bool getMutiple, bool getFreeTags, List<int> selectedTagIDs, List<Enums.ContentType> getContentTypes)
         {
             _name = getName;
             _vid = getVID;
             _required = getRequired;
             _multiple = getMutiple;
             _freeTags = getFreeTags;
             _contentTypes = getContentTypes;

             //load terms
             XmlRpcStruct[] terms = DrupServ.TaxonomyGetTree(_vid);
             //save terms
             foreach (XmlRpcStruct tag in terms)
             {
                 int currentTagID = Convert.ToInt32(tag["tid"]);
                 bool selected = false;
                 foreach (int id in selectedTagIDs)//search if selected
                     if (id == currentTagID)
                     {
                         selected = true;
                         continue;
                     }
                 _terms.Add(new TaxonomyTerms(currentTagID, tag["name"].ToString(), selected));
             }
         }*/
        public TaxonomyVocabulary(XmlRpcStruct vocLib)
        {
            _vid = Convert.ToInt32(vocLib["vid"]);
            _name = (vocLib["name"]).ToString();

            if (vocLib["required"].ToString() == "1")
                _required = true;
            if (vocLib["multiple"].ToString() == "1")
                _multiple = true;
            if (vocLib["tags"].ToString() == "1")
                _freeTags = true;
            //find what content type is valid for this vocab
            foreach (Enums.ContentType ct in Enum.GetValues(typeof(Enums.ContentType)))
            {
                try
                {
                    if (((vocLib["nodes"] as XmlRpcStruct))[StringEnum.StrVal(ct)] != null)
                        _contentTypes.Add(ct);
                }
                catch { }//catch argument null expetion -->'System.ArgumentNullException'
            }
            //save terms
            XmlRpcStruct[] terms = DrupServ.TaxonomyGetTree(_vid);
            foreach (XmlRpcStruct term in terms)
                _terms.Add(new TaxonomyTerm(term));
        }
        public string Name { get { return _name; } }
        public int VID { get { return _vid; } }
        public List<TaxonomyTerm> Terms { get { return _terms; } /*set { _terms = value; } */}
        public bool Required
        {
            get { return _required; }
            // set { _required = value; }
        }
        public bool Multiple
        {
            get { return _multiple; }
            // set { _required = value; }
        }
        public bool FreeTags
        {
            get { return _freeTags; }
            // set { _required = value; }
        }
        /// <summary>
        /// Return true if the vocabulaty is defined for the given content type
        /// </summary>
        public bool HasContentType(Enums.ContentType contentType)
        {
            if (contentType == Enums.ContentType.Undefined)
                return true;
            else
                return _contentTypes.Contains(contentType);
        }
        public TaxonomyTerm FindTerm(int tid)
        {
            return _terms.Find(delegate(TaxonomyTerm t) { return t.TID == tid; });
        }
        /// <summary>
        /// if tag is mandatory check that one tag is selected
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            if (Required)
            {
                foreach (TaxonomyTerm term in this.Terms)
                    if (term.IsSelected)
                        return true;
            }
            else
                return true;

            return false;
        }
    }
    public class OrganicGroup : IConnection
    {
        string _name;
        int _ID;
        bool _isAdmin;
        List<TaxonomyVocabulary> _vocabularies;
        /// <summary>
        /// Create group without reading vocab
        /// </summary>
        public OrganicGroup(int getID, string getName, bool getIsAdmin)
        {
            _name = getName;
            _ID = getID;
            _isAdmin = getIsAdmin;
        }
        /// <summary>
        /// Ovelerload that reads vocabs for the group too
        /// </summary>
        public void LoadVocabulary()
        {
            getOrganicGroupVocabularies(_ID);
        }
        public string Name { get { return _name; } }
        public int ID { get { return _ID; } }
        public bool IsAdmin { get { return _isAdmin; } }
        public List<TaxonomyVocabulary> VocabularyLists { get { return (_vocabularies); } }
        //private List<ListViewItem> freeTagsLVIs = new List<ListViewItem>();
        public TaxonomyVocabulary FindVocabulary(int vid)
        {
            return _vocabularies.Find(delegate(TaxonomyVocabulary v) { return v.VID == vid; });
        }
        public void SetSelectedTags(XmlRpcStruct selectedTags)
        {
            try
            {
                //read selected ids
                if (selectedTags != null) //read selected tag for the content
                    foreach (XmlRpcStruct tagStruct in selectedTags.Values)//support for mutiple groups
                        FindVocabulary(Convert.ToInt32(tagStruct["vid"])).FindTerm(Convert.ToInt32(tagStruct["tid"])).IsSelected = true;
            }
            catch (Exception ex)
            {
                errorMessage("Can't parse section of vocabulary : " + ex.Message, Enums.MessageSender.API_OG_SetTag);
            }
        }
        /// <summary>
        /// reads and save vocabulary from xmlRCP struct
        /// </summary>
        private void getOrganicGroupVocabularies(int groupID)
        {
            _vocabularies = new List<TaxonomyVocabulary>();
            try
            {
                XmlRpcStruct vocabGroups = DrupServ.OGgetVocab(groupID);
                if (vocabGroups != null)
                {
                    foreach (XmlRpcStruct vocLib in vocabGroups.Values)//support for mutiple groups
                        _vocabularies.Add(new TaxonomyVocabulary(vocLib));
                }
            }
            catch (Exception ex)
            {
                errorMessage("Can't parse vocabulary : " + ex.Message,Enums.MessageSender.API_OG_GetVocab);
            }
        }
        /// <summary>
        /// return an array of Organic groups, this will return groups where the user is an admin
        /// </summary>
        /// <param name="isAdmin">return only groups where user is admin  </param>
        public static OrganicGroup[] AdminOrganicGroupsArray(List<OrganicGroup> ogList)
        {
            List<OrganicGroup> adminGroups = new List<OrganicGroup>();
            foreach (OrganicGroup g in ogList)
            {
                if (g._isAdmin)
                    adminGroups.Add(g);
            }
            return adminGroups.ToArray();
        }
        /// <summary>
        /// Fill a listView with the groups tags
        /// </summary>
        /// <param name="listViewTags"></param>
        public void LoadVocabInListView(ListView listViewTags, Enums.ContentType contentType)
        {
            listViewTags.Items.Clear();
            listViewTags.Groups.Clear();
            if (this.VocabularyLists != null)
            {
                //add an event to handle multiple/single selection
                listViewTags.ItemChecked -= new ItemCheckedEventHandler(listViewTags_ItemChecked);

                foreach (TaxonomyVocabulary tagList in this.VocabularyLists)
                {
                    if ((tagList.Terms.Count > 0) && (tagList.HasContentType(contentType)))
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
        public bool ValidateTags()
        {
            foreach (TaxonomyVocabulary vocab in this.VocabularyLists)
            {
                if (!vocab.Validate())
                    return false;
            }
            return true;
        }
    }
}
