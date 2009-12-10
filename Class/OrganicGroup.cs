using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace DrutNET
{
    public class OrganicGroup : DrutNETBase
    {
        string _name;
        int _ID;
        bool _isAdmin;
        List<TaxonomyVocabulary> _vocabularies;
        Services _servicesCon;
        /// <summary>
        /// Create group without reading vocab
        /// </summary>
        public OrganicGroup(int getID, string getName, bool getIsAdmin,Services servicesCon)
        {
            _servicesCon = servicesCon;
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
                sendLogEvent("Can't parse section of vocabulary : " + ex.Message,"Organic Group",Enums.MessageType.Error);
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
                XmlRpcStruct vocabGroups = _servicesCon.OGgetVocab(groupID);
                if (vocabGroups != null)
                {
                    foreach (XmlRpcStruct vocLib in vocabGroups.Values)//support for mutiple groups
                        _vocabularies.Add(new TaxonomyVocabulary(vocLib,_servicesCon));
                }
            }
            catch (Exception ex)
            {
                sendLogEvent("Can't parse vocabulary : " + ex.Message, "Organic Group",Enums.MessageType.Error);
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
