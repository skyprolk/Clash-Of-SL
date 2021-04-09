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

using System.Collections.Generic;
using System.IO;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class BoostBuildingCommand : Command
    {
        #region Public Constructors

        public BoostBuildingCommand(CoCSharpPacketReader br)
        {
            BuildingIds = new List<int>();
            BoostedBuildingsCount = br.ReadInt32WithEndian();
            for (var i = 0; i < BoostedBuildingsCount; i++)
            {
                BuildingIds.Add(br.ReadInt32WithEndian());
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var ca = level.GetPlayerAvatar();
            foreach (var buildingId in BuildingIds)
            {
                var go = level.GameObjectManager.GetGameObjectByID(buildingId);

                var b = (ConstructionItem) go;
                var costs = ((BuildingData) b.GetConstructionItemData()).BoostCost[b.UpgradeLevel];
                if (ca.HasEnoughDiamonds(costs))
                {
                    b.BoostBuilding();
                    ca.SetDiamonds(ca.GetDiamonds() - costs);
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        public int BoostedBuildingsCount { get; set; }
        public List<int> BuildingIds { get; set; }

        #endregion Public Properties
    }
}