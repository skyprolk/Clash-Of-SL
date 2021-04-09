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
    internal class ChangeAvatarNameMessage : Message
    {
        #region Public Constructors

        public ChangeAvatarNameMessage(PacketProcessing.Client client, CoCSharpPacketReader br)
            : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string PlayerName { get; set; }

        public int PlayerNameLength { get; set; }

        public byte Unknown1 { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                PlayerName = br.ReadScString();
                Unknown1 = br.ReadByte();
            }
        }

        public override void Process(Level level)
        {
            level.GetPlayerAvatar().SetName(PlayerName);
            var p = new AvatarNameChangeOkMessage(Client);
            p.SetAvatarName(level.GetPlayerAvatar().GetAvatarName());
            PacketManager.ProcessOutgoingPacket(p);
        }

        #endregion Public Methods
    }
}