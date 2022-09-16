using static RoblotV2.Utils;

namespace RoblotV2
{
    internal static class Program
    {
        public static List<Client> Clients = new List<Client>();
        public static int maxclients = 1;
        [STAThread]
        static void Main()
        {
            new Mutex(true, "ROBLOX_singletonMutex");
            NativeMethods.AllocConsole();
            WebSocket.Initialize();
            Log(ConsoleColor.Cyan, "Socket Has Started");
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}