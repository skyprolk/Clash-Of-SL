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
    internal class SaveAccountGameOpCommand : GameOpCommand
    {
        #region Public Constructors

        public SaveAccountGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(5);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                DatabaseManager.Singelton.Save(level);
                var p = new GlobalChatLineMessage(level.GetClient());
                p.SetChatMessage("Game Successfuly Saved!");
                p.SetPlayerId(0);
                p.SetLeagueId(22);
                p.SetPlayerName("css Bot");
                PacketManager.ProcessOutgoingPacket(p);
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