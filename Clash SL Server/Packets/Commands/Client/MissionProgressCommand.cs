using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 519
    internal class MissionProgressCommand : Command
    {
        public MissionProgressCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.MissionID = this.Reader.ReadUInt32();
            this.Tick = this.Reader.ReadUInt32();
        }

        public uint MissionID;

        public uint Tick;

        internal override void Process()
        {
        }
    }
}