using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using CSS.Core;
using CSS.Core.Checker;
using CSS.Core.Network;
using CSS.Core.Settings;
using CSS.Core.Threading;
using CSS.Core.Web;
using CSS.Helpers;
using CSS.WebAPI;
using static CSS.Core.Logger;

namespace CSS
{
    internal class Program
    {
        internal static int OP = 0;
        internal static string Title = $"Clash SL Server v{Constants.Version} Build: {Constants.Build} - ©CSS | Online Players: ";
        public static Stopwatch _Stopwatch = new Stopwatch();
        public static string Version { get; set; }

        internal static void Main()
        {
            int GWL_EXSTYLE = -20;
            int WS_EX_LAYERED = 0x80000;
            uint LWA_ALPHA = 0x2;
            IntPtr Handle = GetConsoleWindow();
            SetWindowLong(Handle, GWL_EXSTYLE, (int)GetWindowLong(Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED);
            Console.SetWindowSize(92,32);

            if (Utils.ParseConfigBoolean("Animation"))
            {

                new Thread(() =>
                {
                    for (int i = 20; i < 227; i++)
                    {
                        if (i < 100)
                        {
                            SetLayeredWindowAttributes(Handle, 0, (byte)i, LWA_ALPHA);
                            Thread.Sleep(5);
                        }
                        else
                        {
                            SetLayeredWindowAttributes(Handle, 0, (byte)i, LWA_ALPHA);
                            Thread.Sleep(15);
                        }
                    }
                }).Start();
            }
            else
            {
                SetLayeredWindowAttributes(Handle, 0, 227, LWA_ALPHA);
            }

            if (Constants.LicensePlanID == 3)
            {
                Console.Title = Title + OP;
            }
            else if(Constants.LicensePlanID == 2)
            {
                Console.Title = Title + OP + "/700";
            }
            else if (Constants.LicensePlanID == 1)
            {
                Console.Title = Title + OP + "/350";
            }

            Say();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(
                @"
╔═══╗╔═══╗╔═══╗╔╗╔═╗
╚╗╔╗║║╔═╗║║╔═╗║║║║╔╝
─║║║║║║─║║║╚═╝║║╚╝╝
─║║║║║╚═╝║║╔╗╔╝║╔╗║
╔╝╚╝║║╔═╗║║║║╚╗║║║╚╗
╚═══╝╚╝─╚╝╚╝╚═╝╚╝╚═╝");

            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Logger.WriteCenter("+-------------------------------------------------------+");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Logger.WriteCenter("|This program is made by the Sky Production Development Team.|");
            Logger.WriteCenter("|    CSS is not affiliated to \"Supercell, Oy\".    |");
            Logger.WriteCenter("|        This program is copyrighted worldwide.         |");
            Logger.WriteCenter("|         Modified by DARK to Ensure Functionality      |");
            Console.ForegroundColor = ConsoleColor.Blue;
            Logger.WriteCenter("+-------------------------------------------------------+");
            Console.ResetColor();

            Say();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[CSS]    ");
            Version = VersionChecker.GetVersionString();

            _Stopwatch.Start();

            if (Version == Constants.Version)
            {
                Console.WriteLine($"> CSS is up-to-date: {Constants.Version}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Say("Preparing Server...\n");
                Resources.Initialize();
            }
            else if (Version == "Error")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("> An Error occured when requesting the Version number.");
                Console.WriteLine();
                Logger.Say("Aborting...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> CSS is not up-to-date! New Version: {Version}. Aborting...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static void UpdateTitle()
        {
            if (Constants.LicensePlanID == 3)
            {
                Console.Title = Title + OP;
            }
            else if (Constants.LicensePlanID == 2)
            {
                Console.Title = Title + OP + "/700";
            }
            else if (Constants.LicensePlanID == 1)
            {
                Console.Title = Title + OP + "/350";
            }
        }

        public static void TitleU()
        {
            if (Constants.LicensePlanID == 3)
            {
                Console.Title = Title + ++OP;
            }
            else if(Constants.LicensePlanID == 2)
            {
                Console.Title = Title + ++OP + "/700";
            }
            else if (Constants.LicensePlanID == 1)
            {
                Console.Title = Title + ++OP + "/350";
            }
        }

        public static void TitleD()
        {
            if (Constants.LicensePlanID == 3)
            {
                Console.Title = Title + --OP;
            }
            else if(Constants.LicensePlanID == 2)
            {
                Console.Title = Title + --OP + "/700";
            }
            else if(Constants.LicensePlanID == 1)
            {
                Console.Title = Title + --OP + "/350";
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
