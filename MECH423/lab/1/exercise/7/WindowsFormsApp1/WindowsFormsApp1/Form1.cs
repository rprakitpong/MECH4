using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
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

        public Form1()
        {
            InitializeComponent();

            stateBox.Text = state0; // initialize state

            axBox.Text = "130"; // set up initial values as it would usually be on real board
            ayBox.Text = "130";
            azBox.Text = "150";

            newDataBtn_Click(null, null); // record initial data point
        }

        private void newDataBtn_Click(object sender, EventArgs e)
        {
            int ax = int.Parse(axBox.Text); // trust user to put only int
            int ay = int.Parse(ayBox.Text);
            int az = int.Parse(azBox.Text);
            string curState = stateBox.Text;

            string newState = GetNewState(ax, ay, az, curState);

            dataHistBox.Text += "(" + ax + ", " + ay + ", " + az + ", " + newState + "), ";
            stateBox.Text = newState;
        }

        private string GetNewState(int ax, int ay, int az, string curState)
        {
            bool axSwitch = ax < ax_threshold && ax_thresholdCrossed; // check if value has come down from crossing threshold
            bool aySwitch = ay < ay_threshold && ay_thresholdCrossed;
            bool azSwitch = az < az_threshold && az_thresholdCrossed;
            ax_thresholdCrossed = ax >= ax_threshold;
            ay_thresholdCrossed = ay >= ay_threshold;
            az_thresholdCrossed = az >= az_threshold;

            switch (curState)
            {
                case state0:
                    if (axSwitch)
                    {
                        return state1;
                    } else if (azSwitch)
                    {
                        return state4;
                    } else if (aySwitch)
                    {
                        return state0;
                    }
                    break;
                case state1:
                    if (aySwitch)
                    {
                        return state2;
                    } else if (axSwitch || azSwitch)
                    {
                        return state0;
                    }
                    break;
                case state2:
                    if (azSwitch)
                    {
                        return state3;
                    } else if (axSwitch || aySwitch)
                    {
                        return state0;
                    }
                    break;
                case state3:
                    if (axSwitch || aySwitch || azSwitch)
                    {
                        return state0;
                    }
                    break;
                case state4:
                    if (axSwitch)
                    {
                        return state5;
                    } else if (aySwitch || azSwitch)
                    {
                        return state0;
                    }
                    break;
                case state5:
                    if (axSwitch || aySwitch || azSwitch)
                    {
                        return state0;
                    }
                    break;
            }
            return curState; // no change detected, return current state
        }
    }
}
