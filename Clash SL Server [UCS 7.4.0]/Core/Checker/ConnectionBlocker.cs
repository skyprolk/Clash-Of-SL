using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CSS.Core.Logger;

namespace CSS.Core.Checker
{
    class ConnectionBlocker
    {
        public static List<string> Banned_IPs = new List<string>();

        public ConnectionBlocker()
        {
            if (!File.Exists("Banned_IP's.ini"))
            {
                File.Create("Banned_IP's.ini").Close();

                using (StreamReader sr = new StreamReader("Banned_IP's.ini"))
                {
                    string[] s = sr.ReadToEnd().Split('*');

                    foreach (string st in s)
                    {
                        Banned_IPs.Add(st);
                    }

                    sr.Close();
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader("Banned_IP's.ini"))
                {
                    string[] s = sr.ReadToEnd().Split('*');

                    foreach (string st in s)
                    {
                        Banned_IPs.Add(st);
                    }

                    sr.Close();
                }
            }
        }

        public static void AddNewIpToBlackList(string s)
        {
            if (!s.Contains("."))
            {
                Say("The IP '" + s + "' is not valid. (Example: '127.0.0.1')");
            }
            else
            {
                using (StreamWriter sw = new StreamWriter("Banned_IP's.ini"))
                {
                    sw.Write("*" + s);
                }

                Banned_IPs.Add(s);

                Say("The IP '" + s + "' has been added to the Blacklist.");
            }
        }

        public static bool IsAddressBanned(string s)
        {
            if (Banned_IPs.Contains(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void GetBannedIPs()
        {
            int count = 0;
            foreach (string s in Banned_IPs)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    count++;
                    Say(count + ". " + s);
                }
                else if(Banned_IPs.Count < 2)
                {
                    Say("No banned IP's.");
                }
            }
        }
    }
}
