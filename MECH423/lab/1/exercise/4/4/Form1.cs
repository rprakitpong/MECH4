using System;
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
        string serialDataString = "";
        

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames());

            this.FormClosing += CloseSerial;

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += updateDisplayedData;
            t.Start();

            Console.WriteLine("finish instantiate");
        }

        private void CloseSerial(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("close auto");

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
                serialPort.Open();
                serialPort.Write("A");
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler);
            }
            
            Console.WriteLine("set up complete");
        }

        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("write");

            int newByte = 0;
            int bytesToRead;
            bytesToRead = serialPort.BytesToRead;
            while (bytesToRead != 0)
            {
                newByte = serialPort.ReadByte();
                serialDataString = serialDataString + newByte.ToString() + ", ";
                bytesToRead = serialPort.BytesToRead;
            }
        }

        public void updateDisplayedData(Object myObject, EventArgs myEventArgs)
        {
            Console.WriteLine("display");

            if (serialPort != null && serialPort.IsOpen)
                byteCountBox.Text = serialPort.BytesToRead.ToString();
            strLenBox.Text = serialDataString.Length.ToString();
            displayBox.AppendText(serialDataString);
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
        }


    }
}
