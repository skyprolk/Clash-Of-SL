using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSS.Logic.Enums;
using static System.Convert;

namespace CSS.Core
{
    internal class Logger
    {
        static bool ValidLogLevel;
        static int getlevel = ToInt32(ConfigurationManager.AppSettings["LogLevel"]);
        static string timestamp = Convert.ToString(DateTime.Today).Remove(10).Replace(".", "-").Replace("/", "-");
        static string path = "Logs/log_" + timestamp + "_.txt";
        static SemaphoreSlim _fileLock = new SemaphoreSlim(1);

        public static async void Write(string text)
        {
            if (getlevel != 0)
            {
                try
                {
                    await _fileLock.WaitAsync();
                    if (getlevel == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("[LOG]  " + DateTime.Now + "  " + text);
                        Console.ResetColor();
                    }
                    using (StreamWriter sw = new StreamWriter(path,true))
                    await sw.WriteLineAsync("[LOG]  " + DateTime.Now + "  " + text);
                }
                finally
                {
                    _fileLock.Release();
                }
            }
        }

        public static async void WriteError(string text)
        {
            if (getlevel != 0)
            {
                try
                {
                    await _fileLock.WaitAsync();
                    if (getlevel == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[LOG]  " + DateTime.Now + "  " + text);
                        Console.ResetColor();
                    }
                    using (StreamWriter sw = new StreamWriter(path, true))
                        await sw.WriteLineAsync("[LOG]  " + DateTime.Now + "  " + text);
                }
                finally
                {
                    _fileLock.Release();
                }
            }
        }

        public static void WriteCenter(string _String)
        {
            Console.SetCursorPosition((Console.WindowWidth - _String.Length) / 2, Console.CursorTop);
            Console.WriteLine(_String);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }

        public static void WriteColored(string _String, int ColorID = 1)
        {
            if (ColorID == 1)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(_String);
                Console.ResetColor();
            }
        }

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void Say(string message, bool write = false)
        {
            if (!write)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("[CSS]  " + DateTime.Now + "  ");
                Console.ResetColor();
                Console.WriteLine(message);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("[CSS]  " + DateTime.Now + "  ");
                Console.ResetColor();
                Console.Write(message);
            }
        }

        public static void Say()
        {
            Console.WriteLine();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR]  " + DateTime.Now + "  " + message);
            Console.ResetColor();
        }

        public Logger()
        {
            if(getlevel > 2)
            {
                ValidLogLevel = false;
                LogLevelError();
            }
            else
            {
                ValidLogLevel = true;
            }
            
            if (getlevel != 0 || ValidLogLevel == true)
            {
                if (!File.Exists("Logs/log_" + timestamp + "_.txt"))
                    using (StreamWriter sw = new StreamWriter("Logs/log_" + timestamp + "_.txt"))
                    {
                        sw.WriteLineAsync("Log file created at " + DateTime.Now);
                        sw.WriteLineAsync();
                    }
            }
        }

        public void LogLevelError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("[ERROR]  " + DateTime.Now + "Please choose a valid Log Level");
            Console.WriteLine("CSS Emulator is now closing...");
            Console.ResetColor();
            Thread.Sleep(5000);
            Environment.Exit(0);
        }
    }
}
