namespace CSS.Packets.Messages.Server
{
    // Packet 25006
    internal class AllianceWarAttackEventMessage : Message
    {
        public AllianceWarAttackEventMessage(Device client) : base(client)
        {
            this.Identifier = 25006;
        }
    }
}