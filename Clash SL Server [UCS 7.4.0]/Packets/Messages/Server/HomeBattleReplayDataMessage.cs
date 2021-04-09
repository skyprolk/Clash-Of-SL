namespace CSS.Packets.Messages.Server
{
    // Packet 24114
    internal class HomeBattleReplayDataMessage : Message
    {
        public HomeBattleReplayDataMessage(Device client) : base(client)
        {
            this.Identifier = 24114;
        }
    }
}