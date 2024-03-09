using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CSP
{
    internal class Proxy
    {
        public const string hostname = "gamea.clashofclans.com";
        public const int port = 9339;
        public static Stopwatch _Stopwatch = new Stopwatch();

        private static void Main()
        {
            int GWL_EXSTYLE = -20;
            int WS_EX_LAYERED = 0x80000;
            uint LWA_ALPHA = 0x2;
            IntPtr Handle = GetConsoleWindow();
            SetWindowLong(Handle, GWL_EXSTYLE, (int)GetWindowLong(Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED);
            SetLayeredWindowAttributes(Handle, 0, 227, LWA_ALPHA);

            Console.Title = $"Clash SL Proxy v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

            if (!Directory.Exists("Packets"))
            {
                Directory.CreateDirectory("Packets");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                @"
╔═══╗╔═══╗╔═══╗╔╗╔═╗
╚╗╔╗║║╔═╗║║╔═╗║║║║╔╝
─║║║║║║─║║║╚═╝║║╚╝╝
─║║║║║╚═╝║║╔╗╔╝║╔╗║
╔╝╚╝║║╔═╗║║║║╚╗║║║╚╗
╚═══╝╚╝─╚╝╚╝╚═╝╚╝╚═╝");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("          Clash SL");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Proxy");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[PROXY]");
            Console.ResetColor();
            Console.WriteLine("    -> You can find the source at www.github.com/skyprolk");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[PROXY]");
            Console.ResetColor();
            Console.WriteLine("    -> Clash SL Proxy is now starting...");
            _Stopwatch.Start();

            Console.WriteLine();

            try
            {
                Server server = new Server(port);
                server.StartServer();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();
    }
}
