namespace CSS.Packets.Messages.Server
{
    // Packet 24308
    internal class AllianceMemberMessage : Message
    {
        public AllianceMemberMessage(Device client) : base(client)
        {
            this.Identifier = 24308;
        }
    }
}