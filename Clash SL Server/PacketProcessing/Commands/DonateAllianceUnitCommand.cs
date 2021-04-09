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

namespace CSS.PacketProcessing.Commands
{
    //Commande 0x04
    internal class DonateAllianceUnitCommand : Command
    {
        #region Public Constructors

        public DonateAllianceUnitCommand(CoCSharpPacketReader br)
        {
            Unknown1 = br.ReadUInt32WithEndian();
            PlayerId = br.ReadUInt32WithEndian();
            UnitType = br.ReadUInt32WithEndian();
            Unknown2 = br.ReadUInt32WithEndian();
            Unknown3 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        //00 00 48 7A

        //00 00 00 04 00 00 00 00 49 79 1C DD 00 3D 09 00 00 00 00 08 00 00 48 7A

        #region Public Properties

        public uint PlayerId { get; set; }

        //49 79 1C DD
        public uint UnitType { get; set; }

        public uint Unknown1 { get; set; } //00 00 00 00

        //00 3D 09 00
        public uint Unknown2 { get; set; } //00 00 00 08

        public uint Unknown3 { get; set; }

        #endregion Public Properties
    }
}