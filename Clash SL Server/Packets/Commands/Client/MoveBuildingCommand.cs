using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 501
    internal class MoveBuildingCommand : Command
    {
        public MoveBuildingCommand(Reader reader, Device client, int id) : base(reader, client, id)
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
            GameObject go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            go.SetPositionXY(X, Y, this.Device.Player.Avatar.m_vActiveLayout);
        }

        public int BuildingId;
        public uint Unknown1;
        public int X;
        public int Y;
    }
}