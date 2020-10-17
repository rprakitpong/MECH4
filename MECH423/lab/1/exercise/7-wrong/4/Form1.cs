using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private int currData = 0; // 1 = Ax, 2 = Ay, 3 = Az, 4 = 255
        private bool firstData = true;

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 50;
            t.Tick += updateDisplayedData; // set up display update
            t.Start();

            disconnectBtn.Enabled = false; // turn off since there's nothing to disconnect now
            processNewDataPtBtn.Enabled = false; // turn off since there's no data source
        }

        private void CloseSerial(object sender, FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close(); // close serial port if not closed already
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string portName = comboBox1.SelectedItem.ToString();

            if (serialPort == null) // set up new serial connection
            {
                serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                while (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
                serialPort.ReadTimeout = 300;

                disconnectBtn.Enabled = true; // now that there's something to disconnect, turn disconnect button on
                processNewDataPtBtn.Enabled = true; // now that there's new data, turn process button on

                // save state and A values in tags, to encapsulate work of saving data into a single object
                stateBox.Tag = 0; // set state to default value
                axBox.Tag = 130; // set to default value
                ayBox.Tag = 130;
                azBox.Tag = 130;
            }
        }

        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //Console.WriteLine("read");

            int newByte = 0;
            int bytesToRead;
            bytesToRead = serialPort.BytesToRead;
            while (bytesToRead != 0)
            {
                newByte = serialPort.ReadByte();
                dataQueue.Enqueue(newByte);       
                bytesToRead = serialPort.BytesToRead;
            }
            serialPort.DiscardInBuffer(); // discard what's already read
        }

        public void updateDisplayedData(Object myObject, EventArgs myEventArgs)
        {
            int res = 0;
            while (dataQueue.TryDequeue(out res))
            {
                if (res == 255)
                {
                    if (firstData)
                    {
                        firstData = false; // neglect all data before first data 
                    }
                    else
                    {
                        int state = GetDataState((Int32)axBox.Tag, (Int32)ayBox.Tag, (Int32)azBox.Tag, (Int32)stateBox.Tag); // get current state 
                        string toWrite = "(" + axBox.Text + "," + ayBox.Text + "," + azBox.Text + "," + state.ToString() + "), "; 
                        dataHistoryBox.AppendText(toWrite); // append data to display
                        stateBox.Text = state.ToString(); // display current state
                        stateBox.Tag = state; // save current state
                    }
                    currData = 1;
                } else
                {
                    switch (currData)
                    {
                        case 1:
                            axBox.Text = res.ToString();
                            axBox.Tag = res; 
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
                    currData = currData + 1; // move to next data
                }
            }
        }

        private int GetDataState(int ax, int ay, int az, int curState)
        {
            // real implementation should return new state based on new data and current state
            return 0; // to do in exercise 8, right now it's an placeholder implementation
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null; // reset serial port variable

            disconnectBtn.Enabled = false; // turn buttons off
            processNewDataPtBtn.Enabled = false;
            firstData = true; // reset variable
        }
                
        private void processNewDataPtBtn_Click(object sender, EventArgs e)
        {
            serialPort.DiscardInBuffer(); // remove stuff 
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler); // start handling data
        }
    }
}
