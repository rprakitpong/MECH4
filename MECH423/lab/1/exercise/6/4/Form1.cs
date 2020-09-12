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
        private StreamWriter outputFile = null;

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); 
            t.Interval = 100;
            t.Tick += updateDisplayedData; // set up display update
            t.Start();

            disconnectBtn.Enabled = false; // turn disconnect button off since there's nothing to disconnect now
            saveToFileCheck.Enabled = false; // turn off since no directory to save to yet

            //Console.WriteLine("finish instantiate");
        }

        private void CloseSerial(object sender, FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                //Console.WriteLine("close auto");
            }
            if (outputFile != null)
            {
                outputFile.Close();
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
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler);
                serialPort.DiscardInBuffer(); // remove stuff 

                disconnectBtn.Enabled = true; // now that there's something to disconnect, turn disconnect button on
            }

            //Console.WriteLine("set up complete");
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
            //Console.WriteLine("display");

            if (serialPort != null && serialPort.IsOpen)
            {
                byteCountBox.Text = serialPort.BytesToRead.ToString();
            }
            strLenBox.Text = dataQueue.Count.ToString();
            StringBuilder newData = new StringBuilder();
            int res = 0;
            while (dataQueue.TryDequeue(out res))
            {
                newData.Append(res.ToString());
                newData.Append(", ");

                if (res == 255)
                {
                    if (saveToFileCheck.Checked)
                    {
                        outputFile.WriteLine(axBox.Text + "," + ayBox.Text + "," + azBox.Text);
                    }
                    currData = 1;
                } else
                {
                    switch (currData)
                    {
                        case 1:
                            axBox.Text = res.ToString();
                            break;
                        case 2:
                            ayBox.Text = res.ToString();
                            break;
                        case 3:
                            azBox.Text = res.ToString();
                            break;
                    }
                    currData = currData + 1;
                }
            }
            displayBox.AppendText(newData.ToString());
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null;

            disconnectBtn.Enabled = false;
        }

        private void selectFilenameBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.ShowDialog();

            filenameBox.Text = sfd.FileName;

            if (outputFile != null)
            {
                outputFile.Close();
            }
            outputFile = new StreamWriter(sfd.FileName, true);
            outputFile.Flush();

            saveToFileCheck.Enabled = true; // enable it, given new directory
        }

        private void saveToFileCheck_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}
