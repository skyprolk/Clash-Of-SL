using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class NpcData : Data
    {
        public NpcData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public bool AlwaysUnlocked { get; set; }
        public int Elixir { get; set; }
        public int ExpLevel { get; set; }
        public int Gold { get; set; }
        public string LevelFile { get; set; }
        public string MapDependencies { get; set; }
        public string MapInstanceName { get; set; }
        public string TID { get; set; }
        public int UnitCount { get; set; }
        public string UnitType { get; set; }
    }
}
