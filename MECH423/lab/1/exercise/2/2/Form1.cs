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

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += UpdateQueue; // set up display update
            t.Start();
        }

        private void enqueueBtn_Click(object sender, EventArgs e)
        {
            Int32 num;
            if (Int32.TryParse(enqueueBox.Text, out num))
            {
                dataQueue.Enqueue(num); // enqueue integer to queue
            } else
            {
                MessageBox.Show("Please enqueue int32."); // don't enqueue if parsing fails
            }
        }

        private void dequeueBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 num = dataQueue.Dequeue(); // dequeue
                dequeueBox.Text = num.ToString(); // display dequeued number
            } catch (InvalidOperationException ex)
            {
                MessageBox.Show("Queue is empty."); // queue is empty
            }
        }

        private void dequeueAndAverageBtn_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(nBox_dequeAndAvg.Text, out n))
            {
                if (n > dataQueue.Count)
                {
                    MessageBox.Show("Please select N smaller than queue count."); // N is too large for queue
                } else
                {
                    double ret = 0;
                    for (int i = 0; i < n; i++)
                    {
                        ret += Convert.ToDouble(dataQueue.Dequeue()); // convert to double for more precision in averaging
                    }
                    avgBox_dequeAndAvg.Text = (ret / n).ToString(); // display average
                }
            }
            else
            {
                MessageBox.Show("Please use int for N."); // invalid N is entered
            }
        }

        private void UpdateQueue(Object myObject, EventArgs myEventArgs)
        {
            Queue<Int32> toDisplay = new Queue<Int32>(dataQueue); // copy queue so they can be dequeued and displayed
            numItemsBox.Text = toDisplay.Count.ToString(); // update size display
            try
            {
                StringBuilder display = new StringBuilder(toDisplay.Dequeue().ToString()); // use stringbuilder to append items
                while (toDisplay.Count > 0)
                {
                    display.Append("," + toDisplay.Dequeue().ToString());
                }
                queueItemsBox.Text = display.ToString(); // update display with items stringbuilder
            } catch (InvalidOperationException ex)
            {
                queueItemsBox.Text = ""; // any invalid queue (ex: empty queue) displays empty string
            }
        }
    }
}
