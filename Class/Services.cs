using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CookComputing.XmlRpc;
using System.Security.Cryptography;
using System.Net;
using DrutNET;


namespace DrutNET
{
    // Since we don't want an error to pop up if the returned Drupal user object 
    // contains a field which does not map to a field or struct has we need to 
    // add the MappingOption.
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct DrupalUser
    {
        // Fields
        public string name;
        public string uid;
        public string mail;
    }
    public struct Drupal
    {
        public string sessid;
    }
    public struct DrupalCon
    {
        public string sessid;
        public DrupalUser user;
    }
    public interface IServiceSystem : IXmlRpcProxy
    {
        [XmlRpcMethod("system.connect")]
        Drupal Connect();

        [XmlRpcMethod("user.login")]
        DrupalCon Login(string sessid, string username, string password);

        [XmlRpcMethod("user.logout")]
        bool Logout(string sessid);

        [XmlRpcMethod("user.get")]
        XmlRpcStruct UserGet(string sessid, int uid);

        [XmlRpcMethod("node.get")]
        XmlRpcStruct NodeGet(string sessid, int nid, object fields);

        [XmlRpcMethod("node.save")]
        string NodeSave(string sessid, object fields);

        [XmlRpcMethod("views.get")]
        XmlRpcStruct[] ViewsGet(string sessid, string view_name,string display_id,object arrayfields,
                object arrayargs, int intoffset, int intlimit);
       
        [XmlRpcMethod("file.save")]
        XmlRpcStruct FileSave(string sessid, string name, string path, int status);

        [XmlRpcMethod("taxonomy.getTree")]
        XmlRpcStruct[] TaxonomyGetTree(string sessid, int vid);

        [XmlRpcMethod("og_vocab.getVocabs")]
        object OGgetVocab(string sessid, int ogID);

    }
   
    //----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// enable to connect to drupal services using XML RPC 
    /// </summary>
    public class Services : DrutNETBase, IConnection
    {
        //privates                                                      
       
        IServiceSystem drupalServiceSystem;
        string sessionID;
        string _uID;
        int _errorCode = 0;
        /// <summary>
        /// last error code return by exeption
        /// </summary>
        public int ErrorCode { get { return _errorCode; } }
        string _serverURL;
        /// <summary>
        /// Single Tone constructor
        /// </summary>
        public Services(string serverURL)
        {
            _serverURL = serverURL;

        }
       
        private void errorMessage(string msg)
        {
            sendLogEvent(msg, "Services", Enums.MessageType.Error);
        }
        
        //<summary>
         //User ID of the logged-in user
         //</summary>
        public string UserID
        {
            get { return _uID; }
        }
       
        public XmlRpcStruct OGgetVocab(int ogID)
        {
            try
            {
                object vocabs= drupalServiceSystem.OGgetVocab(sessionID, ogID);
                if (vocabs is XmlRpcStruct)
                    return (vocabs as XmlRpcStruct);
                else
                    return null;
            }
            catch (XmlRpcFaultException ex)
            {
                _errorCode = ex.FaultCode;
                errorMessage(ex.Message);
                return null;
            }
        }
        public XmlRpcStruct[] TaxonomyGetTree(int vid)
        {
            try
            {
                return drupalServiceSystem.TaxonomyGetTree(sessionID, vid);
            }
            catch (XmlRpcFaultException ex)
            {
                _errorCode = ex.FaultCode;
                errorMessage(ex.Message);
                return null;
            }
        }
        public XmlRpcStruct NodeGet(int nid)
        {
            try
            {

                //get node example
                object ob = new object();//= new object();//dummy object to send
               // ob[0] = "";
               return drupalServiceSystem.NodeGet(sessionID, nid, ob);
            }
            catch (XmlRpcFaultException ex)
            {
                _errorCode = ex.FaultCode;
                errorMessage(ex.Message);
                return null;
            }
        }
        public XmlRpcStruct NodeGet(int nid,string[] fields)
        {
            try
            {

                //get node example
               // Object ob = new object();//dummy object to send
                return drupalServiceSystem.NodeGet(sessionID, nid, fields);
            }
            catch (XmlRpcFaultException ex)
            {
                _errorCode = ex.FaultCode;
                errorMessage(ex.Message);
                return null;
            }
        }
        public XmlRpcStruct UserGet(int nid)
        {
            try
            {
                //get user 
                Object ob = new object();//dummy object to send
                return drupalServiceSystem.UserGet(sessionID, nid);
            }
            catch (XmlRpcFaultException ex)
            {
                errorMessage(ex.Message);
                _errorCode = ex.FaultCode;
                return null;
            }
        }
        /// <summary>
        /// Get logged in user information
        /// </summary>
        /// <returns></returns>
        public XmlRpcStruct UserGet()
        {
            try
            {
                int userID = Convert.ToInt32(this._uID);
                //get user 
                Object ob = new object();//dummy object to send
                return drupalServiceSystem.UserGet(sessionID, userID);
            }
            catch (XmlRpcFaultException ex)
            {
                errorMessage(ex.Message);
                _errorCode = ex.FaultCode;
                return null;
            }
        }
        public int NodeSave(XmlRpcStruct node)
        {
            try
            {
                //string filePath = DrupalServices.SERVERURL + ((taskNode["field_dispatch_zip_file"] as object[])[0] as XmlRpcStruct)["filepath"];
                //drupServ.DownloadFile(filePath, @"c:\test\1234\" + nextTaskNid.ToString() + ".zip");//save zip file locally
                //Now activaing mtm or other task

                //update task node
                //System.Windows.Forms.MessageBox.Show(((taskNode["field_dispatch_state"] as object[])[0] as XmlRpcStruct)["value"].ToString());
                // ((taskNode["field_dispatch_state"] as object[])[0] as XmlRpcStruct)["value"] = "Completed";//Change status to completed
                //  ((taskNode["field_dispatch_log"] as object[])[0] as XmlRpcStruct)["value"] = "what ever";

                // (taskNode["title"]) = "brice_eee";
                //example of log update
                //((taskNode["field_dispatch_log"] as object[])[0] as XmlRpcStruct)["value"] += "yalla beitar" ;//update log
                //int newnodeID = drupServ.NodeSave(taskNode);

                // XmlRpcStruct fref=drupServ.FileSave(@"c:\test\1234\rani.pds", "cad_file");
                
                //(node["title"]) =DateTime.Now.ToString();//update log
                //((node["field_file_upload"] as object[])[0] as XmlRpcStruct)["filepath"] = @"c:\test.PDS";
                string res = drupalServiceSystem.NodeSave(sessionID, node);
                return Convert.ToInt32(res);
            }
            catch (XmlRpcFaultException ex)
            {
                errorMessage(ex.Message);
                _errorCode = ex.FaultCode;
                return 0;
            }
        }
        public XmlRpcStruct[] ViewsGet(string viewName, string[] args)
        {
            return this.ViewsGet(viewName, 0, args);
        }
        public XmlRpcStruct[] ViewsGet(string viewName, int limit)
        {
            XmlRpcStruct arrayArgs = new XmlRpcStruct();
          //  string[] arrayArgs=null;// = new string[]();
            return this.ViewsGet(viewName, limit, arrayArgs);
        }
        public XmlRpcStruct[] ViewsGet(string viewName, int limit, object args)
        {
            try
            {
                XmlRpcStruct o1 = new XmlRpcStruct();
               // string o2 = "";
                return drupalServiceSystem.ViewsGet(sessionID, viewName, "default", o1, args, 0, limit);


            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                _errorCode = 0;
                  if  (ex is XmlRpcFaultException)
                      _errorCode = (ex as XmlRpcFaultException).FaultCode;
                return null;
            }
        }
        //--------------------------------------------------------------------------------------
        public string ParseFieldArray(Enum arrayfieldName,Enum fieldName, XmlRpcStruct parentStruct)
        {
            //List<string> temp = new List<string>();
            //try
            //{
            //    foreach (XmlRpcStruct val in (parentStruct[fieldName] as object[]))
            //        temp.Add(val["value"].ToString());
            //    return temp.ToArray();
            //}
            try
            {
                if ((parentStruct[StringEnum.StrVal(arrayfieldName)] as object[]).Length > 0)
                    if ((((parentStruct[StringEnum.StrVal(arrayfieldName)] as object[])
                        [0] as XmlRpcStruct))!=null)
                    return
                        (((parentStruct[StringEnum.StrVal(arrayfieldName)] as object[])
                        [0] as XmlRpcStruct)[StringEnum.StrVal(fieldName)].ToString());
                
                 return "";
            }
            catch
            {
                errorMessage("Unable to read " + arrayfieldName.ToString());
                return "";

            }
        }
        public string ParseField(Enum fieldName, XmlRpcStruct nodeStruct)
        {
            try
            {
                //if ((nodeStruct[StringEnum.StrVal(fieldName)] != null))
                    return (nodeStruct[StringEnum.StrVal(fieldName)].ToString());
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return "";

            }
        }
        public string ParseField(Enum fieldName, XmlRpcStruct nodeStruct,int arrayIndex )
        {
            try
            {
                //if ((nodeStruct[StringEnum.StrVal(fieldName)] != null))
                return (((nodeStruct[StringEnum.StrVal(fieldName)] as object[])
                        [arrayIndex]as XmlRpcStruct)["value"].ToString());
                //((nodeStruct[StringEnum.StrVal(fieldName)]as object[])[0] as XmlRpcStruct)["value"].ToString()
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return "";

            }
        }
        public string ParseField(string fieldName, XmlRpcStruct nodeStruct)
        {
            try
            {
                //if ((nodeStruct[StringEnum.StrVal(fieldName)] != null))
                return (nodeStruct[fieldName].ToString());
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return "";

            }
        }
        //public XmlRpcStruct FileSave(string sourceFullPath, string targetFolder)
        //{
        //    try
        //    {
        //        return drupalServiceSystem.FileSave(sessionID, sourceFullPath, targetFolder, 1);

        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage(ex.Message);
        //        return null;
        //    }
        //}
        //public bool DownloadFile(string httpPath, string savePAth)
        //{
        //    try
        //    {
        //        WebClient Client = new WebClient();
        //        Client.DownloadFile(httpPath, savePAth);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage(ex.Message);
        //        return false;
        //    }
        //}

        //----------------------------------------------------------------------------------------------------------
        // Get the current Unix time stamp.
        private string GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return Convert.ToString(Convert.ToUInt64(ts.TotalSeconds));
        }

        // Similar to the 'user_password' function Drupal uses.
        private string GetNonce(int length)
        {
            string allowedCharacters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            StringBuilder password = new StringBuilder();
            Random rand = new Random();

            for (int i = 0; i < length; i++)
            {
                password.Append(allowedCharacters[rand.Next(0, (allowedCharacters.Length - 1))]);
            }
            return password.ToString();
        }

        // Compute the hash value.
        private string GetHMAC(string message, string key)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageByte = encoding.GetBytes(message);

            HMACSHA256 hmac = new HMACSHA256(keyByte);
            byte[] hashMessageByte = hmac.ComputeHash(messageByte);

            string sbinary = String.Empty;
            for (int i = 0; i < hashMessageByte.Length; i++)
            {
                // Converting to hex, but using lowercase 'x' to get lowercase characters
                sbinary += hashMessageByte[i].ToString("x2");
            }

            return sbinary;
        }

        #region IConnection Members


        public bool ReLogin()
        {
           return Login(_username, _password);
        }
       
        public string Username
        {
            get { return _username; }
        }

        bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
             get { return this._isLoggedIn; }
        }

        string _username;
        string _password;
        public bool Login(string user, string password)
        {
            try
            {
                _username = user;
                _password = password;
                drupalServiceSystem = XmlRpcProxyGen.Create<IServiceSystem>();
                drupalServiceSystem.Url = _serverURL + "services/xmlrpc" ;
                Drupal cnct = drupalServiceSystem.Connect();
                DrupalCon lgn = drupalServiceSystem.Login(cnct.sessid, user, password);
                sessionID = lgn.sessid;
                _uID = lgn.user.uid;//returned from login
                _isLoggedIn = true;

            }
            catch (Exception ex)
            {
                _errorCode = 0;
                if (ex is XmlRpcFaultException)
                {
                    _errorCode = (ex as XmlRpcFaultException).FaultCode;
                }
                if (ex is XmlRpcServerException)
                {
                    // _errorCode = (ex as XmlRpcServerException).
                }
                errorMessage(ex.Message);
                _isLoggedIn = false;
            }
            return _isLoggedIn;
        }
        public bool Logout()
        {
            try
            {
                drupalServiceSystem.Logout(sessionID);
                _isLoggedIn = false;
                return true;
            }
            catch (XmlRpcFaultException ex)
            {
                _errorCode = ex.FaultCode;
                errorMessage(ex.Message);
                return false;
            }
        }

        public string ServerURL
        {
            get { return _serverURL; }
        }

     /*   public Services ServicesCon
        {
            get { return this; }
        }
        */
        #endregion
    }
    
    
}
