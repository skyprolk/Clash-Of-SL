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
    internal class HandshakeRequest : Message
    {

        public HandshakeRequest(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        public string Hash;
        public int MajorVersion;
        public int MinorVersion;
        public int Unknown1;
        public int Unknown2;
        public int Unknown4;
        public int Unknown6;
        public int Unknown7;

        public override void Decode()
        {
            using (var reader = new BinaryReader(new MemoryStream(GetData())))
            {
                Unknown1 = reader.ReadInt32();
                Unknown2 = reader.ReadInt32();
                MajorVersion = reader.ReadInt32();
                Unknown4 = reader.ReadInt32();
                MinorVersion = reader.ReadInt32();
                Hash = reader.ReadString();
                Unknown6 = reader.ReadInt32();
                Unknown7 = reader.ReadInt32();
            }
            if (MajorVersion == 8)
                Client.CState = 1; // Clash of Clans 8.x
            else
                Client.CState = 0; // Old Clash of Clans client
        }

        public override void Process(Level level)
        {
            PacketManager.ProcessOutgoingPacket(new HandshakeSuccess(Client, this));
        }

    }
}