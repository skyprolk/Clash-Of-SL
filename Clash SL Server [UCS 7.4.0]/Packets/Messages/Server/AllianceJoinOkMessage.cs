using System.Collections.Generic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24303
    internal class AllianceJoinOkMessage : Message
    {
        public AllianceJoinOkMessage(Device client) : base(client)
        {
            this.Identifier = 24303;
        }
    }
}