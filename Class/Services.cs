using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CookComputing.XmlRpc;
using System.Security.Cryptography;
using System.Net;
using DrutNET;
using System.IO;


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
        DrupalCon Login(ref string hash, string domain_name, ref string timestamp, string nonce, string sessid, string username, string password);
        [XmlRpcMethod("user.login")]
        DrupalCon Login(string sessid, string username, string password);
        [XmlRpcMethod("user.login")]
        DrupalCon Login( string username, string password);

        [XmlRpcMethod("user.logout")]
        bool Logout(string sessid);
        [XmlRpcMethod("user.logout")]
        bool Logout();


        [XmlRpcMethod("user.retrieve")]
        XmlRpcStruct UserGet(string sessid, int uid);
        [XmlRpcMethod("user.retrieve")]
        XmlRpcStruct UserGet( int uid);

        [XmlRpcMethod("node.retrieve")]
        XmlRpcStruct NodeGet(ref string hash,string domain_name,ref string timestamp,string nonce, string sessid, int nid, object fields);
        [XmlRpcMethod("node.retrieve")]
        XmlRpcStruct NodeGet(string sessid, int nid, object fields);
        [XmlRpcMethod("node.retrieve")]
        XmlRpcStruct NodeGet(int nid, object fields);

        [XmlRpcMethod("node.save")]
        string NodeSave(string sessid, object fields);
        [XmlRpcMethod("node.save")]
        string NodeSave(object fields);

        [XmlRpcMethod("views.retrieve")]
        XmlRpcStruct[] ViewsGet(string sessid, string view_name,string display_id,object arrayfields,
                object arrayargs, int intoffset, int intlimit);
        [XmlRpcMethod("views.retrieve")]
        XmlRpcStruct[] ViewsGet(string view_name, string display_id, object arrayfields,
                object arrayargs, int intoffset, int intlimit);
       
        [XmlRpcMethod("file.save")]
        string FileSave(string sessid, object file);
        [XmlRpcMethod("file.save")]
        string FileSave(object file);

        [XmlRpcMethod("file.retrieve")]
        XmlRpcStruct FileGet(string sessid, int fid);
        [XmlRpcMethod("file.retrieve")]
        XmlRpcStruct FileGet(int fid);


        [XmlRpcMethod("taxonomy.getTree")]
        XmlRpcStruct[] TaxonomyGetTree(string sessid, int vid);
        [XmlRpcMethod("taxonomy.getTree")]
        XmlRpcStruct[] TaxonomyGetTree( int vid);

        [XmlRpcMethod("og_vocab.getVocabs")]
        object OGgetVocab(string sessid, int ogID);
        [XmlRpcMethod("og_vocab.getVocabs")]
        object OGgetVocab( int ogID);

    }
    /// <summary>
    /// Configuration settings for drupal services module
    /// </summary>
    public class ServicesSettings
    {
        bool _cleanURL = false;
        public bool CleanURL
        {
            get { return _cleanURL; }
            set { _cleanURL = value; }
        }

        string _drupalURL="";
        /// <summary>
        /// URL to the drupal site
        /// </summary>
        public string DrupalURL
        {
            get { return _drupalURL; }
            set { _drupalURL = value; }
        }

        string _endPoint = "";
        /// <summary>
        /// Path to endpoint.
        /// </summary>
        public string EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        bool _useSessionID = false;
        /// <summary>
        /// If session ID feature is enabled in services module
        /// </summary>
        public bool UseSessionID
        {
            get { return _useSessionID; }
            set { _useSessionID = value; }
        }

        bool _useKeys = false;
        /// <summary>
        /// Specifies is provate key is required
        /// </summary>
        public bool UseKeys
        {
            get { return _useKeys; }
            set 
            {
                if (value == true)
                    throw new Exception("The private key feature is not supported yet");
                //_useKeys = value; 
            }
        }

        string _key = "";
        /// <summary>
        /// Private Key, this is acquired from services module settings
        /// </summary>
        public string Key 
        {
            get { return _key; }
            set { _key = value; }
        }
        string _domainName = "";
        /// <summary>
        /// Domain restriction used for the private key
        /// </summary>
        public string DomainName
        {
            get { return _domainName; }
            set { _domainName = value; }
        }



    }
    //----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// enable to connect to drupal services using XML RPC 
    /// </summary>
    public class Services : DrutNETBase, IConnection
    {
        //privates                                                      
       
        IServiceSystem drupalServiceSystem;
        string _sessionID=" ";
        string _uID;
        int _errorCode = 0;
        string _errorMsg = "";
        /// <summary>
        /// last error code return by exeption
        /// </summary>
        public int ErrorCode { get { return _errorCode; } }
        public string ErrorMessage { get { return _errorMsg; } }
        ServicesSettings _settings;
        /// <summary>
        /// Single Tone constructor
        /// </summary>
        public Services(ServicesSettings settings)
        {
            _settings = settings;
        }
       
        private void errorMessage(string msg)
        {
            sendLogEvent(msg, "Services", Enums.MessageType.Error);
        }
        private XmlRpcStruct handleExeption(Exception ex)
        {
            return handleExeption(ex, "");
        }
        private XmlRpcStruct handleExeption(Exception ex,string functionName)
        {
            if (ex is XmlRpcFaultException)
            {
                _errorCode = (ex as XmlRpcFaultException).FaultCode;
                _errorMsg = (ex as XmlRpcFaultException).Message;
            }
            else
            {
                _errorMsg = ex.Message;
            }
            errorMessage(functionName + " - " + ex.Message + "\n");
            return null;
        }
        private string handleExeptionStr(Exception ex, string functionName)
        {
            if (ex is XmlRpcFaultException)
            {
                _errorCode = (ex as XmlRpcFaultException).FaultCode;
                _errorMsg = (ex as XmlRpcFaultException).Message;
            }
            errorMessage(functionName + " - " + ex.Message + "\n");
            return "";
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
                object vocabs;
                if (_settings.UseSessionID)
                    vocabs = drupalServiceSystem.OGgetVocab(_sessionID, ogID);
                else
                    vocabs = drupalServiceSystem.OGgetVocab(ogID);

                if (vocabs is XmlRpcStruct)
                    return (vocabs as XmlRpcStruct);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return handleExeption(ex,"OG Get Vocabulary");
            }
        }
        public XmlRpcStruct[] TaxonomyGetTree(int vid)
        {

            try
            {
                if (_settings.UseSessionID)
                    return drupalServiceSystem.TaxonomyGetTree(_sessionID, vid);
                else
                    return drupalServiceSystem.TaxonomyGetTree(vid);
            }
            catch (Exception ex)
            {
                handleExeption(ex, "Taxonomy Get Tree");
                return null;
            }
        }
        
        /// <summary>
        /// Get file structure information
        /// </summary>
        /// <param name="fid">file id</param>
        /// <returns>File object structure</returns>
        public XmlRpcStruct FileGet(int fid)
        {
            try
            {
                if (_settings.UseSessionID)
                {
                    // Use of key together with SID.
                    if (_settings.UseKeys)
                    {
                        /*string hash = GetHMAC("", _settings.Key);
                        string timestamp = GetUnixTimestamp();
                        string nonce = GetNonce(10);
                        return drupalServiceSystem.NodeGet(ref hash, _settings.DomainName, ref timestamp, nonce, _sessionID, nid, ob);
                        */
                        throw new NotImplementedException("Private key not implemented yet");
                        //return null;
                    }
                    else
                        return drupalServiceSystem.FileGet(_sessionID, fid);

                }
                else
                    return drupalServiceSystem.FileGet(fid);
            }
            catch (Exception ex)
            {
                return handleExeption(ex, "File Get");
            }
        }
      /// <summary>
        /// Add an already upload file to cck file field, without saving the node
      /// </summary>
      /// <param name="fileFieldName">CCK field name</param>
      /// <param name="fid">File ID to attach</param>
      /// <param name="fileIndex">file index in case of multiple file field, for single file use 0</param>
      /// <param name="node">Node to attach file to (alresdy load with node load)</param>
        public bool AttachFileToNode(string fileFieldName, int fid, int fileIndex, XmlRpcStruct node)
        {
            XmlRpcStruct filenode = this.FileGet(fid);
            if (filenode == null)
            {
                return false;
            }
            else
            {
                if (node[fileFieldName] == null)
                    node[fileFieldName] = new object[1];
                // Index is not within range of lenght + 1
                if ((node[fileFieldName] as object[]).Length < fileIndex)
                {
                    handleExeption(new IndexOutOfRangeException(), "Attach file to Node");
                    return false;
                }
                else
                {
                    // index is one bigger than existing array, we need to add one object manually
                    List<object> objectList = new List<object>();
                    if ((node[fileFieldName] as object[]).Length == fileIndex)
                    {
                        foreach (object ob in (node[fileFieldName] as object[]))
                            objectList.Add(ob);
                        objectList.Add(new object());
                        node[fileFieldName] = objectList.ToArray();
                        // Add index list required for multiple files.
                        filenode.Add("list", (fileIndex + 1).ToString());
                    }
                    else
                        // Case a file was removed form the node, we need to reformat the object
                        if (((node[fileFieldName] as object[]).Length == 1) &&
                            ((node[fileFieldName] as object[])[0] is string))
                        {
                            objectList.Add(new object());
                            node[fileFieldName] = objectList.ToArray();
                            filenode.Add("list", (fileIndex + 1).ToString());
                        }
                        else
                        {
                            filenode.Add("list", (fileIndex + 1).ToString());
                        }

                    (node[fileFieldName] as object[])[fileIndex] = filenode;
                    return true;
                }
            }
        }
        /// <summary>
        /// Add an already upload file to cck file field, also saving node
        /// </summary>
        /// <param name="fileFieldName">CCK field name</param>
        /// <param name="fid">File ID to attach</param>
        /// <param name="fileIndex">file index in case of multiple file field, for single file use 0</param>
        /// <param name="node">Node ID attach file to</param>
        public bool AttachFileToNode(string fileFieldName, int fid, int fileIndex, int nid)
        {
            XmlRpcStruct node = this.NodeGet(nid);
            if (node != null)
            {
                AttachFileToNode(fileFieldName, fid, fileIndex, node);
                if (this.NodeSave(node) != 0)
                    return true;
            }
            else
                handleExeption(new Exception("Unable to load node " + nid.ToString()), "AttachFileToNode");
                //errorMessage("Unable to load node " + nid.ToString());
            return false;
        }

        /// <summary>
        /// Return node structure
        /// </summary>
        /// <param name="nid">Node ID</param>
        /// <returns>Node structure</returns>
        public XmlRpcStruct NodeGet(int nid)
        {
            try
            {
                // Dummy object to send.
                object ob = new object();
                if (_settings.UseSessionID)
                {
                    // Use of key together with SID.
                    if (_settings.UseKeys)
                    {
                        string hash = GetHMAC("",_settings.Key);
                        string timestamp = GetUnixTimestamp();
                        string nonce = GetNonce(10);
                        return drupalServiceSystem.NodeGet(ref hash, _settings.DomainName, ref timestamp, nonce, _sessionID, nid,ob);

                    }
                    else
                        return drupalServiceSystem.NodeGet(_sessionID, nid, ob);

                }
                else
                    return drupalServiceSystem.NodeGet(nid, ob);
            }
            catch (Exception ex)
            {
                return handleExeption(ex, "Node Get");
            }
        }
        public XmlRpcStruct NodeGet(int nid, string[] fields)
        {
            try
            {
                if (_settings.UseSessionID)
                    return drupalServiceSystem.NodeGet(_sessionID, nid, fields);
                else
                    return drupalServiceSystem.NodeGet(nid, fields);

            }
            catch (Exception ex)
            {
                return handleExeption(ex, "Node Get");
            }
        }
        
        public XmlRpcStruct UserGet(int nid)
        {
            
                try
                {
                    //get user 
                    if (_settings.UseSessionID)
                    return drupalServiceSystem.UserGet(_sessionID, nid);
                    else
                        return drupalServiceSystem.UserGet( nid);

                }
                catch (Exception ex)
                {
                    return handleExeption(ex, "User Get");
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
                if (_settings.UseSessionID)
                    return drupalServiceSystem.UserGet(_sessionID, userID);
                else
                    return drupalServiceSystem.UserGet( userID);


            }
            catch (Exception ex)
            {
                return handleExeption(ex, "User Get");
            }
        }
        /// <summary>
        /// Create a drupal file structure
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private XmlRpcStruct buildFileStruct(string filePath,string serverPath)
        {
            // Encode file to base64
            FileStream fs = new FileStream(filePath,FileMode.Open,FileAccess.Read);
            byte[] filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            string encodedFile = Convert.ToBase64String(filebytes, Base64FormattingOptions.InsertLineBreaks);
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);

            CookComputing.XmlRpc.XmlRpcStruct fileStruct = new CookComputing.XmlRpc.XmlRpcStruct();
            fileStruct.Add("file", encodedFile);
            fileStruct.Add("filename", fi.Name);
            fileStruct.Add("filepath", serverPath + fi.Name);
            fileStruct.Add("filesize",fi.Length.ToString());
            fileStruct.Add("timestamp",GetUnixTimestamp());
            fileStruct.Add("uid", this.UserID);//Form Login

            return fileStruct;
        }
        /// <summary>
        /// Save file structure
        /// </summary>
        /// <param name="fid">file name</param>
        /// <returns>File ID</returns>
        public string FileSave(string filePath,string serverPath)
        {
            try
            {
                if (_settings.UseSessionID)
                {
                    // Use of key together with SID.
                    if (_settings.UseKeys)
                    {
                        /*string hash = GetHMAC("", _settings.Key);
                        string timestamp = GetUnixTimestamp();
                        string nonce = GetNonce(10);
                        return drupalServiceSystem.NodeGet(ref hash, _settings.DomainName, ref timestamp, nonce, _sessionID, nid, ob);
                        */
                        throw new NotImplementedException("Private key not implemented yet");
                        //return null;
                    }
                    else
                        return drupalServiceSystem.FileSave(_sessionID, buildFileStruct(filePath, serverPath));

                }
                else
                    return drupalServiceSystem.FileSave(buildFileStruct(filePath, serverPath));
            }
            catch (Exception ex)
            {
                return handleExeptionStr(ex, "File Save");
            }
        }
        public bool FileUpload(string filePath,string fieldName,int fileIndex,int nodeID)
        {
            return FileUpload(filePath, fieldName, fileIndex, nodeID, @"sites/default/files/");
        }
        //
        //</param>
        //</param>

        /// <summary>
        /// Upload and attach a file to an existing NODE.
        /// </summary>
        /// <param name="filePath">Local path to file</param>
        /// <param name="fieldName">CCK field name of the file field in the node</param>
        /// <param name="fileIndex">file index in case of multiple file field, for single file use 0</param>
        /// <param name="nodeID">Node ID to attache file to</param>
        /// <param name="serverDirectory">Server directory path e.g: sites/default/files/ </param>
        /// <returns></returns>
        public bool FileUpload(string filePath,string fieldName,int fileIndex,int nodeID, string serverDirectory)
        {
            string fid = FileSave(filePath, serverDirectory);
            if (fid != "")
            {
                int file_id = Convert.ToInt32(fid);
                this.AttachFileToNode(fieldName, file_id, fileIndex, nodeID);
                return true;
            }
            return false;
        }
        public int NodeSave(XmlRpcStruct node)
        {
            try
            {
                string res;
                if (_settings.UseSessionID)
                    res = drupalServiceSystem.NodeSave(_sessionID, node);
                else
                    res = drupalServiceSystem.NodeSave(node);

                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                handleExeption(ex, "Node Save");
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
                if (_settings.UseSessionID)
                    return drupalServiceSystem.ViewsGet(_sessionID, viewName, "default", o1, args, 0, limit);
                else
                    return drupalServiceSystem.ViewsGet(viewName, "default", o1, args, 0, limit);
            }
            catch (Exception ex)
            {
                _errorCode = 0;
                handleExeption(ex, "View Get");
                return null;
            }
        }
        //--------------------------------------------------------------------------------------
        public string ParseFieldArray(Enum arrayfieldName,Enum fieldName, XmlRpcStruct parentStruct)
        {
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
                //errorMessage("Unable to read " + arrayfieldName.ToString());
                handleExeption(new Exception("Unable to read " + arrayfieldName.ToString()), "ParseFieldArray");

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
                //errorMessage(ex.Message);
                handleExeption(ex, "ParseField");

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
                //errorMessage(ex.Message);
                handleExeption(ex, "ParseField");

                return "";

            }
        }
        public string ParseField(string fieldName, XmlRpcStruct nodeStruct)
        {
            try
            {
                return (nodeStruct[fieldName].ToString());
            }
            catch (Exception ex)
            {
                //errorMessage(ex.Message);
                handleExeption(ex, "ParseField");

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
            return Login(user, password, "");
        }
        public bool Login(string user, string password, string debugURL)
        {
            try
            {
                _username = user;
                _password = password;
                drupalServiceSystem = XmlRpcProxyGen.Create<IServiceSystem>();
                string xmlrpcServer;
                // Clean URL pref
                if (!_settings.CleanURL)
                     xmlrpcServer = "?q=" + _settings.EndPoint;
                else
                    xmlrpcServer = "/" +_settings.EndPoint;

                drupalServiceSystem.Url = _settings.DrupalURL + xmlrpcServer + debugURL;
                Drupal cnct = drupalServiceSystem.Connect();
                DrupalCon lgn ;
                // SesionID pref

                if (_settings.UseSessionID)
                {
                    if (_settings.UseKeys)
                    {
                        string hash = "";// GetHMAC("", _settings.Key);
                        string timestamp = "";// = GetUnixTimestamp();
                        string nonce = GetNonce(10);
                        lgn = drupalServiceSystem.Login(ref hash, _settings.DomainName, ref timestamp, nonce, cnct.sessid, user, password);
                    }
                    else
                        lgn = drupalServiceSystem.Login(cnct.sessid, user, password);

                }
                else
                {
                    lgn = drupalServiceSystem.Login(user, password);
                }
                // Check that login was succesfull by comparing username returned.
                if (lgn.user.name == _username)
                {
                    _sessionID = lgn.sessid;
                    _uID = lgn.user.uid; //returned from login
                    _isLoggedIn = true;
                }
                else
                {
                    _isLoggedIn = false;
                    handleExeption(new Exception("Unable to login"), "Login");
                }

            }
            catch (Exception ex)
            {
                handleExeption(ex, "Login");
                _isLoggedIn = false;
            }
            return _isLoggedIn;
        }
        public bool Logout()
        {
            try
            {
                if (_settings.UseSessionID)
                    drupalServiceSystem.Logout(_sessionID);
                else
                    drupalServiceSystem.Logout();
                _isLoggedIn = false;
                return true;
            }
            catch (Exception ex)
            {
                handleExeption(ex, "Logout");
                return false;
            }
        }
         public string ServerURL
        {
            get { return _settings.DrupalURL; }
        }

        #endregion
    }
    
    
}
