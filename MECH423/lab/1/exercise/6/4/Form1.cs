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
        private ConcurrentQueue<Int32> dataQueue = new ConcurrentQueue<Int32>(); // temp queue to store bytes between serial read and display
        private int currData = 0; // 1 = Ax, 2 = Ay, 3 = Az, 4 = 255
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
            if (serialPort != null && serialPort.IsOpen) // close open serial before app is closed
            {
                serialPort.Close();
            }
            if (outputFile != null) // close open file before app is closed
            {
                outputFile.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string portName = comboBox1.SelectedItem.ToString(); // set up serial port once user selected which port to use

            if (serialPort == null) // if no port is prevoiusly set up
            {
                serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                while (!serialPort.IsOpen)
                {
                    serialPort.Open();
                } // put in loop so open can be retried if somehow not successful
                serialPort.ReadTimeout = 300;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler);
                serialPort.DiscardInBuffer(); // remove stuff that was there before this read session

                disconnectBtn.Enabled = true; // now that there's something to disconnect, turn disconnect button on
            }
        }

        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead;
            bytesToRead = serialPort.BytesToRead;
            while (bytesToRead != 0) // keep reading until there's nothing left to read
            {
                dataQueue.Enqueue(serialPort.ReadByte()); // enqueue byte that's read into queue
                bytesToRead = serialPort.BytesToRead; // reset bytesToRead
            }
        }

        public void updateDisplayedData(Object myObject, EventArgs myEventArgs)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                byteCountBox.Text = serialPort.BytesToRead.ToString(); // update count of bytes to read
            }
            queueCountBox.Text = dataQueue.Count.ToString(); // update count of temp queue
            strLenBox.Text = "0"; // implemented using queue, not temp string, so length is always zero

            StringBuilder newData = new StringBuilder(); // temp stringbuilder to do string concatenation
            int res = 0;
            while (dataQueue.TryDequeue(out res))
            {
                newData.Append(res.ToString()); // add data to display
                newData.Append(", "); 

                if (res == 255)
                {
                    if (saveToFileCheck.Checked) // only save data to file if checkbox is checked
                    {
                        outputFile.WriteLine(axBox.Text + "," + ayBox.Text + "," + azBox.Text + "," + DateTime.Now.ToString("HH:mm:ss:ffff")); // get A values from textboxes, no need to initialize separate variables for them
                    }
                    currData = 1; // reset currData
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
                    currData = currData + 1; // move currData up
                }
            }
            displayBox.AppendText(newData.ToString()); // append newData to diplayed text
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null; // reset serialPort to null

            disconnectBtn.Enabled = false; // turn off disconnect button now that there's no serial port to disconnect
        }

        private void selectFilenameBtn_Click(object sender, EventArgs e)
        {
            // open dialog 
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.ShowDialog();

            filenameBox.Text = sfd.FileName; // get file directory from dialog and display it

            if (outputFile != null) // close previous file if it's not closed yet, before opening a new one
            {
                outputFile.Close();
            }
            outputFile = new StreamWriter(sfd.FileName, true); // open new file
            outputFile.Flush();

            saveToFileCheck.Enabled = true; // enable it, given that there's a file now available for writing
        }

        private void saveToFileCheck_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}
