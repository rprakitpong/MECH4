﻿using System;
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

            this.FormClosing += CloseSerial; // set up auto serial port close

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); 
            t.Interval = 100;
            t.Tick += updateDisplayedData; // set up display update
            t.Start();

            disconnectBtn.Visible = false; // turn disconnect button off since there's nothing to disconnect now

            //Console.WriteLine("finish instantiate");
        }

        private void CloseSerial(object sender, FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                //Console.WriteLine("close auto");
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

                disconnectBtn.Visible = true; // now that there's something to disconnect, turn disconnect button on
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
                newData.Append(",");
            }
            displayBox.AppendText(newData.ToString());
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null;

            disconnectBtn.Visible = false;
        }
    }
}
