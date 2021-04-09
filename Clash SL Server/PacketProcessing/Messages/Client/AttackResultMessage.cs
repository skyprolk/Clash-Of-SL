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
using CSS.Core;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Messages.Client
{
    internal class AttackResultMessage : Message
    {
        #region Public Constructors

        public AttackResultMessage(PacketProcessing.Client client, CoCSharpPacketReader br)
            : base(client, br)
        {

        }

        #endregion Public Constructors

        #region Public Methods

        public override void Decode()
        {
            //Console.WriteLine("Packet Attack Result : " + Encoding.UTF8.GetString(GetData()));
        }

        public override void Process(Level level)
        {
        }

        #endregion Public Methods
    }
}