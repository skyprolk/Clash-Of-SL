using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 20113
    internal class SetDeviceTokenMessage : Message
    {
        public SetDeviceTokenMessage(Device client) : base(client)
        {
            this.Identifier = 20113;
        }

        internal override void Encode()
        {
            this.Data.AddString(this.Device.Player.Avatar.UserToken);
        }
    }
}