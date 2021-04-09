using System.IO;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.Packets.Commands.Client
{
    // Packet ?
    internal class DonateAllianceUnitCommand : Command
    {
        public DonateAllianceUnitCommand()
        {
        }

        public override void Execute(Level level)
        {
        }

        public int _MessageID { get; set; }

        public CombatItemData _Unit { get; set; }

        public void Tick(Level level) => level.Tick();

        public void SetMessageID(int id) => _MessageID = id;

        public void SetUnit(CombatItemData cd) => _Unit = cd;
    }
}
