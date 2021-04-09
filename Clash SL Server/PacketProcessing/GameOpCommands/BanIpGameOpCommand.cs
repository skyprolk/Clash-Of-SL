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
using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.GameOpCommands
{
    internal class BanIpGameOpCommand : GameOpCommand
    {
        #region Private Fields

        readonly string[] m_vArgs;

        #endregion Private Fields

        #region Public Constructors

        public BanIpGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(3);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
                if (m_vArgs.Length >= 1)
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var l = ResourcesManager.GetPlayer(id);
                        if (l != null)
                            if (l.GetAccountPrivileges() < level.GetAccountPrivileges())
                            {
                                //l.BanIP();
                                l.SetAccountStatus(99);
                                l.SetAccountPrivileges(0);
                                if (ResourcesManager.IsPlayerOnline(l))
                                {
                                    var p = new OutOfSyncMessage(l.GetClient());
                                    PacketManager.ProcessOutgoingPacket(p);
                                }
                                //ObjectManager.LoadBannedIPs();
                            }
                            else
                                Console.WriteLine("Ban IP failed: insufficient privileges");
                        else
                            Console.WriteLine("Ban IP failed: id " + id + " not found");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ban IP failed with error: " + ex);
                    }
                else
                    SendCommandFailedMessage(level.GetClient());
        }

        #endregion Public Methods
    }
}