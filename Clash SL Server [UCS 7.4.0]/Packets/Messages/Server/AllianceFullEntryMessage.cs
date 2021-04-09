using System.Collections.Generic;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24324
    internal class AllianceFullEntryMessage : Message
    {
        readonly Alliance m_vAlliance;

        public AllianceFullEntryMessage(Device client, Alliance alliance) : base(client)
        {
            this.Identifier = 24324;
            this.m_vAlliance = alliance;
        }

        internal override void Encode()
        {
            this.Data.AddString(m_vAlliance.m_vAllianceDescription);
            this.Data.AddInt(0); //War state:
            // 0 - Not started
            // 1 - Search enemy (old war type)
            // 2 - Search enemy (new war type)
            // 3 - Unknown
            // 4 - Preparation day
            // 5 - Battle day
            // 6 - War end 
            this.Data.AddInt(0); //Unknown

            this.Data.Add(0); //0 if no war
            //pack.AddLong(WarID);
            this.Data.Add(0);
            this.Data.AddInt(0);
            this.Data.AddRange(m_vAlliance.EncodeFullEntry());
        }
    }
}
