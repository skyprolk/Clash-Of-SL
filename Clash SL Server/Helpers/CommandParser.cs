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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using CSS.Core;
using CSS.Core.Network;
using CSS.PacketProcessing;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.Helpers
{
    internal class CommandParser
    {
        #region Public Methods

        public static void Parse(string Command)
        {
            switch (Command)
            {
                case "/help":
                    Console.WriteLine("[CSS][MENU]  -> /status      - Shows the actual css status.");
                    Console.WriteLine("[CSS][MENU]  -> /clear       - Clears the console screen.");
                    Console.WriteLine("[CSS][MENU]  -> /shutdown    - Shuts css down instantly.");
                    Console.WriteLine("[CSS][MENU]  -> /gui      - Shows the css interface.");
                    break;

                case "/status":
                    Console.WriteLine("");
                    Console.WriteLine("[CSS][INFO]  -> IP Address:     " +
                                      Dns.GetHostByName(Dns.GetHostName()).AddressList[0]);
                    Console.WriteLine("[CSS][INFO]  -> Online players:         " +
                                      ResourcesManager.GetOnlinePlayers().Count);
                    Console.WriteLine("[CSS][INFO]  -> Connected players:      " +
                                      ResourcesManager.GetConnectedClients().Count);
                    Console.WriteLine("[CSS][INFO]  -> Clash Version: 8.332.16");
                    break;

                case "/clear":
                    Console.Clear();
                    break;

                case "/shutdown":
                    Environment.Exit(0);
                    break;
                case "/gui":
                    Application.EnableVisualStyles();
                    Application.Run(new css.CSSUI());
                    break;

                default:
                    Console.WriteLine(
                        "[CSS]    Unknown command, type \"/help\" for a list containing all available commands.");
                    break;
            }
        }

        #endregion Public Methods
    }
}
