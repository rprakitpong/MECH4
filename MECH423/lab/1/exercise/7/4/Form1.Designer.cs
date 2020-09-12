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
            this.axBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ayBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.azBox = new System.Windows.Forms.TextBox();
            this.processNewDataPtBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.stateBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataHistoryBox = new System.Windows.Forms.TextBox();
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
            // axBox
            // 
            this.axBox.Location = new System.Drawing.Point(43, 43);
            this.axBox.Name = "axBox";
            this.axBox.Size = new System.Drawing.Size(100, 22);
            this.axBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Ax";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(149, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Ay";
            // 
            // ayBox
            // 
            this.ayBox.Location = new System.Drawing.Point(179, 43);
            this.ayBox.Name = "ayBox";
            this.ayBox.Size = new System.Drawing.Size(100, 22);
            this.ayBox.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(285, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Az";
            // 
            // azBox
            // 
            this.azBox.Location = new System.Drawing.Point(315, 43);
            this.azBox.Name = "azBox";
            this.azBox.Size = new System.Drawing.Size(100, 22);
            this.azBox.TabIndex = 15;
            // 
            // processNewDataPtBtn
            // 
            this.processNewDataPtBtn.Location = new System.Drawing.Point(13, 71);
            this.processNewDataPtBtn.Name = "processNewDataPtBtn";
            this.processNewDataPtBtn.Size = new System.Drawing.Size(206, 23);
            this.processNewDataPtBtn.TabIndex = 19;
            this.processNewDataPtBtn.Text = "Process New Datapoint";
            this.processNewDataPtBtn.UseVisualStyleBackColor = true;
            this.processNewDataPtBtn.Click += new System.EventHandler(this.processNewDataPtBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(268, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Current State";
            // 
            // stateBox
            // 
            this.stateBox.Location = new System.Drawing.Point(366, 98);
            this.stateBox.Name = "stateBox";
            this.stateBox.Size = new System.Drawing.Size(100, 22);
            this.stateBox.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "Data History";
            // 
            // dataHistoryBox
            // 
            this.dataHistoryBox.Location = new System.Drawing.Point(13, 153);
            this.dataHistoryBox.Multiline = true;
            this.dataHistoryBox.Name = "dataHistoryBox";
            this.dataHistoryBox.Size = new System.Drawing.Size(453, 343);
            this.dataHistoryBox.TabIndex = 23;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 508);
            this.Controls.Add(this.dataHistoryBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stateBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processNewDataPtBtn);
            this.Controls.Add(this.azBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ayBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.axBox);
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
        private System.Windows.Forms.TextBox axBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ayBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox azBox;
        private System.Windows.Forms.Button processNewDataPtBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox stateBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox dataHistoryBox;
    }
}

