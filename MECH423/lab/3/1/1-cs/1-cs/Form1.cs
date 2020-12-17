using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1_cs
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort = null;

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close, in case user didn't click disconnect

            disconnectBtn.Enabled = false; // turn disconnect button off since there's nothing to disconnect now
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

                disconnectBtn.Enabled = true; // now that there's something to disconnect, turn disconnect button on
            }
        }

        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // do nothing
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null; // close and null the serial port

            disconnectBtn.Enabled = false; // turn off disconnect button since there's nothing to disconnect now

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            byte[] send = new byte[5];
            send[0] = 0xFF;
            if (checkBox1.Checked)
            {
                send[1] = 0x00; // cw
            }
            else
            {
                send[1] = 0x01; // ccw
            }
            int intValue = int.Parse(textBox1.Text);
            send[2] = (byte)(intValue >> 8);
            send[3] = (byte)intValue;
            send[4] = 0x00;
            if (send[2] == 0xFF)
            {
                send[4] = 0x02;
            }
            if (send[3] == 0xFF)
            {
                send[4] = 0x01;
                if (send[2] == 0xFF)
                {
                    send[4] = 0x03;
                }
            }
            Console.WriteLine(send.ToString());
            serialPort.Write(send, 0, 5);
        }
    }
}
