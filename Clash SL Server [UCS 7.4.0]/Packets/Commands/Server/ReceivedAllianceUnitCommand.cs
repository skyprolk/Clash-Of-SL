using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Commands.Server
{
    internal class ReceivedAllianceUnitCommand : Command
    {
        public ReceivedAllianceUnitCommand(Device client) : base(client)
        {
            this.Identifier = 5;
        }

        internal override void Encode()
        {
            this.Data.AddString(Donator_Name);
            this.Data.AddInt(0);
            this.Data.AddInt(TroopID);
            this.Data.AddInt(Troop_Level);
            this.Data.AddInt(1);
            this.Data.AddInt(0);
        }

        public string Donator_Name { get; set; }

        public int TroopID { get; set; }

        public int Troop_Level { get; set; }
    }
}
