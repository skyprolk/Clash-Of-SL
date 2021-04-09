using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Commands.Server
{
    //Command 2
    internal class LeavedAllianceCommand : Command
    {
        private Alliance m_vAlliance;
        private int m_vReason;

        public LeavedAllianceCommand(Device client) : base(client)
        {
            this.Identifier = 2;
        }

        internal override void Encode()
        {
            this.Data.AddLong(m_vAlliance.m_vAllianceId);
            this.Data.AddInt(m_vReason);
            this.Data.AddInt(-1); //Tick Probably
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

        public void SetReason(int reason)
        {
            m_vReason = reason;
        }

        //00 00 07 3A
        //00 00 00 01 ////reason? 1= leave, 2=kick

        //00 00 00 3B 00 0A 40 1E
    }
}
