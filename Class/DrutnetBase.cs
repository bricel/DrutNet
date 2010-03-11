
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;

namespace DrutNET
{
    /// <summary>
    /// Base Class with general functions
    /// </summary>
    public class DrutNETBase
    {
        //Log Event -----------------------------------------
        public delegate void UpdateLog(string str, string mSender, Enums.MessageType mType,bool verbose);
        public delegate void CurlDataProgressDel(object info);
        //Statics
        static public event UpdateLog OnUpdateLog;
        static public event CurlDataProgressDel OnCurlDataProgress;

        #region general methods
        static public void OpenWebPage(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
        static public void CurlDataProgress(object info)
        {
            if (OnCurlDataProgress != null)
            {
                OnCurlDataProgress(info);
            }
        }
        #endregion

        #region log file and messages
        /// <summary>
        /// Send a message event 
        /// </summary>
        /// <param name="verbose"></param>
        static public void sendLogEvent(string message, string mSender, Enums.MessageType mType)
        {
           sendLogEvent(message,mSender,mType,false);

        }
        static public void sendLogEvent(string message, string mSender, Enums.MessageType mType, bool verbose)
        {
            sendLogEvent(message, "", mSender, mType, verbose);

        }
        static public void sendLogEvent(string message, string errorFile, string mSender, Enums.MessageType mType)
        {
           sendLogEvent(message,errorFile,mSender,mType,false);
        }
        static public void sendLogEvent(string message, string errorFile, string mSender, Enums.MessageType mType, bool verbose)
        {
            if (OnUpdateLog != null)
            {
                //set error file as link for dialog and regular for log file
                string errorFileLink = "";
                if (errorFile != "")
                    errorFileLink = "file:///" + errorFile.Replace(" ", "%20");
                OnUpdateLog(message + errorFileLink, mSender, mType, verbose);
                // Always write verbose message to log file
                writeLogToFile(message + errorFile, mSender, mType);
            }
        }
        static private void writeLogToFile(string message, string mSender, Enums.MessageType mType)
        {
            TextWriter logFile = new StreamWriter(Enums.LOGFILE, true);
            string date = DateTime.Now.ToString();
            string prefix = date + " - " + mSender.ToString() + " - ";
            string msg = "";

            if (mType == Enums.MessageType.Info)
            {
                msg = prefix + " - " + message;
            }
            else
                if (mType == Enums.MessageType.Error)
                {
                    msg = prefix + " - ## ERROR ## - " + message;
                }
                else
                    if (mType == Enums.MessageType.Warning)
                    {
                        msg = prefix + " - ## Warning ## - " + message;
                    }
            logFile.WriteLine(msg);
            logFile.Close();

        }
        private void sendErrorLogEvent(string message,bool verbose)
        {
            if (OnUpdateLog != null)
            {
                OnUpdateLog(message, "", Enums.MessageType.Error, verbose);
            }
        }
        private void sendErrorLogEvent(string message)
        {
            if (OnUpdateLog != null)
            {
                OnUpdateLog(message, "", Enums.MessageType.Error, false);
            }
        }
        #endregion

        #region AutoComplete
        static public void WriteAutoCompleteFile(string autoCompleteStr, string path)
        {
            DrutNETBase.CreateDir(DrutNETBase.GetPath(path));//create directory if not existing
            TextWriter autoCompleteFile = new StreamWriter(path, true);
            autoCompleteFile.WriteLine(autoCompleteStr);
            autoCompleteFile.Close();

        }
        static public string[] ReadAutoCompleteFile(string path)
        {
            List<string> valList = new List<string>();
            if (DrutNETBase.FileExists(path))
            {
                TextReader autoCompleteFile = new StreamReader(path);
                string val;
                while ((val = autoCompleteFile.ReadLine()) != null)
                    valList.Add(val);
                autoCompleteFile.Close();
            }
            return valList.ToArray();
        }
        #endregion

        #region File Reading Methods
        ///-----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// read int32 
        /// </summary>
        /// <param name="data">byte array</param>
        /// <param name="startIndex">start index</param>
        /// <returns>value</returns>
        protected List<int> readInt32(BinaryReader ruleFileBinaryReader, int numOfIntToRead)
        {
            List<int> values = new List<int>();
            try
            {
                for (int i = 0; i < numOfIntToRead; i++)
                {
                    values.Add(ruleFileBinaryReader.ReadInt32());
                }
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return null;
            }
            return values;
        }
        /// <summary>
        /// read single int 32
        /// </summary>
        /// <param name="ruleFileBinaryReader"></param>
        /// <returns></returns>
        protected int readInt32(BinaryReader ruleFileBinaryReader)
        {
            int i;
            try
            {
                i = ruleFileBinaryReader.ReadInt32();
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return -1;
            }
            return i;
        }
        /// <summary>
        /// Read floats into an array
        /// </summary>
        /// <param name="ruleFileBinaryReader"></param>
        /// <param name="numOfIFloatToRead"></param>
        /// <returns></returns>
        protected List<float> readFloat(BinaryReader ruleFileBinaryReader, int numOfIFloatToRead)
        {

            List<float> values = new List<float>();
            try
            {
                for (int i = 0; i < numOfIFloatToRead; i++)
                {
                    values.Add(ruleFileBinaryReader.ReadSingle());
                }
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return null;
            }
            return values;
        }
        /// <summary>
        /// read single float
        /// </summary>
        /// <param name="ruleFileBinaryReader"></param>
        /// <returns></returns>
        protected float readFloat(BinaryReader ruleFileBinaryReader)
        {
            float f;
            try
            {
                f = ruleFileBinaryReader.ReadSingle();
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return -1;
            }
            return f;
        }
        ///-----------------------------------------------------------------------------------------------------------
        protected string readString(BinaryReader ruleFileBinaryReader, int numOfCharToRead)
        {

            try
            {
                char[] ch = ruleFileBinaryReader.ReadChars(numOfCharToRead);
                string s = "";
                for (int i = 0; i < ch.Length - 1; i++)//not saving \0 
                {
                    s += ch[i];
                }
                return s;
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return null;
            }
        }
        protected byte readByte(BinaryReader ruleFileBinaryReader)
        {
            try
            {
                return ruleFileBinaryReader.ReadByte();
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return 0;
            }
        }
        ///-----------------------------------------------------------------------------------------------------------
        #endregion

        #region File Writing Methods

        protected bool writeStringToFile(BinaryWriter ruleFileBinaryWriter, string s)
        {
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ruleFileBinaryWriter.Write(s[i]);
                }
                ruleFileBinaryWriter.Write('\0');
                return true;
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return false;
            }
        }
        protected bool writeStringToFile(BinaryWriter ruleFileBinaryWriter, string s, bool endChar)
        {
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ruleFileBinaryWriter.Write(s[i]);
                }
                if (endChar)
                    ruleFileBinaryWriter.Write('\0');
                return true;
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return false;
            }
        }
        protected bool writeStringAndLengthToFile(BinaryWriter ruleFileBinaryWriter, string s)
        {
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ruleFileBinaryWriter.Write(s[i]);
                }
                ruleFileBinaryWriter.Write('\0');
                return true;
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return false;
            }
        }
        protected bool writeInt32ToFile(BinaryWriter ruleFileBinaryWriter, int i)
        {
            try
            {
                ruleFileBinaryWriter.Write(i);
                return true;
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return false;
            }
        }
        protected bool writeFloatToFile(BinaryWriter ruleFileBinaryWriter, float f)
        {
            try
            {
                ruleFileBinaryWriter.Write(f);
                return true;
            }
            catch (Exception e)
            {
                sendErrorLogEvent(e.Message);
                return false;
            }
        }
        ///-----------------------------------------------------------------------------------------------------------
        #endregion


        #region file and folder methods
        /// <summary>
        /// Return the path of the file
        /// </summary>
        public static string GetPath(string FileName)
        {
            if (FileName != "")
                return System.IO.Path.GetDirectoryName(FileName);
            else
                return "";
            //return System.IO.Path.GetFullPath(FileName);
        }
        /// <summary>
        /// Remove the extension and path of a filename
        /// </summary>
        public static string GetFileNameWithoutExtension(string FileName)
        {
            return GetFileNameWithoutExtension(FileName, false);
        }
        /// <summary>
        /// remove extension and keep path if withPath is set to true
        /// </summary>
        public static string GetFileNameWithoutExtension(string FileName, bool withPath)
        {
            if (withPath)
                return System.IO.Path.GetDirectoryName(FileName) + "\\" + System.IO.Path.GetFileNameWithoutExtension(FileName); 
            else
                return System.IO.Path.GetFileNameWithoutExtension(FileName);
        }
        /// <summary>
        /// replace the filename extension
        /// </summary>
        /// <param name="FileName">original file name to change</param>
        /// <param name="newExtension">new extension , with the dot</param>
        /// <returns></returns>
        public static string ReplaceFileExtension(string FileName,string newExtension)
        {
            return System.IO.Path.GetDirectoryName(FileName) + "\\"+ System.IO.Path.GetFileNameWithoutExtension(FileName) + newExtension;
        }
        /// <summary>
        /// Return file name and extension without path
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetFileName(string FileName)
        {
            return System.IO.Path.GetFileName(FileName);
        }
        /// <summary>
        /// return file extension with leading point
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetExtension(string FileName)
        {
            return System.IO.Path.GetExtension(FileName);
        }
        public static DirectoryInfo CreateDir(string dir)
        {
            return Directory.CreateDirectory(dir);
        }
        public static bool FileExists(string file)
        {
            return File.Exists(file);
        }
        public static bool DirectoryExists(string dir)
        {
            return Directory.Exists(dir);
        }  

        public static void UnZip(string pathToUnZip, string targetFolder)
        {
            FastZip fz = new FastZip();
            try
            {
                fz.ExtractZip(pathToUnZip, targetFolder, "");
            }
            catch (Exception e)
            {
                DrutNETBase.sendLogEvent("Unzip Error  : " + e.Message, "ZIP", Enums.MessageType.Error);
            }

        }
        /// <summary>
        /// check if filename exist , and return a filename_num that is unique
        /// </summary>
        /// <param name="sourceFN"></param>
        /// <returns></returns>
        public static string GetUniqueFileName(string sourceFN)
        {
            return GetUniqueFileName(sourceFN, true);
        }
        public static string GetUniqueFileName(string sourceFN,bool useUnderScore)
        {
            //rename file if it exists
            int i = 0;
            string org = sourceFN;
            while (DrutNETBase.FileExists(sourceFN))
            {
                string separator= "-";
                if (useUnderScore)
                    separator = "_";
                sourceFN =System.IO.Path.GetDirectoryName(org)+"\\"+ System.IO.Path.GetFileNameWithoutExtension(org) +
                    separator + i.ToString() +
                   System.IO.Path.GetExtension(org);
                i++;

            }
            return sourceFN;
        }
        /// <summary>
        /// return a unique dir name
        /// </summary>
        /// <param name="dir">Directory without ending slash </param>
        public static string GetUniqueDirectory(string dir, bool useUnderScore)
        {
            //rename file if it exists
            int i = 0;
            string org = dir;
            while (DrutNETBase.DirectoryExists(dir))
            {
                string separator = "-";
                if (useUnderScore)
                    separator = "_";
                dir = org + separator + i.ToString();
                i++;

            }
            return dir;
        }
        public static void DeleteAllFiles(string directory,string fileNotToDelete)
        {
            Array.ForEach(Directory.GetFiles(directory),
              delegate(string path) 
              { 
                  if (path!=fileNotToDelete)
                  File.Delete(path); 
              });

        }


        #endregion

        #region file download
        /// <summary>
        /// Download a public file from a URL
        /// </summary>
        /// <param name="httpPath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        static public bool DownloadHTTPFile(string httpPath, string savePath)
        {
            try
            {
                System.Net.WebClient Client = new System.Net.WebClient();
                Client.DownloadFile(httpPath, savePath);
                if (FileExists(savePath))
                    return true;
                else
                    return false;
            }

            catch (Exception ex)
            {
                sendLogEvent(ex.Message,"General",Enums.MessageType.Error);
                return false;
            }
        }
       
        #endregion
    }
}
