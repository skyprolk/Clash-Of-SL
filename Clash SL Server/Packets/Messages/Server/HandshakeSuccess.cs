using CSS.Core.Crypto;
using CSS.Helpers;
using CSS.Helpers.List;
using CSS.Logic.Enums;
using CSS.Packets.Messages.Client;
using CSS.Utilities.Blake2B;

namespace CSS.Packets.Messages.Server
{
    // Packet 20100
    internal class HandshakeSuccess : Message
    {

        public HandshakeSuccess(Device client, SessionRequest cka) : base(client)
        {
            this.Identifier = 20100;
            this.Device.PlayerState = State.SESSION_OK;
        }

        internal override void Encode()
        {
            this.Data.AddInt(24);
            this.Data.AddRange(Key.NonceKey);
        }
    }
}
