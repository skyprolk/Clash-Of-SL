namespace CSS.Packets.Messages.Server
{
    // Packet 24344
    internal class BookmarkRemoveAllianceMessage : Message
    {
        public BookmarkRemoveAllianceMessage(Device client) : base(client)
        {
            this.Identifier = 24344;
        }

        internal override void Encode()
        {
            this.Data.Add(1);
        }
    }
}