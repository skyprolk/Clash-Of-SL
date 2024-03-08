using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class CombatItemData : Data
    {
        public CombatItemData(CSVRow row, DataTable dt) : base(row, dt)
        {
        }

        public virtual int GetCombatItemType() => -1;

        public virtual int GetHousingSpace() => -1;

        public virtual int GetRequiredLaboratoryLevel(int level) => -1;

        public virtual int GetRequiredProductionHouseLevel() => -1;

        public virtual int GetTrainingCost(int level) => -1;

        public virtual ResourceData GetTrainingResource() => null;

        public virtual int GetTrainingTime(int level) => -1;

        public virtual int GetUpgradeCost(int level) => -1;

        public virtual int GetUpgradeLevelCount() => -1;

        public virtual ResourceData GetUpgradeResource(int level) => null;

        public virtual int GetUpgradeTime(int level) => -1;
    }
}
