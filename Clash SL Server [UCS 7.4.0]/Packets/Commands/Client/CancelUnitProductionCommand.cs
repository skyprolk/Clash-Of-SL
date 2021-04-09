using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers.Binary;
using CSS.Logic;
using System.Collections.Generic;

namespace CSS.Packets.Commands.Client
{
    // Packet 509
    internal class CancelUnitProductionCommand : Command
    {
        public CancelUnitProductionCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.UnitType = this.Reader.ReadInt32();
            this.Count = this.Reader.ReadInt32();

        }

        internal override void Process()
        {
            List<DataSlot> _PlayerUnits = this.Device.Player.Avatar.GetUnits();
            List<DataSlot> _PlayerSpells = this.Device.Player.Avatar.GetSpells();
            if (UnitType.ToString().StartsWith("400"))
            {
                CombatItemData _Troop = (CombatItemData)CSVManager.DataTables.GetDataById(UnitType); ;
                DataSlot _DataSlot = _PlayerUnits.Find(t => t.Data.GetGlobalID() == _Troop.GetGlobalID());
                if (_DataSlot != null)
                {
                    _DataSlot.Value = _DataSlot.Value - Count;
                }
            }
            else if (UnitType.ToString().StartsWith("260"))
            {
                SpellData _Spell = (SpellData)CSVManager.DataTables.GetDataById(UnitType); ;
                DataSlot _DataSlot = _PlayerSpells.Find(t => t.Data.GetGlobalID() == _Spell.GetGlobalID());
                if (_DataSlot != null)
                {
                    _DataSlot.Value = _DataSlot.Value - Count;
                }
            }
        }

        public int BuildingId;
        public int Count;
        public int UnitType;
    }
}