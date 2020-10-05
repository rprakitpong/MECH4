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

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 50;
            t.Tick += updateDisplayedData; // set up display update
            t.Tick += serialDataHandler;
            t.Start();

            disconnectBtn.Enabled = false; // turn off since there's nothing to disconnect now
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
            int baud = int.Parse(baudrateBox.Text);

            if (serialPort == null) // set up new serial connection
            {
                serialPort = new SerialPort(portName, baud, Parity.None, 8, StopBits.One);
                while (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
                serialPort.ReadTimeout = 300;
                serialPort.DiscardInBuffer(); // discard what's already read


                disconnectBtn.Enabled = true; // now that there's something to disconnect, turn disconnect button on
                Console.WriteLine("yes");
            }
        }

        private void serialDataHandler(object sender, EventArgs e)
        {

            if (serialPort != null)
            {
                int newByte = 0;
                int bytesToRead;
                bytesToRead = serialPort.BytesToRead;
                while (bytesToRead != 0)
                {
                    newByte = serialPort.ReadByte();
                    dataQueue.Enqueue(newByte);
                    bytesToRead = serialPort.BytesToRead;
                }
            }
        }

        public void updateDisplayedData(Object myObject, EventArgs myEventArgs)
        {
            //Console.WriteLine("eyee");
            int res = 0;
            while (dataQueue.TryDequeue(out res))
            {
                dataHistoryBox.Text += "," + res.ToString();
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
        }
                
        private void processNewDataPtBtn_Click(object sender, EventArgs e)
        {
            serialPort.DiscardInBuffer(); // remove stuff 
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler); // start handling data
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (serialPort != null)
            {
                serialPort.Write(byteToWriteBox.Text);
            }
        }
    }
}
