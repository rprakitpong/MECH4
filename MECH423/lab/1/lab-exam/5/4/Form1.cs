using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
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
        private ConcurrentQueue<Int32> dataQueue = new ConcurrentQueue<Int32>();
        private int currData = 0; // 1 = Ax, 2 = Ay, 3 = Az

        private Queue<int> ax_100points = new Queue<int>();
        private Queue<int> ay_100points = new Queue<int>();
        private Queue<int> az_100points = new Queue<int>();

        public ChartValues<MeasureModel> axChartValues { get; set; }
        public ChartValues<MeasureModel> ayChartValues { get; set; }
        public ChartValues<MeasureModel> azChartValues { get; set; }

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames()); //set up combo box

            this.FormClosing += CloseSerial; // set up auto serial port close, in case user didn't disconnect before exit

            // set up timer
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); 
            t.Interval = 100;
            t.Tick += updateDisplayedData; // set up display update
            t.Start();

            disconnectBtn.Visible = false; // turn disconnect button off since there's nothing to disconnect now

            stateBox.Text = state0;


            SetUpCharts();

        }

        private void SetUpCharts()
        {
            // chart stuff
            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the ChartValues property will store our values array
            axChartValues = new ChartValues<MeasureModel>();
            axChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = axChartValues,
                    PointGeometrySize = 18,
                    StrokeThickness = 4
                }
            };
            axChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new System.DateTime((long)value).ToString("mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            SetAxisLimits("x");

            //-------------------------

            //the ChartValues property will store our values array
            ayChartValues = new ChartValues<MeasureModel>();
            ayChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ayChartValues,
                    PointGeometrySize = 18,
                    StrokeThickness = 4
                }
            };
            ayChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new System.DateTime((long)value).ToString("mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            SetAxisLimits("y");

            //---------------------------

            //the ChartValues property will store our values array
            azChartValues = new ChartValues<MeasureModel>();
            azChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = azChartValues,
                    PointGeometrySize = 18,
                    StrokeThickness = 4
                }
            };
            azChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new System.DateTime((long)value).ToString("mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            SetAxisLimits("z");
        }

        //chart stuff
        private void SetAxisLimits(string s)
        {
            System.DateTime now = System.DateTime.Now;

            LiveCharts.WinForms.CartesianChart c = axChart;
            if (s == "y")
            {
                c = ayChart;
            } else if (s == "z")
            {
                c = azChart;
            }

            c.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(0).Ticks; // lets force the axis to be 100ms ahead
            c.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(2).Ticks; //we only care about the last 8 seconds
        }

        private void AddChartValue(ChartValues<MeasureModel> cv, double val, string axis)
        {
            var now = System.DateTime.Now;

            cv.Add(new MeasureModel
            {
                DateTime = now,
                Value = val
            });

            SetAxisLimits(axis);

            //lets only use the last 30 values
            if (cv.Count > 30) cv.RemoveAt(0);
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
                serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One); // instantiate serial port
                while (!serialPort.IsOpen)
                {
                    serialPort.Open();
                } // open in a loop in case open function failss
                serialPort.ReadTimeout = 300;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataHandler); // handler for new data
                serialPort.DiscardInBuffer(); // remove data before session is supposed to start

                disconnectBtn.Visible = true; // now that there's something to disconnect, turn disconnect button on

                axBox.Tag = 130; // set to default value
                ayBox.Tag = 130;
                azBox.Tag = 130;
            }
        }

        private void serialDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = serialPort.BytesToRead; // check if there're bytes to read
            while (bytesToRead != 0) // read till there's nothing left to read
            {
                dataQueue.Enqueue(serialPort.ReadByte()); // read and enqueue   
                bytesToRead = serialPort.BytesToRead; // update bytes to read
            }
        }

        public void updateDisplayedData(Object myObject, EventArgs myEventArgs)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                byteCountBox.Text = serialPort.BytesToRead.ToString(); // update bytes left to read count
            }
            queueCountBox.Text = dataQueue.Count.ToString(); // update temp queue count

            int res = 0;
            while (dataQueue.TryDequeue(out res)) // dequeue while data queue is not empty
            {
                if (res == 255)
                {
                    currData = 1;

                    string orientation = GetOrientation((Int32)axBox.Tag, (Int32)ayBox.Tag, (Int32)azBox.Tag);
                    orientationBox.Text = orientation;

                    string curState = stateBox.Text;
                    string state = GetNewState((Int32)axBox.Tag, (Int32)ayBox.Tag, (Int32)azBox.Tag, curState);
                    stateBox.Text = state;

                } else
                {
                    switch (currData)
                    {
                        case 1:
                            axBox.Text = res.ToString();
                            axBox.Tag = res; // save int as tag to excapsulate work of keeping track of ax into axBox object, no separate variable for this
                            SetAverage(res, ax_100points, axAvgBox);
                            AddChartValue(axChartValues, res, "x");
                            break;
                        case 2:
                            ayBox.Text = res.ToString();
                            ayBox.Tag = res;
                            SetAverage(res, ay_100points, ayAvgBox);
                            AddChartValue(ayChartValues, res, "y");
                            break;
                        case 3:
                            azBox.Text = res.ToString();
                            azBox.Tag = res;
                            SetAverage(res, az_100points, azAvgBox);
                            AddChartValue(azChartValues, res, "z");
                            break;
                    }
                    currData = currData + 1;
                }
            }
        }

        private void SetAverage(int res, Queue<int> points, TextBox AvgBox)
        {
            points.Enqueue(res);
            while (points.Count > 100)
            {
                points.Dequeue();
            }

            if (points.Count == 100) 
            {
                double avg = Queryable.Average(points.AsQueryable());
                AvgBox.Text = avg.ToString();
            } else if (points.Count < 100)
            {
                AvgBox.Text = "Points count: " + points.Count.ToString();
            } else
            {
                // shouldn't ever reach here
                AvgBox.Text = "Error";
            }
        }

        private string GetOrientation(int ax, int ay, int az)
        {
            int defaultVal = 130; // neutral value for each A 
            int diffX = ax - defaultVal; // get how much offset that A is from neutral
            int diffY = ay - defaultVal;
            int diffZ = az - defaultVal;

            // displays orientation that is has the most offset from neutral
            if (Math.Abs(diffX) > Math.Abs(diffY) && Math.Abs(diffX) > Math.Abs(diffZ)) // if true, then Ax is the most offset since it's bigger than both Ay and Az
            {
                if (diffX > 0)
                {
                    return "+X"; //left
                } else
                {
                    return "-X"; //right
                }
            } else if (Math.Abs(diffY) > Math.Abs(diffZ))  // if true, then Ay is most offset, since Ax isn't biggest and Ay is bigger than Az
            {
                if (diffY > 0)
                {
                    return "+Y"; //front
                } else
                {
                    return "-Y"; //back
                }
            } else // if this is true, then Az is most offset by default
            {
                if (diffZ > 0)
                {
                    return "+Z"; //up
                } else
                {
                    return "-Z"; //down
                }
            }
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            serialPort = null;

            disconnectBtn.Visible = false; // turn off button since there's nothing to disconnect
        }

        int ax_threshold = 170; // got these thresholds by reviewing data
        int ay_threshold = 170;
        int az_threshold = 190;

        const string state0 = "0 initial";
        const string state1 = "1 simple punch";
        const string state2 = "2";
        const string state3 = "3 right hook";
        const string state4 = "4";
        const string state5 = "5 high punch";

        bool ax_thresholdCrossed = false;
        bool ay_thresholdCrossed = false;
        bool az_thresholdCrossed = false;


        private DateTime lastStateChangeTime = DateTime.Now;
        private string GetNewState(int ax, int ay, int az, string curState)
        {
            bool axSwitch = ax < ax_threshold && ax_thresholdCrossed; // check if value has come down from crossing threshold
            bool aySwitch = ay < ay_threshold && ay_thresholdCrossed;
            bool azSwitch = az < az_threshold && az_thresholdCrossed;
            ax_thresholdCrossed = ax >= ax_threshold;
            ay_thresholdCrossed = ay >= ay_threshold;
            az_thresholdCrossed = az >= az_threshold;

            Console.WriteLine((DateTime.Now - lastStateChangeTime).TotalSeconds.ToString());
            bool someTimeGap = (DateTime.Now - lastStateChangeTime).TotalSeconds > 1;

            string newState = curState;
            switch (curState)
            {
                case state0:
                    if (axSwitch)
                    {
                        newState = state1;
                    }
                    else if (azSwitch)
                    {
                        newState = state4;
                    }
                    else if (aySwitch)
                    {
                        newState = state0;
                    }
                    break;
                case state1:
                    if (aySwitch)
                    {
                        newState = state2;
                    }
                    else if (axSwitch || azSwitch)
                    {
                        newState = state0;
                    }
                    break;
                case state2:
                    if (azSwitch)
                    {
                        newState = state3;
                    }
                    else if (axSwitch || aySwitch)
                    {
                        newState = state0;
                    }
                    break;
                case state3:
                    if (axSwitch || aySwitch || azSwitch)
                    {
                        newState = state0;
                    }
                    break;
                case state4:
                    if (axSwitch)
                    {
                        newState = state5;
                    }
                    else if (aySwitch || azSwitch)
                    {
                        newState = state0;
                    }
                    break;
                case state5:
                    if (axSwitch || aySwitch || azSwitch)
                    {
                        newState = state0;
                    }
                    break;
            }

            
            if (someTimeGap && newState != curState)
            {
                lastStateChangeTime = DateTime.Now;
                return newState;
            } else
            {
                return curState; // no change detected OR not enough time elapsed since last changed
            }
            
        }
    }

    public class MeasureModel
    {
        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

}
