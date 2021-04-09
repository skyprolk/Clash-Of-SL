using System.Collections.Generic;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24304
    internal class JoinableAllianceListMessage : Message
    {
        internal List<Alliance> Alliances;

        public JoinableAllianceListMessage(Device client) : base(client)
        {
            this.Identifier = 24304;
            this.Alliances = new List<Alliance>();
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.Alliances.Count);

            foreach (Alliance alliance in this.Alliances)
            {
                if (alliance != null)
                {
                    this.Data.AddRange(alliance.EncodeFullEntry());
                }
            }
        }
    }
}