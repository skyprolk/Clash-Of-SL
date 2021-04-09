using System.IO;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 510
    internal class BuyTrapCommand : Command
    {
        public BuyTrapCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.TrapId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;

            var td = (TrapData)CSVManager.DataTables.GetDataById(TrapId);
            var t = new Trap(td, this.Device.Player);

            if (ca.HasEnoughResources(td.GetBuildResource(0), td.GetBuildCost(0)))
            {
                if (this.Device.Player.HasFreeWorkers())
                {
                    var rd = td.GetBuildResource(0);
                    ca.CommodityCountChangeHelper(0, rd, -td.GetBuildCost(0));

                    t.StartConstructing(X, Y);
                    this.Device.Player.GameObjectManager.AddGameObject(t);
                }
            }
        }

        public int TrapId;
        public uint Unknown1;
        public int X;
        public int Y;
    }
}