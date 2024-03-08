using System.Collections.Generic;
using CSS.Core;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class SpellData : CombatItemData
    {
        public SpellData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string BigPicture { get; set; }

        public int BoostTimeMS { get; set; }

        public int BuildingDamageBoostPercent { get; set; }

        public string ChargingEffect { get; set; }

        public int ChargingTimeMS { get; set; }

        public int Damage { get; set; }

        public int DamageBoostPercent { get; set; }

        public string DeployEffect { get; set; }

        public string DeployEffect2 { get; set; }

        public int DeployEffect2Delay { get; set; }

        public int DeployTimeMS { get; set; }

        public bool DisableProduction { get; set; }

        public int FreezeTimeMS { get; set; }

        public string HitEffect { get; set; }

        public int HitTimeMS { get; set; }

        public int HousingSpace { get; set; }

        public string IconExportName { get; set; }

        public string IconSWF { get; set; }

        public string InfoTID { get; set; }

        public int JumpBoostMS { get; set; }

        public int JumpHousingLimit { get; set; }

        public List<int> LaboratoryLevel { get; set; }

        public int NumberOfHits { get; set; }

        public int NumObstacles { get; set; }

        public string PreDeployEffect { get; set; }

        public int Radius { get; set; }

        public int RandomRadius { get; set; }

        public bool RandomRadiusAffectsOnlyGfx { get; set; }

        public string SpawnObstacle { get; set; }

        public int SpeedBoost { get; set; }

        public int SpeedBoost2 { get; set; }

        public int SpellForgeLevel { get; set; }

        public int StrengthWeight { get; set; }

        public string TID { get; set; }

        public int TimeBetweenHitsMS { get; set; }

        public List<int> TrainingCost { get; set; }

        public string TrainingResource { get; set; }

        public List<int> TrainingTime { get; set; }

        public List<int> UpgradeCost { get; set; }

        public List<string> UpgradeResource { get; set; }

        public List<int> UpgradeTimeH { get; set; }

        public override int GetCombatItemType() => 1;

        public override int GetHousingSpace()  => HousingSpace;

        public override int GetRequiredLaboratoryLevel(int level) => LaboratoryLevel[level];

        public override int GetRequiredProductionHouseLevel() => SpellForgeLevel;

        public override int GetTrainingCost(int level) => TrainingCost[level];

        public override ResourceData GetTrainingResource()    => CSVManager.DataTables.GetResourceByName(TrainingResource);

        public override int GetTrainingTime(int level) => TrainingTime[level];

        public override int GetUpgradeCost(int level) => UpgradeCost[level];

        public override int GetUpgradeLevelCount() => UpgradeCost.Count;

        public override ResourceData GetUpgradeResource(int level) => CSVManager.DataTables.GetResourceByName(UpgradeResource[level]);

        public override int GetUpgradeTime(int level) => UpgradeTimeH[level] * 3600;
    }
}
