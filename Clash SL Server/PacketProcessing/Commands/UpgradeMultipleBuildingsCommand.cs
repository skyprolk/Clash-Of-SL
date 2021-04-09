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
    internal class UpgradeMultipleBuildingsCommand : Command
    {
        #region Public Constructors

        public UpgradeMultipleBuildingsCommand(CoCSharpPacketReader br)
        {
            m_vIsAltResource = br.ReadByte();
            m_vBuildingIdList = new List<int>();
            var buildingCount = br.ReadInt32WithEndian();
            for (var i = 0; i < buildingCount; i++)
            {
                var buildingId = br.ReadInt32WithEndian();
                m_vBuildingIdList.Add(buildingId);
            }
            br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var ca = level.GetPlayerAvatar();

            foreach (var buildingId in m_vBuildingIdList)
            {
                var b = (Building) level.GameObjectManager.GetGameObjectByID(buildingId);
                if (b.CanUpgrade())
                {
                    var bd = b.GetBuildingData();
                    var cost = bd.GetBuildCost(b.GetUpgradeLevel() + 1);
                    ResourceData rd;
                    if (m_vIsAltResource == 0)
                        rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                    else
                        rd = bd.GetAltBuildResource(b.GetUpgradeLevel() + 1);
                    if (ca.HasEnoughResources(rd, cost))
                    {
                        if (level.HasFreeWorkers())
                        {
                            ca.SetResourceCount(rd, ca.GetResourceCount(rd) - cost);
                            b.StartUpgrading();
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Fields

        readonly List<int> m_vBuildingIdList;
        readonly byte m_vIsAltResource;

        #endregion Private Fields
    }
}