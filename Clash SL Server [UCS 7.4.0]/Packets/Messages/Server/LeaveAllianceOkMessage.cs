using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24111
    internal class LeaveAllianceOkMessage : Message
    {
        public LeaveAllianceOkMessage(Device client, Alliance alliance) : base(client)
        {
            this.Identifier = 24111;

            this.ServerCommandType = 0x02;
            this.Alliance = alliance;
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.ServerCommandType);
            this.Data.AddLong(this.Alliance.m_vAllianceId);
            this.Data.AddInt(1); // 1 = leave, 2 = kick
            this.Data.AddInt(-1);
        }

        internal Alliance Alliance;
        internal int ServerCommandType;
    }
}