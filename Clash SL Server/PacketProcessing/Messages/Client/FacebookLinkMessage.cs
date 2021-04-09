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
using System.Text;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Messages.Client
{
    internal class FacebookLinkMessage : Message
    {
        #region Public Constructors

        public FacebookLinkMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                /*
                Console.WriteLine("Boolean -> " + br.ReadBoolean());
                Console.WriteLine("Unknown 1 -> " + br.ReadInt32());
                Console.WriteLine("Unknown 2 -> " + br.ReadInt32());
                Console.WriteLine("Unknown 3 -> " + br.ReadInt32());
                Console.WriteLine("Unknown 4 -> " + br.ReadInt16());
                Console.WriteLine("Token -> " + br.ReadString());
                */
                Console.WriteLine(Encoding.ASCII.GetString(br.ReadAllBytes()));
            }
        }

        public override void Process(Level level)
        {
        }

        #endregion Public Methods
    }
}