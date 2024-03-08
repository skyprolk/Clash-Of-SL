using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 24340
    internal class PromoteAllianceMemberOkMessage : Message
    {
        public PromoteAllianceMemberOkMessage(Device client) : base(client)
        {
            this.Identifier = 24306;
        }

        internal override void Encode()
        {
            this.Data.AddLong(this.Id);
            this.Data.AddInt(this.Role);
        }

        internal long Id;
        internal int Role;
    }
}