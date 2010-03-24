using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LibCurl = SeasideResearch.LibCurlNet;
using System.IO;

namespace DrutNET
{
    /// <summary>
    /// enable to connect to drupal using cURL -- Single Tone --
    /// </summary>
    public class Curl : DrutNETBase , IConnection
    {
        
        LibCurl.Easy easy;
        string _htmlDataIn = "";
        string HtmlHeaderIn = "";
        string HtmlText="";
        string EndText = "";
        FileStream fileDownload;
        int HttpConnectCode = -1;
        public int HttpConnectionCode { get { return HttpConnectCode; } }
        LibCurl.Easy.WriteFunction wf;
        string _serverURL;
       
        /// <summary>
        /// Constructor, init curl service.
        /// </summary>
        public Curl(string serverURL)
        {
            _serverURL = serverURL;
            LibCurl.Curl.GlobalInit((int)LibCurl.CURLinitFlag.CURL_GLOBAL_DEFAULT);
            easy = new LibCurl.Easy();
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_TIMEOUT, 300);
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_COOKIEFILE,Enums.COOKIESFILE);
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_COOKIEJAR, Enums.COOKIESFILE);
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_FOLLOWLOCATION, true);
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_POST, true);

            LibCurl.Easy.DebugFunction df = new LibCurl.Easy.DebugFunction(OnDebug);
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_DEBUGFUNCTION, df);
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_VERBOSE, true);

            LibCurl.Easy.ProgressFunction pf = new LibCurl.Easy.ProgressFunction(OnProgress);
            easy.SetOpt(LibCurl.CURLoption.CURLOPT_PROGRESSFUNCTION, pf);
            
            wf = new LibCurl.Easy.WriteFunction(OnWriteData);
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
                if (_isloggedIn)
                {
                    easy.SetOpt(LibCurl.CURLoption.CURLOPT_WRITEFUNCTION, wf);

                    fileDownload = new FileStream(savePath, FileMode.Create);
                    ///replace space with %20
                    string a = " ";
                    string b = "&20";
                    httpPath = httpPath.Replace(a, b);

                    easy.SetOpt(LibCurl.CURLoption.CURLOPT_URL, httpPath);
                    LibCurl.CURLcode exec = easy.Perform();
                    //CURLcode exec = DrupCurlPerform();

                    double d = 0;
                    easy.GetInfo(LibCurl.CURLINFO.CURLINFO_CONTENT_LENGTH_DOWNLOAD, ref d);
                    //easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION,null);
                    fileDownload.Close();
                    if ((d == 0) || (this.HttpConnectCode != 200) || (!DrutNETBase.FileExists(savePath)))
                    {
                        sendLogEvent("Can't download file with Curl: " + exec.ToString(), "Curl", Enums.MessageType.Error);
                        return false;
                    }

                    return true;
                }
                else
                {
                    sendLogEvent("CURL not logged-in", "Curl", Enums.MessageType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Upload a file to Drupal using the form-file module and CURL 
        /// </summary>
        /// <param name="localPath">Local file location</param>
        /// <returns></returns>
        public bool UploadFile(string localPath)
        {
           return UploadFile(localPath,"");
        }
        /// <summary>
        /// Upload a file to Drupal using the form-file module and CURL 
        /// </summary>
        /// <param name="localPath">Local file location</param>
        /// <param name="serverDirectory">Save to a specific directory in drupal</param>
        public bool UploadFile(string localPath, string serverDirectory)
        {
            LibCurl.MultiPartForm mf = new LibCurl.MultiPartForm();
            AddFormFile(mf, localPath, "files[file_upload]");
            // Optional parameter - save to a different directory
            AddFormFile(mf, serverDirectory, "file_directory");
            ClearDataIn();//clear html data in return 
            AddFormField(mf, "op", "Upload file");
            EasyCurl.SetOpt(LibCurl.CURLoption.CURLOPT_HTTPPOST, mf);
            LibCurl.CURLcode exec = DrupCurlPerform();

            string tempHttp = "";
            EasyCurl.GetInfo(LibCurl.CURLINFO.CURLINFO_EFFECTIVE_URL, ref tempHttp);
            if (((exec != 0)) || (tempHttp == ServerURL + "file-form"))
            {
                sendLogEvent("Error uploading file, Curl error no:" + exec, "Curl", Enums.MessageType.Error);
                return false;
            }
            else
                return true;
        }

        #region Private Methods
        private LibCurl.CURLcode DrupCurlPerform()
        {
            try
            {
                return easy.Perform();
            }
            catch (Exception e)
            {
                if (e.Data != null)
                    if ((int)e.Data["Code"] == 1)
                    {
                        if (ReLogin())
                            return easy.Perform();
                    }
                return LibCurl.CURLcode.CURLE_COULDNT_CONNECT;
            }
        }
        private Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object extraData)
        {
            //Console.Write(System.Text.Encoding.UTF8.GetString(buf));
            if (fileDownload.CanWrite)
                fileDownload.Write(buf, 0, size * nmemb);
            return size * nmemb;
        }
        private LibCurl.CURLFORMcode AddFormField(LibCurl.MultiPartForm mf, object fieldName, object Value)
        {

            LibCurl.CURLFORMcode res = mf.AddSection(LibCurl.CURLformoption.CURLFORM_COPYNAME, fieldName,
                     LibCurl.CURLformoption.CURLFORM_COPYCONTENTS, Value, LibCurl.CURLformoption.CURLFORM_END);

            if (res != LibCurl.CURLFORMcode.CURL_FORMADD_OK)
                sendLogEvent("Can't add Curl field: " + res.ToString(), "Curl", Enums.MessageType.Error);
            return res;
        }
        private bool AddFormFile(LibCurl.MultiPartForm mf, string fileName, Enums.HTMLField field)
        {
            return AddFormFile(mf, fileName, StringEnum.StrVal(field));
        }
        /// <summary>
        /// add information to upload a file, also perform check of file size and file existing
        /// </summary>
        private bool AddFormFile(LibCurl.MultiPartForm mf, string fileName, string field)
        {
            if (fileName != "")
            {
                if (!DrutNETBase.FileExists(fileName))
                {
                    sendLogEvent("Can't find file : " + fileName, "Curl", Enums.MessageType.Error);
                    return false;
                }
                else
                    if ((new FileInfo(fileName).Length / 1024) > Enums.MAXFILESIZEKB)
                    {
                        sendLogEvent(fileName + " size is bigger than limit (" + (Enums.MAXFILESIZEKB / 1024).ToString() + "MB)",
                            "Curl", Enums.MessageType.Error);
                        return false;
                    }
                    else
                    {
                        LibCurl.CURLFORMcode res = mf.AddSection(LibCurl.CURLformoption.CURLFORM_COPYNAME, field,
                        LibCurl.CURLformoption.CURLFORM_FILE, fileName, LibCurl.CURLformoption.CURLFORM_END);
                        if (res != LibCurl.CURLFORMcode.CURL_FORMADD_OK)
                        {
                            sendLogEvent("Can't add Curl file: " + res.ToString(),"Curl", Enums.MessageType.Error);
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
            sendLogEvent(msg,"Curl",Enums.MessageType.Error);
        }
        private LibCurl.Easy EasyCurl
        {
            get { return easy; }
        }
        Regex rxFindHttpCode = new Regex(@"HTTP/1.1 ([^ ]*)");
        private void ClearDataIn()
        {
            _htmlDataIn = "";
        }
        private string HtmlDataIn
        {
            get { return _htmlDataIn; }
        }
        private void OnDebug(LibCurl.CURLINFOTYPE infoType, String msg, Object extraData)
        {
            if ((infoType == LibCurl.CURLINFOTYPE.CURLINFO_DATA_IN))
                _htmlDataIn += msg;
            if ((infoType == LibCurl.CURLINFOTYPE.CURLINFO_HEADER_IN))
            {
                Match httpCode = rxFindHttpCode.Match(msg);//get hettp code
                if (httpCode.Groups.Count > 1)
                    HttpConnectCode = Convert.ToInt32(httpCode.Groups[1].Value);
                HtmlHeaderIn += msg;
            }
            if ((infoType == LibCurl.CURLINFOTYPE.CURLINFO_TEXT))
                HtmlText += msg;
            if ((infoType == LibCurl.CURLINFOTYPE.CURLINFO_END))
                EndText += msg;
        }
        private void CleanupCurl()
        {
            easy.Cleanup();
            LibCurl.Curl.GlobalCleanup();
        }
        #endregion

        #region IConnection Members
        string _password;
        public bool Login(string user, string password)
        {
            try
            {
                _username = user;
                _password = password;
                string login_url = _serverURL+ "/user/login";
                easy.SetOpt(LibCurl.CURLoption.CURLOPT_URL, login_url);
                string loginFields =
                "name=" + user +
                "&pass=" + password +
                "&form_id=user_login" +
                "&op=Log in";
                easy.SetOpt(LibCurl.CURLoption.CURLOPT_POSTFIELDS, loginFields);
                LibCurl.CURLcode exec = easy.Perform();
                string retUrl = "";
                EasyCurl.GetInfo(LibCurl.CURLINFO.CURLINFO_EFFECTIVE_URL, ref retUrl);
                if ((retUrl == login_url) && (HttpConnectCode == 200)) //case already loggedin, page is returning 403, and not 200
                {
                    errorMessage("Coudn't login to your site with cURL");
                    _isloggedIn = false;
                }
                else if (HttpConnectCode == 403)//need to logout before 
                {
                    if (Logout())
                        Login(user,password);
                    else
                    {
                        errorMessage("Coudn't logout from previous logged-in user");
                        return false;
                    }
                }
                else
                    if (HttpConnectCode == 200)
                        _isloggedIn = true;
                    else
                        _isloggedIn = false;
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                _isloggedIn = false;
            }
            return _isloggedIn;
        }

        public bool ReLogin()
        {
           return Login(_username, _password);
        }

        public bool Logout()
        {
            try
            {
                easy.SetOpt(LibCurl.CURLoption.CURLOPT_URL, _serverURL + @"/logout" );
                LibCurl.CURLcode exec = easy.Perform();
                string retUrl = "";
                EasyCurl.GetInfo(LibCurl.CURLINFO.CURLINFO_EFFECTIVE_URL, ref retUrl);
                _isloggedIn = false;
                return (retUrl == _serverURL);
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }
        }
        string _username;
        public string Username
        {
            get { return _username; }
        }
        bool _isloggedIn = false;
        public bool IsLoggedIn
        {
            get { return _isloggedIn; }  
        }

        public string ServerURL
        {
            get { return _serverURL; }
        }

        #endregion

       
    }
}
