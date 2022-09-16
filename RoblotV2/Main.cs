using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RoblotV2
{
    public partial class Main : Form
    {
        public static bool usingbots = false;
        public static string[] cookies = File.ReadAllLines("cookies.txt");
        private static string gameid = string.Empty;
        public Main()
        {
            InitializeComponent();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            Program.Clients.Clear();
            Program.maxclients = Int32.Parse(textBox3.Text);
            usingbots = !usingbots;
            Handler.isbot = true;
            if (usingbots)
            {
                button1.Text = "Stop Bots";
                gameid = textBox1.Text;
                if (checkBox1.Checked)
                {
                    Process.Start("rbxsilent.exe");
                }
                for (int clients = 0; clients < Program.maxclients; clients++)
                {
                    Client RobloxClient = new Client();
                    Program.Clients.Add(RobloxClient);
                    RobloxClient.auth = cookies[clients];
                    await RobloxClient.Start(gameid);
                }
            }
            else
            {
                button1.Text = "Start Bots";
                System.Diagnostics.Process secprocess = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo secstartInfo = new System.Diagnostics.ProcessStartInfo();
                secstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                secstartInfo.FileName = "cmd.exe";
                secstartInfo.Arguments = "/C TASKKILL /F /IM RobloxPlayerBeta.exe";
                secprocess.StartInfo = secstartInfo;
                secprocess.Start();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("jump");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("printnum");
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            gameid = textBox2.Text;
            WebSocket.SendMessage($"joinworld|{gameid}");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach(Client BotClient in Program.Clients)
            {
                Utils.Log(ConsoleColor.DarkGreen, $"Username: {BotClient.username}, ID: {BotClient.id}, BotNum: {BotClient.botnum}");
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            WebSocket.SendMessage("printplayers");
        }
    }
}
