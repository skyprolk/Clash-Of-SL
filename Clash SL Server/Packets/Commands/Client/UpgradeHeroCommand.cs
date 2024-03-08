using CSS.Core;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 527
    internal class UpgradeHeroCommand : Command
    {
        public UpgradeHeroCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var b = (Building) go;
                var hbc = b.GetHeroBaseComponent();
                if (hbc != null)
                {
                    if (hbc.CanStartUpgrading())
                    {
                        var hd = CSVManager.DataTables.GetHeroByName(b.GetBuildingData().HeroType);
                        var currentLevel = ca.GetUnitUpgradeLevel(hd);
                        var rd = hd.GetUpgradeResource(currentLevel);
                        var cost = hd.GetUpgradeCost(currentLevel);
                        if (ca.HasEnoughResources(rd, cost))
                        {
                            if (this.Device.Player.HasFreeWorkers())
                            {
                                Logger.Write("Hero To Upgrade : " + b.GetData().GetName() + " (" + BuildingId + ')');
                                hbc.StartUpgrading();
                            }
                        }
                    }
                }
            }
        }

        public int BuildingId;
        public uint Unknown1;
    }
}