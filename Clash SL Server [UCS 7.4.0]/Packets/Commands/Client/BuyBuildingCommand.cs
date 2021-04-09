using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 500
    internal class BuyBuildingCommand : Command
    {
        public BuyBuildingCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var bd = (BuildingData)CSVManager.DataTables.GetDataById(BuildingId);
            var b = new Building(bd, this.Device.Player);

            if (ca.HasEnoughResources(bd.GetBuildResource(0), bd.GetBuildCost(0)))
            {
                if (bd.IsWorkerBuilding() || this.Device.Player.HasFreeWorkers())
                {
                    var rd = bd.GetBuildResource(0);
                    ca.CommodityCountChangeHelper(0, rd, -bd.GetBuildCost(0));

                    b.StartConstructing(X, Y);
                    this.Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
        }

        public int BuildingId;
        public uint Unknown1;
        public int X;
        public int Y;
    }
}