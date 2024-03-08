using System;
using System.Collections.Generic;
using CSS.Files.Logic;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 526
    internal class BoostBuildingCommand : Command
    {
        public BoostBuildingCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
            this.BuildingIds = new List<int>();
        }

        internal override void Decode()
        {
            this.BoostedBuildingsCount = this.Reader.ReadInt32();
            for (int i = 0; i < BoostedBuildingsCount; i++)
            {
                this.BuildingIds.Add(this.Reader.ReadInt32());
            }
        }

        internal override void Process()
        {
            ClientAvatar ca = this.Device.Player.Avatar;

            foreach(int buildingId in BuildingIds)
            {
                GameObject go = this.Device.Player.GameObjectManager.GetGameObjectByID(buildingId);
                ConstructionItem b = (ConstructionItem)go;
                int costs = ((BuildingData)b.GetConstructionItemData()).BoostCost[b.UpgradeLevel];
                if (ca.HasEnoughDiamonds(costs))
                {
                    b.BoostBuilding();
                    ca.m_vCurrentGems = ca.m_vCurrentGems - costs;
                }
            }
        }

        public int BoostedBuildingsCount { get; set; }
        public List<int> BuildingIds { get; set; }
    }
}