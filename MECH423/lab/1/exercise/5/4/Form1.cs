using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort = null;
        private ConcurrentQueue<Int32> dataQueue = new ConcurrentQueue<Int32>();
        private int currData = 0; // 1 = Ax, 2 = Ay, 3 = Az
        private StringBuilder outputFile = new StringBuilder(); // slow to open and close

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close, in case user didn't disconnect before exit

            // set up timer
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); 
            t.Interval = 100;
            t.Tick += updateDisplayedData; // set up display update
            t.Start();

            disconnectBtn.Visible = false; // turn disconnect button off since there's nothing to disconnect now
        }

        private void CloseSerial(object sender, FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string portName = comboBox1.SelectedItem.ToString();

            if (serialPort == null)
            {
                serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One); // instantiate serial port
                while (!serialPort.IsOpen)
                {
                    serialPort.Open();
                } // open in a loop in case open function failss
                serialPort.ReadTimeout = 300;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler); // handler for new data
                serialPort.DiscardInBuffer(); // remove data before session is supposed to start

                disconnectBtn.Visible = true; // now that there's something to disconnect, turn disconnect button on

                axBox.Tag = 130; // set to default value
                ayBox.Tag = 130;
                azBox.Tag = 130;
            }
        }

        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = serialPort.BytesToRead; // check if there're bytes to read
            while (bytesToRead != 0) // read till there's nothing left to read
            {
                dataQueue.Enqueue(serialPort.ReadByte()); // read and enqueue   
                bytesToRead = serialPort.BytesToRead; // update bytes to read
            }
        }

        public void updateDisplayedData(Object myObject, EventArgs myEventArgs)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                byteCountBox.Text = serialPort.BytesToRead.ToString(); // update bytes left to read count
            }
            strLenBox.Text = "0"; // implementation uses queue, not string, so no temp string
            queueCountBox.Text = dataQueue.Count.ToString(); // update temp queue count

            StringBuilder newData = new StringBuilder(); // use stringbuilder to make string array
            int res = 0;
            while (dataQueue.TryDequeue(out res)) // dequeue while data queue is not empty
            {
                newData.Append(res.ToString());
                newData.Append(", ");

                if (res == 255)
                {
                    currData = 1;

                    string orientation = GetOrientation((Int32)axBox.Tag, (Int32)ayBox.Tag, (Int32)azBox.Tag);
                    orientationBox.Text = orientation;

                } else
                {
                    switch (currData)
                    {
                        case 1:
                            axBox.Text = res.ToString();
                            axBox.Tag = res; // save int as tag to excapsulate work of keeping track of ax into axBox object, no separate variable for this
                            break;
                        case 2:
                            ayBox.Text = res.ToString();
                            ayBox.Tag = res;
                            break;
                        case 3:
                            azBox.Text = res.ToString();
                            azBox.Tag = res;
                            break;
                    }
                    currData = currData + 1;
                }
            }
            displayBox.AppendText(newData.ToString()); // append dequeued data to display box
        }

        private string GetOrientation(int ax, int ay, int az)
        {
            int defaultVal = 130; // neutral value for each A 
            int diffX = ax - defaultVal; // get how much offset that A is from neutral
            int diffY = ay - defaultVal;
            int diffZ = az - defaultVal;

            // displays orientation that is has the most offset from neutral
            if (Math.Abs(diffX) > Math.Abs(diffY) && Math.Abs(diffX) > Math.Abs(diffZ)) // if true, then Ax is the most offset since it's bigger than both Ay and Az
            {
                if (diffX > 0)
                {
                    return "left";
                } else
                {
                    return "right";
                }
            } else if (Math.Abs(diffY) > Math.Abs(diffZ))  // if true, then Ay is most offset, since Ax isn't biggest and Ay is bigger than Az
            {
                if (diffY > 0)
                {
                    return "front";
                } else
                {
                    return "back";
                }
            } else // if this is true, then Az is most offset by default
            {
                if (diffZ > 0)
                {
                    return "up";
                } else
                {
                    return "down";
                }
            }
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null;

            disconnectBtn.Visible = false; // turn off button since there's nothing to disconnect
        }
    }
}
