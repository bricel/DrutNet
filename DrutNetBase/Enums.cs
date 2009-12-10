//#define LOCAL
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace SimplePDM
{
    static public class Enums
    {
        #region Messages
        public enum OptitexApp { PDS, Marker };
        public enum MessageType { Info, Warning, Error };
        public enum MessageSender
        {
            Installer,
            ShellLib,
            Uploader,
            AutoGrade,
            Batch,
            TaskSpooler,
            TaskSpooler_CheckQueue,
            API_Task_Save,
            API_Task_Load,
            Task_PlottingFiles,
            Task_CadConverter,
            TaskBase_LoadAll,
            Task_CreateThumb,
            Task_GetMeasurements,
            Task_PieceTable,
            TaskBase,
            API,
            API_Content_CurlWriteData,
            API_Content_Load,
            API_Content_Save,
            API_Content_SetOG,
            API_Content_SaveService,
            API_Content_LoadNativeUser,
            API_Content_GetThumb,
            API_User_SetOrganicGroup,
            API_OG_GetVocab,
            API_OG_SetTag,
            API_Style_Save,
            Measure,
            RuleFile,
            Curl,
            XML_RCP,
            ZIP, SVG,
            OptitexAddon, Other, Updater, Services
        };
        public enum SubTaskName
        {
            LoadSaveDialogSaveDone, UpdateFilesDone, TaskSave, MTMdone, ImageSaveDone,
            ScanDirDone, FileULDone, AllFileWhereUL, FileULStarting, SaveStyleDone, ZipFileULDone, SaveDone
        };
        #endregion

        #region General
        public enum WorkingUnits { CM, Inch };
        /*******************************************************
         * don't change number value, those are used to parce PDML
         ********************************************************/
        public enum Operation { Add = 0, Sub = 128, None }
        public enum MeasureMethod { X = 2, Y = 3, Straight = 1, Contour = 0, None }
        //  #if LOCAL
        // public const String SERVERURL = "http://127.0.0.1:5313/";
        // public const String SERVERURL = "http://127.0.0.1/simplepdm/";
        // public const String SERVERURL = "http://10.0.0.5/simplepdm/";
        //  #else
        private static string serverURL = "http://www.simplepdm.com/";//64.57.246.102; //default value
        /// <summary>
        /// Drupal server URL 
        /// </summary>
        public static String SERVERURL { get { return serverURL; } set { serverURL = value; } }
        // #endif
        public const String SERVERURLDEB = "";//"?XDEBUG_SESSION_START=ECLIPSE_DBGP&KEY=12457733559554/";
        public const String FILESERVERURL = "system/files/";
        public const String FILEFOLDER = "/var/www/vhosts/simplepdm.com/private/";
        public const int MAXFILESIZEKB = 2048;
        static public string TEMPSAVEFOLDER { get { return System.IO.Path.GetTempPath(); } }
        static public string TEMPBATCH { get { return BaseSimplePDM.GetUniqueFileName(TEMPSAVEFOLDER + @"tempBatch.btf"); } }
        static public string COOKIESFILE { get { return TEMPSAVEFOLDER + @"cookie.txt"; } }
        static string _logFile = TEMPSAVEFOLDER + @"simplePDMLog.txt";
        /// <summary>
        /// to set a new log file , just give a file name, the defualt temp folder is added automatically
        /// </summary>
        static public string LOGFILE
        {
            get { return _logFile; }
            set { _logFile = TEMPSAVEFOLDER + value; }
        }
        /// <summary>
        /// The file containing the version of the server, to check if any 
        /// update in the API is required.
        /// </summary>
        static public string SIMPLEPDM_INI_HTTP_PATH { get { return SERVERURL + "simplePDM_Utilities.txt"; } }

        static public string BridgeHttpPackage { get { return @"http://wiki.simplepdm.com/images/BridgeInstaller.exe"; } }
        static public string UploaderHttpPackage { get { return @"http://wiki.simplepdm.com/images/SimpleUploaderSetup.exe"; } }
        static public string inskapeEXE { get { return "C:\\program files\\inkscape\\inkscape.exe"; } }
        /// <summary>
        /// This value set the compatibility with the site version, this should 
        /// be the same version number as the install build to assure overwire 
        /// of install
        /// </summary>
        static public int ApiVersion { get { return 2; } } //release 2 - 1/7/2009
        /// <summary>
        /// Use\my Documets\simplePDM
        /// </summary>
        static public string DOCUMENTFOLDER { get { return DocFolder + @"\simplePDM\"; } }
        /// <summary>
        /// Used to replace default folder, to overcome a bug in 
        /// optitex 10.0 10.1 10.2 and 10.3, where the open command cannot open 
        /// files from the user folder. 
        /// </summary>
        static public string LOCALFOLDERPLUGIN { get { return @"C:\simplepdm\"; } }
        //public const String CURLTEMPFILESAVE = @"c:\temp.zip";
        static public string DocFolder
        {
            get
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (docPath != "")
                    return docPath;
                else
                {
                    //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\DocFolderPaths
                    //administrator
                    RegistryKey ckey = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\DocFolderPaths", true);
                    // Good to always do error checking!
                    if (ckey.GetValue("Administrator") != null)
                        if (ckey.GetValue("Administrator").ToString() != "")
                            return ckey.GetValue("Administrator").ToString(); //TODO : test this on different computers
                    return "c:";
                }
            }
        }
        public enum OpenWebPageMode
        {
            View, New, Edit, Revisons
        }
        public enum ProductionState
        {
            [DisplayValue("Draft")]
            [StringValue("draft")]
            Draft = 0,
            [DisplayValue("Needs review")]
            [StringValue("review")]
            NeedsReview = 1,
            [DisplayValue("Needs fix")]
            [StringValue("fix")]
            NeedsFix = 2,
            [DisplayValue("Ready for production")]
            [StringValue("complete")]
            ReadyForProduction = 3,
        };
        public enum ContentType
        {
            Undefined = 0,//default
            [StringValue("style")]
            Style,
            [StringValue("user")]
            User,
            [StringValue("og")]
            OrganicGroup,
            [StringValue("marker")]
            Marker,
            [StringValue("spec")]
            Spec,
            [StringValue("image")]
            Image,
            [StringValue("simplepatterns")]
            SimplePatterns,
            [StringValue("customfit")]
            CustomFit,
        }
        static public string FieldTaskType(Enums.ContentType ct)
        {
            return "field_" + StringEnum.StrVal(ct) + "_task_type";
        }
        public enum XMLRPCField
        {
            [StringValue("field_dispatch_zip_file")]
            ZipFile,
            [StringValue("field_dispatch_log")]
            Log,
            [StringValue("field_task_state")]
            TaskStatus,
            [StringValue("field_task_type")]
            TaskType,
            //[StringValue("field_style_task_type")]
            //StyleTaskType ,
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
            [StringValue("field_description")]
            Description,
            [StringValue("field_production_state")]
            ProductionState,
            [StringValue("taxonomy")]
            Tags,
            [StringValue("field_assigned")]
            AssignedTo,
            [StringValue("field_file_upload")]
            ContentFileNode,
            [StringValue("filepath")]
            FilePath,
            [StringValue("value")]
            Value,
            [StringValue("field_icon_image")]
            ImageIconArray,
            [StringValue("type")]
            ContentType,
            /* [StringValue("field_mtm_state")]
             MtmState,
             [StringValue("field_style_type")]
             StyleType,*/
        };
        public enum XMLRPCViewField
        {
            [StringValue("node_type")]
            ContentType = 1,
            [StringValue("node_title")]
            Title = 2,
            [StringValue("users_node_data_field_assigned_uid")]
            AssignedToID = 3,
            [StringValue("node_data_field_production_state_field_production_state_value")]
            ProductionState = 4,
            [StringValue("node_data_field_icon_image_field_icon_image_fid")]
            ImageFileID = 5,
        }
        public enum HTMLField
        {
            [StringValue("title")]
            Title,
            [StringValue("taxonomy")]
            Tags,
            [StringValue("field_description[0][value]")]
            Description,
            [StringValue("field_production_state[value]")]
            ProductionState,
            [StringValue("log")]
            RevisionLog,
            [StringValue("files[field_file_upload_0]")]
            ContentFileName,
            [StringValue("form_id")]
            FormID,
            [StringValue("op")]
            Operation,
            [StringValue("og_groups")]
            Group,
            [StringValue("field_description[0][format]")]
            InputFormat,
            [StringValue("status")]
            Published,
            [StringValue("og_public")]
            OgPublic,
            [StringValue("sticky")]
            Sticky,
            [StringValue("promote")]
            Promote,
            [StringValue("files[field_style_measurements_file_0]")]
            StyleCSV,
            [StringValue("field_style_measurements_file[0][fid]")]
            StyleCSVFileFID,
            [StringValue("field_style_measurements_file[0][list]")]
            StyleCSVFileList,
            [StringValue("field_assigned[uid][uid]")]
            AssignedTo,
            [StringValue("form_token")]
            FormToken,
            [StringValue("form_build_id")]
            FormBuildID,
            [StringValue("field_multi_content_reference[")]
            ContentRef,
            [StringValue("files[field_icon_image_0]")]
            ImageFilename,
            /// <summary>
            /// MTM or regular
            /// </summary>
            /*[StringValue("field_style_type[value]")]
            StyleFileType ,*/
            [StringValue("files[field_cad_file_dxf_0]")]
            DXFFile,
            [StringValue("files[field_cad_file_gerber_0]")]
            GerberFile,
            [StringValue("files[field_cad_file_lectra_0]")]
            LectraFile,
            [StringValue("files[field_cad_file_optitex9_0]")]
            Optitex9File,
            [StringValue("files[field_cad_file_optitex_0]")]
            Optitex10File,
            [StringValue("files[field_plot_file_hpgl_0]")]
            HpglFile,
            [StringValue("files[field_cad_file_pdf_0]")]
            PDFFile,
            [StringValue("files[field_cad_file_svg_0]")]
            SVGFile,
            [StringValue("field_report_style_measurements[0][value]")]
            MeasurmentResultCSV,
            [StringValue("field_report_style_measurements_data[0][format]")]
            MeasurmentResultCSVHtmlFormat,
            /*[StringValue("field_mtm_state[value]")]
            MtmState,*/
            //[StringValue("field_report_piece_table_data[0][value]")]
            //PieceTableCSV,
            // [StringValue("field_report_piece_table_image")]
            // PieceTableImageFile,
            //[StringValue("field_report_piece_table_data[0][format]")]
            //PieceTableHtmlFormat,

        };
        #endregion

        public enum views
        {
            [StringValue("gizra_pending_task")]
            PendingTasks = 0,
            [StringValue("og_ghp_ron_gizra")]
            SearchContentTitle = 1,

        }

        #region task
        public enum PieceTableField
        {
            [StringValue("name")]
            Name,
            [StringValue("code")]
            Code,
            [StringValue("description")]
            Description,//field_report_pt_description[1][value]
            [StringValue("material")]
            Material,
            [StringValue("quantity")]
            Quantity,
            [StringValue("image")]
            ImageFile,//files[field_report_pt_image_0]
        }
        /// <summary>
        /// return field name with [index][value]  
        /// </summary>
        static public string PieceTableHTMLField(Enums.PieceTableField field, int index)
        {
            //field_report_pt_name[0][value]
            return "field_report_pt_" + StringEnum.StrVal(field) + "[" + index.ToString() + "][value]";

        }
        /// <summary>
        /// return field name only
        /// </summary>
        static public string PieceTableHTMLField(Enums.PieceTableField field)
        {
            //field_report_pt_name[0][value]
            return "field_report_pt_" + StringEnum.StrVal(field);

        }
        public enum TaskHTMLField
        {
            [StringValue("field_dispatch_log[0][value]")]
            DispatchTaskLog,
            [StringValue("field_dispatch_log[0][format]")]
            DispatchTaskLogFormat,
            [StringValue("field_task_log_admin[0][value]")]
            AdminTaskLog,
            [StringValue("field_task_log_admin[0][format]")]
            AdminTaskLogFormat,
            [StringValue("field_task_log[0][value]")]
            UserTaskLog,
            [StringValue("field_task_log[0][format]")]
            UserTaskLogFormat,
            [StringValue("field_task_state[value]")]
            TaskStatus,
            [StringValue("field_task_state_measurements[value]")]
            GetMeasurmentTaskStatus,
            [StringValue("field_task_state_cad_convert[value]")]
            CadConvertTaskStatus,
            [StringValue("field_task_state_piece_table[value]")]
            PieceTableTaskStatus,
            [StringValue("field_task_state_thumbnail[value]")]
            CreateThumbnailTaskStatus,
        }
        public enum TaskStatus
        {
            [StringValue("pending")]
            Pending,
            [StringValue("completed")]
            Completed,
            [StringValue("error")]
            Error,
            [StringValue("")]
            Null,
        };
        public enum TaskType
        { //Values here are matching fields value in simplepdm, don't edit***
            undefined = 0,
            [StringValue("Custom Fit")]
            custom_fit = 1,
            [StringValue("Spec report")]
            spec = 3,
            [StringValue("CAD convert")]
            cad_convert = 4,
            [StringValue("Get measurments")]
            get_measurements,
            [StringValue("Piece table")]
            piece_table,
            [StringValue("Plot/Print export")]
            plot_print_export,
            [StringValue("Create thumbnail")]
            create_thumbnail,
            [StringValue("Simple patterns")]
            simple_patterns,
        };
        #endregion

        #region style
        /*public enum StyleType
        {
            //[StringValue("regular")]
           // Undefined = 0,
            [StringValue("regular")]
            RegularStyle = 0,
            [StringValue("mtm")]
            MTMStyle = 2,
        }
        public enum MTMstate
        {
           // [StringValue("invalid")]
           // Undefined = 0,
            [StringValue("invalid")]
            Invalid = 0,
            [StringValue("valid")]
            Valid = 1,
            
        }*/
        // public enum StyleTaskType { undefined = 0, convert = 1, };

        #endregion

        #region User
        public enum UserHTMLField
        {
            [StringValue("profile_cad_system")]
            CadSystem = 0,
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
            [StringValue("content provider")]
            ContentProvider = 106,
            [StringValue("content provider trial")]
            ContentProviderTrial = 110,
            [StringValue("content provider pending")]
            ContentProviderPending = 113,
            [StringValue("group manager")]
            GroupManager = 112,
            [StringValue("simplePDM robot")]
            SimplePDMRobot = 111
        }
        #endregion

        #region File Type extentions
        /// <summary>
        /// return the etention type cad type
        /// </summary>
        static public Enum GetCadType(string styleFilename)
        {
            FileInfo styleFile = new FileInfo(styleFilename);
            // return ((Enums.CadSystem)StringEnum.Parse(typeof(Enums.CadSystem),
            //     styleFile.Extension, true));
            string fileExtension = styleFile.Extension;
            foreach (Enums.OptitexFileTypes10 e in Enum.GetValues(typeof(Enums.OptitexFileTypes10)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return e;
            }
            foreach (Enums.OptitexFileTypes9 e in Enum.GetValues(typeof(Enums.OptitexFileTypes9)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return e;
            }
            foreach (Enums.GerberFileTypes e in Enum.GetValues(typeof(Enums.GerberFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return e;
            }
            foreach (Enums.DxfFileTypes e in Enum.GetValues(typeof(Enums.DxfFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return e;
            }
            foreach (Enums.LectraFileTypes e in Enum.GetValues(typeof(Enums.LectraFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                {
                    return e;
                }
            }
            foreach (Enums.HpglFileTypes e in Enum.GetValues(typeof(Enums.HpglFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                {
                    return e;
                }
            }
            return Enums.CadSystem.Undefined;
        }
        public static Enums.ContentType GetContentType(string fileExtension)
        {
            //look for style and marker
            foreach (Enums.OptitexFileTypes10 e in Enum.GetValues(typeof(Enums.OptitexFileTypes10)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    if ((e == OptitexFileTypes10.Style) || (e == OptitexFileTypes10.PDML))
                        return Enums.ContentType.Style;
                    else if ((e == OptitexFileTypes10.Marker) || (e == OptitexFileTypes10.MRKML))
                        return Enums.ContentType.Marker;
            }
            foreach (Enums.OptitexFileTypes9 e in Enum.GetValues(typeof(Enums.OptitexFileTypes9)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    if (e == OptitexFileTypes9.Style)
                        return Enums.ContentType.Style;
                    else if (e == OptitexFileTypes9.Marker)
                        return Enums.ContentType.Marker;
            }
            foreach (Enums.GerberFileTypes e in Enum.GetValues(typeof(Enums.GerberFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return Enums.ContentType.Style;//TODO: this can also be a marker
            }
            foreach (Enums.DxfFileTypes e in Enum.GetValues(typeof(Enums.DxfFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return Enums.ContentType.Style; //TODO: this can also be a marker
            }
            foreach (Enums.LectraFileTypes e in Enum.GetValues(typeof(Enums.LectraFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    if (e == LectraFileTypes.Style)
                        return Enums.ContentType.Style;
                    else if (e == LectraFileTypes.Marker)
                        return Enums.ContentType.Marker;

            }
            //images
            foreach (Enums.ImageFileTypes e in Enum.GetValues(typeof(Enums.ImageFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return Enums.ContentType.Image;
            }
            //specs
            foreach (Enums.SpecFileTypes e in Enum.GetValues(typeof(Enums.SpecFileTypes)))
            {
                if (StringEnum.StrVal(e).ToLower() == fileExtension.ToLower())
                    return Enums.ContentType.Spec;
            }
            return Enums.ContentType.Undefined;
        }
        public enum ImageFileTypes
        {
            [StringValue(".png")]
            png = 0,
            [StringValue(".jpg")]
            jpg = 1,
            [StringValue(".jpeg")]
            jpeg = 2,
            [StringValue(".gif")]
            gif = 3,
        }
        public enum SpecFileTypes
        {
            [StringValue(".xls")]
            excel = 0,
            [StringValue(".doc")]
            wordDoc = 1,
            [StringValue(".csv")]
            commaSeparated = 2,
            // [StringValue(".gif")]
            //gif = 3,
        }
        public enum OptitexFileTypes10
        {
            [StringValue(".pds")]
            Style,
            [StringValue(".pdml")]
            PDML,
            [StringValue(".mrk")]
            Marker,
            [StringValue(".mrkml")]
            MRKML,
        }
        public enum OptitexFileTypes9
        {
            [StringValue(".dsn")]
            Style,
            [StringValue(".dsp")]
            Marker,
        }
        public enum DxfFileTypes
        {
            [StringValue(".dxf")]
            DXF,
            [StringValue(".aama")]
            AAMA,
            [StringValue(".astm")]
            ASTM,
        }
        public enum GerberFileTypes
        {
            [StringValue(".zip")]
            ZIP,
        }
        public enum LectraFileTypes
        {
            [StringValue(".mdl")]
            Style,
            [StringValue(".plx")]
            Marker,
        }
        public enum HpglFileTypes
        {
            [StringValue(".plt")]
            HPGL_plt,
            [StringValue(".hpg")]
            HPGL_hpg,
            [StringValue(".hp2")]
            HPGL2,
        }
        public enum CadSystem
        {
            Undefined = 0,
            [StringValue("DXF")]
            DXF = 1,
            [StringValue("Gerber")]
            Gerber = 2,
            [StringValue("Lectra")]
            Lectra = 3,
            [StringValue("OptiTex version 9")]
            OptiTex9 = 4,
            [StringValue("OptiTex")]
            OptiTex10 = 5,
        }
        #endregion

        #region OptiTex Info
        static public string Ver10Key = "SOFTWARE\\OptiTex Installation";
        static public string Ver11Key = "SOFTWARE\\OptiTex 11 Installation";
        static public string OptitexPath(string versionKey) //version10 / 11 path
        {
            string path = "";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(versionKey);
            if (key != null)
                if ((path = key.GetValue("AppPath").ToString()) == null)
                    path = "";
            return path;
        }
        /// <summary>
        /// Return an array of the 4 sp number
        /// </summary>
        static public int[] OptitexSPVersion(string versionKey)
        {
            string temp;
            List<int> _optitexBuildNum = new List<int>();
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(versionKey);
                if (key != null)
                    if ((temp = key.GetValue("BuildNum").ToString()) != null)
                    {
                        string[] splitVal = null;
                        string delimStr = ".";
                        char[] delimiter = delimStr.ToCharArray();

                        splitVal = temp.Split(delimiter);
                        if (splitVal.Length == 4)
                        {
                            _optitexBuildNum.Add(Convert.ToInt32(splitVal[0]));
                            _optitexBuildNum.Add(Convert.ToInt32(splitVal[1]));
                            _optitexBuildNum.Add(Convert.ToInt32(splitVal[2]));
                            _optitexBuildNum.Add(Convert.ToInt32(splitVal[3]));
                        }
                    }
            }
            catch (Exception ex)
            {
                SimplePDM.BaseSimplePDM.sendLogEvent(ex.Message, SimplePDM.Enums.MessageSender.Installer, SimplePDM.Enums.MessageType.Error);

            }
            return _optitexBuildNum.ToArray();
        }

        #endregion
    }
}
    

