using System;
using System.Collections.Generic;
using System.Text;
using SeasideResearch.LibCurlNet;
using CookComputing.XmlRpc;
using System.Collections;
using SimplePDM;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace SimplePDM
{
  
    /// <summary>
    /// represent a user role
    /// </summary>
    public class SimplePDMRole 
    {
        int _ID;
        Enums.Roles _name = Enums.Roles.None;
        public SimplePDMRole(int getID, Enums.Roles getName)
        {
            _ID = getID;
            _name = getName;
        }
        public int ID { get { return _ID; } }
        public Enums.Roles Name { get { return _name; } }

    }
    public class SimplePDMUser  : SimplePDMConnection
    {
        XmlRpcStruct _userNode;
        Enums.CadSystem _CADSystem = Enums.CadSystem.DXF;
        string _name = "Anonymous";
        List<SimplePDMRole> roles = new List<SimplePDMRole>();
        List<OrganicGroup> _organicGroups = new List<OrganicGroup>();
        public List<OrganicGroup> OrganicGroups { get { return _organicGroups; } }
        public OrganicGroup FindOrganicGroup(int id)
        {
            return _organicGroups.Find(delegate(OrganicGroup og) { return og.ID == id; });
        }
        int _uid;
        public SimplePDMUser() : base() 
        {
        }
        /// <summary>
        /// load a none logged in user info
        /// </summary>
        public SimplePDMUser(int userID): base()
        {
            _userNode = this.DrupServ.UserGet(userID);
            if (userID!=0)//anonimous
              loadUserFields(_userNode);
        }
        /// <summary>
        /// return autor name , username , but not nesserly the logged in user
        /// </summary>
        public string Name { get { return _name; } }
        public int ID { get { return _uid; } }
        /// <summary>
        /// load  user used at loggin 
        /// </summary>
        /// <returns></returns>
        public bool LoadUser()
        {
            _userNode = this.DrupServ.UserGet();
            return loadUserFields(_userNode);
        }
        private bool loadUserFields(XmlRpcStruct getUserNode)
        {
            if (getUserNode != null)
            {
                try
                {

                    _uid = Convert.ToInt32(getUserNode[StringEnum.StrVal(Enums.UserHTMLField.UserID)]);
                    this._name = getUserNode[StringEnum.StrVal(Enums.UserHTMLField.AuthorName)].ToString();
                    setUserOrganicGroup(getUserNode);
                    try
                    {
                        _CADSystem = (Enums.CadSystem)StringEnum.Parse(typeof(Enums.CadSystem),
                             getUserNode[StringEnum.StrVal(Enums.UserHTMLField.CadSystem)] as string);
                    }
                    catch
                    {
                        sendLogEvent(string.Format("Can't recognize user: {0} (uid:{1}) CAD file format , using DXF instead",_name,_uid), 
                            Enums.MessageSender.API, Enums.MessageType.Warning);
                        return false;
                    }
                    XmlRpcStruct rolesStruct = (getUserNode[StringEnum.StrVal(Enums.UserHTMLField.Roles)] as XmlRpcStruct);
                    if (rolesStruct != null)
                    {
                        roles.Clear();
                        foreach (System.Collections.DictionaryEntry rol in rolesStruct)
                        {
                            this.roles.Add(new SimplePDMRole(Convert.ToInt32(rol.Key),
                             (Enums.Roles)StringEnum.Parse(typeof(Enums.Roles), rol.Value.ToString())));
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    sendLogEvent(ex.Message, Enums.MessageSender.API, Enums.MessageType.Error);
                    return false;
                }
            }
            else
                return false;
        }
        public Enums.CadSystem CADSystem
        {
            get { return _CADSystem; }
        }
        public bool IsRoleMember(Enums.Roles roleName)
        {
            foreach (SimplePDMRole r in roles)
            {
                if (roleName == r.Name)
                    return true;
            }
            return false;
        }
        private bool setUserOrganicGroup(XmlRpcStruct xmlStruc)
        {
            try
            {
                this.OrganicGroups.Clear();
                XmlRpcStruct groups = (xmlStruc[StringEnum.StrVal(Enums.HTMLField.Group)] as XmlRpcStruct);
                if (groups != null)
                {
                    foreach (XmlRpcStruct values in groups.Values)
                    {
                        try
                        {
                            bool isAdmin = false;
                            if (values["is_admin"].ToString() == "1")
                                isAdmin = true;
                            int og_id = Convert.ToInt32(values["nid"]);
                            OrganicGroup group;

                            group = new OrganicGroup(og_id, values["title"].ToString(), isAdmin);
                            group.LoadVocabulary();

                            this.OrganicGroups.Add(group);
                        }
                        catch (Exception ex)
                        {
                            this.errorMessage("Cant find vocabulary: " + ex.Message, Enums.MessageSender.API_User_SetOrganicGroup);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                this.errorMessage("Can't parse organic group: " + e.Message, Enums.MessageSender.API_User_SetOrganicGroup);
                return false;
            }
        }
    }
    public class SimplePDMConnection : BaseSimplePDM
    {
        string _username;
        string _password;
        public SimplePDMConnection()
        {
          
        }
        public bool Login(string username, string password)
        {
            _username = username;
            _password = password;
            return DrupServ.Login(username, password) && DrupCurl.Login(username, password);
        }
        /// <summary>
        /// retry to login with las username and password used
        /// </summary>
        /// <returns></returns>
        public bool ReLogin()
        {
            return DrupServ.Login(_username, _password) && DrupCurl.Login(_username, _password);
        }
        public string Username {  get { return _username; }   }
        /// <summary>
        /// retrun the singleton instance of drupal services
        /// </summary>
        public DrupalServices DrupServ
        {
            get
            {
                DrupalServices ds = DrupalServices.GetInstance();
                return ds;
            }
        }
        public DrupalCurl DrupCurl
        {
            get
            {
                DrupalCurl dc = DrupalCurl.GetInstance();
                return dc;
            }
        }
        public CURLcode DrupCurlPerform()
        {
            try
            {
                // CURLcode exec = DrupCurl.EasyCurl.Perform();
                return DrupCurl.EasyCurl.Perform();
            }
            catch (Exception e)
            {
                if (e.Data != null)
                    if ((int)e.Data["Code"] == 1)
                    {
                        if (ReLogin())
                           return DrupCurl.EasyCurl.Perform();
                    }
                return CURLcode.CURLE_COULDNT_CONNECT;
            }
        }
        public void errorMessage(string msg,Enums.MessageSender sender )
        {
            sendLogEvent(msg, sender, Enums.MessageType.Error);
        }
        public virtual bool LoggedIn
        {
            get { return DrupCurl.LoggedIn && DrupServ.LoggedIn; }
        }
        public virtual bool Logout()
        {
            bool res = DrupCurl.Logout() && DrupServ.Logout();
            DrupalCurl.KillInstance();
            DrupalServices.KillInstance();
            return res;
        }
    }
  
}
