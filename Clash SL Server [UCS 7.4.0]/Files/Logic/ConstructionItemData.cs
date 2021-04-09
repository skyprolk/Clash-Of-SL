using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class ConstructionItemData : Data
    {
        public ConstructionItemData(CSVRow row, DataTable dt) : base(row, dt)
        {
        }

        public virtual int GetBuildCost(int level) => -1;

        public virtual ResourceData GetBuildResource(int level) => null;

        public virtual int GetConstructionTime(int level) => -1;

        public virtual int GetRequiredTownHallLevel(int level) => -1;

        public virtual int GetUpgradeLevelCount() => -1;

        public virtual bool IsTownHall() => false;
    }
}
