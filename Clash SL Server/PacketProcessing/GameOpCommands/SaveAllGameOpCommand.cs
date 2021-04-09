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

using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.GameOpCommands
{
    internal class SaveAllGameOpCommand : GameOpCommand
    {
        #region Public Constructors

        public SaveAllGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(3);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                //Use this Command before you restart your server!

                /* Starting saving of players */
                var pm = new GlobalChatLineMessage(level.GetClient());
                pm.SetChatMessage("Starting saving process of every player!");
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName("css Bot");
                PacketManager.ProcessOutgoingPacket(pm);
                DatabaseManager.Singelton.Save(ResourcesManager.GetInMemoryLevels());
                var p = new GlobalChatLineMessage(level.GetClient());
                /* Confirmation */
                p.SetChatMessage("All Players are saved!");
                p.SetPlayerId(0);
                p.SetLeagueId(22);
                p.SetPlayerName("css Bot");
                PacketManager.ProcessOutgoingPacket(p);
                /* Starting saving of Clans */
                var pmm = new GlobalChatLineMessage(level.GetClient());
                pmm.SetPlayerId(0);
                pmm.SetLeagueId(22);
                pmm.SetPlayerName("css Bot");
                pmm.SetChatMessage("Starting with saving of every Clan!");
                PacketManager.ProcessOutgoingPacket(pmm);
                /* Confirmation */
                DatabaseManager.Singelton.Save(ObjectManager.GetInMemoryAlliances());
                var pmp = new GlobalChatLineMessage(level.GetClient());
                pmp.SetPlayerId(0);
                pmp.SetLeagueId(22);
                pmp.SetPlayerName("css Bot");
                pmp.SetChatMessage("All Clans are saved!");
                PacketManager.ProcessOutgoingPacket(pmp);
            }
            else
            {
                var p = new GlobalChatLineMessage(level.GetClient());
                p.SetChatMessage("GameOp command failed. Access to Admin GameOP is prohibited.");
                p.SetPlayerId(0);
                p.SetLeagueId(22);
                p.SetPlayerName("css Bot");
                PacketManager.ProcessOutgoingPacket(p);
            }
        }

        #endregion Public Methods

        #region Private Fields

        static DatabaseManager m_vDatabase;
        readonly string[] m_vArgs;

        #endregion Private Fields
    }
}