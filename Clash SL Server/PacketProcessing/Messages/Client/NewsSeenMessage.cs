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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Messages.Client
{
    class NewsSeenMessage : Message
    {
        public NewsSeenMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {

        }

        public override void Decode()
        {

        }

        public override void Process(Level level)
        {

        }
    }
}
