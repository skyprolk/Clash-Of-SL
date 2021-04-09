using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 24318
    internal class AllianceStreamEntryRemovedMessage : Message
    {
        public AllianceStreamEntryRemovedMessage(Device client, int i) : base(client)
        {
            this.Identifier = 24318;
            m_vId = i;
        }

        public int m_vId;

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(m_vId);
        }
    }
}
