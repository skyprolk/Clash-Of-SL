/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System;
using System.Threading;
using static System.Configuration.ConfigurationManager;
using static CSS.Helpers.Utils;
using static CSS.Helpers.CommandParser;
using static System.Console;
using static System.Environment;
using CSS.Core;
using System.IO;

namespace CSS.Core.Threading
{
    internal class ConsoleThread
    {
        #region Private Fields
        static string Command;
        static string Title, Ta;

        #endregion Private Fields

        #region Private Properties

        static Thread T { get; set; }

        #endregion Private Properties

        #region Public Methods

        internal static void Start()
        {
            T = new Thread(() =>
            {
                Title = "Clash Of SL Server v0.7.1.0 - © 2021";
                foreach (char t in Title)
                {
                    Ta += t;
                    Console.Title = Ta;
                    Thread.Sleep(20);
                }
                ForegroundColor = ConsoleColor.Red;
                WriteLine(@"
╔═══╗╔═══╗╔═══╗╔╗╔═╗
╚╗╔╗║║╔═╗║║╔═╗║║║║╔╝
─║║║║║║─║║║╚═╝║║╚╝╝
─║║║║║╚═╝║║╔╗╔╝║╔╗║
╔╝╚╝║║╔═╗║║║║╚╗║║║╚╗
╚═══╝╚╝─╚╝╚╝╚═╝╚╝╚═╝");

                ResetColor();
                _Logger.Print("     -> This program is made by the DARK Team Of Sky Production.", Types.INFO);
                _Logger.Print("     -> You can find the source at https://github.com/skyprolk", Types.INFO);
                _Logger.Print("     -> Don't forget to visit https://facebook.com/skyprolk daily for updates!", Types.INFO);

                //VersionChecker.VersionMain();

                _Logger.Print("     -> CSS is now starting...", Types.INFO);
                WriteLine("");
                if (!File.Exists("restarter.bat")) { 
                    using (StreamWriter sw = new StreamWriter("restarter.bat"))
                    {
                        sw.WriteLine("echo off");
                        sw.WriteLine("echo .");
                        sw.WriteLine("echo .");
                        sw.WriteLine("taskkill /f /im CSS.exe -t");
                        sw.WriteLine("start CSS.exe");
                        sw.WriteLine("exit");

                    }
                    _Logger.Print("     -> Created Restarter.bat", Types.DEBUG);
                }
                MemoryThread.Start();
                NetworkThread.Start();
                while ((Command = ReadLine()) != null)
                    Parse(Command);
            });
            T.Start();
        }

        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }

        #endregion Public Methods
    }
}
