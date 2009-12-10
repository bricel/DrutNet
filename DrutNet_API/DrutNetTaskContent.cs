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
    /// <summary>Represent a task node, which can be of any content type </summary>
    public class SimplePDMTask : SimplePDMContent
    {
        #region Properties and Privates
        string _HTTPZipPath;
       
        //XmlRpcStruct _taskNode;
        //public string LocalZipFile { get { return _localZipFile; } }
        //string _localZipFile; //Replaced with content prop
        /// <summary>
        /// Contain the full path of the result of the task // if mtm a pds file if spec a report file and so on
        /// </summary>
        public string ResultFile { get { return _resultFile; } set { _resultFile = value; } }
        string _resultFile = "";

        //end task related
        #endregion

        #region Constructors
        public SimplePDMTask( Enums.ContentType contentType,SimplePDMUser user)
            : base(contentType, user)
        {
            
        }
        #endregion
       

        #region Save / Load Methods
        /// <summary>
        /// update changes in simple PDM  
        /// </summary>
        /// <returns>NID</returns>
        public override bool Load(int nodeIDToLoad,
            string localSaveFolder,
            bool saveFileLocaly,
            bool backupIfExist,
            bool promptOverwrite, Form owner,bool loadUser)
        {
            bool res = base.Load(nodeIDToLoad, localSaveFolder, saveFileLocaly, backupIfExist, promptOverwrite, owner,loadUser);
            try
            {
                //load zip file for all task
                object[] fileNode = (ContentNode[StringEnum.StrVal(Enums.XMLRPCField.ZipFile)] as object[]);
                ////
                if (fileNode.Length == 0)//task missing zip file
                {
                    sendLogEvent("No ZIP file attached to task in task id : " + NodeID, Enums.MessageSender.API_Task_Load, Enums.MessageType.Error);
                    //UpdateTaskLog("No ZIP file attached to task in task id : \n" + NodeID, true, true);
                    Valid = false;
                }
                else //download ZIP file
                {
                    //convert drupal path to real path
                    string httpRealPath = (fileNode[0] as XmlRpcStruct)["filepath"].ToString().Replace(Enums.FILEFOLDER, "");
                    _HTTPZipPath = Enums.SERVERURL + Enums.FILESERVERURL + httpRealPath;
                    BaseSimplePDM.CreateDir(localSaveFolder);
                   // _localZipFile = localSaveFolder + NodeID.ToString() + ".zip";
                    this.ContentFile = localSaveFolder + NodeID.ToString() + ".zip";
                    //save zip file locally ffrom server
                    if (!DrupCurl.DownloadFile(_HTTPZipPath, this.ContentFile/* _localZipFile*/))
                    {
                        Valid = false;
                        errorMessage("Error downloading ZIP file ", Enums.MessageSender.API_Task_Load);
                    }
                }
            }
            catch (Exception e)
            {
                sendLogEvent(e.Message, Enums.MessageSender.API_Task_Load, Enums.MessageType.Error);
                return false;
            }
            return res;

        }

        public override bool Save(bool asNew, bool overwriteThumb, bool uploadContentFile)
        {
            try
            {
                if (!DrupCurl.LoggedIn)
                {
                    SubTaskDone(this, Enums.SubTaskName.TaskSave);//raise event to renew task auto check
                    return false;
                }
                else
                {
                    string url = base.CurlGetURL(asNew);
                    DrupCurl.EasyCurl.SetOpt(CURLoption.CURLOPT_URL, url);
                    MultiPartForm mf = new MultiPartForm();
                    if (!base.CurlWriteContentData(mf, url, uploadContentFile, asNew))
                        return false;
                    DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.DispatchTaskLog), AdminTaskLog);
                    DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.DispatchTaskLogFormat), "2");
                    DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.UserTaskLog), UserTaskLog);
                    DrupCurl.AddFormField(mf, StringEnum.StrVal(Enums.TaskHTMLField.UserTaskLogFormat), "2");
                    
                    return CurlSave(mf, asNew, Enums.SubTaskName.TaskSave, url);
                }

            }
            catch (Exception ex)
            {
                SubTaskDone(this, Enums.SubTaskName.TaskSave);//raise event to renew task auto check
                sendLogEvent(ex.Message, Enums.MessageSender.API_Task_Save, Enums.MessageType.Error);
                return false;
            }

        }
        /// <summary>
        /// save with services
        /// </summary>
        /// <returns></returns>
      /*  public override bool Save(Enums.TaskStatus status)
        {
            TaskStatus = status;
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
                    (((ContentNode["field_dispatch_log"]) as object[])[0] as XmlRpcStruct)["value"] = AdminTaskLog;
                    (((ContentNode["field_dispatch_log"]) as object[])[0] as XmlRpcStruct)["format"] = "2";
                    (((ContentNode["field_task_state"]) as object[])[0] as XmlRpcStruct)["value"] = StringEnum.StrVal(status);
                    DrupServ.NodeSave(ContentNode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                SubTaskDone(false, Enums.SubTaskName.TaskSave);
                errorMessage(ex.Message);
                return false;
            }
        }*/
        #endregion
    }
    public class simplePDMSimplePattern : SimplePDMTask
    {
        public simplePDMSimplePattern(SimplePDMUser user)
            : base(Enums.ContentType.SimplePatterns, user)
        {
            TaskType.Add(Enums.TaskType.simple_patterns);
        }
 
    }
    public class simplePDMCustomFit : SimplePDMTask
    {
        public simplePDMCustomFit(SimplePDMUser user)
            : base(Enums.ContentType.CustomFit, user)
        {
            TaskType.Add(Enums.TaskType.custom_fit);
        }

    }
}
