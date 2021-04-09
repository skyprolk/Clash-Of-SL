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
    //Commande 0x0211
    internal class ToggleHeroSleepCommand : Command
    {
        #region Public Constructors

        public ToggleHeroSleepCommand(CoCSharpPacketReader br)
        {
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            FlagSleep = br.ReadByte();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        //00 00 02 11 1D CD 65 06 00 00 01 04 CA

        #region Public Properties

        public uint BuildingId { get; set; }
        public byte FlagSleep { get; set; }
        public uint Unknown1 { get; set; }

        #endregion Public Properties
    }
}