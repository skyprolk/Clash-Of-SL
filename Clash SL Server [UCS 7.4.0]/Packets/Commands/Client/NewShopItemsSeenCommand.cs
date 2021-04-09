using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 532
    internal class NewShopItemsSeenCommand : Command
    {
        public NewShopItemsSeenCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        public uint NewShopItemNumber;
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;
    }
}