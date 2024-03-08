using System;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24338
    internal class AllianceWarHistoryMessage : Message
    {
        readonly Alliance m_vHomeAlliance;
        public AllianceWarHistoryMessage(Device client, Alliance home) : base(client)
        {
            this.Identifier = 24338;
            m_vHomeAlliance = home;
        }

        internal override void Encode()
        {
            this.Data.AddInt(1);

            this.Data.AddLong(m_vHomeAlliance.m_vAllianceId); // 1 Alliance ID
            this.Data.AddString(m_vHomeAlliance.m_vAllianceName); // 1 Alliance Name
            this.Data.AddInt(m_vHomeAlliance.m_vAllianceBadgeData); // 1 Alliance Badge
            this.Data.AddInt(m_vHomeAlliance.m_vAllianceLevel); // 1 Alliance Level

            this.Data.AddLong(9999); // 2 Alliance ID
            this.Data.AddString("CSS"); // 2 Alliance Name
            this.Data.AddInt(0); // 2 Alliance Badge
            this.Data.AddInt(1); // 2 Alliance Level

            this.Data.AddInt(9999); // 1 Stars
            this.Data.AddInt(0); // 2 Stars

            this.Data.AddInt(0); // 1 Village Destroyed
            this.Data.AddInt(100); // 2 Village Destroyed

            this.Data.AddInt(0); // 1 Unknown
            this.Data.AddInt(0); // 2 Unknown

            this.Data.AddInt(100); // Attack Used
            this.Data.AddInt(4000); // XP Earned

            this.Data.AddLong(515631654); // War ID
            this.Data.AddLong(40); // Members Count

            this.Data.AddInt(1); // War Won Count

            this.Data.Add(99);
            this.Data.AddInt((int) TimeSpan.FromDays(1).TotalSeconds);
            this.Data.AddLong((int) (TimeSpan.FromDays(1).TotalSeconds - TimeSpan.FromDays(0.5).TotalSeconds));
        }
    }
}
