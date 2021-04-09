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
    internal class JoinRequestAllianceMessage : Message
    {
        #region Public Constructors

        public JoinRequestAllianceMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public static string Message { get; set; }
        public static bool Unknown1 { get; set; }
        public static long Unknown2 { get; set; }
        public static int Unknown3 { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                Unknown1 = br.ReadBoolean();
                Unknown2 = br.ReadInt64();
                Unknown3 = br.ReadInt16();
                Message = br.ReadString();
            }
        }

        public override void Process(Level level)
        {
            PacketManager.ProcessOutgoingPacket(new AnswerJoinRequestAllianceMessage(Client));
        }

        #endregion Public Methods
    }
}