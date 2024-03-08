using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class AchievementData : Data
    {
        public AchievementData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Action { get; set; }
        public int ActionCount { get; set; }
        public string ActionData { get; set; }
        public string AndroidID { get; set; }
        public string CompletedTID { get; set; }
        public int DiamondReward { get; set; }
        public int ExpReward { get; set; }
        public string IconExportName { get; set; }
        public string IconSWF { get; set; }
        public string InfoTID { get; set; }
        public int Level { get; set; }
        public bool ShowValue { get; set; }
        public string TID { get; set; }
    }
}
