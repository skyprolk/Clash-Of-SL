using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 515
    internal class CancelUpgradeUnitCommand : Command
    {
        public CancelUpgradeUnitCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public uint BuildingId { get; set; }
        public uint Unknown1 { get; set; }
    }
}