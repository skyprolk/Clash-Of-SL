using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 24104
    internal class OutOfSyncMessage : Message
    {
        public OutOfSyncMessage(Device client) : base(client)
        {
            this.Identifier = 24104;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
        }
    }
}
