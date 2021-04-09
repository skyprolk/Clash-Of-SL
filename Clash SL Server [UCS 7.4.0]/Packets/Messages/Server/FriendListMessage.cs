using System.Collections.Generic;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 20105
    internal class FriendListMessage : Message
    {
        public FriendListMessage(Device client) : base(client)
        {
            this.Identifier = 20105;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddDataSlots(new List<DataSlot>());
        }
    }
}