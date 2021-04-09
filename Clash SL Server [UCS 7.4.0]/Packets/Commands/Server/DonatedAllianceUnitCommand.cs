using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Commands.Server
{
    internal class DonatedAllianceUnitCommand : Command
    {
        public DonatedAllianceUnitCommand(Device client) : base(client)
        {
            this.Identifier = 4;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(MessageID);
            this.Data.AddInt(0);
            this.Data.AddInt(TroopID);
            this.Data.AddInt(4);
            this.Data.AddInt(0); // Tick
        }

        public int MessageID { get; set; }

        public int TroopID { get; set; }

        public void Tick(Level level) => level.Tick();
    }
}
