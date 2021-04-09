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
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class UpgradeUnitCommand : Command
    {
        #region Public Constructors

        public UpgradeUnitCommand(CoCSharpPacketReader br)
        {
            BuildingId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
            UnitData = (CombatItemData) br.ReadDataReference();
            Unknown2 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var ca = level.GetPlayerAvatar();
            var go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            var b = (Building) go;
            var uuc = b.GetUnitUpgradeComponent();
            var unitLevel = ca.GetUnitUpgradeLevel(UnitData);
            if (uuc.CanStartUpgrading(UnitData))
            {
                var cost = UnitData.GetUpgradeCost(unitLevel);
                var rd = UnitData.GetUpgradeResource(unitLevel);
                if (ca.HasEnoughResources(rd, cost))
                {
                    ca.SetResourceCount(rd, ca.GetResourceCount(rd) - cost);
                    uuc.StartUpgrading(UnitData);
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        public int BuildingId { get; set; }
        public CombatItemData UnitData { get; set; }
        public uint Unknown1 { get; set; } //00 00 00 00

        //00 3D 09 00
        public uint Unknown2 { get; set; }

        #endregion Public Properties
    }
}