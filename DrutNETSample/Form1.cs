using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DrutNET;
using CookComputing.XmlRpc;
using System.Collections;

using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace DrutNETSample
{
    public partial class Form1 : Form
    {
        XmlRpcStruct _node;
        Services _serviceCon;
        Curl _curlCon;
        public Form1()
        {
            InitializeComponent();
            // This handle all message throw by the system.
            DrutNETBase.OnUpdateLog += new DrutNETBase.UpdateLog(DrutNETBase_OnUpdateLog);
            textBox_userName.Text = "admin";
            textBox_password.Text = "1234";
            textBox_url.Text = "http://10.0.0.8/drupal7";
            textBox_endpoint.Text = "test";



        }

        bool _started = false;
        /// <summary>
        /// Progress upload of file with CURL 
        /// </summary>
        /// <param name="info"></param>
        void DrutNETBase_OnCurlDataProgress(ProgressDataStruct info)
        {
            
            if (info.ulTotal > 0)
            {
                progressBar1.Maximum = Convert.ToInt32(info.ulTotal);
                _started = true;
            }
            if (_started)
            {
                progressBar1.Value = Convert.ToInt32(info.ulNow);
                if ((info.ulTotal == info.ulNow))
                {
                    _started = false;
                    DrutNETBase.OnCurlDataProgress -= new DrutNETBase.CurlDataProgressDel(DrutNETBase_OnCurlDataProgress);
                }
            }
        }
        /// <summary>
        /// Log event message handeling
        /// </summary>
        void DrutNETBase_OnUpdateLog(string str, string mSender, Enums.MessageType mType, bool verbose)
        {
            // Write log messages on error window.
            richTextBox_messages.Text = mType.ToString() + " - " + mSender + ": " + str + richTextBox_messages.Text;
        }
        /// <summary>
        /// Load a node
        /// </summary>
        private void button_load_Click(object sender, EventArgs e)
        {
            // Node to load
            if (_serviceCon != null)
            {
                _node = _serviceCon.NodeGet(Convert.ToInt32(textBox_nodeID.Text));
                if (_node != null)
                {
                    XmlRpcStruct body = _node["body"] as XmlRpcStruct;
                    //body["und"] as XmlRpcStruct
                    richTextBox1.Text = _node["body"].ToString();
                }
            }
        }
        /// <summary>
        /// Save node
        /// </summary>
        private void button_save_Click(object sender, EventArgs e)
        {
            if (_node != null)
            {
                // Reload node to prevent access restriction, by other user.
                _node = _serviceCon.NodeGet(Convert.ToInt32(textBox_nodeID.Text));
                // Update node.
                _node["body"] = richTextBox1.Text;
                _serviceCon.NodeSave(_node);
            }
            else
                DrutNETBase.sendLogEvent("No node was loaded, load a node first \n", "My Sample", Enums.MessageType.Error);
        }
        /// <summary>
        /// Update HTML preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = richTextBox1.Text;
        }
        /// <summary>
        /// Upload selected file
        /// </summary>
        private void button_upload_Click(object sender, EventArgs e)
        {
            DrutNETBase.OnCurlDataProgress += new DrutNETBase.CurlDataProgressDel(DrutNETBase_OnCurlDataProgress);
            progressBar1.Value = 0;
            int fid;
            if ((fid = _curlCon.FileUpload(textBox1.Text)) == -1)
                DrutNETBase_OnUpdateLog("Unable to upload file \n", "Drutnet Sample", Enums.MessageType.Error, false);
            else
            {
                // Add a file to the node
                if (textBox_fileNode.Text!="")
                {
                    int nid =  Convert.ToInt32(textBox_fileNode.Text);
                    _serviceCon.AttachFileToNode(textBox_fieldName.Text, fid, 0,nid);
                }
                DrutNETBase_OnUpdateLog("File uploaded successfully to FID :" + fid + "\n", "Drutnet Sample", Enums.MessageType.Info, false);

            }
        }
        /// <summary>
        /// Browse file to upload.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            // Create a settings object to define connection settings.
            ServicesSettings settings = new ServicesSettings();
            settings.DrupalURL = textBox_url.Text; // 
            settings.UseSessionID = checkBox_sessionID.Checked;
            settings.EndPoint = textBox_endpoint.Text;
            settings.CleanURL = true;
            
            /*settings.UseKeys = true;//Not Implemented yet
            //settings.Key = "03cfd62180a67dcbcb1be9a7f78dc726";
            settings.DomainName = "localhost";*/

            // Create a connection object
            _serviceCon = new Services(settings);
            

           
               if (_serviceCon.Login(textBox_userName.Text, textBox_password.Text))
                   DrutNETBase.sendLogEvent("Login to services successful \n", "Sample", Enums.MessageType.Info);
        }
        private void button_logout_Click(object sender, EventArgs e)
        {
            _serviceCon.Logout();
        }

        private void button_login_curl_Click(object sender, EventArgs e)
        {
            ServicesSettings settings = new ServicesSettings();
            settings.DrupalURL = textBox_url.Text; // 
            settings.UseSessionID = checkBox_sessionID.Checked;
            // Login to drupal
            _curlCon = new Curl(settings.DrupalURL);

             if (_curlCon.Login(textBox_userName.Text, textBox_password.Text))
                 DrutNETBase.sendLogEvent("Login to curl successful \n","Sample",Enums.MessageType.Info);
        }

        private void button_logout_curl_Click(object sender, EventArgs e)
        {
            _curlCon.Logout();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
