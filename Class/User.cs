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
    /// represent a user role
    /// </summary>
    public class Role
    {
        int _ID;
        Enums.Roles _name = Enums.Roles.None;
        public Role(int getID, Enums.Roles getName)
        {
            _ID = getID;
            _name = getName;
        }
        public int ID { get { return _ID; } }
        public Enums.Roles Name { get { return _name; } }

    }
    public class User : DrutNETBase, IConnection
    {
        XmlRpcStruct _userNode;
        string _name = "Anonymous";
        List<Role> roles = new List<Role>();
        int _uid;
        Services _servicesCon;
        ServicesSettings _settings;
        //string _serversURL;
        /// <summary>
        /// Not loading any user, just init class
        /// </summary>
        /// <param name="serviceCon"></param>
        public User(ServicesSettings settings)
        {
            _servicesCon = new Services(settings);
            _settings = settings;
           // _serversURL = serverURL;
        }
        /// <summary>
        /// load a none logged in user ID info
        /// </summary>
        public User(int userID, Services serviceCon)
        {
            _servicesCon = serviceCon;
            _userNode = _servicesCon.UserRetrieve(userID);
            if (userID != 0)//anonimous
                loadUserFields(_userNode);
        }
        /// <summary>
        /// return autor name , username , but not nesserly the logged in user
        /// </summary>
        public string Name { get { return _name; } }
        public int UID { get { return _uid; } }
        /// <summary>
        /// load  user used at loggin 
        /// </summary>
        /// <returns></returns>
        public bool LoadUser()
        {
            _userNode = _servicesCon.UserRetrieve();
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

                    //setUserOrganicGroup(getUserNode);

                    // Load permission roles of the user
                    XmlRpcStruct rolesStruct = (getUserNode[StringEnum.StrVal(Enums.UserHTMLField.Roles)] as XmlRpcStruct);
                    if (rolesStruct != null)
                    {
                        roles.Clear();
                        foreach (System.Collections.DictionaryEntry rol in rolesStruct)
                        {
                            this.roles.Add(new Role(Convert.ToInt32(rol.Key),
                             (Enums.Roles)StringEnum.Parse(typeof(Enums.Roles), rol.Value.ToString())));
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    sendLogEvent(ex.Message, "User", Enums.MessageType.Error);
                    return false;
                }
            }
            else
                return false;
        }
        public bool IsRoleMember(Enums.Roles roleName)
        {
            foreach (Role r in roles)
            {
                if (roleName == r.Name)
                    return true;
            }
            return false;
        }
      
        #region IConnection Members

        public bool Login(string username, string password)
        {
            return _servicesCon.Login(username, password);
        }

        public bool ReLogin()
        {
            return _servicesCon.ReLogin();
        }

        public bool Logout()
        {
            return _servicesCon.Logout();
        }

        public string Username
        {
            get { return _servicesCon.Username; }
        }

        public bool IsLoggedIn
        {
            get { return _servicesCon.IsLoggedIn; }
        }

        public string ServerURL
        {
            get { return _settings.DrupalURL; }
        }

        public Services ServicesCon
        {
            get { return _servicesCon; }
        }

        #endregion

    }

}
