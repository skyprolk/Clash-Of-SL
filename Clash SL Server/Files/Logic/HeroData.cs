using System.Collections.Generic;
using CSS.Core;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class HeroData : CombatItemData
    {
        public HeroData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string AbilityAffectsCharacter { get; set; }

        public bool AbilityAffectsHero { get; set; }

        public string AbilityBigPictureExportName { get; set; }

        public int AbilityCooldown { get; set; }

        public int AbilityDamageBoostOffset { get; set; }

        public int AbilityDamageBoostPercent { get; set; }

        public int AbilityDelay { get; set; }

        public string AbilityDescTID { get; set; }

        public int AbilityHealthIncrease { get; set; }

        public string AbilityIcon { get; set; }

        public bool AbilityOnce { get; set; }

        public int AbilityRadius { get; set; }

        public int AbilityShieldProjectileDamageMod { get; set; }

        public int AbilityShieldProjectileSpeed { get; set; }

        public int AbilitySpeedBoost { get; set; }

        public int AbilitySpeedBoost2 { get; set; }

        public string AbilitySpell { get; set; }

        public int AbilitySpellLevel { get; set; }

        public bool AbilityStealth { get; set; }

        public string AbilitySummonTroop { get; set; }

        public int AbilitySummonTroopCount { get; set; }

        public string AbilityTID { get; set; }

        public int AbilityTime { get; set; }

        public string AbilityTriggerEffect { get; set; }

        public bool AirTargets { get; set; }

        public int AlertRadius { get; set; }

        public string AltModeAnimation { get; set; }

        public bool AltModeFlying { get; set; }

        public string Animation { get; set; }

        public int AttackCount { get; set; }

        public string AttackEffect { get; set; }

        public string AttackEffectAlt { get; set; }

        public string AttackEffectShared { get; set; }

        public int AttackRange { get; set; }

        public int AttackSpeed { get; set; }

        public string AuraBigPictureExportName { get; set; }

        public string AuraDescTID { get; set; }

        public string AuraSpell { get; set; }

        public int AuraSpellLevel { get; set; }

        public string AuraTID { get; set; }

        public string BigPicture { get; set; }

        public string BigPictureSWF { get; set; }

        public string CelebrateEffect { get; set; }

        public int CoolDownOverride { get; set; }

        public int DamageRadius { get; set; }

        public string DeployEffect { get; set; }

        public string DieEffect { get; set; }

        public int DPS { get; set; }

        public int EnemyGroupWeight { get; set; }

        public bool FightWithGroups { get; set; }

        public int FriendlyGroupWeight { get; set; }

        public bool GroundTargets { get; set; }

        public bool HasAltMode { get; set; }

        public string HitEffect { get; set; }

        public List<int> Hitpoints { get; set; }

        public int HousingSpace { get; set; }

        public string IconExportName { get; set; }

        public string IconSWF { get; set; }

        public string InfoTID { get; set; }

        public bool IsFlying { get; set; }

        public int MaxSearchRadiusForDefender { get; set; }

        public bool NoAttackOverWalls { get; set; }

        public int PatrolRadius { get; set; }

        public string PreferedTargetBuilding { get; set; }

        public string PreferedTargetBuildingClass { get; set; }

        public int PreferedTargetDamageMod { get; set; }

        public string Projectile { get; set; }

        public string RageProjectile { get; set; }

        public int RegenerationTimeMinutes { get; set; }

        public List<int> RequiredTownHallLevel { get; set; }

        public string RetributionSpell { get; set; }

        public int RetributionSpellLevel { get; set; }

        public int RetributionSpellTriggerHealth { get; set; }

        public int Scale { get; set; }

        public int SleepOffsetX { get; set; }

        public int SleepOffsetY { get; set; }

        public string SmallPicture { get; set; }

        public string SmallPictureSWF { get; set; }

        public bool SmoothJump { get; set; }

        public string SpecialAbilityEffect { get; set; }

        public int Speed { get; set; }

        public int StrengthWeight { get; set; }

        public int StrengthWeight2 { get; set; }

        public string SWF { get; set; }

        public int TargetedEffectOffset { get; set; }

        public bool TargetGroups { get; set; }

        public int TargetGroupsMinWeight { get; set; }

        public int TargetGroupsRadius { get; set; }

        public int TargetGroupsRange { get; set; }

        public string TID { get; set; }

        public int TrainingCost { get; set; }

        public string TrainingResource { get; set; }

        public int TrainingTime { get; set; }

        public List<int> UpgradeCost { get; set; }

        public List<string> UpgradeResource { get; set; }

        public List<int> UpgradeTimeH { get; set; }

        public int WakeUpSpace { get; set; }

        public int WakeUpSpeed { get; set; }

        public override int GetCombatItemType() => 2;

        public int GetRequiredTownHallLevel(int level) => RequiredTownHallLevel[level];

        public override int GetUpgradeCost(int level) => UpgradeCost[level];

        public override int GetUpgradeLevelCount() => UpgradeCost.Count;

        public override ResourceData GetUpgradeResource(int level) => CSVManager.DataTables.GetResourceByName(UpgradeResource[level]);

        public override int GetUpgradeTime(int level) => UpgradeTimeH[level] * 3600;
    }
}
