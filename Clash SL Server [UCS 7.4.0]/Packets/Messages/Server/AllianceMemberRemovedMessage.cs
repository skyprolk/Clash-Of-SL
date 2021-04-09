namespace CSS.Packets.Messages.Server
{
    // Packet 24309
    internal class AllianceMemberRemovedMessage : Message
    {
        public AllianceMemberRemovedMessage(Device client) : base(client)
        {
            this.Identifier = 24309;
        }
    }
}