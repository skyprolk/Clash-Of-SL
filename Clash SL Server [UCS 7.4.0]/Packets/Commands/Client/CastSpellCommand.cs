using System.Collections.Generic;
using CSS.Files.Logic;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands
{
    // Packet 604
    internal class CastSpellCommand : Command
    {
        public CastSpellCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.Spell = (SpellData) this.Reader.ReadDataReference();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            List<DataSlot> _PlayerSpells = this.Device.Player.Avatar.GetSpells();

            DataSlot _DataSlot = _PlayerSpells.Find(t => t.Data.GetGlobalID() == Spell.GetGlobalID());
            if (_DataSlot != null)
            {
                _DataSlot.Value = _DataSlot.Value - 1;
            }
        }

        public SpellData Spell;
        public uint Unknown1;
        public int X;
        public int Y;
    }
}