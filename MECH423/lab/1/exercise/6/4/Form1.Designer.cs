namespace _4
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.disconnectBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.byteCountBox = new System.Windows.Forms.TextBox();
            this.strLenBox = new System.Windows.Forms.TextBox();
            this.queueCountBox = new System.Windows.Forms.TextBox();
            this.displayBox = new System.Windows.Forms.TextBox();
            this.axBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ayBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.azBox = new System.Windows.Forms.TextBox();
            this.saveToFileCheck = new System.Windows.Forms.CheckBox();
            this.selectFilenameBtn = new System.Windows.Forms.Button();
            this.filenameBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(13, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.Location = new System.Drawing.Point(151, 13);
            this.disconnectBtn.Name = "disconnectBtn";
            this.disconnectBtn.Size = new System.Drawing.Size(172, 23);
            this.disconnectBtn.TabIndex = 1;
            this.disconnectBtn.Text = "Disconnect Serial";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            this.disconnectBtn.Click += new System.EventHandler(this.disconnectBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial Bytes To Read";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Temp String Length";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Items In Queue";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Serial Data Stream";
            // 
            // byteCountBox
            // 
            this.byteCountBox.Location = new System.Drawing.Point(164, 62);
            this.byteCountBox.Name = "byteCountBox";
            this.byteCountBox.Size = new System.Drawing.Size(100, 22);
            this.byteCountBox.TabIndex = 6;
            // 
            // strLenBox
            // 
            this.strLenBox.Location = new System.Drawing.Point(164, 99);
            this.strLenBox.Name = "strLenBox";
            this.strLenBox.Size = new System.Drawing.Size(100, 22);
            this.strLenBox.TabIndex = 7;
            // 
            // queueCountBox
            // 
            this.queueCountBox.Location = new System.Drawing.Point(164, 138);
            this.queueCountBox.Name = "queueCountBox";
            this.queueCountBox.Size = new System.Drawing.Size(100, 22);
            this.queueCountBox.TabIndex = 8;
            // 
            // displayBox
            // 
            this.displayBox.Location = new System.Drawing.Point(17, 218);
            this.displayBox.Multiline = true;
            this.displayBox.Name = "displayBox";
            this.displayBox.Size = new System.Drawing.Size(771, 188);
            this.displayBox.TabIndex = 9;
            // 
            // axBox
            // 
            this.axBox.Location = new System.Drawing.Point(46, 416);
            this.axBox.Name = "axBox";
            this.axBox.Size = new System.Drawing.Size(100, 22);
            this.axBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 416);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Ax";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(152, 416);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Ay";
            // 
            // ayBox
            // 
            this.ayBox.Location = new System.Drawing.Point(182, 416);
            this.ayBox.Name = "ayBox";
            this.ayBox.Size = new System.Drawing.Size(100, 22);
            this.ayBox.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(288, 416);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Az";
            // 
            // azBox
            // 
            this.azBox.Location = new System.Drawing.Point(318, 416);
            this.azBox.Name = "azBox";
            this.azBox.Size = new System.Drawing.Size(100, 22);
            this.azBox.TabIndex = 15;
            // 
            // saveToFileCheck
            // 
            this.saveToFileCheck.AutoSize = true;
            this.saveToFileCheck.Location = new System.Drawing.Point(17, 444);
            this.saveToFileCheck.Name = "saveToFileCheck";
            this.saveToFileCheck.Size = new System.Drawing.Size(104, 21);
            this.saveToFileCheck.TabIndex = 16;
            this.saveToFileCheck.Text = "Save to File";
            this.saveToFileCheck.UseVisualStyleBackColor = true;
            this.saveToFileCheck.CheckedChanged += new System.EventHandler(this.saveToFileCheck_CheckedChanged);
            // 
            // selectFilenameBtn
            // 
            this.selectFilenameBtn.Location = new System.Drawing.Point(17, 471);
            this.selectFilenameBtn.Name = "selectFilenameBtn";
            this.selectFilenameBtn.Size = new System.Drawing.Size(138, 23);
            this.selectFilenameBtn.TabIndex = 17;
            this.selectFilenameBtn.Text = "Select Filename";
            this.selectFilenameBtn.UseVisualStyleBackColor = true;
            this.selectFilenameBtn.Click += new System.EventHandler(this.selectFilenameBtn_Click);
            // 
            // filenameBox
            // 
            this.filenameBox.Location = new System.Drawing.Point(164, 471);
            this.filenameBox.Name = "filenameBox";
            this.filenameBox.ReadOnly = true;
            this.filenameBox.Size = new System.Drawing.Size(624, 22);
            this.filenameBox.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.filenameBox);
            this.Controls.Add(this.selectFilenameBtn);
            this.Controls.Add(this.saveToFileCheck);
            this.Controls.Add(this.azBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ayBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.axBox);
            this.Controls.Add(this.displayBox);
            this.Controls.Add(this.queueCountBox);
            this.Controls.Add(this.strLenBox);
            this.Controls.Add(this.byteCountBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.disconnectBtn);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button disconnectBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox byteCountBox;
        private System.Windows.Forms.TextBox strLenBox;
        private System.Windows.Forms.TextBox queueCountBox;
        private System.Windows.Forms.TextBox displayBox;
        private System.Windows.Forms.TextBox axBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ayBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox azBox;
        private System.Windows.Forms.CheckBox saveToFileCheck;
        private System.Windows.Forms.Button selectFilenameBtn;
        private System.Windows.Forms.TextBox filenameBox;
    }
}

