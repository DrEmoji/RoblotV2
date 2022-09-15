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

        private string LaunchRoblox(string authcode)
        {
            Random rnd = new Random();
            long browserTrackerId = 55393295400 + rnd.Next(1, 100);
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int launchTime = (int)t.TotalSeconds * 1000;

            string url = $@"roblox-player:1+launchmode:play+gameinfo:{authcode}+launchtime:{launchTime}+placelauncherurl:https://assetgame.roblox.com/game/PlaceLauncher.ashx?request=RequestGame&browserTrackerId=" + browserTrackerId + "&placeId=" + gameid + "&isPlayTogetherGame=false+browsertrackerid:" + browserTrackerId + "+robloxLocale:en_us+gameLocale:en_us";

            return url;
        }

        public async Task<string> Visit(string cookie)
        {
            try
            {
                var baseAddress = new Uri("https://auth.roblox.com/v1/authentication-ticket/");
                var cookieContainer = new CookieContainer();
                var request = new HttpRequestMessage(HttpMethod.Post, baseAddress);
                request.Headers.Add("User-Agent", "Roblox/WinInet");
                request.Headers.Add("Referer", "https://www.roblox.com/develop");
                request.Headers.Add("RBX-For-Gameauth", "true");

                var request2 = new HttpRequestMessage(HttpMethod.Post, baseAddress);
                request2.Headers.Add("User-Agent", "Roblox/WinInet");
                request2.Headers.Add("Referer", "https://www.roblox.com/develop");
                request2.Headers.Add("RBX-For-Gameauth", "true");

                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = false })
                using (var client = new HttpClient(handler))
                {
                    cookieContainer.Add(baseAddress, new Cookie(".ROBLOSECURITY", cookie));
                    var result = await client.SendAsync(request);



                    if (result.Headers.Contains("X-CSRF-TOKEN"))
                    {
                        var xcsrf = (String[])result.Headers.GetValues("X-CSRF-TOKEN");
                        request2.Headers.Add("X-CSRF-TOKEN", xcsrf[0]);
                        result = await client.SendAsync(request2);
                        var authcode = (String[])result.Headers.GetValues("rbx-authentication-ticket");
                        return authcode[0];
                    }
                    else
                    {
                        Utils.Log(ConsoleColor.Red,"Your cookie is not valid");
                        return "false";
                    }
                }

            }
            catch (Exception exc)
            {

                Utils.Log(ConsoleColor.Red, exc.ToString());
            }
            return "";
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            int maxclients = Int32.Parse(textBox3.Text);
            usingbots = !usingbots;
            Handler.isbot = true;
            if (usingbots)
            {
                gameid = textBox1.Text;
                if (checkBox1.Checked)
                {
                    Process.Start("rbxsilent.exe");
                }
                for (int clients = 0; clients < maxclients;)
                {
                    var task1 = Visit(cookies[clients]);
                    var results = await Task.WhenAll(task1);
                    if (!(results[0] == "false") && !String.IsNullOrEmpty(results[0]))
                    {
                        Utils.Log(ConsoleColor.Cyan,"Got token");
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = true;
                        startInfo.FileName = LaunchRoblox(results[0]);
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        var game = Process.Start(startInfo);
                        Utils.Log(ConsoleColor.Blue, "Launched");
                        game.WaitForExit();
                        Utils.Log(ConsoleColor.Green, "Done");
                        clients++;
                    }
                    else
                    {
                        clients = maxclients;
                        continue;
                    }
                }
                if (checkBox1.Checked)
                {
                    new Thread((ThreadStart)delegate ()
                    {
                        Thread.Sleep(5000);
                        System.Diagnostics.Process secprocess = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo secstartInfo = new System.Diagnostics.ProcessStartInfo();
                        secstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        secstartInfo.FileName = "cmd.exe";
                        secstartInfo.Arguments = "/C TASKKILL /F /IM rbxsilent.exe";
                        secprocess.StartInfo = secstartInfo;
                        secprocess.Start();
                        Handler.isbot = false;
                    }).Start();
                }
                button1.Text = "Stop Bots";

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
    }
}
