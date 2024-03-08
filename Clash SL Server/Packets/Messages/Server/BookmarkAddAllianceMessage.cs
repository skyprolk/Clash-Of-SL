namespace CSS.Packets.Messages.Server
{
    // Packet 24343
    internal class BookmarkAddAllianceMessage : Message
    {
        public BookmarkAddAllianceMessage(Device client) : base(client)
        {
            this.Identifier = 24343;
        }

        internal override void Encode()
        {
            this.Data.Add(1);
        }
    }
}