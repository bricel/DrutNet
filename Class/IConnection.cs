using System;
using System.Collections.Generic;
using System.Text;
using SeasideResearch.LibCurlNet;
using CookComputing.XmlRpc;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace DrutNET
{
    public interface IConnection
    {

        bool Login(string username, string password);
        /// <summary>
        /// retry to login with las username and password used
        /// </summary>
        /// <returns></returns>
        bool ReLogin();
        bool Logout();
        string Username { get; }
        bool IsLoggedIn { get; }
        string ServerURL { get; }
       // Services ServicesCon { get; }

    }
}
