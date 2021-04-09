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
    internal class ToggleHeroAttackModeCommand : Command
    {
        #region Public Constructors

        public ToggleHeroAttackModeCommand(CoCSharpPacketReader br)
        {
            /*
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadByte();
            Unknown2 = br.ReadUInt32WithEndian();
            Unknown3 = br.ReadUInt32WithEndian();
            */
        }

        #endregion Public Constructors

        #region Public Properties

        public uint BuildingId { get; set; }
        public byte Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }

        #endregion Public Properties
    }
}