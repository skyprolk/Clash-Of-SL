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

using System.IO;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Commands
{
    internal class MyLeagueCommand : Command
    {
        #region Public Constructors

        public MyLeagueCommand(CoCSharpPacketReader br)
        {

        }

        public override void Execute(Level level)
        {
            //PacketManager.ProcessOutgoingPacket(new LeaguePlayersMessage(level.GetClient()));
        }

        #endregion Public Constructors
    }
}
