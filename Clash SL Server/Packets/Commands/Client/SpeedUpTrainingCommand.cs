using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 513
    internal class SpeedUpTrainingCommand : Command
    {
        readonly int m_vBuildingId;

        public SpeedUpTrainingCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadUInt32();
        }
    }
}