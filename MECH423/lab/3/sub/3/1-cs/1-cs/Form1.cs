using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
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
        bool clicked = true;
        Queue<int> serialIn = new Queue<int>();

        public ChartValues<MeasureModel> ChartValues_pos { get; set; }
        public ChartValues<MeasureModel> ChartValues_vel { get; set; }

        private int numOfPointsOnChart = 100;

        public Form1()
        {
            InitializeComponent();

            // serial
            #region
            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box
            this.FormClosing += CloseSerial; // set up auto serial port close, in case user didn't click disconnect
            disconnectBtn.Enabled = false; // turn disconnect button off since there's nothing to disconnect now
            #endregion

            // dc motor pwm
            #region
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
            #endregion

            // livecharts
            #region

            //To handle live data easily, in this case we built a specialized type
            //the MeasureModel class, it only contains 2 properties
            //DateTime and Value
            //We need to configure LiveCharts to handle MeasureModel class
            //The next code configures MEasureModel  globally, this means
            //that livecharts learns to plot MeasureModel and will use this config every time
            //a ChartValues instance uses this type.
            //this code ideally should only run once, when application starts is reccomended.
            //you can configure series in many ways, learn more at http://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.index)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the ChartValues property will store our values array
            ChartValues_pos = new ChartValues<MeasureModel>();
            posChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues_pos,
                    PointGeometrySize = 5,
                    StrokeThickness = 4
                }
            };
            posChart.AxisY.Clear();
            posChart.AxisY.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 255
            });
            posChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
            });
            SetAxisLimits_posChart(0);

            //the ChartValues property will store our values array
            ChartValues_vel = new ChartValues<MeasureModel>();
            velChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues_vel,
                    PointGeometrySize = 5,
                    StrokeThickness = 4
                }
            };
            velChart.AxisY.Clear();
            velChart.AxisY.Add(new Axis
            {
                MinValue = -1,
                MaxValue = 1
            });
            velChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
            });
            for (int i = 0; i < numOfPointsOnChart; i++)
            {
                ChartValues_vel.Add(new MeasureModel
                {
                    index = i,
                    Value = 0
                });
            }
            SetAxisLimits_velChart(0);

            #endregion
        }

        // serial stuff
        #region
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

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null; // close and null the serial port

            disconnectBtn.Enabled = false; // turn off disconnect button since there's nothing to disconnect now

        }

        #endregion

        // dc motor pwm stuff
        #region

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
        #endregion

        // dc motor pos and speed and livecharts
        #region
        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead;
            bytesToRead = serialPort.BytesToRead;
            while (bytesToRead != 0) // keep reading until there's nothing left to read
            {
                serialIn.Enqueue(serialPort.ReadByte()); // enqueue byte that's read into queue
                bytesToRead = serialPort.BytesToRead; // reset bytesToRead
            }
            if (serialIn.Count >= 5)
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { getNewData(); });
                }
            }
        }

        DateTime prevTime = DateTime.Now;
        double prevPos = 0;
        int velcounter = 0;
        double vel = 0;
        private void getNewData()
        {
            while (serialIn.Peek() != 255)
            {
                serialIn.Dequeue();
            }

            int discardInit255 = serialIn.Dequeue();
            int direction = serialIn.Dequeue();
            int first = serialIn.Dequeue();
            int second = serialIn.Dequeue();
            int yy = serialIn.Dequeue();

            DateTime timenow = DateTime.Now;

            if (yy == 3)
            {
                first = 255;
                second = 255;
            } else if (yy == 2)
            {
                first = 255;
            } else if (yy == 1)
            {
                second = 255;
            }
            int pos = first * 255 + second;

            //posValBox.Text = discardInit255.ToString() + "," + direction.ToString() + "," + first.ToString() + "," + second.ToString() + "," + yy.ToString();

            if (velcounter > 15)
            {
                TimeSpan t = timenow.Subtract(prevTime);
                vel = (pos - prevPos) / t.TotalMilliseconds;
                if (t.TotalMilliseconds == 0)
                {
                    if (pos - prevPos < 0)
                    {
                        vel = -999;
                    }
                    else
                    {
                        vel = 999;
                    }
                }
                prevTime = timenow;
                prevPos = pos;

                velcounter = 0;
            }
            else
            {
                velcounter++;
            }


            setNewPos(pos % 255);
            setNewVel(vel);
        }

        private int velIndex = 0;
        private void setNewVel(double data)
        {
            velValBox.Text = data.ToString();
            ChartValues_vel.Add(new MeasureModel
            {
                index = posIndex,
                Value = data
            });
            SetAxisLimits_velChart(velIndex);
            velIndex++;

            if (ChartValues_vel.Count > numOfPointsOnChart) ChartValues_vel.RemoveAt(0);
        }

        private int posIndex = 0;
        private void setNewPos(double data)
        {
            posValBox.Text = data.ToString();
            ChartValues_pos.Add(new MeasureModel
            {
                index = posIndex,
                Value = data
            });
            SetAxisLimits_posChart(posIndex);
            posIndex++;

            if (ChartValues_pos.Count > numOfPointsOnChart) ChartValues_pos.RemoveAt(0);
        }

        private void SetAxisLimits_posChart(int index)
        {
            posChart.AxisX[0].MaxValue = (double)(index - numOfPointsOnChart); // lets force the axis to be 100ms ahead
            posChart.AxisX[0].MinValue = (double)index; //we only care about the last 8 seconds
        }
        private void SetAxisLimits_velChart(int index)
        {
            velChart.AxisX[0].MaxValue = (double)(index - numOfPointsOnChart); // lets force the axis to be 100ms ahead
            velChart.AxisX[0].MinValue = (double)index; //we only care about the last 8 seconds
        }
        #endregion
    }

    // livecharts
    public class MeasureModel
    {
        public double index { get; set; }
        public double Value { get; set; }
    }
}
