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
            this.label2 = new System.Windows.Forms.Label();
            this.dataHistoryBox = new System.Windows.Forms.TextBox();
            this.baudrateBox = new System.Windows.Forms.TextBox();
            this.byteToWriteBox = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(139, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.Location = new System.Drawing.Point(277, 12);
            this.disconnectBtn.Name = "disconnectBtn";
            this.disconnectBtn.Size = new System.Drawing.Size(172, 23);
            this.disconnectBtn.TabIndex = 1;
            this.disconnectBtn.Text = "Disconnect Serial";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            this.disconnectBtn.Click += new System.EventHandler(this.disconnectBtn_Click);
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
            // baudrateBox
            // 
            this.baudrateBox.Location = new System.Drawing.Point(17, 12);
            this.baudrateBox.Name = "baudrateBox";
            this.baudrateBox.Size = new System.Drawing.Size(100, 22);
            this.baudrateBox.TabIndex = 24;
            this.baudrateBox.Text = "9600";
            // 
            // byteToWriteBox
            // 
            this.byteToWriteBox.Location = new System.Drawing.Point(17, 49);
            this.byteToWriteBox.Name = "byteToWriteBox";
            this.byteToWriteBox.Size = new System.Drawing.Size(100, 22);
            this.byteToWriteBox.TabIndex = 25;
            this.byteToWriteBox.Text = "a";
            // 
            // sendBtn
            // 
            this.sendBtn.Location = new System.Drawing.Point(139, 49);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(133, 23);
            this.sendBtn.TabIndex = 26;
            this.sendBtn.Text = "Send byte";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 508);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.byteToWriteBox);
            this.Controls.Add(this.baudrateBox);
            this.Controls.Add(this.dataHistoryBox);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox dataHistoryBox;
        private System.Windows.Forms.TextBox baudrateBox;
        private System.Windows.Forms.TextBox byteToWriteBox;
        private System.Windows.Forms.Button sendBtn;
    }
}

