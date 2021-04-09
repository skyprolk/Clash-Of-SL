using CSS.Logic.StreamEntry;

namespace CSS.Packets.Messages.Server
{
    // Packet 24312
    internal class AllianceStreamEntryMessage : Message
    {
        internal StreamEntry StreamEntry;

        public AllianceStreamEntryMessage(Device client) : base(client)
        {
            this.Identifier = 24312;
        }

        internal override void Encode()
        {
            this.Data.AddRange(StreamEntry.Encode());
        }
    }
}