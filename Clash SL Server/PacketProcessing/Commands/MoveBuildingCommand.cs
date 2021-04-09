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

namespace CSS.PacketProcessing.Commands
{
    internal class MoveBuildingCommand : Command
    {
        #region Public Constructors

        public MoveBuildingCommand(CoCSharpPacketReader br)
        {
            X = br.ReadInt32WithEndian();
            Y = br.ReadInt32WithEndian();
            BuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            go.SetPositionXY(X, Y);
        }

        #endregion Public Methods

        #region Public Properties

        public int BuildingId { get; set; }

        //1D CD 65 06 some unique id
        public uint Unknown1 { get; set; }

        public int X { get; set; } //00 00 00 13
        public int Y { get; set; }

        #endregion Public Properties
    }
}