using System.IO;
using UCS.Files.Logic;
using UCS.Helpers;

namespace UCS.Logic
{
    class UnitSlot
    {
        public int Count;
        public int Level;
        public CombatItemData UnitData;

        public UnitSlot(CombatItemData cd, int level, int count)
        {
            UnitData = cd;
            Level = level;
            Count = count;
        }

        public void Decode(PacketReader br)
        {
            UnitData = (CombatItemData) br.ReadDataReference();
            Level = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
        }
    }
}
