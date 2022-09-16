using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using static RoblotV2.Utils;
using WebSocketSharp;

namespace RoblotV2
{
    class WebSocket
    {
        public static WebSocketServer WSServer;
        public static void Initialize()
        {
            try
            {
                WSServer = new WebSocketServer("ws://localhost:5000");
                WSServer.AddWebSocketService<Handler>("/Boblox");
                WSServer.Start();
            }
            catch (Exception e) { }
        }

        public static void SendMessage(string Message)
        {
            WSServer.WebSocketServices.Broadcast(Message);
        }
    }

    internal class Handler : WebSocketBehavior
    {
        public static int botnum = 0;
        public static bool isbot = false;
        protected override void OnMessage(MessageEventArgs Message)
        {
            try
            {
                string[] CMDInfo = Message.Data.Split('/');
                string CMD = CMDInfo[0];
                string data = CMDInfo[1];
                if (CMD == "Auth")
                {
                    botnum += 1;
                    WebSocket.SendMessage($"sendid/{data}/{botnum}/{isbot}");
                    if (isbot)
                    {
                        Log(ConsoleColor.Blue, $"{data} [{botnum}] Has Connected to Socket");
                        Client BotClient = Program.Clients[botnum - 1];
                        BotClient.botnum = botnum;
                        BotClient.username = data;
                        BotClient.id = CMDInfo[2];
                        BotClient.Loaded = true;
                    }
                    if (botnum == Program.maxclients)
                    {
                        isbot = false;
                        System.Diagnostics.Process secprocess = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo secstartInfo = new System.Diagnostics.ProcessStartInfo();
                        secstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        secstartInfo.FileName = "cmd.exe";
                        secstartInfo.Arguments = "/C TASKKILL /F /IM rbxsilent.exe";
                        secprocess.StartInfo = secstartInfo;
                        secprocess.Start();
                        Log(ConsoleColor.Green, "You Can Now Launch The Game");
                    }
                    if (CMD == "Log")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(data);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (CMD == "ResetNum")
                    {
                        botnum = 0;
                    }
                }
            }
            catch { }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            Console.WriteLine("Error Thrown in Socket" + e);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine($"Bot Has disconnected from the server");
            botnum -= 1;
        }
    }
}
