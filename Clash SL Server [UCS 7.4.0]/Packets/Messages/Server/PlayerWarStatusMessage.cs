using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    internal class PlayerWarStatusMessage : Message
    {
        public PlayerWarStatusMessage(Device client) : base(client)
        {
            this.Identifier = 24111;
        }

        internal int Status;

        internal override void Encode()
        {
            this.Data.AddInt(14);
            this.Data.AddInt(Status);
            this.Data.AddInt(0);
        }
    }
}