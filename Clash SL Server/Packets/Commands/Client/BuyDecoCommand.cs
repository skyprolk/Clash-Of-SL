using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 512
    internal class BuyDecoCommand : Command
    {
        public BuyDecoCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.DecoId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            ClientAvatar ca = this.Device.Player.Avatar;

            DecoData dd = (DecoData)CSVManager.DataTables.GetDataById(DecoId);

            if (ca.HasEnoughResources(dd.GetBuildResource(), dd.GetBuildCost()))
            {
                ResourceData rd = dd.GetBuildResource();
                ca.CommodityCountChangeHelper(0, rd, -dd.GetBuildCost());

                Deco d = new Deco(dd, this.Device.Player);
                d.SetPositionXY(X, Y, this.Device.Player.Avatar.m_vActiveLayout);
                this.Device.Player.GameObjectManager.AddGameObject(d);
            }
        }

        public int DecoId;
        public uint Unknown1;
        public int X;
        public int Y;
    }
}