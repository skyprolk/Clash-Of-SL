using System;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24301
    internal class AllianceDataMessage : Message
    {
        readonly Alliance m_vAlliance;

        public AllianceDataMessage(Device client, Alliance alliance) : base(client)
        {
            this.Identifier = 24301;
            m_vAlliance = alliance;
        }

        internal override async void Encode()
        {
            try
            {
                var allianceMembers = m_vAlliance.GetAllianceMembers();

                this.Data.AddRange(m_vAlliance.EncodeFullEntry());
                this.Data.AddString(m_vAlliance.m_vAllianceDescription);
                this.Data.AddInt(0);
                this.Data.Add(0);
                this.Data.AddInt(0);
                this.Data.Add(0);

                this.Data.AddInt(allianceMembers.Count);

                foreach (AllianceMemberEntry m in allianceMembers)
                {
                    this.Data.AddRange(await m.Encode());
                }

                this.Data.AddInt(0);
                this.Data.AddInt(32);
            }
            catch (Exception) { }
        }
    }
}
