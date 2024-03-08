using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using static CSS.Core.Logger;
using System.Net.Sockets;
using CSS.Core.Settings;

namespace CSS.Core.Checker
{
    internal class LicenseChecker
    {
        private string Key;
        private int responseData;
        public LicenseChecker()
        {
            try
            {
                Program._Stopwatch.Stop();

                back:
                Key = GetKey();
                if (Key == "Sky Production")
                {
                    Say("CSS is activated !");
                }
                else
                {
                    Say("You entered a wrong Key! Please try again.");
                    goto back;
                }

                Program._Stopwatch.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Say("CSS will be closed now...");
                Thread.Sleep(4000);
                Environment.Exit(0);
            }
        }

        private static string GetKey()
        {
            back:
            Say("Enter now your License Key:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[Enter Your License key ] >  ");
            Console.ResetColor();
            string Key = Console.ReadLine();
            return Key;
        }

    }
}
