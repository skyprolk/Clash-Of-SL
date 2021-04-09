using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using Microsoft.Win32;

namespace CSS.Core.Web
{
    internal class VersionChecker
    {
        internal static void VersionMain()
        {
            try
            {
                WebClient wc = new WebClient();
                string Version = wc.DownloadString("https://clashoflights.cf/css/version.txt");
                if (Version == "0.7.1.0")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[CSS]    -> Your css is up-to-date!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[CSS][ERROR]  ->  css has a new update. Download the latest version from GitHub");
                    Console.WriteLine("[CSS][ERROR]  ->  Current new version is : " + Version);
                    Console.ResetColor();
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[CSS][ERROR]  ->  Problem by checking css version, check your Network");
                Console.ResetColor();
            }
        }
    }
}