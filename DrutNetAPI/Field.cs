using System.Collections.Generic;
using System;
using System.Text;
using SeasideResearch.LibCurlNet;
using CookComputing.XmlRpc;
using System.Collections;
using DrutNet;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DrutNet
{
    /// <summary>
    /// represent a field inside a content, still in development
    /// </summary>
    class SimplepdmField
    {
        string _xmlRpcField;
        string _htmlField;
        object _value;
        object _defaultValue;

        public SimplepdmField()
        {
        }
        public bool Read()
        { return true; }

        public bool Save()
        { return true; }
        


    }
    public class ConvertedFile : IConnection
    {
        string _ext = "";
        string _baseFilename = "";
        Enums.HTMLField _htmlField;
        public string FileName { get { return _baseFilename + _ext; } }
       // bool _upload = false;
        //public bool Upload { set { _upload = value; } }
        public ConvertedFile(string extention, string baseFilename,Enums.HTMLField htmlField)
        {
            _htmlField = htmlField;
            _ext = extention;
            _baseFilename = baseFilename;
        }
        public bool Save(MultiPartForm mf)
        {
           // if (_upload)
           // {
            if (FileExists(FileName))
                return DrupCurl.AddFormFile(mf, FileName, _htmlField);
            else
            {
                sendLogEvent("Can't convert to file " + FileName, Enums.MessageSender.Task_CadConverter, Enums.MessageType.Error);
                return false;
            }
           // }
           // return true;
        }
    }

   public class ConvertedFiles : IConnection
    {
        #region Private and Properties
        public ConvertedFile DXFfile;
        public ConvertedFile SVGfile;
        public ConvertedFile GerberFile ;
        public ConvertedFile GerberCutFile;
        public ConvertedFile LectraFile;
        public ConvertedFile Optitex9File;
        public ConvertedFile HpglFile;
        public ConvertedFile PDFFile;
        public ConvertedFile Optitex10File;
        #endregion
        public ConvertedFiles()
        {
        }
        public bool SaveConvertedCadFiles(MultiPartForm mf)
        {
            bool Status = true;
            if (DXFfile != null)
                Status &= DXFfile.Save(mf);
            if (GerberFile != null)
                Status &= GerberFile.Save(mf);
            if (LectraFile != null)
                Status &= LectraFile.Save(mf);
            if (Optitex9File != null)
                Status &= Optitex9File.Save(mf);
            if (GerberFile != null)
                Status &= GerberFile.Save(mf);
            // TODO: change this when patch for Zorana is replaced by custom format implementation.
            if (GerberCutFile != null)
                Status &= GerberCutFile.Save(mf);
            if (HpglFile != null)
                Status &= HpglFile.Save(mf);
            if (Optitex10File != null)
                Status &= Optitex10File.Save(mf);
            if (PDFFile != null)
                Status &= PDFFile.Save(mf);
            if (SVGfile!= null)
                Status &= SVGfile.Save(mf);
            return Status;


            /*DrupCurl.AddFormFile(mf, GerberFile, Enums.HTMLField.GerberFile);
            DrupCurl.AddFormFile(mf, LectraFile, Enums.HTMLField.LectraFile);
            DrupCurl.AddFormFile(mf, DXFfile, Enums.HTMLField.DXFFile);
            DrupCurl.AddFormFile(mf, Optitex10File, Enums.HTMLField.Optitex10File);
            DrupCurl.AddFormFile(mf, HpglFile, Enums.HTMLField.HpglFile);
            DrupCurl.AddFormFile(mf, Optitex9File, Enums.HTMLField.Optitex9File);
            DrupCurl.AddFormFile(mf, PDFFile, Enums.HTMLField.PDFFile);*/
        }
    }
    
}
