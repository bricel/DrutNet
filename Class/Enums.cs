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
        static string _logFile = TEMPSAVEFOLDER + @"simplePDMLog.txt";
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
        public enum ContentType
        {
            [StringValue("")]
            Undefined = 0,//default
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
    

