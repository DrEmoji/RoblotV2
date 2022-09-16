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
        public static bool usingfollow = false;

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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("AddPos/X");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("AddPos/Y");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("AddPos/Z");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("RemovePos/X");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("RemovePos/Y");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("RemovePos/Z");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!usingfollow)
            {
                button1.Text = "Stop Follow";
                WebSocket.SendMessage($"follow/{textBox1.Text}");
                usingfollow = true;
            }
            else
            {
                button1.Text = "Follow";
                WebSocket.SendMessage("stopmimic");
                usingfollow = false;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage($"tpto/{textBox2.Text}");
        }
    }
}
