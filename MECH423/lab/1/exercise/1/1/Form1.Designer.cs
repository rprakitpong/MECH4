namespace _1
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
            this.xCoord = new System.Windows.Forms.TextBox();
            this.yCoord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.clickCoord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mouseBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mouseBox)).BeginInit();
            this.SuspendLayout();
            // 
            // xCoord
            // 
            this.xCoord.Location = new System.Drawing.Point(43, 10);
            this.xCoord.Name = "xCoord";
            this.xCoord.Size = new System.Drawing.Size(178, 22);
            this.xCoord.TabIndex = 0;
            // 
            // yCoord
            // 
            this.yCoord.Location = new System.Drawing.Point(42, 37);
            this.yCoord.Name = "yCoord";
            this.yCoord.Size = new System.Drawing.Size(178, 22);
            this.yCoord.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y:";
            // 
            // clickCoord
            // 
            this.clickCoord.Location = new System.Drawing.Point(4, 100);
            this.clickCoord.Multiline = true;
            this.clickCoord.Name = "clickCoord";
            this.clickCoord.Size = new System.Drawing.Size(216, 338);
            this.clickCoord.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Recorded Clicks:";
            // 
            // mouseBox
            // 
            this.mouseBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mouseBox.Location = new System.Drawing.Point(229, 7);
            this.mouseBox.Name = "mouseBox";
            this.mouseBox.Size = new System.Drawing.Size(566, 430);
            this.mouseBox.TabIndex = 6;
            this.mouseBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mouseBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clickCoord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yCoord);
            this.Controls.Add(this.xCoord);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.mouseBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox xCoord;
        private System.Windows.Forms.TextBox yCoord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox clickCoord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox mouseBox;
    }
}

