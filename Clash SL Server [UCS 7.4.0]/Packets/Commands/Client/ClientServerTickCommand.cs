using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 553
    internal class ClientServerTickCommand : Command
    {
        public int Tick;
        public int Unknown1;

        public ClientServerTickCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Unknown1 = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }
    }
}