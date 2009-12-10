namespace SimplePDM
{
    partial class LoadContentDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSaveStyle = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openStyleInBrowserAfterSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.promptOverwriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.useNodeIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useStyleTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbLoadNodeID = new System.Windows.Forms.TextBox();
            this.txbMessages = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Style Title";
            // 
            // buttonSaveStyle
            // 
            this.buttonSaveStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveStyle.Location = new System.Drawing.Point(133, 83);
            this.buttonSaveStyle.Name = "buttonSaveStyle";
            this.buttonSaveStyle.Size = new System.Drawing.Size(100, 22);
            this.buttonSaveStyle.TabIndex = 3;
            this.buttonSaveStyle.Text = "Load";
            this.buttonSaveStyle.UseVisualStyleBackColor = true;
            this.buttonSaveStyle.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(246, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openStyleInBrowserAfterSaveToolStripMenuItem,
            this.promptOverwriteToolStripMenuItem,
            this.toolStripSeparator1,
            this.useNodeIDToolStripMenuItem,
            this.useStyleTitleToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // openStyleInBrowserAfterSaveToolStripMenuItem
            // 
            this.openStyleInBrowserAfterSaveToolStripMenuItem.Checked = true;
            this.openStyleInBrowserAfterSaveToolStripMenuItem.CheckOnClick = true;
            this.openStyleInBrowserAfterSaveToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openStyleInBrowserAfterSaveToolStripMenuItem.Name = "openStyleInBrowserAfterSaveToolStripMenuItem";
            this.openStyleInBrowserAfterSaveToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.openStyleInBrowserAfterSaveToolStripMenuItem.Text = "Open Content in Browser";
            // 
            // promptOverwriteToolStripMenuItem
            // 
            this.promptOverwriteToolStripMenuItem.Checked = true;
            this.promptOverwriteToolStripMenuItem.CheckOnClick = true;
            this.promptOverwriteToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.promptOverwriteToolStripMenuItem.Name = "promptOverwriteToolStripMenuItem";
            this.promptOverwriteToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.promptOverwriteToolStripMenuItem.Text = "Prompt Overwrite";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);
            // 
            // useNodeIDToolStripMenuItem
            // 
            this.useNodeIDToolStripMenuItem.Name = "useNodeIDToolStripMenuItem";
            this.useNodeIDToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.useNodeIDToolStripMenuItem.Text = "Use Node ID";
            this.useNodeIDToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // useStyleTitleToolStripMenuItem
            // 
            this.useStyleTitleToolStripMenuItem.Checked = true;
            this.useStyleTitleToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useStyleTitleToolStripMenuItem.Name = "useStyleTitleToolStripMenuItem";
            this.useStyleTitleToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.useStyleTitleToolStripMenuItem.Text = "Use Content Title";
            this.useStyleTitleToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpPageToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpPageToolStripMenuItem
            // 
            this.helpPageToolStripMenuItem.Name = "helpPageToolStripMenuItem";
            this.helpPageToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.helpPageToolStripMenuItem.Text = "Help Page";
            this.helpPageToolStripMenuItem.Click += new System.EventHandler(this.helpPageToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(79, 56);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(154, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Group";
            // 
            // txbLoadNodeID
            // 
            this.txbLoadNodeID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbLoadNodeID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbLoadNodeID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbLoadNodeID.Location = new System.Drawing.Point(79, 27);
            this.txbLoadNodeID.Name = "txbLoadNodeID";
            this.txbLoadNodeID.Size = new System.Drawing.Size(155, 20);
            this.txbLoadNodeID.TabIndex = 10;
            // 
            // txbMessages
            // 
            this.txbMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMessages.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txbMessages.Location = new System.Drawing.Point(8, 111);
            this.txbMessages.Multiline = true;
            this.txbMessages.Name = "txbMessages";
            this.txbMessages.ReadOnly = true;
            this.txbMessages.Size = new System.Drawing.Size(226, 53);
            this.txbMessages.TabIndex = 8;
            // 
            // LoadContentDialog
            // 
            this.AcceptButton = this.buttonSaveStyle;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(246, 176);
            this.Controls.Add(this.txbLoadNodeID);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbMessages);
            this.Controls.Add(this.buttonSaveStyle);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(262, 214);
            this.Name = "LoadContentDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SimplePDM Bridge";
            this.Shown += new System.EventHandler(this.SaveLoadContentDialog_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadContentDialog_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSaveStyle;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openStyleInBrowserAfterSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem useNodeIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useStyleTitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpPageToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbLoadNodeID;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem promptOverwriteToolStripMenuItem;
        private System.Windows.Forms.TextBox txbMessages;
    }
}