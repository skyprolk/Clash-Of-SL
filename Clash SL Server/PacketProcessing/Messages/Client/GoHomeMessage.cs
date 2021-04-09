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

namespace CSS.PacketProcessing.Messages.Client
{
    internal class GoHomeMessage : Message
    {
        #region Public Constructors

        public GoHomeMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
            level.Tick();

            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            //player.GetPlayerAvatar().Clean();
            PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(Client, level));
            if (alliance != null)
                PacketManager.ProcessOutgoingPacket(new AllianceStreamMessage(Client, alliance));
        }

        #endregion Public Methods
    }
}