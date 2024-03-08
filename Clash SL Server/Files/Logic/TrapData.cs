using System.Collections.Generic;
using CSS.Core;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class TrapData : ConstructionItemData
    {
        public TrapData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int ActionFrame { get; set; }

        public bool AirTrigger { get; set; }

        public string AppearEffect { get; set; }

        public string BigPicture { get; set; }

        public string BigPictureSWF { get; set; }

        public List<int> BuildCost { get; set; }

        public List<string> BuildResource { get; set; }

        public List<int> BuildTimeD { get; set; }

        public List<int> BuildTimeH { get; set; }

        public List<int> BuildTimeM { get; set; }

        public List<int> Damage { get; set; }

        public string DamageEffect { get; set; }

        public int DamageMod { get; set; }

        public int DamageRadius { get; set; }

        public int DurationMS { get; set; }

        public string Effect { get; set; }

        public string Effect2 { get; set; }

        public string EffectBroken { get; set; }

        public int EjectHousingLimit { get; set; }

        public bool EjectVictims { get; set; }

        public string ExportName { get; set; }

        public string ExportNameBroken { get; set; }

        public string ExportNameBuildAnim { get; set; }

        public string ExportNameTriggered { get; set; }

        public bool GroundTrigger { get; set; }

        public int Height { get; set; }

        public int HitCnt { get; set; }

        public int HitDelayMS { get; set; }

        public string InfoTID { get; set; }

        public int MinTriggerHousingLimit { get; set; }

        public bool Passable { get; set; }

        public string PickUpEffect { get; set; }

        public string PlacingEffect { get; set; }

        public string PreferredTarget { get; set; }

        public int PreferredTargetDamageMod { get; set; }

        public string Projectile { get; set; }

        public List<int> RearmCost { get; set; }

        public int SpeedMod { get; set; }

        public string Spell { get; set; }

        public int StrengthWeight { get; set; }

        public string SWF { get; set; }

        public string TID { get; set; }

        public List<int> TownHallLevel { get; set; }

        public int TriggerRadius { get; set; }

        public int Width { get; set; }

        public override int GetBuildCost(int level) => BuildCost[level];

        public override ResourceData GetBuildResource(int level) => CSVManager.DataTables.GetResourceByName(BuildResource[level]);

        public override int GetConstructionTime(int level) => BuildTimeM[level] * 60 + BuildTimeH[level] * 60 * 60 + BuildTimeD[level] * 60 * 60 * 24;

        public override int GetRequiredTownHallLevel(int level) => TownHallLevel[level] - 1;

        public int GetSellPrice(int level)
        {
            var calculation = (int) (((long) BuildCost[level] * 2 * 1717986919) >> 32);
            return (calculation >> 2) + (calculation >> 31);
        }

        public override int GetUpgradeLevelCount() => BuildCost.Count;
    }
}
