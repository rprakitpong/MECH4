using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            mouseBox.MouseMove += UpdateXYCoord; // add XY display updater as event handler
            mouseBox.MouseClick += UpdateClickCoord; // add click display updater as event handler
        }

        private void UpdateXYCoord(object sender, MouseEventArgs e)
        {
            xCoord.Text = e.X.ToString(); // update X display
            yCoord.Text = e.Y.ToString(); // update Y display
        }

        private void UpdateClickCoord(object sender, MouseEventArgs e)
        {
            clickCoord.Text += "(" + e.X.ToString() + "," + e.Y.ToString() + ")" + Environment.NewLine; // append new line to list of click coordinates
        }
    }
}
