namespace CSS.Logic
{
    internal class NpcLevel
    {
        int m_vType = 0x01036640;
        int Id => m_vType + Index;
        int Index { get; set; }
        int LootedElixir { get; set; }
        int LootedGold { get; set; }
        string Name { get; set; }
        int Stars { get; set; }

        public NpcLevel()
        {
        }

        public NpcLevel(int index)
        {
            Index        = index;
            Stars        = 0;
            LootedGold   = 0;
            LootedElixir = 0;
        }
    }
}