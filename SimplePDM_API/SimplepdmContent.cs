using System.Collections.Generic;
using System;
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
    public class SimplePDMContent : SimplePDMConnection
    {
        #region Properties and privates
        protected XmlRpcStruct ContentNode { get { return _contentNode; } set { _contentNode = value; } }
        XmlRpcStruct _contentNode;
        /// <summary>
        /// Define the type of Task to be performed on the style, when having multiple task only
        /// otherwise single tasks are sort in conent base class under _taskType
        /// </summary>
        public List<Enums.TaskType> TaskType { get { return _taskType; } protected set { _taskType = value; } }
        List<Enums.TaskType> _taskType = new List<Enums.TaskType>();
       // public Enums.TaskStatus CreateThumbnailTaskStatus { get { return _createThumbnailTaskStatus; } set { _createThumbnailTaskStatus = value; } }
       // Enums.TaskStatus _createThumbnailTaskStatus = Enums.TaskStatus.Null;
        
        //image thumb
        string _imagefile = "";
        public string Imagefile { get { return _imagefile; } set { _imagefile = value; } }
        //int _imagefileID = -1;
        public string HttpImagePath { get { return _httpImagePath; } }// set { _httpImagePath = value; } }
        string _httpImagePath = "";
        //task status
        public Enums.TaskStatus TaskStatus { get { return _taskStatus; } set { _taskStatus = value; } }
        Enums.TaskStatus _taskStatus = Enums.TaskStatus.Null;

        public string RevisonLog { get { return _revLog; } set { _revLog = value; } }
        string _revLog = "";
       
        public string ContentFile { get { return _contentFile; } set { _contentFile = value; } }
        string _contentFile = "";
        /// <summary>
        /// working with content object because the nid is not always known ahead of time
        /// </summary>
        public List<SimplePDMContent> ContentRef
        {
            get { return _contentRef; }
            set { _contentRef = value; }
        }
        List<SimplePDMContent> _contentRef = new List<SimplePDMContent>();
        /// <summary>
        /// referenced content ids
        /// </summary>
        public List<int> ContentRefIds
        {
            get { return _contentRefIds; }
            set { _contentRefIds = value; }
        }
        List<int> _contentRefIds = new List<int>();
        public int NodeID
        {
            get { return _nodeID; }
            set { _nodeID = value; }
        }
        int _nodeID = -1;
        public SimplePDMUser User
        {
            get
            {
                return _user;
            }
            protected set
            {
                _user = value;
            }
        }
        SimplePDMUser _user = null;
        public Enums.ContentType ContentType
        {
            get { return _content_type; }
        }
        Enums.ContentType _content_type;//content can be part of one group only
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        string _title = "";
        public int AssignToID
        {
            get
            {
                if ((_assignToID == -1) && (this.User != null))
                    return User.ID;
                else
                    return _assignToID;
            }
            set { _assignToID = value; }
        }
        int _assignToID = -1;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        string _description = "";
        public Enums.ProductionState ProductionState
        {
            get { return _productionState; }
            set { _productionState = value; }
        }
        Enums.ProductionState _productionState = Enums.ProductionState.Draft;
        public OrganicGroup OrganicGroup
        {
            get
            {
                if ((_organicGroup == null) && (this.User.OrganicGroups.Count > 0))
                    return _user.OrganicGroups[0];//default group
                else
                    return _organicGroup;
            }
            set { _organicGroup = value; }
        }
        OrganicGroup _organicGroup;
        public string HTTPLink
        {
            get
            {
                if (_nodeID != -1)
                    return Enums.SERVERURL + "node/" + this.NodeID.ToString();
                else
                    return "";
            }
        }
        /// <summary>
        /// Create a node/4445 style link for browsing within drupal
        /// </summary>
        public string DrupalShortHTTPLink
        {
            get
            {
                if (_nodeID != -1)
                    return "/node/" + this.NodeID.ToString();
                else
                    return "";
            }
        }
        public bool OverwriteThumb { set { _valid = value; } }
        protected bool _overwriteThumb = false;

        /// <summary>
        /// Give an indication for error found while reading from simple pdm
        /// </summary>
        public bool Valid { get { return _valid; } set { _valid = value; } }
        bool _valid = true;
        public int ContentfileID { get { return _contentfileID; } }//set { _fileID = value; } }
        int _contentfileID = 0;
        public bool OGpublic { get { return _OGpublic; } set { _OGpublic = value; } }
        bool _OGpublic = false;

        bool _hasProductionState = false;
        bool _hasDescription = false;
        bool _hasContentFile = false;
        bool _hasImage = false;

        public string UserTaskLog { get { return _userTaskLog; } } //protected set { _userTaskLog = value; } }
        string _userTaskLog = "";
        public string AdminTaskLog { get { return _adminTaskLog; } }//protected set { _adminTaskLog = value; } }
        string _adminTaskLog = "";
        #endregion

        #region Constructor errormessages and weblink methods
        public SimplePDMContent(Enums.ContentType getContent_type, SimplePDMUser user)
            : base()
        {
            _content_type = getContent_type;
            _user = user;
            if ((getContent_type == Enums.ContentType.Image) ||
                (getContent_type == Enums.ContentType.Marker) ||
                (getContent_type == Enums.ContentType.Spec)||
                (getContent_type == Enums.ContentType.Style)
                )
            {
                _hasImage = true;
                _hasDescription = true;
                _hasContentFile = true;
                _hasProductionState = true;
            }
        }
        public SimplePDMContent(Enums.ContentType getContent_type, SimplePDMUser user,string contentFile)
            :this (getContent_type,user)
        {
            _contentFile = contentFile;
        }
        public void OpenContentWebPage(Enums.OpenWebPageMode pageMode)
        {
            try
            {
                if (pageMode == Enums.OpenWebPageMode.New)
                    System.Diagnostics.Process.Start(Enums.SERVERURL + "node/add/" +
                        StringEnum.StrVal(_content_type) + "?edit[title]=" + this.Title);
                else
                    if (pageMode == Enums.OpenWebPageMode.Edit)
                        System.Diagnostics.Process.Start(Enums.SERVERURL + "node/" + this.NodeID.ToString() + "/edit");
                    else
                        if (pageMode == Enums.OpenWebPageMode.View)
                            System.Diagnostics.Process.Start(Enums.SERVERURL + "node/" + this.NodeID.ToString());
                        else if (pageMode == Enums.OpenWebPageMode.Edit)
                            System.Diagnostics.Process.Start(Enums.SERVERURL + "node/" + this.NodeID.ToString() + "/revisions");
            }
            catch
            {
                sendLogEvent("Can't open web page, problem with browser", Enums.MessageSender.API, Enums.MessageType.Error);
            }
        }
        public new void errorMessage(string msg,Enums.MessageSender sender )
        {
            sendLogEvent(msg, sender, Enums.MessageType.Error);
        }
        #endregion

        #region Curl - Save
        private int CurlWriteDefaultValues(MultiPartForm mf, string url)
        {
            DrupCurl.ClearDataIn();
            DrupCurl.EasyCurl.SetOpt(CURLoption.CURLOPT_URL, url);
            CURLcode exec = DrupCurlPerform();

            // this regular function does - : 
            // 1. remove input type : submit
            // 2. remove id  name starting with 'edit-search'
            // 3. 
            Regex rxTitle = new Regex(@"<title>(?<title>.*)</title>");
            MatchCollection fieldsTitle = rxTitle.Matches(DrupCurl.HtmlDataIn);
            if (fieldsTitle.Count > 0)
                if (fieldsTitle[0].Groups["title"].Value == "Access denied | simplePDM")
                    return -1;//access denied code
            //Regex rx = new Regex(@"<input.*type=""(?<type>(?!^*submit)[^""]*).*name=""(?<name>[^""]*).*id=""(?<id>(?!^*edit-search)edit[^""]*).*value=""(?<value>[^""]*)""");
            Regex rx = new Regex(@"</?input+((\s+(\w+\s*)=(\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>"); //return all propeties of <input...

            MatchCollection fields = rx.Matches(DrupCurl.HtmlDataIn);
            foreach (Match m in fields)//add values with default
            {
                string id = "";
                string type = "";
                string name = "";
                string value = "";
                string checkedState = "";
                //store regex value in the proper variable
                for (int i = 0; i < m.Groups[3].Captures.Count; i++)
                {
                    if (m.Groups[3].Captures[i].Value == "id")
                        id = m.Groups[4].Captures[i].Value.Trim("\"".ToCharArray());
                    else if (m.Groups[3].Captures[i].Value == "type")
                        type = m.Groups[4].Captures[i].Value.Trim("\"".ToCharArray());
                    else if (m.Groups[3].Captures[i].Value == "name")
                        name = m.Groups[4].Captures[i].Value.Trim("\"".ToCharArray());
                    else if (m.Groups[3].Captures[i].Value == "value")
                        value = m.Groups[4].Captures[i].Value.Trim("\"".ToCharArray());
                    else if (m.Groups[3].Captures[i].Value == "checked")
                        checkedState = m.Groups[4].Captures[i].Value.Trim("\"".ToCharArray());
                }
                if (((name == "form_token") && (id != "edit-" + StringEnum.StrVal(ContentType) + "-node-form-form-token"))
                    || ((name == "form_id") && (id != "edit-" + StringEnum.StrVal(ContentType) + "-node-form")) ||
                     (name == "field_style_task_type[value][convert]") || //TODO: remove ? 
                      (name == "field_style_task_type[value][MTM]") || //remove?
                      (name == "field_user_measurements_file[0][list]") ||//remove?
                     (name == "field_user_measurements_file[0][fid]") ||//remove?
                     (name == "") || (value == "") ||
                     (name == "op") || (type == "submit")||
                    (name=="changed")
                    )
                {
                    continue; //jump add field if wrong token or form id
                }
                if (type == "radio") //radio button
                {
                    if (checkedState != "checked")
                    {
                        DrupCurl.AddFormField(mf, name, value);
                    }
                }
                else if (type == "checkbox")//checkbox handle
                {
                    if (checkedState == "checked")
                        DrupCurl.AddFormField(mf, name, value);
                }
                else
                    DrupCurl.AddFormField(mf, name, value);
            }
            return fields.Count;
        }
        protected string GetThumbnail(bool overwriteThumb,string contFile)
        {
            if (  (BaseSimplePDM.FileExists(contFile)) &&  ((overwriteThumb) ||
                     ((_imagefile == "")&& (_httpImagePath == "")))) //case no thumb
            {
                string thumbFN;
                ThumbnailCreator t = new ThumbnailCreator();
                Bitmap thumb = null;
                try
                {
                    thumb = t.GetThumbNail(contFile);
                    thumbFN = BaseSimplePDM.GetUniqueFileName(Enums.TEMPSAVEFOLDER +
                            BaseSimplePDM.GetFileNameWithoutExtension(contFile) + ".jpg");
                    thumb.Save(thumbFN, System.Drawing.Imaging.ImageFormat.Jpeg);
                    this._imagefile = thumbFN;
                    //thumnail status - all oter status are updated in the relevent task object
                   // this.CreateThumbnailTaskStatus = Enums.TaskStatus.Completed;
                }
                catch (Exception ex)
                {
                    errorMessage("Failed to get thumbnail: " + ex.Message, Enums.MessageSender.API_Content_GetThumb);
                   // this.CreateThumbnailTaskStatus = Enums.TaskStatus.Error;
                    //continue without thumb
                    return _imagefile;
                }
                return thumbFN;
            }
            else
                return _imagefile;
        }
        protected bool CurlWriteContentData(MultiPartForm mf, string url, bool uploadContentFile, bool asNew)
        {
            string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).Version.ToString();
            if (_revLog == "")
                _revLog = DateTime.Now.ToString() + " - Uploaded by simplePDM API ver : " + ver;
            if (CurlWriteDefaultValues(mf, url) == -1)//includes token and form id too
            {
                errorMessage("Access Denied - You do not have access to edit this page", Enums.MessageSender.API_Content_CurlWriteData);
                return false;
            }
            if ((_imagefile != "") && (_hasImage))
                DrupCurl.AddFormFile(mf, this._imagefile, Enums.HTMLField.ImageFilename);//image file name to upload

            if (_hasDescription)
                DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.Description), this.Description);//description
            if (_hasProductionState)
                DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.ProductionState), StringEnum.StrVal(this.ProductionState));//production state
            if ((uploadContentFile) && (_hasContentFile))
                if (SameContentFileRevision(asNew))
                    DrupCurl.AddFormFile(mf, _contentFile, Enums.HTMLField.ContentFileName);
                else
                {
                    errorMessage("Content file revision changed, canceling save of content", Enums.MessageSender.API_Content_CurlWriteData);
                    return false;
                }
            //global task status
            if (TaskStatus != Enums.TaskStatus.Null)
                DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.TaskStatus), StringEnum.StrVal(TaskStatus));



            //General  fields for all content types
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.Title), this.Title);//Title
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.RevisionLog), _revLog);//revision log
            if (User.Name != "Anonymous")
                DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.UserHTMLField.AuthorName), this.User.Name);
            if (OGpublic)
                DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.OgPublic), "1");
            else
                DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.OgPublic), "0");

            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.Promote), "0");
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.Sticky), "0");
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.Group), this.OrganicGroup.ID.ToString());
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.AssignedTo), AssignToID.ToString());


            int i = 0;
            //content reference
            foreach (SimplePDMContent refID in _contentRef)//objects
            {
                if (refID.NodeID != -1)
                    DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.ContentRef) + i + "][nid][nid]", "[nid:" + refID.NodeID.ToString() + "]");
                i++;
            }
            foreach (int refID in _contentRefIds)//ids
            {
                if (refID != -1)
                    DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.ContentRef) + i + "][nid][nid]", "[nid:" + refID.ToString() + "]");
                i++;
            }
            //OG tag selections
            if (this.OrganicGroup.VocabularyLists != null)
            {
                foreach (TaxonomyVocabulary tagList in this.OrganicGroup.VocabularyLists)
                {
                    // if (tagList != null)
                    foreach (TaxonomyTerm tag in tagList.Terms)
                    {
                        if ((tag.IsSelected) && (tag.TID != -1))
                            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.Tags) + "[" + tagList.VID.ToString() + "][]", tag.TID.ToString());
                        else if (tag.TID == -1) //new tag
                            DrupCurl.AddFormField(mf,
                                StringEnum.StrVal(Enums.HTMLField.Tags) + "[tags][" + tagList.VID.ToString() + "]", tag.Name);


                    }
                }
            }
            return true;
        }
        /// <summary>
        /// return true if the reviosn of the content file is the same as when loaded last time
        /// </summary>
        /// <returns></returns>
        private bool SameContentFileRevision(bool asNew)
        {
            if (asNew) //when uploading new there are no previous content
                return true;
            List<string> fields = new List<string>();
            fields.Add(StringEnum.StrVal(Enums.XMLRPCField.ContentFileNode));
            XmlRpcStruct fileNode = DrupServ.NodeGet(this.NodeID, fields.ToArray());
            int currenfileID = 0;
            currenfileID = Convert.ToInt32(DrupServ.ParseFieldArray(Enums.XMLRPCField.ContentFileNode,
                Enums.XMLRPCField.FileID, fileNode));
            if (_contentfileID == currenfileID)
                return true;
            else
                //  }
                return false;
        }
        protected bool CurlSave(MultiPartForm mf, bool asNew, Enums.SubTaskName taskName, string url)
        {
            if (_content_type == Enums.ContentType.Undefined)
            {
                return false;
            }
            DrupCurl.ClearDataIn();//clear html data in return 
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.Operation), "Save");
            DrupCurl.EasyCurl.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
            CURLcode exec = DrupCurlPerform();

            string tempHttp = "";
            DrupCurl.EasyCurl.GetInfo(CURLINFO.CURLINFO_EFFECTIVE_URL, ref tempHttp);
            if (((exec != 0)) || (tempHttp == url))
            {
                sendLogEvent("Can't save " + this._content_type.ToString() + " with Curl: " + exec.ToString(), Enums.MessageSender.API, Enums.MessageType.Error);
                if (exec == 0)
                    sendLogEvent("Error message can be seen in file: ", CurlDrupalFormErrorsFile(), Enums.MessageSender.API, Enums.MessageType.Error);
                if (asNew)
                    this.NodeID = -1;
                SubTaskDone(this, taskName);//raise event to renew task auto check
                return false;
            }
            else
            {
                if (asNew)
                {
                    this.NodeID = Convert.ToInt32(tempHttp.Substring(Enums.SERVERURL.Length + Enums.SERVERURLDEB.Length + 5));// 5 = "node/"
                    sendLogEvent(this._content_type.ToString() + " saved: " + this.HTTPLink, Enums.MessageSender.API, Enums.MessageType.Info);
                }
                else
                {
                    sendLogEvent(this._content_type.ToString() + " " + NodeID.ToString() + " updated", Enums.MessageSender.API, Enums.MessageType.Info);
                }
                SubTaskDone(this, taskName);//raise event to renew task auto check
                return true;
            }
        }
        protected string CurlGetURL(bool asNew)
        {
            return CurlGetURL(asNew, "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asNew"></param>
        /// <param name="parameters">append url with given string</param>
        protected string CurlGetURL(bool asNew, string parameters)
        {
            if (ContentType == Enums.ContentType.Undefined)
            {
                sendLogEvent("Content type not defined", Enums.MessageSender.Curl, Enums.MessageType.Error);
                return "";
            }
            else
                if (asNew) //replace '_' with '-' in content name
                    return Enums.SERVERURL + "node/add/" + StringEnum.StrVal(ContentType).Replace('_','-') + "?gids[]=" + this.OrganicGroup.ID.ToString() + "&" + parameters;
                else //edit
                    if (parameters == "")
                        return Enums.SERVERURL + "node/" + this.NodeID.ToString() + "/edit";
                    else
                        return Enums.SERVERURL + "node/" + this.NodeID.ToString() + "/edit?" + parameters;

        }
        /// <summary>
        /// Save html error to file in temp dir 
        /// </summary>
        private string CurlDrupalFormErrorsFile()
        {
            //string retMsg = "";
            //Regex messages = new Regex(@"<li>(.*?)</li>");
            //MatchCollection matchMsgs = messages.Matches(DrupCurl.HtmlDataIn);
            //for (int i = 0; i < matchMsgs.Count - 4; i++) // -5 remove extra line found
            //{
            //    retMsg += matchMsgs[i].Groups[1].Value + " \n ";
            //}
            //return retMsg;
            string filename = BaseSimplePDM.GetUniqueFileName(Enums.TEMPSAVEFOLDER + this.NodeID + ".html");
            TextWriter htmlFile = new StreamWriter(filename, false);
            htmlFile.Write(DrupCurl.HtmlDataIn);
            htmlFile.Close();
            return filename;

        }
        /// <summary>
        /// save after tak run
        /// </summary>
        public virtual bool Save(Enums.TaskStatus status)
        {
            return this.Save(status, _overwriteThumb, false);
        }
        public virtual bool Save(Enums.TaskStatus status, bool overwriteThumb, bool uploadContentFile)
        { // work around to change user assigned to robot
            TaskStatus = status;
            return Save(false, overwriteThumb, uploadContentFile);
        }
        public virtual bool Save(bool asNew, bool overwriteThumb, bool uploadContentFile)
        {
            try
            {
                if (!DrupCurl.LoggedIn)
                {
                    sendLogEvent("Curl is not logged in - trying to re-login.", Enums.MessageSender.API, Enums.MessageType.Error);
                    if (!ReLogin())
                    {
                        sendLogEvent("Unable to login", Enums.MessageSender.API, Enums.MessageType.Error);
                        SubTaskDone(false, Enums.SubTaskName.SaveDone);
                        return false;
                    }
                    else
                        sendLogEvent("Re-login successfull.", Enums.MessageSender.API, Enums.MessageType.Error);
                }
                string url = CurlGetURL(asNew);
                DrupCurl.EasyCurl.SetOpt(CURLoption.CURLOPT_URL, url);
                MultiPartForm mf = new MultiPartForm();
                if (!CurlWriteContentData(mf, url, uploadContentFile, asNew))
                    return false;
                return CurlSave(mf, asNew, Enums.SubTaskName.SaveDone, url);

            }
            catch (Exception ex)
            {
                SubTaskDone(false, Enums.SubTaskName.SaveDone);
                sendLogEvent(ex.Message, Enums.MessageSender.API_Content_Save, Enums.MessageType.Error);

                return false;
            }
        }
        /// <summary>
        /// Append log with given string
        /// </summary>
        /// <param name="append">append Existing log or overwite</param>
        /// <param name="admin">Set the log message visible to admin only</param>
        public virtual void UpdateUserTaskLog(string log, bool append)
        {
            //update task node
            if (append)
                _userTaskLog += log + Environment.NewLine;
            else
                _userTaskLog = log;
        }
        public virtual void UpdateAdminTaskLog(string log, bool append)
        {
            //update task node
            if (append)
                _adminTaskLog += log;
            else
                _adminTaskLog = log;
        }
         public  bool SaveService()
        {
            //TaskStatus = status;
            //Service save try
            try
            {
                if (!DrupServ.LoggedIn)
                {
                    SubTaskDone(false, Enums.SubTaskName.TaskSave);
                    return false;
                }
                else
                {
                    //(((ContentNode["field_dispatch_log"]) as object[])[0] as XmlRpcStruct)["value"] = AdminTaskLog;
                    //(((ContentNode["field_dispatch_log"]) as object[])[0] as XmlRpcStruct)["format"] = "2";
                    //(((ContentNode["field_task_state"]) as object[])[0] as XmlRpcStruct)["value"] = StringEnum.StrVal(status);
                    ContentNode["title"] = this.Title;
                    DrupServ.NodeSave(ContentNode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                SubTaskDone(false, Enums.SubTaskName.TaskSave);
              // errorMessage(ex.Message);
                sendLogEvent(ex.Message, Enums.MessageSender.API_Content_SaveService, Enums.MessageType.Error);

                return false;
            }
        }
        #endregion

        #region XML-RPC  Load
        /// <summary>
        /// Load the user set in the Node
        /// </summary>
        public bool loadNativeUser()
        {
            try
            {
                //load user owner of the node instead of robot ,used in case of task
                this._user = new SimplePDMUser(
                    Convert.ToInt32(this._contentNode[StringEnum.StrVal(Enums.XMLRPCField.UserID)]));
            }
            catch (Exception ex)
            {
                sendLogEvent(ex.Message, Enums.MessageSender.API_Content_LoadNativeUser, Enums.MessageType.Error);

              // errorMessage(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// load info without files
        /// </summary>
        public virtual bool Load(int nodeIDToLoad)
        {

            bool res = Load(nodeIDToLoad, "", false, false, false, null,false);
            return res;
        }
        public virtual bool Load(string localSaveFolder, bool saveFileLocaly, bool backupIfExist, bool promptOverwrite, Form ownerDialog)
        {
            return Load(this.NodeID, localSaveFolder, saveFileLocaly, backupIfExist, promptOverwrite, ownerDialog,false);
        }
        /// <summary>
        /// Load Content from simplepdm
        /// </summary>
        /// <param name="nodeIDToLoad">node id to load</param>
        /// <param name="localSaveFolder">where to save content file</param>
        /// <param name="saveFileLocaly">save content file locally</param>
        /// <param name="backupIfExist">backup content file if it exeist in target, to temp folder</param>
        /// <param name="promptOverwrite">display dialog when file in target already exist</param>
        /// <returns></returns>
        public virtual bool Load(int nodeIDToLoad,
            string localSaveFolder,
            bool saveFileLocaly,
            bool backupIfExist,
            bool promptOverwrite,
            Form ownerDialog,bool loadUser)
        {
            
            if (!DrupServ.LoggedIn || !DrupCurl.LoggedIn)  //check if drupal login was seccessful
            {
                if (!ReLogin())
                {
                    sendLogEvent("Could'nt login to Drupal services.", Enums.MessageSender.API, Enums.MessageType.Error);
                    return false;
                }
                else
                    sendLogEvent("Re-login successfull.", Enums.MessageSender.API, Enums.MessageType.Error);

            }
            try
            {
                _contentNode = DrupServ.NodeGet(nodeIDToLoad);
                if (loadUser)
                    if (!loadNativeUser())//error loading user
                    {
                        sendLogEvent("Unable to load content user", Enums.MessageSender.API, Enums.MessageType.Error);
                        return false;
                    }
                // int nid = Convert.ToInt32(DrupServ.ParseField(Enums.XMLRPCField.NodeID, _contentNode));
                if (_contentNode == null)
                {
                    if (DrupServ.ErrorCode == 1)
                        sendLogEvent("Permision error, you do not have access to this node", Enums.MessageSender.API, Enums.MessageType.Error);
                    return false;
                }

                NodeID = nodeIDToLoad;
                Title = DrupServ.ParseField(Enums.XMLRPCField.Title, _contentNode);
                setOrganicGroup(_contentNode);//read group name, vocab and selected vocab
                AssignToID = Convert.ToInt32(DrupServ.ParseFieldArray(Enums.XMLRPCField.AssignedTo, Enums.XMLRPCField.UserID, _contentNode));
                OGpublic = Convert.ToBoolean(DrupServ.ParseField(Enums.XMLRPCField.OGpublic, _contentNode));
                
                if (_hasImage)
                    _httpImagePath = DrupServ.ParseFieldArray(Enums.XMLRPCField.ImageIconArray, Enums.XMLRPCField.FilePath, _contentNode);

               if (_hasDescription)
                   Description = DrupServ.ParseFieldArray(Enums.XMLRPCField.Description, Enums.XMLRPCField.Value, _contentNode);
               if (_hasProductionState)
                    ProductionState = (Enums.ProductionState)StringEnum.Parse(typeof(Enums.ProductionState),
                        DrupServ.ParseFieldArray(Enums.XMLRPCField.ProductionState, Enums.XMLRPCField.Value, _contentNode));
                if (_hasContentFile)
                {
                object[] _contentFileNode;
                    _contentFileNode = (_contentNode[StringEnum.StrVal(Enums.XMLRPCField.ContentFileNode)] as object[]);

                    if (_contentFileNode.Length > 0)// missing  file
                        if (_contentFileNode[0] as XmlRpcStruct != null)
                        {
                            if (_contentFileNode[0].ToString() != "")
                            { //get file id for revison conmpare later
                                _contentfileID = Convert.ToInt32(DrupServ.ParseField(Enums.XMLRPCField.FileID,
                                       (_contentFileNode[0] as XmlRpcStruct)));
                                //save style file locally from server

                                if (saveFileLocaly)
                                {

                                    string httpRealPath = DrupServ.ParseField(Enums.XMLRPCField.FilePath,
                                   (_contentFileNode[0] as XmlRpcStruct)).Replace(Enums.FILEFOLDER, "");
                                    string fileName = DrupServ.ParseField(Enums.XMLRPCField.FileName,
                                        (_contentFileNode[0] as XmlRpcStruct)).Replace(Enums.FILEFOLDER, "");
                                    string HTTPContentFilePath = Enums.SERVERURL + Enums.FILESERVERURL + httpRealPath;
                                    BaseSimplePDM.CreateDir(localSaveFolder);
                                    ContentFile = localSaveFolder + fileName;

                                    if (BaseSimplePDM.FileExists(ContentFile))
                                    {   //display overwrite warning
                                        if (promptOverwrite)
                                            if (!(MessageBox.Show(ownerDialog, "The folder aready contains a file named '" + fileName + "'\n " +
                                                   "Would you like to overwrite the existing file ?",
                                                   "Confirm Overwrite ?", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                            {
                                                return false;//exit if user say no
                                            }
                                        if (backupIfExist)//backup overwited file to temp folder
                                        {
                                            string backupDir = Enums.TEMPSAVEFOLDER + @"\simplePDM_Backup\";
                                            BaseSimplePDM.CreateDir(backupDir);
                                            File.Copy(ContentFile, BaseSimplePDM.GetUniqueFileName(backupDir + fileName));
                                        }
                                    }
                                    if (!DrupCurl.DownloadFile(HTTPContentFilePath, ContentFile))
                                    {
                                        if (DrupCurl.HttpConnectionCode == 403)
                                            errorMessage("Cannot access file, permission error in node: " + this.NodeID.ToString(), Enums.MessageSender.API_Content_Load);
                                        else
                                            errorMessage("Error downloading " + this.ContentType.ToString() + " file from node: " + this.NodeID.ToString(), Enums.MessageSender.API_Content_Load);
                                        this.Valid = false; //set content not valid for task
                                        ContentFile = "";
                                    }
                                    return true;
                                }
                                return true;
                            }
                        }
                    //else
                    sendLogEvent("No " + this.ContentType.ToString() + " file attached to node : " + NodeID.ToString(), Enums.MessageSender.API, Enums.MessageType.Error);
                    this.Valid = false; //for task 
                    return false;


                }//of has content file
            }
            catch (Exception ex)
            {
                if (DrupServ.ErrorCode == 1)
                    errorMessage("Access Denied", Enums.MessageSender.API_Content_Load);
                else
                    errorMessage("Error reading " + this.ContentType.ToString() + " node: " + ex.Message, Enums.MessageSender.API_Content_Load);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Load organics group with XML-RPC
        /// </summary>
        protected bool setOrganicGroup(XmlRpcStruct xmlStruc)
        {
            try
            {
                XmlRpcStruct groupsNames = (xmlStruc[StringEnum.StrVal(Enums.XMLRPCField.OrganicGroupName)] as XmlRpcStruct);
                if (groupsNames.Count == 1)//allow only and at least one group
                {
                    foreach (System.Collections.DictionaryEntry value in groupsNames)
                    {
                        int og_id = Convert.ToInt32(value.Key);
                        if ((User.ID != 0)&&(User.ID != 1)) // in case anonimous or admin , keep the group id of the node
                            this.OrganicGroup = User.FindOrganicGroup(og_id);//set group ref from user class //new OrganicGroup(og_id, value.Value.ToString(), false);
                        else
                            this.OrganicGroup = new OrganicGroup(og_id, value.Value.ToString(), true);
                        //set selected tags
                       this.OrganicGroup.SetSelectedTags(xmlStruc[StringEnum.StrVal(Enums.XMLRPCField.Tags)] as XmlRpcStruct);

                    }
                    return true;
                }
                else
                {
                    this.errorMessage(this._content_type.ToString() + " is not part of any group", Enums.MessageSender.API_Content_SetOG);
                    return false;
                }

            }
            catch (Exception e)
            {
                this.errorMessage("Cant parse organic group: " + e.Message, Enums.MessageSender.API_Content_SetOG);
                return false;
            }
        }
        #endregion

        #region Static methods
        /// <summary>
        /// search a content title in the given group id, and return the list of all node id matching the title
        /// </summary>
        /// <returns>Content list </returns>
        static public List<SimplePDMContent> SearchContent(string contentTitle, OrganicGroup organicGroupRef,
            Enums.ContentType contentType, SimplePDMUser user)
        {
            List<SimplePDMContent> res = new List<SimplePDMContent>();
            List<string> args = new List<string>();
            args.Add(organicGroupRef.ID.ToString());
            args.Add(contentTitle);
            XmlRpcStruct[] contentView = user.DrupServ.ViewsGet(
                StringEnum.StrVal(Enums.views.SearchContentTitle), 0, args.ToArray());//retrieve styles with same type in list
            if (contentView != null)
                if (contentView.Length > 0)//view not empty
                {
                    foreach (XmlRpcStruct cont in contentView)
                    {
                        try
                        {
                            Enums.ContentType currentConType = (Enums.ContentType)StringEnum.Parse(typeof(Enums.ContentType),
                                cont[StringEnum.StrVal(Enums.XMLRPCViewField.ContentType)].ToString());
                            if (currentConType != Enums.ContentType.Undefined)
                            {
                                if ((contentType == currentConType) || (contentType == Enums.ContentType.Undefined))
                                {//check if content type is of the asked type
                                    SimplePDMContent temp = new SimplePDMContent(contentType, user);
                                    //get the first task ID in the list
                                    temp.Load(Convert.ToInt32(cont[StringEnum.StrVal(Enums.XMLRPCField.NodeID)]));
                                    /*temp.NodeID = Convert.ToInt32(cont[StringEnum.StrVal(Enums.XMLRPCField.NodeID)]);
                                    temp.Title = cont[StringEnum.StrVal(Enums.XMLRPCViewField.Title)].ToString();
                                    temp.AssignToID = Convert.ToInt32(cont[StringEnum.StrVal(Enums.XMLRPCViewField.AssignedToID)]);
                                    temp.OrganicGroup = organicGroupRef;
                                    temp.OrganicGroup.SetSelectedTags();
                                    temp.ProductionState = (Enums.ProductionState)StringEnum.Parse(typeof(Enums.ContentType),
                                        cont[StringEnum.StrVal(Enums.XMLRPCViewField.ProductionState)].ToString());
                                    if (cont[StringEnum.StrVal(Enums.XMLRPCViewField.ImageFileID)].ToString()!="")
                                        temp._imagefileID = Convert.ToInt32(cont[StringEnum.StrVal(Enums.XMLRPCViewField.ImageFileID)]);
                                    */
                                    res.Add(temp);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            sendLogEvent("Error searching Content view: " +
                                ex.Message, Enums.MessageSender.API, Enums.MessageType.Error);
                        }
                    }
                }
            return res;
        }
        #endregion

    }
    public class SimplePDMPieceTable
    {
        public string ImageFile { get { return _imageFile; } }// set { _imageFiles = value; } }
        string _imageFile = "";
        public string Name { get { return _name; } }// set { _name = value; } }
        string _name = "";
        public string Description { get { return checkNull(_description); } }// set { _imageFiles = value; } }
        string _description = "";
        public string Code { get { return checkNull(_code); } }// set { _imageFiles = value; } }
        string _code = "";
        public int Quantity { get { return _quantity; } }// set { _imageFiles = value; } }
        int _quantity = 0;
        public string Material { get { return checkNull(_material); } }// set { _imageFiles = value; } }
        string _material = "";
        public SimplePDMPieceTable(string getName, string getDescription, string getCode, string getMaterial, int getQuantity, string getImage)
        {
            _imageFile = getImage;
            _name = getName;
            _description = getDescription;
            _code = getCode;
            _material = getMaterial;
            _quantity = getQuantity;

        }
        /// <summary>
        /// Return URL parmamters
        /// </summary>
        public static string URL(int numOfRows)
        {
            string param = "";
            param += "cck_n_fields[" + Enums.PieceTableHTMLField(Enums.PieceTableField.Name) + "]=" + numOfRows + "&";
            param += "cck_n_fields[" + Enums.PieceTableHTMLField(Enums.PieceTableField.Code) + "]=" + numOfRows + "&";
            param += "cck_n_fields[" + Enums.PieceTableHTMLField(Enums.PieceTableField.Description) + "]=" + numOfRows + "&";
            param += "cck_n_fields[" + Enums.PieceTableHTMLField(Enums.PieceTableField.ImageFile) + "]=" + numOfRows + "&";
            param += "cck_n_fields[" + Enums.PieceTableHTMLField(Enums.PieceTableField.Material) + "]=" + numOfRows + "&";
            param += "cck_n_fields[" + Enums.PieceTableHTMLField(Enums.PieceTableField.Quantity) + "]=" + numOfRows + "&";
            return param; //remo
        }
        private string checkNull(string val)
        {
            if (val == "")
                return " ";
            else
                return val;
        }
    }
    public class SimplePDMCadContent : SimplePDMContent
    {
        ConvertedFiles _cadConvert = new ConvertedFiles();
        public ConvertedFiles CadConvert { get { return _cadConvert; } set { _cadConvert = value; } }
       // public Enums.TaskStatus CadConvertTaskStatus { get { return _cadConvertTaskStatus; } set { _cadConvertTaskStatus = value; } }
       // Enums.TaskStatus _cadConvertTaskStatus = Enums.TaskStatus.Null;
        public Enum CadType
        {
            get
            {
                if (_cadType == null)
                  return  Enums.GetCadType(this.ContentFile);
                else
                   return _cadType;
            }
            protected set { _cadType = value; }
        }
        Enum _cadType =null;

        public SimplePDMCadContent(Enums.ContentType conType,SimplePDMUser user)
            : base(conType, user)
        {
        }
        #region Load XML-RPC
        public override bool Load(int nodeIDToLoad,
            string localSaveFolder,
            bool saveFileLocaly,
            bool backupIfExist,
            bool promptOverwrite,
            Form ownerDialog, bool loadUser)
        {
            bool res = base.Load(nodeIDToLoad, localSaveFolder, saveFileLocaly, backupIfExist, promptOverwrite, ownerDialog, loadUser);
            try
            {
                int numOfTasks = 0;
                try
                {
                    numOfTasks = (this.ContentNode[Enums.FieldTaskType(this.ContentType)] as object[]).Length;
                }
                catch //case old content type , where the field does not exist.
                {
                    this.TaskStatus= Enums.TaskStatus.Error;
                    sendLogEvent("Can't read task type field", Enums.MessageSender.API_Content_Load, Enums.MessageType.Error);
                }
                for (int i = 0; i < numOfTasks; i++)
                {
                    string val = (((this.ContentNode[Enums.FieldTaskType(this.ContentType)] as object[])
                                           [i] as XmlRpcStruct)["value"].ToString());
                    if (val != "")
                        this.TaskType.Add((Enums.TaskType)Enum.Parse(typeof(Enums.TaskType), val.ToString(), true));
                }
                //find Cad type
                if (this.ContentFile != "")
                {
                    _cadType = Enums.GetCadType(this.ContentFile);
                }

            }
            catch (Exception e)
            {
                sendLogEvent(e.Message, Enums.MessageSender.API, Enums.MessageType.Error);
                return false;
            }
            return res;
        }
        #endregion

        public void SaveCADdata(MultiPartForm mf)
        {
            //thumbnail task
          //  if (CreateThumbnailTaskStatus != Enums.TaskStatus.Null)
           //     DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.CreateThumbnailTaskStatus),
           //         StringEnum.StrVal(CreateThumbnailTaskStatus));
         

           // if (_cadConvertTaskStatus != Enums.TaskStatus.Null)
            //    DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.CadConvertTaskStatus),
           //         StringEnum.StrVal(_cadConvertTaskStatus));

            if (_cadConvert.SaveConvertedCadFiles(mf))
                TaskStatus = Enums.TaskStatus.Completed;//update status to  
            else
                TaskStatus = Enums.TaskStatus.Error;//update status to  

            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.UserTaskLog), UserTaskLog);
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.UserTaskLogFormat), "2");
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.AdminTaskLog), AdminTaskLog);
            DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.AdminTaskLogFormat), "2");


        }
    }
    public class SimplePDMStyle : SimplePDMCadContent
    {
        #region Private and Properties
        #region  Tasks privates
        public string StyleCSVfile { get { return _styleCSVfile; } set { _styleCSVfile = value; } }
        string _styleCSVfile = "";
        public string MeasurmentCSV { get { return _measurmentCSV; } set { _measurmentCSV = value; } }
        string _measurmentCSV = "";
        public List<SimplePDMPieceTable> PieceTableRows { get { return _pieceTableRows; } set { _pieceTableRows = value; } }
        List<SimplePDMPieceTable> _pieceTableRows = new List<SimplePDMPieceTable>();
        //task status
      /* public Enums.TaskStatus GetMeasurmentTaskStatus { get { return _getMeasurmentTaskStatus; } set { _getMeasurmentTaskStatus = value; } }
        Enums.TaskStatus _getMeasurmentTaskStatus = Enums.TaskStatus.Null;
        public Enums.TaskStatus PieceTableTaskStatus { get { return _pieceTableTaskStatus; } set { _pieceTableTaskStatus = value; } }
        Enums.TaskStatus _pieceTableTaskStatus = Enums.TaskStatus.Null;*/
       
        #endregion
      
        /// <summary>
        /// Define the style type  MTM or Regular
        /// </summary>
       // public Enums.StyleType StyleType { set { _styleType = value; } }
       // Enums.StyleType _styleType = Enums.StyleType.RegularStyle;//default
        //Enums.MTMstate _mtmState = Enums.MTMstate.Invalid;//default
      
       
        #endregion

        #region Constructors
        public SimplePDMStyle(SimplePDMUser user)
            : base(Enums.ContentType.Style, user)
        {
        }
        public SimplePDMStyle(SimplePDMUser user, string getLocalFilePath, string getTitle)
            : this(user)
        {
            Title = getTitle;
            ContentFile = getLocalFilePath;
            CadType = Enums.GetCadType(getLocalFilePath);
        }
        #endregion
        #region Load XML-RPC

        /*   public override bool Load(int nodeIDToLoad,
            string localSaveFolder,
            bool saveFileLocaly,
            bool backupIfExist,
            bool promptOverwrite,
            Form ownerDialog, bool loadUser)
        {
            bool res = base.Load(nodeIDToLoad, localSaveFolder, saveFileLocaly, backupIfExist, promptOverwrite, ownerDialog,loadUser);
            try
            {
                int numOfTasks = 0;
                try
                {
                    numOfTasks = (this.ContentNode[StringEnum.StrVal(Enums.XMLRPCField.StyleTaskType)] as object[]).Length;
                }
                catch //case old content type , where the field does not exist.
                { }
                for (int i = 0; i < numOfTasks; i++)
                {
                    string val = (((this.ContentNode[StringEnum.StrVal(Enums.XMLRPCField.StyleTaskType)] as object[])
                                           [i] as XmlRpcStruct)["value"].ToString());
                    if (val != "")
                        this.TaskType.Add((Enums.TaskType)Enum.Parse(typeof(Enums.TaskType), val.ToString(), true));
                }
                //find Cad type
                if (this.ContentFile != "")
                {
                    CadType = Enums.GetCadType(this.ContentFile);
                }
                //MTM fields
               // _styleType = (Enums.StyleType)StringEnum.Parse(typeof(Enums.StyleType),
                //    DrupServ.ParseField(Enums.XMLRPCField.StyleType, ContentNode, 0));
               // _mtmState = (Enums.MTMstate)StringEnum.Parse(typeof(Enums.MTMstate),
                //    DrupServ.ParseField(Enums.XMLRPCField.MtmState, ContentNode, 0));
                         
                }
            catch (Exception e)
            {
                sendLogEvent(e.Message, Enums.MessageSender.API, Enums.MessageType.Error);
                return false;
            }
            return res;
        }*/
        #endregion
        #region Curl Save
        public void savePieceTableRow(MultiPartForm mf, int index, SimplePDMPieceTable row)
        {

            DrupCurl.AddFormField(mf, Enums.PieceTableHTMLField(Enums.PieceTableField.Name, index), row.Name);
            DrupCurl.AddFormField(mf, Enums.PieceTableHTMLField(Enums.PieceTableField.Code, index), row.Code);
            DrupCurl.AddFormField(mf, Enums.PieceTableHTMLField(Enums.PieceTableField.Description, index), row.Description);
            DrupCurl.AddFormField(mf, Enums.PieceTableHTMLField(Enums.PieceTableField.Material, index), row.Material);
            DrupCurl.AddFormField(mf, Enums.PieceTableHTMLField(Enums.PieceTableField.Quantity, index), row.Quantity.ToString());

            //image
            DrupCurl.AddFormFile(mf, row.ImageFile, "files[" + Enums.PieceTableHTMLField(Enums.PieceTableField.ImageFile) + "_" + index.ToString() + "]");
            DrupCurl.AddFormField(mf, Enums.PieceTableHTMLField(Enums.PieceTableField.ImageFile) + "[" + index.ToString() + "][list]", "1");
            DrupCurl.AddFormField(mf, Enums.PieceTableHTMLField(Enums.PieceTableField.ImageFile) + "[" + index.ToString() + "][fid]", "0");
        }
        public override bool Save(bool asNew, bool overwriteThumb, bool uploadContentFile)
        {
            try
            {
                if (!DrupCurl.LoggedIn)
                {
                    SubTaskDone(false, Enums.SubTaskName.SaveStyleDone);
                    return false;
                }
                else
                {
                    string url;
                    if (PieceTableRows.Count > 1)
                        url = base.CurlGetURL(asNew, SimplePDMPieceTable.URL(PieceTableRows.Count));
                    else
                        url = base.CurlGetURL(asNew);

                    DrupCurl.EasyCurl.SetOpt(CURLoption.CURLOPT_URL, url);
                    MultiPartForm mf = new MultiPartForm();

                    if (CadConvert.Optitex10File != null)//use converted file to generate thumb
                        this.Imagefile = GetThumbnail(overwriteThumb, CadConvert.Optitex10File.FileName);
                    else
                        this.Imagefile = GetThumbnail(overwriteThumb, ContentFile);

                    //Content fields
                    if (!base.CurlWriteContentData(mf, url, uploadContentFile, asNew))
                        return false;
                    SaveCADdata(mf);

                    #region task Piece Table
                    int i = 0;
                    foreach (SimplePDMPieceTable pt in PieceTableRows)
                    {
                        savePieceTableRow(mf, i, pt);
                        i++;
                    }
                   /* if (_pieceTableTaskStatus != Enums.TaskStatus.Null)
                        DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.PieceTableTaskStatus),
                            StringEnum.StrVal(_pieceTableTaskStatus));
                    */
                    #endregion
                    #region Task get measurment
                    if (_measurmentCSV != "")
                    {
                        DrupCurl.AddFormField(mf, StringEnum.StrVal(
                            Enums.HTMLField.MeasurmentResultCSV), _measurmentCSV);
                        // DrupCurl.AddFormField(mf, StringEnum.StrVal(
                        //   Enums.HTMLField.MeasurmentResultCSVHtmlFormat), "1");
                    }
                   /* if (_getMeasurmentTaskStatus != Enums.TaskStatus.Null)
                        DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.GetMeasurmentTaskStatus),
                            StringEnum.StrVal(_getMeasurmentTaskStatus));
                    */
                    #endregion
                    #region MTM fields
                    // if (_styleType != Enums.StyleType.Undefined)
                    // DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.StyleFileType), StringEnum.StrVal(this._styleType));//MTM or Regular

                    // if (_mtmState!=Enums.MTMstate.Undefined)
                    // DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.MtmState), StringEnum.StrVal(this._mtmState));//invalid or valid
                    DrupCurl.AddFormFile(mf, _styleCSVfile, Enums.HTMLField.StyleCSV);
                    //TODO: Check if this is not required 
                    //DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.StyleCSVFileFID), "0");
                    //DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.HTMLField.StyleCSVFileList), "1");
                    #endregion

                    return CurlSave(mf, asNew, Enums.SubTaskName.SaveStyleDone, url);
                }
            }
            catch (Exception ex)
            {
                SubTaskDone(false, Enums.SubTaskName.SaveStyleDone);
                errorMessage(ex.Message, Enums.MessageSender.API_Style_Save);
                return false;
            }

        }
        #endregion
        #region Static Methods
        /// <summary>
        /// get a zip file with all files for mtm and u.l it
        /// </summary>
        static public bool UploadMTMZip(string styleTitle, string zipFullPath, string tempUnZipFolder, SimplePDMUser author)
        {
            return UploadMTMZip(styleTitle, zipFullPath, tempUnZipFolder, author, Enums.ProductionState.NeedsReview, tempUnZipFolder + "style.pds", true);
        }
        static public bool UploadMTMZip(string styleTitle, string zipFullPath, string tempUnZipFolder,
            SimplePDMUser author, Enums.ProductionState productionState, string styleFile, bool mtm)
        {
            BaseSimplePDM.UnZip(zipFullPath, tempUnZipFolder);
            SimplePDMStyle style = new SimplePDMStyle(author, styleFile, styleTitle);
            style.ProductionState = productionState;
            style.OrganicGroup = author.OrganicGroups[0];
            if (mtm)
            {
                style.StyleCSVfile = tempUnZipFolder + "style.csv";
              //  style.StyleType = Enums.StyleType.MTMStyle;
            }

            //automatically find an image in zip and upload the first one found
            List<string> images = new List<string>();
            images.AddRange(System.IO.Directory.GetFiles(tempUnZipFolder, "*.png"));
            images.AddRange(System.IO.Directory.GetFiles(tempUnZipFolder, "*.jpg"));
            //get the first image found as style image
            if (images.Count > 0)
                style.Imagefile = images[0];

            //find style file
            if (styleFile == "")
            {
                List<string> cadFiles = new List<string>();
                cadFiles.AddRange(System.IO.Directory.GetFiles(tempUnZipFolder, "*.PDS"));
                cadFiles.AddRange(System.IO.Directory.GetFiles(tempUnZipFolder, "*.DXF"));
                cadFiles.AddRange(System.IO.Directory.GetFiles(tempUnZipFolder, "*.DSN"));
                cadFiles.AddRange(System.IO.Directory.GetFiles(tempUnZipFolder, "*.MDL"));
                //get the first image found as style image
                if (cadFiles.Count > 0)
                    styleFile = cadFiles[0];
            }

            if (!style.Save(true, false, true))
            {
                sendLogEvent("Can't upload style", Enums.MessageSender.API, Enums.MessageType.Error);
                SubTaskDone(style, Enums.SubTaskName.ZipFileULDone);
                return false;
            }
            else if (mtm) //continue with mtm rul files //TODO: replace this with the new grading rul location -> under style 
            {
                List<string> rules = new List<string>();
                rules.AddRange(System.IO.Directory.GetFiles(tempUnZipFolder, "*.rul"));
                // foreach (System.IO.FileInfo F in MyFiles)//upload all rul files
                foreach (string F in rules)//upload all rul files
                {
                    //TODO : implement 
                    //upload rule file
                    /*SimplePDMGradingRule gradeRule = new SimplePDMGradingRule(author);
                    gradeRule.OrganicGroup = author.OrganicGroups[0];
                    sendLogEvent("Uploading " + F, Enums.MessageSender.API, Enums.MessageType.Info);
                    if (!gradeRule.Save(BaseSimplePDM.GetFileNameWithoutExtension(F), Enums.ProductionState.ReadyForProduction, F, style.NodeID))
                    {
                        sendLogEvent("Can't upload grade rule " + F, Enums.MessageSender.API, Enums.MessageType.Error);

                    }*/
                    // }
                }
            }
            SubTaskDone(style, Enums.SubTaskName.ZipFileULDone);
            return true;

        }
        #endregion
    }
    public class SimplePDMMarker : SimplePDMCadContent
    {
                    
        #region Constructors
        public SimplePDMMarker(SimplePDMUser user)
            : base(Enums.ContentType.Marker, user)
        {  }
        public SimplePDMMarker(SimplePDMUser user, string getLocalFilePath, string getTitle)
            : this(user)
        {
            Title = getTitle;
            ContentFile = getLocalFilePath;
            CadType = Enums.GetCadType(getLocalFilePath);
        }
        #endregion

       
        #region Curl Save

       
        public override bool Save(bool asNew, bool overwriteThumb, bool uploadContentFile)
        {
            try
            {
                if (!DrupCurl.LoggedIn)
                {
                    SubTaskDone(false, Enums.SubTaskName.SaveStyleDone);
                    return false;
                }
                else
                {
                    string url;
                    url = base.CurlGetURL(asNew);
                    DrupCurl.EasyCurl.SetOpt(CURLoption.CURLOPT_URL, url);
                    MultiPartForm mf = new MultiPartForm();
                    //Content fields
                    
                   this.Imagefile= GetThumbnail(overwriteThumb, ContentFile);

                   if (!base.CurlWriteContentData(mf, url, uploadContentFile, asNew))
                       return false;
                   SaveCADdata(mf);
                   //task within content
                   return CurlSave(mf, asNew, Enums.SubTaskName.SaveStyleDone, url);

                }
            }
            catch (Exception ex)
            {
                SubTaskDone(false, Enums.SubTaskName.SaveStyleDone);
                errorMessage(ex.Message, Enums.MessageSender.API_Style_Save);
                return false;
            }

        }
        
        #endregion

        #region Static Methods
        
        #endregion
    }
    public class SimplePDMImage : SimplePDMContent
    {
        public SimplePDMImage(SimplePDMUser user)
            : base(Enums.ContentType.Image, user)
        {
        }
        public bool Save(string getTitle, Enums.ProductionState productionState,
            string imageFile,string originalImageFile, int styleRefID)
        {
            try
            {
                //Get assembly version
                if (DrupCurl.LoggedIn)
                {
                    this.ProductionState = productionState;
                    this.Title = getTitle;
                    this.Imagefile = imageFile;
                    this.ContentFile = originalImageFile;
                    base.ContentRefIds.Add(styleRefID);
                    string url = base.CurlGetURL(true);
                    DrupCurl.EasyCurl.SetOpt(CURLoption.CURLOPT_URL, url);

                    MultiPartForm mf = new MultiPartForm();
                    if (!base.CurlWriteContentData(mf, url, true, true))
                        return false;
                    return CurlSave(mf, true, Enums.SubTaskName.ImageSaveDone, url);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message, Enums.MessageSender.API_Style_Save);
                return false;
            }

        }
    }
}
