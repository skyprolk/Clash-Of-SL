namespace CSS.Packets.Messages.Server
{  
    // Packet 25000
    internal class AllianceWarVisitDataMessage : Message
    {
        public AllianceWarVisitDataMessage(Device client) : base(client)
        {
            this.Identifier = 25000;
        }
    }
}