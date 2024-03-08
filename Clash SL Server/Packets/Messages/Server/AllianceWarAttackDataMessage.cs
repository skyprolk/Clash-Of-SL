namespace CSS.Packets.Messages.Server
{
    // Packet 25003
    internal class AllianceWarAttackDataMessage : Message
    {
        public AllianceWarAttackDataMessage(Device client) : base(client)
        {
            this.Identifier = 25003;
        }
    }
}