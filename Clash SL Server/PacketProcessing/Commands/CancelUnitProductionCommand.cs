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
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class CancelUnitProductionCommand : Command
    {
        #region Public Constructors

        public CancelUnitProductionCommand(CoCSharpPacketReader br)
        {
            BuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
            UnitType = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
            Unknown3 = br.ReadUInt32WithEndian();
            Unknown4 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if (Count > 0)
            {
                var b = (Building) go;
                var c = b.GetUnitProductionComponent();
                var cd = (CombatItemData) ObjectManager.DataTables.GetDataById(UnitType);
                do
                {
                    //Ajouter gestion remboursement ressources
                    c.RemoveUnit(cd);
                    Count--;
                }
                while (Count > 0);
            }
        }

        #endregion Public Methods

        #region Public Properties

        public int BuildingId { get; set; }
        public int Count { get; set; }
        public int UnitType { get; set; }
        public uint Unknown1 { get; set; } //00 00 00 00

        //00 3D 09 00
        //00 00 00 01
        public uint Unknown3 { get; set; } //00 00 00 00

        public uint Unknown4 { get; set; }

        #endregion Public Properties
    }
}