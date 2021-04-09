using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24715
    internal class GlobalChatLineMessage : Message
    {
        public GlobalChatLineMessage(Device client) : base(client)
        {
            this.Identifier = 24715;

            this.Message = "default";
            this.PlayerName = "default";
            this.HomeId = 1;
            this.CurrentHomeId = 1;
            this.PlayerLevel = 1;
            this.HasAlliance = false;
        }

        internal int PlayerLevel;
        internal int AllianceIcon;
        internal int LeagueId;

        internal long AllianceId;
        internal long CurrentHomeId;
        internal bool HasAlliance;
        internal long HomeId;

        internal string AllianceName;
        internal string Message;
        internal string PlayerName;

        internal override void Encode()
        {
            this.Data.AddString(this.Message);
            this.Data.AddString(this.PlayerName);
            this.Data.AddInt(this.PlayerLevel);
            this.Data.AddInt(this.LeagueId);
            this.Data.AddLong(this.HomeId);
            this.Data.AddLong(this.CurrentHomeId);

            if (this.HasAlliance)
            {
                this.Data.Add(1);
                this.Data.AddLong(this.AllianceId);
                this.Data.AddString(this.AllianceName);
                this.Data.AddInt(this.AllianceIcon);
            }
            else
            {
                this.Data.Add(0);
            }
        }

        internal void SetAlliance(Alliance alliance)
        {
            if(alliance?.m_vAllianceId > 0)
            {
                this.HasAlliance = true;
                this.AllianceId = alliance.m_vAllianceId;
                this.AllianceName = alliance.m_vAllianceName;
                this.AllianceIcon = alliance.m_vAllianceBadgeData;
            }
        }
    }
}