using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RoblotV2
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();
    }

    public static class Utils
    {
        public static void Log(ConsoleColor Color, string Msg)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(Msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string LaunchRoblox(string authcode,string gameid)
        {
            Random rnd = new Random();
            long browserTrackerId = 55393295400 + rnd.Next(1, 100);
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int launchTime = (int)t.TotalSeconds * 1000;

            string url = $@"roblox-player:1+launchmode:play+gameinfo:{authcode}+launchtime:{launchTime}+placelauncherurl:https://assetgame.roblox.com/game/PlaceLauncher.ashx?request=RequestGame&browserTrackerId=" + browserTrackerId + "&placeId=" + gameid + "&isPlayTogetherGame=false+browsertrackerid:" + browserTrackerId + "+robloxLocale:en_us+gameLocale:en_us";

            return url;
        }

        public async static Task<string> Visit(string cookie)
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
                        Utils.Log(ConsoleColor.Red, "Your cookie is not valid");
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
    }
}
