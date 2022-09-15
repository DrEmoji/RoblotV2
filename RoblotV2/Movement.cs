using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoblotV2
{
    public partial class Movement : Form
    {
        public static bool usingmimic = false;
        public static bool usingorbit = false;
        public Movement()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!usingmimic)
            {
                button1.Text = "Stop Mimic";
                WebSocket.SendMessage($"mimic/{textBox1.Text}");
                usingmimic = true;
            }
            else
            {
                button1.Text = "Mimic";
                WebSocket.SendMessage("stopmimic");
                usingmimic = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!usingorbit)
            {
                button2.Text = "Stop Orbit";
                WebSocket.SendMessage($"orbit/{textBox1.Text}");
                usingorbit = true;
            }
            else
            {
                button2.Text = "Orbit";
                WebSocket.SendMessage("stoporbit");
                usingorbit = false;
            }
        }
    }
}
