using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 24115
    internal class ServerErrorMessage : Message
    {
        internal string ErrorMessage;

        public ServerErrorMessage(Device client) : base(client)
        {
            this.Identifier = 24115;
        }

        internal override void Encode()
        {
            this.Data.AddString(this.ErrorMessage);
        }
    }
}
