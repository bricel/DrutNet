namespace DrutNETSample
{
    partial class Form1
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button_save = new System.Windows.Forms.Button();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.button_load = new System.Windows.Forms.Button();
            this.label_nid = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 41);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(678, 360);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(12, 417);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 1;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(12, 446);
            this.textBox_message.Multiline = true;
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(678, 69);
            this.textBox_message.TabIndex = 2;
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(135, 9);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(75, 23);
            this.button_load.TabIndex = 1;
            this.button_load.Text = "Load node";
            this.button_load.UseVisualStyleBackColor = true;
            // 
            // label_nid
            // 
            this.label_nid.AutoSize = true;
            this.label_nid.Location = new System.Drawing.Point(14, 15);
            this.label_nid.Name = "label_nid";
            this.label_nid.Size = new System.Drawing.Size(26, 13);
            this.label_nid.TabIndex = 3;
            this.label_nid.Text = "NID";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(46, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(83, 20);
            this.textBox2.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 527);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label_nid);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Label label_nid;
        private System.Windows.Forms.TextBox textBox2;
    }
}

