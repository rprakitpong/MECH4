using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2
{
    public partial class Form1 : Form
    {
        Queue<Int32> dataQueue = new Queue<Int32>();

        public Form1()
        {
            InitializeComponent();
        }

        private void enqueueBtn_Click(object sender, EventArgs e)
        {
            Int32 num;
            if (Int32.TryParse(enqueueBox.Text, out num))
            {
                dataQueue.Enqueue(num);
                UpdateQueue(); // should try event rerouting
            } else
            {
                MessageBox.Show("Please enqueue int32.");
            }
        }

        private void dequeueBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 num = dataQueue.Dequeue();
                dequeueBox.Text = num.ToString();
                UpdateQueue();
            } catch (InvalidOperationException ex)
            {
                MessageBox.Show("Queue is empty.");
                UpdateQueue();
            }
        }

        private void dequeueAndAverageBtn_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(nBox_dequeAndAvg.Text, out n))
            {
                if (n > dataQueue.Count)
                {
                    MessageBox.Show("Please select N smaller than queue count.");
                } else
                {
                    double ret = 0;
                    for (int i = 0; i < n; i++)
                    {
                        ret += Convert.ToDouble(dataQueue.Dequeue());
                    }
                    avgBox_dequeAndAvg.Text = (ret / n).ToString();
                    UpdateQueue();
                }
            }
            else
            {
                MessageBox.Show("Please use int for N.");
            }
        }

        private void UpdateQueue()
        {
            Queue<Int32> toDisplay = new Queue<Int32>(dataQueue);
            numItemsBox.Text = toDisplay.Count.ToString();
            try
            {
                StringBuilder display = new StringBuilder(toDisplay.Dequeue().ToString());
                while (toDisplay.Count > 0)
                {
                    display.Append("," + toDisplay.Dequeue().ToString());
                }
                queueItemsBox.Text = display.ToString();
            } catch (InvalidOperationException ex)
            {
                queueItemsBox.Text = "";
            }
        }
    }
}
