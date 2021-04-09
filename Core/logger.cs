using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Core;

namespace CSS.Core
{
    /*
     * Logger developed by
     * Alpha Man and CrayCray
     * 
     */ 
    internal class _Logger
    {
        public static void Print(string text, Types types)
        {
            int type = 0;
            string filePath = string.Concat(Environment.CurrentDirectory, @"\Logs\logs.txt");
            int loggerlvl = Convert.ToInt32(ConfigurationManager.AppSettings["logginglvl"]);

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            switch (types)
            {
                case Types.INFO:
                    type = 1;
                    break;
                case Types.DEBUG:
                    type = 2;
                    break;
                case Types.WARNING:
                    type = 3;
                    break;
                case Types.ERROR:
                    type = 4;
                    break;
                case Types.DIRECT:
                    type = 5;
                    break;
            }

            if(loggerlvl == 0)
            {
                Console.WriteLine("[" + types + "] ->    " + text);
            } else if(loggerlvl == 1)
            {
                if(type == 1)
                {
                    using(StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine("[ERROR] -> " + text);
                    }
                }
            } else if(loggerlvl == 2)
            {
                if(type <= 2)
                {
                    using(StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine("["+ types + "] -> " + text);
                    }
                }
            } else if(loggerlvl == 3)
            {
                if(type <= 3)
                {
                    using(StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine("[" + types + "] -> " + text);
                    }
                }
            }
            else if(loggerlvl == 4)
            {
                if(type <= 4)
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine("[" + types + "] -> " + text);
                    }
                }
            }

            if(loggerlvl != 0)
            {
                Console.WriteLine("[CSS][" + types + "] -> " + text);
            }
        }
    }
}