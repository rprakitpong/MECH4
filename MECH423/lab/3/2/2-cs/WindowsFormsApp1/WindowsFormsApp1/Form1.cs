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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort = null;
        bool clicked = false;

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close, in case user didn't click disconnect

            disconnectBtn.Enabled = false; // turn disconnect button off since there's nothing to disconnect now

            this.trackBar1.Scroll += (s, e) =>
            {
                if (clicked)
                    return;
            };

            this.trackBar1.MouseDown += (s,
                                    e) =>
            {
                clicked = true;
            };
            trackBar1.MouseUp += (s,
                                    e) =>
            {
                if (!clicked)
                    return;

                clicked = false;
                textBox1.Text = trackBar1.Value.ToString();
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

        int maxSpeed = 18;

        private void setSpeed(byte command, int speed)
        {
            byte[] send = new byte[5];
            send[0] = 0xFF;
            send[1] = command;
            int intValue = speed;
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

        private void button1_Click(object sender, EventArgs e)
        {
            // cw
            setSpeed(0x02, 0);
            setSpeed(0x04, 100); // a lot of steps, ccw 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ccw
            setSpeed(0x02, 0);
            setSpeed(0x05, 100); // a lot of steps, cw
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
            byte command;
            if (checkBox1.Checked)
            {
                command = 0x02; // ccw
            }
            else
            {
                command = 0x03; // cw
            }
            int intValue = int.Parse(textBox1.Text);
            int speed = 10000 - intValue + maxSpeed;
            setSpeed(command, speed);
        }
    }
}
