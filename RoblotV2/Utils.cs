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
    }
}
