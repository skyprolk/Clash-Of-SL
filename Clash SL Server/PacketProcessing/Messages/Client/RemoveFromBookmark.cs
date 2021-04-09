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
using System.IO;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Messages.Client
{
    internal class RemoveFromBookmarkMessage : Message
    {
        #region Public Constructors

        public RemoveFromBookmarkMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())));
                //Console.WriteLine("ID OF CLAN -> " + br.ReadInt32());
        }

        public override void Process(Level level)
        {
            PacketManager.ProcessOutgoingPacket(new BookmarkRemoveAllianceMessage(Client));
        }

        #endregion Public Methods
    }
}