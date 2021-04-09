using System.IO;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Helpers.Binary;

namespace CSS.Logic
{
    internal class UnitSlot
    {
        public int Count;
        public int Level;
        public CombatItemData UnitData;

        public UnitSlot(CombatItemData cd, int level, int count)
        {
            UnitData = cd;
            Level    = level;
            Count    = count;
        }

        public void Decode(Reader br)
        {
            UnitData = (CombatItemData) br.ReadDataReference();
            Level    = br.ReadInt32();
            Count    = br.ReadInt32();
        }
    }
}
