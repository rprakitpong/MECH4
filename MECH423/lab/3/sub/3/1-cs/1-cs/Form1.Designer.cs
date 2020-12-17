namespace _1_cs
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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.disconnectBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.posChart = new LiveCharts.WinForms.CartesianChart();
            this.velChart = new LiveCharts.WinForms.CartesianChart();
            this.posValBox = new System.Windows.Forms.TextBox();
            this.velValBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 113);
            this.trackBar1.Maximum = 65536;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(776, 56);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.Value = 32768;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "PWM (0 to 65536)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 54);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(233, 21);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Check for CW, uncheck for CCW";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(18, 15);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.Location = new System.Drawing.Point(156, 17);
            this.disconnectBtn.Name = "disconnectBtn";
            this.disconnectBtn.Size = new System.Drawing.Size(92, 23);
            this.disconnectBtn.TabIndex = 4;
            this.disconnectBtn.Text = "Disconnect";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            this.disconnectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(26, 166);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // posChart
            // 
            this.posChart.Location = new System.Drawing.Point(18, 214);
            this.posChart.Name = "posChart";
            this.posChart.Size = new System.Drawing.Size(359, 205);
            this.posChart.TabIndex = 6;
            this.posChart.Text = "posChart";
            // 
            // velChart
            // 
            this.velChart.Location = new System.Drawing.Point(414, 214);
            this.velChart.Name = "velChart";
            this.velChart.Size = new System.Drawing.Size(374, 205);
            this.velChart.TabIndex = 7;
            this.velChart.Text = "cartesianChart2";
            // 
            // posValBox
            // 
            this.posValBox.Location = new System.Drawing.Point(26, 416);
            this.posValBox.Name = "posValBox";
            this.posValBox.Size = new System.Drawing.Size(100, 22);
            this.posValBox.TabIndex = 8;
            // 
            // velValBox
            // 
            this.velValBox.Location = new System.Drawing.Point(414, 416);
            this.velValBox.Name = "velValBox";
            this.velValBox.Size = new System.Drawing.Size(100, 22);
            this.velValBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.velValBox);
            this.Controls.Add(this.posValBox);
            this.Controls.Add(this.velChart);
            this.Controls.Add(this.posChart);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.disconnectBtn);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button disconnectBtn;
        private System.Windows.Forms.TextBox textBox1;
        private LiveCharts.WinForms.CartesianChart posChart;
        private LiveCharts.WinForms.CartesianChart velChart;
        private System.Windows.Forms.TextBox posValBox;
        private System.Windows.Forms.TextBox velValBox;
    }
}

