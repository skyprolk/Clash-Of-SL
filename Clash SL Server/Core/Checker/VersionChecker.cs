using System;
using System.Diagnostics;
using System.IO.Compression;
using Ionic.Zip;
using Ionic.Zlib;
using System.IO;
using CSS.Core.Threading;
using static CSS.Core.Logger;
using System.Net;
using System.Threading;
using System.Reflection;
using CSS.Core.Settings;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CSS.Core.Web
{
    internal class VersionChecker
    {
        public static void DownloadUpdater()
        {
            WebClient client = new WebClient();
            client.DownloadFile("https://CSS-up.000webhostapp.com/CSS_Updater.dat", @"Tools/Updater.exe");
            Thread.Sleep(1000);
            Process.Start(@"Tools/Updater.exe");
            Environment.Exit(0);
        }

        public static string GetVersionString()
        {
            try
            {
                WebClient wc = new WebClient();
                string Version = wc.DownloadString("https://raw.githubusercontent.com/skyprolk/Clash-Of-SL/main/Clash%20SL%20Server/version.txt");
                return Version;
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public static string LatestCoCVersion()
        {

                return "8.709.16";
        }
    }
}
