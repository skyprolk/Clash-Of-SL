namespace CSS.Packets.Messages.Server
{
    // Packet 24317
    internal class AnswerJoinRequestAllianceMessage : Message
    {
        public AnswerJoinRequestAllianceMessage(Device client) : base(client)
        {
            this.Identifier = 24317;
        }
    }
}