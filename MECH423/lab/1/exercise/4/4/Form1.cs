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
        ConcurrentQueue<Int32> dataQueue = new ConcurrentQueue<Int32>();   
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close, in case user didn't click disconnect

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
                serialPort.Close(); // close port if not closed before app is closed
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string portName = comboBox1.SelectedItem.ToString();

            if (serialPort == null) // if the previous port is not closed and null, then don't make a new connection
            {
                serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                while (!serialPort.IsOpen)
                {
                    serialPort.Open();
                } // TA recommended something like this in case open doesn't actually open
                serialPort.ReadTimeout = 300;
                serialPort.DiscardInBuffer(); // remove data before reading is supposed to start
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler);

                disconnectBtn.Visible = true; // now that there's something to disconnect, turn disconnect button on
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
            }
            displayBox.AppendText(newData.ToString()); // append dequeued data to display box
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null; // close and null the serial port

            disconnectBtn.Visible = false; // turn off disconnect button since there's nothing to disconnect now
        }
    }
}
