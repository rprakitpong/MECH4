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

            mouseBox.MouseMove += UpdateXYCoord;
            mouseBox.MouseClick += UpdateClickCoord;
        }

        private void UpdateXYCoord(object sender, MouseEventArgs e)
        {
            xCoord.Text = e.X.ToString();
            yCoord.Text = e.Y.ToString();
        }

        private void UpdateClickCoord(object sender, MouseEventArgs e)
        {
            clickCoord.Text += "(" + e.X.ToString() + "," + e.Y.ToString() + ")" + Environment.NewLine;
        }
    }
}
