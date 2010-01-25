using System.Collections.Generic;
using System;
using System.Text;
using SeasideResearch.LibCurlNet;
using CookComputing.XmlRpc;
using System.Collections;
using DrutNET;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DrutNET
{
    /// <summary>
    /// represent a field inside a content, still in development
    /// </summary>
    class Field
    {
        string _xmlRpcField;
        string _htmlField;
        object _value;
        object _defaultValue;

        public Field()
        {
        }
        public bool Read()
        { return true; }

        public bool Save()
        { return true; }
        
    }
    
}
