using System;
using System.Configuration;
using System.Reflection;
using CSS.Helpers;
using CSS.Core.Checker;

namespace CSS.Core.Settings
{
    internal class Constants
    {
        public static string Version                 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string Build                   = "6";
        
        public static readonly bool UseCacheServer   = Utils.ParseConfigBoolean("CacheServer");
        public const bool Local = false;

        public const int CleanInterval               = 6000;
        public static int MaxOnlinePlayers           = Utils.ParseConfigInt("MaxOnlinePlayers");

        internal const int SendBuffer = 2048;
        internal const int ReceiveBuffer = 2048;
        public static int LicensePlanID;


        internal static readonly string[] AuthorizedIP = { "192.168.0.5" };
    }
}
