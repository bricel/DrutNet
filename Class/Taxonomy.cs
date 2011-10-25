using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DrutNET
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
    public class TaxonomyVocabulary
    {
        string _name;
        int _vid;
        bool _required = false;
        bool _multiple = false;
        bool _freeTags = false;
        List<TaxonomyTerm> _terms = new List<TaxonomyTerm>();
       // private List<string> _contentTypes = new List<string>();
        Services _servicesCon;
        public TaxonomyVocabulary(XmlRpcStruct vocLib,Services servicesCon)
        {
            _servicesCon = servicesCon;
            _vid = Convert.ToInt32(vocLib["vid"]);
            _name = (vocLib["name"]).ToString();

            if (vocLib["required"].ToString() == "1")
                _required = true;
            if (vocLib["multiple"].ToString() == "1")
                _multiple = true;
            if (vocLib["tags"].ToString() == "1")
                _freeTags = true;
            // Find what content type is valid for this vocab.
          /*  foreach (string ct in Enum.GetValues(typeof(Enums.ContentType)))
            {
                try
                {
                    if (((vocLib["nodes"] as XmlRpcStruct))[StringEnum.StrVal(ct)] != null)
                        _contentTypes.Add(ct);

                        
                }
                catch { }
            }*/
            //save terms
            XmlRpcStruct[] terms = servicesCon.TaxonomyGetTree(_vid);
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
     /*   public bool HasContentType(Enums.ContentType contentType)
        {
            if (contentType == Enums.ContentType.Undefined)
                return true;
            else
                return _contentTypes.Contains(contentType);
        }*/
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
}
