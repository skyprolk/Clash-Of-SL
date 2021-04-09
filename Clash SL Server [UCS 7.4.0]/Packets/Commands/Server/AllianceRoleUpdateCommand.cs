using CSS.Helpers.List;
using CSS.Logic;

namespace  CSS.Packets.Commands.Server
{
    internal class AllianceRoleUpdateCommand : Command
    {
        public Alliance m_vAlliance;

        public AllianceRoleUpdateCommand(Device client) : base(client)
        {
            this.Identifier = 8;
        }

        internal override void Encode()
        {
            
            this.Data.AddLong(m_vAlliance.m_vAllianceId);
            this.Data.AddInt(Role);
            this.Data.AddInt(Role);
            this.Data.AddInt(0);
        }

        public int Role { get; set; }

        public void SetAlliance(Alliance a)
        {
            m_vAlliance = a;
        }

        public void SetRole(int role)
        {
            Role = role;
        }

        public void Tick(Level level) => level.Tick();
    }
}
