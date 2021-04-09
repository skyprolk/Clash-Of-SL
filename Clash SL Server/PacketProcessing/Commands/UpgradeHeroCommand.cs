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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class UpgradeHeroCommand : Command
    {
        #region Public Constructors

        public UpgradeHeroCommand(CoCSharpPacketReader br)
        {
            BuildingId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var ca = level.GetPlayerAvatar();
            var go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var b = (Building) go;
                var hbc = b.GetHeroBaseComponent();
                if (hbc != null)
                {
                    if (hbc.CanStartUpgrading())
                    {
                        var hd = ObjectManager.DataTables.GetHeroByName(b.GetBuildingData().HeroType);
                        var currentLevel = ca.GetUnitUpgradeLevel(hd);
                        var rd = hd.GetUpgradeResource(currentLevel);
                        var cost = hd.GetUpgradeCost(currentLevel);
                        if (ca.HasEnoughResources(rd, cost))
                        {
                            if (level.HasFreeWorkers())
                            {
                                hbc.StartUpgrading();
                            }
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        public int BuildingId { get; set; }
        public uint Unknown1 { get; set; }

        #endregion Public Properties
    }
}