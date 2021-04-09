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
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Commands
{
    internal class SearchOpponentCommand : Command
    {
        #region Public Constructors

        public SearchOpponentCommand(CoCSharpPacketReader br)
        {
            br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            DatabaseManager.Singelton.Save(level);

            var l = ObjectManager.GetRandomOnlinePlayer();
            if (l != null)
            {
                l.Tick();
                PacketManager.ProcessOutgoingPacket(new EnemyHomeDataMessage(level.GetClient(), l, level));
            }
           else
                PacketManager.ProcessOutgoingPacket(new EnemyHomeDataMessage(level.GetClient(), l, level));
        }

        #endregion Public Methods
    }
}