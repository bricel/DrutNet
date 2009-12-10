using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SeasideResearch.LibCurlNet;
using SimplePDM;
using System.IO;
//using System.Web;

namespace SimplePDM
{
    /// <summary>
    /// enable to connect to drupal using cURL -- Single Tone --
    /// </summary>
    public class DrupalCurl : SimplePDM.BaseSimplePDM
    {
        static DrupalCurl singleInstance;
         bool _loggedIn = false;
         public bool LoggedIn  { get { return _loggedIn; }  }
        Easy easy;
        string _htmlDataIn = "";
        string HtmlHeaderIn = "";
        string HtmlText="";
        string EndText = "";
        FileStream fileDownload; 
        int HttpConnectCode = -1;
        public int HttpConnectionCode { get { return HttpConnectCode; } }
        Easy.WriteFunction wf;
        /// <summary>
        /// Single Tone constructor
        /// </summary>
        private DrupalCurl()
        {
            Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);
            easy = new Easy();
            easy.SetOpt(CURLoption.CURLOPT_TIMEOUT, 300);
            easy.SetOpt(CURLoption.CURLOPT_COOKIEFILE,Enums.COOKIESFILE);
            easy.SetOpt(CURLoption.CURLOPT_COOKIEJAR, Enums.COOKIESFILE);
            easy.SetOpt(CURLoption.CURLOPT_FOLLOWLOCATION, true);
            easy.SetOpt(CURLoption.CURLOPT_POST, true);
         //   easy.SetOpt(CURLoption.CURLOPT_POSTQUOTE, true);
           

            Easy.DebugFunction df = new Easy.DebugFunction(OnDebug);
            easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);
            easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);

            Easy.ProgressFunction pf = new Easy.ProgressFunction(OnProgress);
            easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);

            wf = new Easy.WriteFunction(OnWriteData);
           
            //Login(user, password);
        }
        public static DrupalCurl GetInstance()
        {
            if (singleInstance == null)
                return (singleInstance = new DrupalCurl());
            else
            {
                if (!singleInstance.LoggedIn)
                    singleInstance.errorMessage("Curl is not connected");
                return singleInstance;
            }
        }
        public static void KillInstance()
        {
            singleInstance = null;
        }
        public Int32 OnProgress(Object extraData, Double dlTotal,
        Double dlNow, Double ulTotal, Double ulNow)
        {
            double[] data = { dlNow, dlTotal, ulNow, ulTotal };
            //SubTaskDone(data, Enums.SubTaskName.DownloadUploadProgress);
            CurlDataProgress(data);
            return 0; // standard return from PROGRESSFUNCTION
        }
        
        public bool DownloadFile(string httpPath, string savePath)
        {
            try
            {
                if (_loggedIn)
                {
                    easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                    fileDownload = new FileStream(savePath, FileMode.Create);
                    ///replace space with %20
                    string a = " ";
                    string b = "&20";
                    httpPath = httpPath.Replace(a, b);

                    easy.SetOpt(CURLoption.CURLOPT_URL, httpPath);
                    CURLcode exec = easy.Perform();
                    //CURLcode exec = DrupCurlPerform();

                    double d = 0;
                    easy.GetInfo(CURLINFO.CURLINFO_CONTENT_LENGTH_DOWNLOAD, ref d);
                    //easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION,null);
                    fileDownload.Close();
                    if ((d == 0) || (this.HttpConnectCode != 200) || (!BaseSimplePDM.FileExists(savePath)))
                    {
                        sendLogEvent("Can't download file with Curl: " + exec.ToString(), Enums.MessageSender.Curl, Enums.MessageType.Error);
                        return false;
                    }

                 return true;
                }
                else
                {
                    sendLogEvent("CURL not logged-in", Enums.MessageSender.Curl, Enums.MessageType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }
        }
        private Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object extraData)
        {
            //Console.Write(System.Text.Encoding.UTF8.GetString(buf));
            if (fileDownload.CanWrite)
                fileDownload.Write(buf, 0, size * nmemb);
            return size * nmemb;
        }
        public CURLFORMcode AddFormField(MultiPartForm mf, object fieldName, object Value)
        {
           
            CURLFORMcode res = mf.AddSection(CURLformoption.CURLFORM_COPYNAME, fieldName,
                     CURLformoption.CURLFORM_COPYCONTENTS, Value, CURLformoption.CURLFORM_END);

            if (res != CURLFORMcode.CURL_FORMADD_OK)
                sendLogEvent("Can't add Curl field: " + res.ToString(), Enums.MessageSender.API, Enums.MessageType.Error);
            return res;
        }
        public bool AddFormFile(MultiPartForm mf, string fileName, Enums.HTMLField field)
        {
            return AddFormFile(mf, fileName, StringEnum.StrVal(field));
        }
        /// <summary>
        /// add information to upload a file, also perform check of file size and file existing
        /// </summary>
        public bool AddFormFile(MultiPartForm mf,  string fileName,string field)
        {
            if (fileName != "")
            {
                if (!BaseSimplePDM.FileExists(fileName))
                {
                    sendLogEvent("Can't find file : " + fileName, Enums.MessageSender.Curl, Enums.MessageType.Error);
                    return false;
                }
                else
                    if ((new FileInfo(fileName).Length / 1024) > Enums.MAXFILESIZEKB)
                    {
                        sendLogEvent(fileName + " size is bigger than limit (" + (Enums.MAXFILESIZEKB / 1024).ToString() + "MB)",
                            Enums.MessageSender.API, Enums.MessageType.Error);
                        return false;
                    }
                    else
                    {
                        CURLFORMcode res = mf.AddSection(CURLformoption.CURLFORM_COPYNAME, field,
                        CURLformoption.CURLFORM_FILE, fileName, CURLformoption.CURLFORM_END);
                        if (res != CURLFORMcode.CURL_FORMADD_OK)
                        {
                            sendLogEvent("Can't add Curl file: " + res.ToString(), Enums.MessageSender.Curl, Enums.MessageType.Error);
                            return false;
                        }
                        else
                            return true;
                    }
            }
            return false;
        }
        private void errorMessage(string msg)
        {
            sendLogEvent(msg,Enums.MessageSender.Curl,Enums.MessageType.Error);
        }
        public Easy EasyCurl
        {
            get { return easy; }
        }
        public bool Login(string user, string password)
        {
            try
            {
                string login_url = Enums.SERVERURL + "user/login"  +Enums.SERVERURLDEB;
                easy.SetOpt(CURLoption.CURLOPT_URL, login_url);
                string loginFields =
                "name=" + user +
                "&pass=" + password +
                "&form_id=user_login" +
                "&op=Log in";
                easy.SetOpt(CURLoption.CURLOPT_POSTFIELDS, loginFields);
                CURLcode exec = easy.Perform();
                string retUrl = "";
                EasyCurl.GetInfo(CURLINFO.CURLINFO_EFFECTIVE_URL, ref retUrl);
                if ((retUrl == login_url) && (HttpConnectCode == 200)) //case already loggedin, page is returning 403, and not 200
                {
                    errorMessage("Coudn't login to cURL");
                    _loggedIn = false;
                }
                else if (HttpConnectCode == 403)//logout before 
                {
                    if (this.Logout())
                        this.Login(user, password);
                    else
                    {
                        errorMessage("Coudn't logout from previous logged in user");
                        return false;
                    }
                }
                else
                    if (HttpConnectCode == 200)
                        _loggedIn = true;
                    else
                        _loggedIn = false;
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                _loggedIn = false;
            }
            return _loggedIn;
        }
        Regex rxFindHttpCode = new Regex(@"HTTP/1.1 ([^ ]*)");
        public void ClearDataIn()
        {
            _htmlDataIn = "";
        }
        public string HtmlDataIn
        {
            get { return _htmlDataIn; }
        }
        private void OnDebug(CURLINFOTYPE infoType, String msg, Object extraData)
        {
            if ((infoType == CURLINFOTYPE.CURLINFO_DATA_IN))
                _htmlDataIn += msg;
            if ((infoType == CURLINFOTYPE.CURLINFO_HEADER_IN))
            {
                Match httpCode = rxFindHttpCode.Match(msg);//get hettp code
                if (httpCode.Groups.Count > 1)
                    HttpConnectCode = Convert.ToInt32(httpCode.Groups[1].Value);
                HtmlHeaderIn += msg;
            }
            if ((infoType == CURLINFOTYPE.CURLINFO_TEXT))
                HtmlText += msg;
            if ((infoType == CURLINFOTYPE.CURLINFO_END))
                EndText += msg;
        }
        public bool Logout()
        {
            try
            {
                easy.SetOpt(CURLoption.CURLOPT_URL, Enums.SERVERURL + "logout"  +Enums.SERVERURLDEB);
                CURLcode exec = easy.Perform();
                string retUrl = "";
                EasyCurl.GetInfo(CURLINFO.CURLINFO_EFFECTIVE_URL, ref retUrl);
                _loggedIn = false;
                return (retUrl == Enums.SERVERURL + Enums.SERVERURLDEB);
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }

        }
        public void CleanupCurl()
        {
            easy.Cleanup();
            Curl.GlobalCleanup();
        }
    }
}
