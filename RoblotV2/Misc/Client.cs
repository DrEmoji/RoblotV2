using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoblotV2
{
    internal class Client
    {
        public string auth = "";
        public int botnum = 0;
        public string username = "";
        public string id = "";
        public bool Loaded = false;
        public async Task Start(string gameid)
        {
            var task1 = Utils.Visit(auth);
            var results = await Task.WhenAll(task1);
            if (!(results[0] == "false") && !String.IsNullOrEmpty(results[0]))
            {
                Utils.Log(ConsoleColor.Cyan, "Got token");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = true;
                startInfo.FileName = Utils.LaunchRoblox(results[0], gameid);
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                var game = Process.Start(startInfo);
                Utils.Log(ConsoleColor.Blue, "Launched");
                game.WaitForExit();
                Utils.Log(ConsoleColor.Green, "Done");
            }
        }
    }
}
