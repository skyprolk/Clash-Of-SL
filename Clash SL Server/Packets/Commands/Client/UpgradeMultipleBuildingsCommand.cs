using System.Collections.Generic;
using System.IO;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 549
    internal class UpgradeMultipleBuildingsCommand : Command
    {
        public UpgradeMultipleBuildingsCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vIsAltResource = this.Reader.ReadByte();
            this.m_vBuildingIdList = new List<int>();
            var buildingCount = this.Reader.ReadInt32();
            for (var i = 0; i < buildingCount; i++)
            {
                var buildingId = this.Reader.ReadInt32();
                this.m_vBuildingIdList.Add(buildingId);
            }
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;

            foreach (var buildingId in m_vBuildingIdList)
            {
                var b = (Building) this.Device.Player.GameObjectManager.GetGameObjectByID(buildingId);
                if (b.CanUpgrade())
                {
                    var bd = b.GetBuildingData();
                    var cost = bd.GetBuildCost(b.GetUpgradeLevel() + 1);
                    ResourceData rd = m_vIsAltResource == 0 ? bd.GetBuildResource(b.GetUpgradeLevel() + 1) : bd.GetAltBuildResource(b.GetUpgradeLevel() + 1);
                    if (ca.HasEnoughResources(rd, cost))
                    {
                        if (this.Device.Player.HasFreeWorkers())
                        {
                            string name = b.GetData().GetName();
                            Logger.Write("Building To Upgrade : " + name + " (" + buildingId + ')');
                            if (string.Equals(name, "Alliance Castle"))
                            {
                                ca.IncrementAllianceCastleLevel();
                                ca.SetAllianceCastleTotalCapacity(bd.GetUnitStorageCapacity(ca.GetAllianceCastleLevel()));
                            }
                            else if (string.Equals(name, "Town Hall"))
                                ca.IncrementTownHallLevel();
                            ca.SetResourceCount(rd, ca.GetResourceCount(rd) - cost);
                            b.StartUpgrading();
                        }
                    }
                }
            }
        }
        internal List<int> m_vBuildingIdList;
        internal byte m_vIsAltResource;
    }
}