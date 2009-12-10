namespace SimplePDM
{
    partial class SaveContentDialog
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
            this.txbMessages = new System.Windows.Forms.TextBox();
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
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbLoadNodeID = new System.Windows.Forms.TextBox();
            this.checkBoxSaveNew = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateNewThumb = new System.Windows.Forms.CheckBox();
            this.columnHeaderTags = new System.Windows.Forms.ColumnHeader();
            this.listViewTags = new System.Windows.Forms.ListView();
            this.buttonLoadTags = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbMessages
            // 
            this.txbMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMessages.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txbMessages.Location = new System.Drawing.Point(7, 181);
            this.txbMessages.Multiline = true;
            this.txbMessages.Name = "txbMessages";
            this.txbMessages.ReadOnly = true;
            this.txbMessages.Size = new System.Drawing.Size(230, 57);
            this.txbMessages.TabIndex = 8;
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
            this.buttonSaveStyle.Location = new System.Drawing.Point(132, 154);
            this.buttonSaveStyle.Name = "buttonSaveStyle";
            this.buttonSaveStyle.Size = new System.Drawing.Size(104, 22);
            this.buttonSaveStyle.TabIndex = 3;
            this.buttonSaveStyle.Text = "Update Style";
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
            this.menuStrip1.Size = new System.Drawing.Size(389, 24);
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
            this.comboBox1.Size = new System.Drawing.Size(158, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
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
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(79, 83);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(159, 44);
            this.textBoxLog.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Log Message";
            // 
            // txbLoadNodeID
            // 
            this.txbLoadNodeID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbLoadNodeID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbLoadNodeID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbLoadNodeID.Location = new System.Drawing.Point(79, 27);
            this.txbLoadNodeID.Name = "txbLoadNodeID";
            this.txbLoadNodeID.Size = new System.Drawing.Size(159, 20);
            this.txbLoadNodeID.TabIndex = 10;
            // 
            // checkBoxSaveNew
            // 
            this.checkBoxSaveNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSaveNew.AutoSize = true;
            this.checkBoxSaveNew.Location = new System.Drawing.Point(7, 158);
            this.checkBoxSaveNew.Name = "checkBoxSaveNew";
            this.checkBoxSaveNew.Size = new System.Drawing.Size(116, 17);
            this.checkBoxSaveNew.TabIndex = 7;
            this.checkBoxSaveNew.Text = "Save as New Style";
            this.checkBoxSaveNew.UseVisualStyleBackColor = true;
            this.checkBoxSaveNew.CheckedChanged += new System.EventHandler(this.checkBoxSaveNew_CheckedChanged);
            // 
            // checkBoxCreateNewThumb
            // 
            this.checkBoxCreateNewThumb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxCreateNewThumb.AutoSize = true;
            this.checkBoxCreateNewThumb.Location = new System.Drawing.Point(7, 133);
            this.checkBoxCreateNewThumb.Name = "checkBoxCreateNewThumb";
            this.checkBoxCreateNewThumb.Size = new System.Drawing.Size(143, 17);
            this.checkBoxCreateNewThumb.TabIndex = 7;
            this.checkBoxCreateNewThumb.Text = "Create a New Thumbnail";
            this.checkBoxCreateNewThumb.UseVisualStyleBackColor = true;
            this.checkBoxCreateNewThumb.CheckedChanged += new System.EventHandler(this.checkBoxSaveNew_CheckedChanged);
            // 
            // columnHeaderTags
            // 
            this.columnHeaderTags.Text = "Tags";
            this.columnHeaderTags.Width = 124;
            // 
            // listViewTags
            // 
            this.listViewTags.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTags.CheckBoxes = true;
            this.listViewTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTags});
            this.listViewTags.LabelEdit = true;
            this.listViewTags.Location = new System.Drawing.Point(244, 27);
            this.listViewTags.Name = "listViewTags";
            this.listViewTags.Size = new System.Drawing.Size(133, 185);
            this.listViewTags.TabIndex = 11;
            this.listViewTags.UseCompatibleStateImageBehavior = false;
            this.listViewTags.View = System.Windows.Forms.View.Details;
            // 
            // buttonLoadTags
            // 
            this.buttonLoadTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadTags.Location = new System.Drawing.Point(243, 218);
            this.buttonLoadTags.Name = "buttonLoadTags";
            this.buttonLoadTags.Size = new System.Drawing.Size(134, 22);
            this.buttonLoadTags.TabIndex = 3;
            this.buttonLoadTags.Text = "Load Tags";
            this.buttonLoadTags.UseVisualStyleBackColor = true;
            this.buttonLoadTags.Click += new System.EventHandler(this.buttonLoadTags_Click);
            // 
            // SaveContentDialog
            // 
            this.AcceptButton = this.buttonSaveStyle;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 243);
            this.Controls.Add(this.listViewTags);
            this.Controls.Add(this.txbLoadNodeID);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxCreateNewThumb);
            this.Controls.Add(this.checkBoxSaveNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.txbMessages);
            this.Controls.Add(this.buttonLoadTags);
            this.Controls.Add(this.buttonSaveStyle);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(405, 281);
            this.Name = "SaveContentDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SimplePDM Bridge";
            this.Shown += new System.EventHandler(this.SaveLoadContentDialog_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveContentDialog_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbMessages;
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
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbLoadNodeID;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem promptOverwriteToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxSaveNew;
        private System.Windows.Forms.CheckBox checkBoxCreateNewThumb;
        private System.Windows.Forms.ColumnHeader columnHeaderTags;
        private System.Windows.Forms.ListView listViewTags;
        private System.Windows.Forms.Button buttonLoadTags;
    }
}