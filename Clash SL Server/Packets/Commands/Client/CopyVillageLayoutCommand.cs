using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 568
    internal class CopyVillageLayoutCommand : Command
    {
        public CopyVillageLayoutCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public int PasteLayoutID;

        public int CopiedLayoutID;

        public int Tick;

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadInt32();
            this.CopiedLayoutID = this.Reader.ReadInt32();
            this.PasteLayoutID = this.Reader.ReadInt32();
        }
    }
}