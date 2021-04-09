namespace CSS.Packets.Messages.Server
{
    // Packet 25892
    internal class DisconnectedMessage : Message
    {
		public DisconnectedMessage(Device client) : base(client)
		{
		    this.Identifier = 25892;
		}
	}
}
