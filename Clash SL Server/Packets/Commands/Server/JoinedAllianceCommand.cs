using CSS.Logic;

namespace  CSS.Packets.Commands.Server
{
    //Command 1
    internal class JoinedAllianceCommand : Command
    {
        private Alliance m_vAlliance;

        public JoinedAllianceCommand(Device client) : base(client)
        {
            this.Identifier = 1;
        }

        internal override void Encode()
        {
            this.Data.AddRange(m_vAlliance.EncodeHeader());
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

    }
}