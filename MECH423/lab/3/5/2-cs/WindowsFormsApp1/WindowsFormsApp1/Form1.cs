﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort = null;
        bool clicked1 = false;
        bool clicked2 = false;

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close, in case user didn't click disconnect

            disconnectBtn.Enabled = false; // turn disconnect button off since there's nothing to disconnect now

            this.trackBar1.Scroll += (s, e) =>
            {
                if (clicked1)
                    return;
            };

            this.trackBar1.MouseDown += (s,
                                    e) =>
            {
                clicked1 = true;
            };
            trackBar1.MouseUp += (s,
                                    e) =>
            {
                if (!clicked1)
                    return;

                clicked1 = false;
                stepperPosBox.Text = trackBar1.Value.ToString();
            };

            this.trackBar2.Scroll += (s, e) =>
            {
                if (clicked2)
                    return;
            };

            this.trackBar2.MouseDown += (s,
                                    e) =>
            {
                clicked2 = true;
            };
            trackBar2.MouseUp += (s,
                                    e) =>
            {
                if (!clicked2)
                    return;

                clicked2 = false;
                dcPosBox.Text = trackBar2.Value.ToString();
            };
        }


        private void CloseSerial(object sender, FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close(); // close port if not closed before app is closed
            }
        }

        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // do nothing
        }

        private void sendData(byte x, byte y, byte speed)
        {
            byte[] send = new byte[5];
            send[0] = 0xFF;
            send[1] = x;
            send[2] = y;
            send[3] = speed;
            send[4] = 0;
            if (serialPort != null)
            {
                serialPort.Write(send, 0, 5);
            }
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null; // close and null the serial port

            disconnectBtn.Enabled = false; // turn off disconnect button since there's nothing to disconnect now

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
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

                disconnectBtn.Enabled = true; // now that there's something to disconnect, turn disconnect button on
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            // do nothing lol
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int stepperDisplace = int.Parse(stepperPosBox.Text) + 19; // scale is -19 to 19
            int dcDisplace = int.Parse(dcPosBox.Text) + 19; // scale is -19 to 19
            int speed = int.Parse(speedBox.Text);
            sendData((byte)stepperDisplace, (byte)dcDisplace, (byte)speed);
        }
    }
}
