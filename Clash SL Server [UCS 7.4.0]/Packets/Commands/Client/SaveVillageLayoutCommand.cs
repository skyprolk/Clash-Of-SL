using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 552
    internal class SaveVillageLayoutCommand : Command
    {
        public SaveVillageLayoutCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.Read();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }
    }
}
