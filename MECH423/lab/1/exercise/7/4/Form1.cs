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
        private int currData = 0; // 1 = Ax, 2 = Ay, 3 = Az
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
                serialPort.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string portName = comboBox1.SelectedItem.ToString();

            if (serialPort == null)
            {
                serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                while (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
                serialPort.ReadTimeout = 300;

                disconnectBtn.Enabled = true; // now that there's something to disconnect, turn disconnect button on
                processNewDataPtBtn.Enabled = true;

                axBox.Tag = -1;
                ayBox.Tag = -1;
                azBox.Tag = -1;
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
                        firstData = false;
                    }
                    else
                    {
                        string state = GetDataState((Int32)axBox.Tag, (Int32)ayBox.Tag, (Int32)azBox.Tag);
                        string toWrite = "(" + axBox.Text + "," + ayBox.Text + "," + azBox.Text + "," + state + "), ";
                        dataHistoryBox.AppendText(toWrite);
                        stateBox.Text = state;
                    }
                    currData = 1;
                } else
                {
                    switch (currData)
                    {
                        case 1:
                            axBox.Text = res.ToString();
                            axBox.Tag = res; // save int as tag so it wouldn't have to parsed into int again for using GetDataState
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
        }

        private string GetDataState(int ax, int ay, int az)
        {
            return "0"; // to do in exercise 8, right now it's an placeholder implementation
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null;

            disconnectBtn.Enabled = false;
            processNewDataPtBtn.Enabled = false;
            firstData = true;
        }
                
        private void processNewDataPtBtn_Click(object sender, EventArgs e)
        {
            serialPort.DiscardInBuffer(); // remove stuff 
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler);
        }
    }
}
