namespace _2
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
            this.euqueueBtn = new System.Windows.Forms.Button();
            this.dequeueBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.enqueueBox = new System.Windows.Forms.TextBox();
            this.dequeueBox = new System.Windows.Forms.TextBox();
            this.numItemsBox = new System.Windows.Forms.TextBox();
            this.dequeueAndAverageBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nBox_dequeAndAvg = new System.Windows.Forms.TextBox();
            this.avgBox_dequeAndAvg = new System.Windows.Forms.TextBox();
            this.queueItemsBox = new System.Windows.Forms.TextBox();
            this.labelaa = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // euqueueBtn
            // 
            this.euqueueBtn.Location = new System.Drawing.Point(7, 9);
            this.euqueueBtn.Name = "euqueueBtn";
            this.euqueueBtn.Size = new System.Drawing.Size(213, 34);
            this.euqueueBtn.TabIndex = 0;
            this.euqueueBtn.Text = "Enqueue";
            this.euqueueBtn.UseVisualStyleBackColor = true;
            this.euqueueBtn.Click += new System.EventHandler(this.enqueueBtn_Click);
            // 
            // dequeueBtn
            // 
            this.dequeueBtn.Location = new System.Drawing.Point(7, 55);
            this.dequeueBtn.Name = "dequeueBtn";
            this.dequeueBtn.Size = new System.Drawing.Size(212, 39);
            this.dequeueBtn.TabIndex = 1;
            this.dequeueBtn.Text = "Dequeue";
            this.dequeueBtn.UseVisualStyleBackColor = true;
            this.dequeueBtn.Click += new System.EventHandler(this.dequeueBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of queue items";
            // 
            // enqueueBox
            // 
            this.enqueueBox.Location = new System.Drawing.Point(228, 10);
            this.enqueueBox.Name = "enqueueBox";
            this.enqueueBox.Size = new System.Drawing.Size(558, 22);
            this.enqueueBox.TabIndex = 3;
            // 
            // dequeueBox
            // 
            this.dequeueBox.Location = new System.Drawing.Point(227, 55);
            this.dequeueBox.Name = "dequeueBox";
            this.dequeueBox.Size = new System.Drawing.Size(558, 22);
            this.dequeueBox.TabIndex = 4;
            // 
            // numItemsBox
            // 
            this.numItemsBox.Location = new System.Drawing.Point(226, 107);
            this.numItemsBox.Name = "numItemsBox";
            this.numItemsBox.Size = new System.Drawing.Size(559, 22);
            this.numItemsBox.TabIndex = 5;
            // 
            // dequeueAndAverageBtn
            // 
            this.dequeueAndAverageBtn.Location = new System.Drawing.Point(10, 153);
            this.dequeueAndAverageBtn.Name = "dequeueAndAverageBtn";
            this.dequeueAndAverageBtn.Size = new System.Drawing.Size(775, 32);
            this.dequeueAndAverageBtn.TabIndex = 6;
            this.dequeueAndAverageBtn.Text = "Dequeue and Average First N Data Points";
            this.dequeueAndAverageBtn.UseVisualStyleBackColor = true;
            this.dequeueAndAverageBtn.Click += new System.EventHandler(this.dequeueAndAverageBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "N";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(405, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Average";
            // 
            // nBox_dequeAndAvg
            // 
            this.nBox_dequeAndAvg.Location = new System.Drawing.Point(34, 204);
            this.nBox_dequeAndAvg.Name = "nBox_dequeAndAvg";
            this.nBox_dequeAndAvg.Size = new System.Drawing.Size(346, 22);
            this.nBox_dequeAndAvg.TabIndex = 9;
            // 
            // avgBox_dequeAndAvg
            // 
            this.avgBox_dequeAndAvg.Location = new System.Drawing.Point(482, 204);
            this.avgBox_dequeAndAvg.Name = "avgBox_dequeAndAvg";
            this.avgBox_dequeAndAvg.Size = new System.Drawing.Size(286, 22);
            this.avgBox_dequeAndAvg.TabIndex = 10;
            // 
            // queueItemsBox
            // 
            this.queueItemsBox.Location = new System.Drawing.Point(13, 253);
            this.queueItemsBox.Multiline = true;
            this.queueItemsBox.Name = "queueItemsBox";
            this.queueItemsBox.Size = new System.Drawing.Size(772, 185);
            this.queueItemsBox.TabIndex = 11;
            // 
            // labelaa
            // 
            this.labelaa.AutoSize = true;
            this.labelaa.Location = new System.Drawing.Point(12, 233);
            this.labelaa.Name = "labelaa";
            this.labelaa.Size = new System.Drawing.Size(115, 17);
            this.labelaa.TabIndex = 12;
            this.labelaa.Text = "Queue Contents:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelaa);
            this.Controls.Add(this.queueItemsBox);
            this.Controls.Add(this.avgBox_dequeAndAvg);
            this.Controls.Add(this.nBox_dequeAndAvg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dequeueAndAverageBtn);
            this.Controls.Add(this.numItemsBox);
            this.Controls.Add(this.dequeueBox);
            this.Controls.Add(this.enqueueBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dequeueBtn);
            this.Controls.Add(this.euqueueBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button euqueueBtn;
        private System.Windows.Forms.Button dequeueBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox enqueueBox;
        private System.Windows.Forms.TextBox dequeueBox;
        private System.Windows.Forms.TextBox numItemsBox;
        private System.Windows.Forms.Button dequeueAndAverageBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nBox_dequeAndAvg;
        private System.Windows.Forms.TextBox avgBox_dequeAndAvg;
        private System.Windows.Forms.TextBox queueItemsBox;
        private System.Windows.Forms.Label labelaa;
    }
}

