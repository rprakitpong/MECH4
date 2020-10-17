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
            this.label3 = new System.Windows.Forms.Label();
            this.byteCountBox = new System.Windows.Forms.TextBox();
            this.queueCountBox = new System.Windows.Forms.TextBox();
            this.axBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ayBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.azBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.orientationBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.axMaxBox = new System.Windows.Forms.TextBox();
            this.azMaxBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ayMaxBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.stateBox = new System.Windows.Forms.TextBox();
            this.axChart = new LiveCharts.WinForms.CartesianChart();
            this.ayChart = new LiveCharts.WinForms.CartesianChart();
            this.azChart = new LiveCharts.WinForms.CartesianChart();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.avgG20Box = new System.Windows.Forms.TextBox();
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
            this.label1.Location = new System.Drawing.Point(49, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial Bytes To Read";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(320, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Items In Queue";
            // 
            // byteCountBox
            // 
            this.byteCountBox.Location = new System.Drawing.Point(197, 73);
            this.byteCountBox.Name = "byteCountBox";
            this.byteCountBox.Size = new System.Drawing.Size(100, 22);
            this.byteCountBox.TabIndex = 6;
            // 
            // queueCountBox
            // 
            this.queueCountBox.Location = new System.Drawing.Point(429, 71);
            this.queueCountBox.Name = "queueCountBox";
            this.queueCountBox.Size = new System.Drawing.Size(100, 22);
            this.queueCountBox.TabIndex = 8;
            // 
            // axBox
            // 
            this.axBox.Location = new System.Drawing.Point(78, 43);
            this.axBox.Name = "axBox";
            this.axBox.Size = new System.Drawing.Size(100, 22);
            this.axBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(49, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Ax";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(184, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Ay";
            // 
            // ayBox
            // 
            this.ayBox.Location = new System.Drawing.Point(214, 43);
            this.ayBox.Name = "ayBox";
            this.ayBox.Size = new System.Drawing.Size(100, 22);
            this.ayBox.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(320, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Az";
            // 
            // azBox
            // 
            this.azBox.Location = new System.Drawing.Point(350, 43);
            this.azBox.Name = "azBox";
            this.azBox.Size = new System.Drawing.Size(100, 22);
            this.azBox.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(49, 140);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Orientation";
            // 
            // orientationBox
            // 
            this.orientationBox.Location = new System.Drawing.Point(133, 137);
            this.orientationBox.Name = "orientationBox";
            this.orientationBox.Size = new System.Drawing.Size(234, 22);
            this.orientationBox.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "1)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "2)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "3)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(49, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 17);
            this.label11.TabIndex = 21;
            this.label11.Text = "Ax_max";
            // 
            // axMaxBox
            // 
            this.axMaxBox.Location = new System.Drawing.Point(109, 103);
            this.axMaxBox.Name = "axMaxBox";
            this.axMaxBox.Size = new System.Drawing.Size(100, 22);
            this.axMaxBox.TabIndex = 22;
            // 
            // azMaxBox
            // 
            this.azMaxBox.Location = new System.Drawing.Point(471, 103);
            this.azMaxBox.Name = "azMaxBox";
            this.azMaxBox.Size = new System.Drawing.Size(100, 22);
            this.azMaxBox.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(411, 106);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 17);
            this.label12.TabIndex = 23;
            this.label12.Text = "Az_max";
            // 
            // ayMaxBox
            // 
            this.ayMaxBox.Location = new System.Drawing.Point(289, 103);
            this.ayMaxBox.Name = "ayMaxBox";
            this.ayMaxBox.Size = new System.Drawing.Size(100, 22);
            this.ayMaxBox.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(229, 106);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 17);
            this.label13.TabIndex = 25;
            this.label13.Text = "Ay_max";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(19, 137);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(21, 17);
            this.label17.TabIndex = 27;
            this.label17.Text = "4)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "6,7,8)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(64, 196);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 17);
            this.label14.TabIndex = 29;
            this.label14.Text = "State";
            // 
            // stateBox
            // 
            this.stateBox.Location = new System.Drawing.Point(111, 193);
            this.stateBox.Name = "stateBox";
            this.stateBox.Size = new System.Drawing.Size(100, 22);
            this.stateBox.TabIndex = 30;
            // 
            // axChart
            // 
            this.axChart.Location = new System.Drawing.Point(13, 266);
            this.axChart.Name = "axChart";
            this.axChart.Size = new System.Drawing.Size(554, 100);
            this.axChart.TabIndex = 31;
            this.axChart.Text = "axChart";
            // 
            // ayChart
            // 
            this.ayChart.Location = new System.Drawing.Point(12, 381);
            this.ayChart.Name = "ayChart";
            this.ayChart.Size = new System.Drawing.Size(549, 100);
            this.ayChart.TabIndex = 32;
            this.ayChart.Text = "ayChart";
            // 
            // azChart
            // 
            this.azChart.Location = new System.Drawing.Point(13, 501);
            this.azChart.Name = "azChart";
            this.azChart.Size = new System.Drawing.Size(554, 100);
            this.azChart.TabIndex = 33;
            this.azChart.Text = "azChart";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(19, 167);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(21, 17);
            this.label15.TabIndex = 34;
            this.label15.Text = "5)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(49, 167);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 17);
            this.label16.TabIndex = 35;
            this.label16.Text = "avgMax_20";
            // 
            // avgMax20Box
            // 
            this.avgG20Box.Location = new System.Drawing.Point(133, 165);
            this.avgG20Box.Name = "avgMax20Box";
            this.avgG20Box.Size = new System.Drawing.Size(234, 22);
            this.avgG20Box.TabIndex = 36;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 613);
            this.Controls.Add(this.avgG20Box);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.azChart);
            this.Controls.Add(this.ayChart);
            this.Controls.Add(this.axChart);
            this.Controls.Add(this.stateBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.ayMaxBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.azMaxBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.axMaxBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.orientationBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.azBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ayBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.axBox);
            this.Controls.Add(this.queueCountBox);
            this.Controls.Add(this.byteCountBox);
            this.Controls.Add(this.label3);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox byteCountBox;
        private System.Windows.Forms.TextBox queueCountBox;
        private System.Windows.Forms.TextBox axBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ayBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox azBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox orientationBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox axMaxBox;
        private System.Windows.Forms.TextBox azMaxBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ayMaxBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox stateBox;
        private LiveCharts.WinForms.CartesianChart axChart;
        private LiveCharts.WinForms.CartesianChart ayChart;
        private LiveCharts.WinForms.CartesianChart azChart;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox avgG20Box;
    }
}

