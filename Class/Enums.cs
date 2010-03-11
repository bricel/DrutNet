//#define LOCAL
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace DrutNET
{
    static public class Enums
    {
        #region Messages
       
        public enum MessageType { Info, Warning, Error };
        
        #endregion

        #region General
        /*******************************************************
         * don't change number value, those are used to parce PDML
         ********************************************************/
        static public string TEMPSAVEFOLDER { get { return System.IO.Path.GetTempPath(); } }
        static public string COOKIESFILE { get { return TEMPSAVEFOLDER + @"cookie.txt"; } }
        static string _logFile = TEMPSAVEFOLDER + @"DrupNET_Log.txt";
        static public int MAXFILESIZEKB = 2048;
        /// <summary>
        /// to set a new log file , just give a file name, the defualt temp folder is added automatically
        /// </summary>
        static public string LOGFILE
        {
            get { return _logFile; }
            set { _logFile = TEMPSAVEFOLDER + value; }
        }
        public enum OpenWebPageMode
        {
            View, New, Edit, Revisons
        }
        public enum XMLRPCField
        {
            [StringValue("og_groups")]
            OrganicGroupID,
            [StringValue("og_groups_both")]
            OrganicGroupName,
            [StringValue("og_public")]
            OGpublic,
            [StringValue("uid")]
            UserID,
            [StringValue("nid")]
            NodeID,
            [StringValue("filename")]
            FileName,
            [StringValue("fid")]
            FileID,
            [StringValue("title")]
            Title,
            [StringValue("taxonomy")]
            Tags,
            [StringValue("filepath")]
            FilePath,
            [StringValue("value")]
            Value,
            [StringValue("type")]
            ContentType,
        };
        public enum HTMLField
        {
            [StringValue("title")]
            Title,
            [StringValue("taxonomy")]
            Tags,
            [StringValue("log")]
            RevisionLog,
            [StringValue("form_id")]
            FormID,
            [StringValue("op")]
            Operation,
            [StringValue("og_groups")]
            Group,
            [StringValue("og_public")]
            OgPublic,
            [StringValue("sticky")]
            Sticky,
            [StringValue("promote")]
            Promote,
            [StringValue("form_token")]
            FormToken,
            [StringValue("form_build_id")]
            FormBuildID,

        };
        #endregion


        #region User
        public enum UserHTMLField
        {
            [StringValue("name")]
            AuthorName = 1,
            [StringValue("uid")]
            UserID = 2,
            [StringValue("roles")]
            Roles = 3,
        }
        public enum Roles
        {
            [StringValue("none")]
            None = 0,
            [StringValue("authenticated user")]
            AuthenticatedUser = 2,
            [StringValue("admin")]
            Admin = 109,
        }
        #endregion

      
      
    }
}
    

