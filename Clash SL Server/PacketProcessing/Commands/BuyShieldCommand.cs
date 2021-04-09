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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class BuyShieldCommand : Command
    {
        #region Public Constructors

        public BuyShieldCommand(CoCSharpPacketReader br)
        {
            ShieldId = br.ReadInt32WithEndian(); //= shieldId - 0x01312D00;
            Unknown1 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            //Console.WriteLine(ShieldId);
            //Console.WriteLine(Unknown1);
        }

        #endregion Public Methods

        #region Public Properties

        public int ShieldId { get; set; }
        public uint Unknown1 { get; set; }

        #endregion Public Properties
    }
}
