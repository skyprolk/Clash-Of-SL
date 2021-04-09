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

namespace CSS.PacketProcessing.Messages.Client
{
    internal class AttackNpcMessage : Message
    {
        #region Public Constructors

        public AttackNpcMessage(PacketProcessing.Client client, CoCSharpPacketReader br)
            : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int LevelId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                LevelId = br.ReadInt32WithEndian();
            }
        }

        public override void Process(Level level)
        {
            var san = new NpcDataMessage(Client, level, this);
            PacketManager.ProcessOutgoingPacket(san);
        }

        #endregion Public Methods
    }
}