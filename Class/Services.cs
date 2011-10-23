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
        DrupalCon Login( string username, string password);

        [XmlRpcMethod("user.logout")]
        bool Logout(string sessid);
        [XmlRpcMethod("user.logout")]
        bool Logout();


        [XmlRpcMethod("user.retrieve")]
        XmlRpcStruct UserRetrieve( int uid);

        [XmlRpcMethod("node.retrieve")]
        XmlRpcStruct NodeRetrieve(ref string hash,string domain_name,ref string timestamp,string nonce, string sessid, int nid, object fields);
        [XmlRpcMethod("node.retrieve")]
        XmlRpcStruct NodeRetrieve(int nid);

        [XmlRpcMethod("node.create")]
        XmlRpcStruct NodeCreate(object fields);

        [XmlRpcMethod("node.update")]
        XmlRpcStruct NodeUpdate(int nid, object fields);

        [XmlRpcMethod("views.retrieve")]
        XmlRpcStruct[] ViewsGet(string view_name, string display_id, object arrayfields,
                object arrayargs, int intoffset, int intlimit);
       
        [XmlRpcMethod("file.create")]
        XmlRpcStruct FileCreate(object file);

        [XmlRpcMethod("file.retrieve")]
        XmlRpcStruct FileRetrieve(int fid, bool includeContent, bool getImageStylePath);


        [XmlRpcMethod("taxonomy.getTree")]
        XmlRpcStruct[] TaxonomyGetTree( int vid);

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
    /// Enable to connect to drupal services using XML RPC 
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
                return drupalServiceSystem.TaxonomyGetTree(vid);
            }
            catch (Exception ex)
            {
                handleExeption(ex, "Taxonomy Get Tree");
                return null;
            }
        }
        
        #region File
        /// <summary>
        /// Get file structure information
        /// </summary>
        /// <param name="fid">file id</param>
        /// <param name="includeContent">Whether to include base64 file content</param>
        /// <param name="getImageStylePath">Whether to include the image style file path</param>
        /// <returns>File object structure</returns>
        public XmlRpcStruct FileRetrieve(int fid, bool includeContent, bool getImageStylePath)
        {
            try
            {
                return drupalServiceSystem.FileRetrieve(fid, includeContent, getImageStylePath);
            }
            catch (Exception ex)
            {
                return handleExeption(ex, "File Retrieve");
            }
        }
      
        
        #region User
        public XmlRpcStruct UserRetrieve(int nid)
        {
            
                try
                {
                    //get user 
                    return drupalServiceSystem.UserRetrieve( nid);

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
        public XmlRpcStruct UserRetrieve()
        {
            try
            {
                int userID = Convert.ToInt32(this._uID);
                //get user 
                return drupalServiceSystem.UserRetrieve( userID);


            }
            catch (Exception ex)
            {
                return handleExeption(ex, "User Get");
            }
        }
        #endregion

        /// <summary>
        /// Create a drupal file structure
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private XmlRpcStruct buildFileStruct(string filePath)
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
            fileStruct.Add("target_uri", fi.Name);
            // TODO: Check for duplaicate files.
            fileStruct.Add("filepath", "public://" + fi.Name);
            fileStruct.Add("filesize", fi.Length.ToString());
            fileStruct.Add("timestamp", (DateTime.UtcNow - new DateTime(1970,1,1,0,0,0)).TotalSeconds);
            fileStruct.Add("uid", this.UserID);

            //object[] fileObjArray = new object[1];
            //fileObjArray[0] = fileStructValue;

            //CookComputing.XmlRpc.XmlRpcStruct fileStruct = new CookComputing.XmlRpc.XmlRpcStruct();
            //fileStruct.Add("und", fileObjArray);


            return fileStruct;
        }
        /// <summary>
        /// Create file structure
        /// </summary>
        /// <param name="fid">file name</param>
        /// <returns>File ID</returns>
        public XmlRpcStruct FileCreate(string filePath)
        {
            try
            {
                 return drupalServiceSystem.FileCreate(buildFileStruct(filePath));
            }
            catch (Exception ex)
            {
                 handleExeptionStr(ex, "File Save");
                 return null;
            }
        }

        /// <summary>
        /// Upload and attach a file to an existing NODE.
        /// </summary>
        /// <param name="filePath">Local path to file</param>
        /// <param name="fieldName">CCK field name of the file field in the node</param>
        /// <param name="fileIndex">file index in case of multiple file field, for single file use 0</param>
        /// <param name="nodeID">Node ID to attache file to</param>
        /// <param name="serverDirectory">Server directory path e.g: sites/default/files/ </param>
        /// <returns></returns>
        public bool FileUpload(string filePath,string fieldName,int fileIndex,int nodeID)
        {
            XmlRpcStruct file = FileCreate(filePath);
            if (file != null)
            {
                this.AttachFileToNode(fieldName, file, fileIndex, nodeID);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add an already upload file to cck file field, without saving the node
        /// </summary>
        /// <param name="fileFieldName">CCK field name</param>
        /// <param name="fid">File ID to attach</param>
        /// <param name="fileIndex">file index in case of multiple file field, for single file use 0</param>
        /// <param name="node">Node to attach file to (alresdy load with node load)</param>
        public bool AttachFileToNode(string fileFieldName, XmlRpcStruct file, int fileIndex, XmlRpcStruct node)
        {
            if (file == null)
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
                        file.Add("list", (fileIndex + 1).ToString());
                    }
                    else
                        // Case a file was removed form the node, we need to reformat the object
                        if (((node[fileFieldName] as object[]).Length == 1) &&
                            ((node[fileFieldName] as object[])[0] is string))
                        {
                            objectList.Add(new object());
                            node[fileFieldName] = objectList.ToArray();
                            file.Add("list", (fileIndex + 1).ToString());
                        }
                        else
                        {
                            file.Add("list", (fileIndex + 1).ToString());
                        }

                    (node[fileFieldName] as object[])[fileIndex] = file;
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
        public bool AttachFileToNode(string fileFieldName, XmlRpcStruct file, int fileIndex, int nid)
        {
            XmlRpcStruct node = this.NodeRetrieve(nid);
            if (node != null)
            {
                AttachFileToNode(fileFieldName, file, fileIndex, node);
                if (this.NodeSave(node) != null)
                    return true;
            }
            else
                handleExeption(new Exception("Unable to load node " + nid.ToString()), "AttachFileToNode");
          
            return false;
        }
        #endregion

        #region Node
        /// <summary>
        /// Create at Update a node, if node->nid is set, will use update, otherwise will use create.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public XmlRpcStruct NodeSave(XmlRpcStruct node)
        {
            try
            {
                XmlRpcStruct res;
                // Create a node.
                if (node["nid"] == null)
                {
                    res = drupalServiceSystem.NodeCreate(node);
                }
                // Update an existing node.
                else
                {
                    int nid = Convert.ToInt32(node["nid"]);
                    res = drupalServiceSystem.NodeUpdate(nid, node);
                }

                return res;
            }
            catch (Exception ex)
            {
                handleExeption(ex, "Node Save");
                return null;
            }
        }

        /// <summary>
        /// Return node structure
        /// </summary>
        /// <param name="nid">Node ID</param>
        /// <returns>Node structure</returns>
        public XmlRpcStruct NodeRetrieve(int nid)
        {
            try
            {
                return drupalServiceSystem.NodeRetrieve(nid);
            }
            catch (Exception ex)
            {
                return handleExeption(ex, "Node Get");
            }
        }

        #endregion

        #region Views
        /// <summary>
        /// Views resources - not updated in Drupal 7 yet.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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
                return drupalServiceSystem.ViewsGet(viewName, "default", o1, args, 0, limit);
            }
            catch (Exception ex)
            {
                _errorCode = 0;
                handleExeption(ex, "View Get");
                return null;
            }
        }
        #endregion

        //--------------------------------------------------------------------------------------

        #region parce fields
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
                handleExeption(new Exception("Unable to read " + arrayfieldName.ToString()), "ParseFieldArray");

                return "";

            }
        }
        public string ParseField(Enum fieldName, XmlRpcStruct nodeStruct)
        {
            try
            {
                return (nodeStruct[StringEnum.StrVal(fieldName)].ToString());
            }
            catch (Exception ex)
            {
                handleExeption(ex, "ParseField");
                return "";

            }
        }
        public string ParseField(Enum fieldName, XmlRpcStruct nodeStruct,int arrayIndex )
        {
            try
            {
                return (((nodeStruct[StringEnum.StrVal(fieldName)] as object[])
                        [arrayIndex]as XmlRpcStruct)["value"].ToString());
            }
            catch (Exception ex)
            {
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
                handleExeption(ex, "ParseField");
                return "";
            }
        }
        #endregion

        //----------------------------------------------------------------------------------------------------------

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
                lgn = drupalServiceSystem.Login(user, password);
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
