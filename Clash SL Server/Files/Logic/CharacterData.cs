/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System.Collections.Generic;
using CSS.Core;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class CharacterData : CombatItemData
    {
        #region Public Constructors

        public CharacterData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public bool AirTargets { get; set; }

        public string Animation { get; set; }

        public bool AreaDamageIgnoresWalls { get; set; }

        public int AttackCount { get; set; }

        public string AttackEffect { get; set; }

        public string AttackEffectLv2 { get; set; }

        public string AttackEffectLv3 { get; set; }

        public string AttackEffectLv4 { get; set; }

        public bool AttackMultipleBuildings { get; set; }

        public int AttackRange { get; set; }

        public int AttackSpeed { get; set; }

        public int AutoMergeDistance { get; set; }

        public int AutoMergeGroupSize { get; set; }

        public int BarrackLevel { get; set; }

        public string BigPicture { get; set; }

        public string BigPictureSWF { get; set; }

        public string ChildTroop { get; set; }

        public int ChildTroop0_X { get; set; }

        public int ChildTroop0_Y { get; set; }

        public int ChildTroop1_X { get; set; }

        public int ChildTroop1_Y { get; set; }

        public int ChildTroop2_X { get; set; }

        public int ChildTroop2_Y { get; set; }

        public int ChildTroopCount { get; set; }

        public string CustomDefenderIcon { get; set; }

        public int DamageRadius { get; set; }

        public string DeployEffect { get; set; }

        public int DieDamage { get; set; }

        public int DieDamageDelay { get; set; }

        public string DieDamageEffect { get; set; }

        public int DieDamageRadius { get; set; }

        public string DieEffect { get; set; }

        public bool DisableProduction { get; set; }

        public int DPS { get; set; }

        public int DPSLv2 { get; set; }

        public int DPSLv3 { get; set; }

        public int EnemyGroupWeight { get; set; }

        public int FriendlyGroupWeight { get; set; }

        public bool GroundTargets { get; set; }

        public int HealthReductionPerSecond { get; set; }

        public string HitEffect { get; set; }

        public int HitEffectOffset { get; set; }

        public int Hitpoints { get; set; }

        public int HousingSpace { get; set; }

        public string IconExportName { get; set; }

        public string IconSWF { get; set; }

        public bool IncreasingDamage { get; set; }

        public string InfoTID { get; set; }

        public int InvisibilityRadius { get; set; }

        public bool IsFlying { get; set; }

        public bool IsJumper { get; set; }

        public bool IsSecondaryTroop { get; set; }

        public bool IsUnderground { get; set; }

        public List<int> LaboratoryLevel { get; set; }

        public int Lv2SwitchTime { get; set; }

        public int Lv3SwitchTime { get; set; }

        public int MovementOffsetAmount { get; set; }

        public int MovementOffsetSpeed { get; set; }

        public bool PickNewTargetAfterPushback { get; set; }

        public string PreferedTargetBuilding { get; set; }

        public string PreferedTargetBuildingClass { get; set; }

        public int PreferedTargetDamageMod { get; set; }

        public string Projectile { get; set; }

        public int ProjectileBounces { get; set; }

        public int PushbackSpeed { get; set; }

        public bool RandomizeSecSpawnDist { get; set; }

        public int SecondarySpawnDist { get; set; }

        public int SecondarySpawnOffset { get; set; }

        public string SecondaryTroop { get; set; }

        public int SecondaryTroopCnt { get; set; }

        public bool SelfAsAoeCenter { get; set; }

        public int SpawnIdle { get; set; }

        public int SpecialMovementMod { get; set; }

        public int Speed { get; set; }

        public int SpeedDecreasePerChildTroopLost { get; set; }

        public int StrengthWeight { get; set; }

        public int SummonCooldown { get; set; }

        public string SummonEffect { get; set; }

        public int SummonLimit { get; set; }

        public string SummonTroop { get; set; }

        public int SummonTroopCount { get; set; }

        public string SWF { get; set; }

        public int TargetedEffectOffset { get; set; }

        public string TID { get; set; }

        public string TombStone { get; set; }

        public List<int> TrainingCost { get; set; }

        public string TrainingResource { get; set; }

        public List<int> TrainingTime { get; set; }

        public string TransitionEffectLv2 { get; set; }

        public string TransitionEffectLv3 { get; set; }

        public string TransitionEffectLv4 { get; set; }

        public int UnitOfType { get; set; }

        public List<int> UpgradeCost { get; set; }

        public List<string> UpgradeResource { get; set; }

        public List<int> UpgradeTimeH { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override int GetCombatItemType()
        {
            return 0;
        }

        public override int GetHousingSpace()
        {
            return HousingSpace;
        }

        public override int GetRequiredLaboratoryLevel(int level)
        {
            return LaboratoryLevel[level];
        }

        public override int GetRequiredProductionHouseLevel()
        {
            return BarrackLevel;
        }

        public override int GetTrainingCost(int level)
        {
            return TrainingCost[level];
        }

        public override ResourceData GetTrainingResource()
        {
            return ObjectManager.DataTables.GetResourceByName(TrainingResource);
        }

        public override int GetTrainingTime(int level)
        {
            return TrainingTime[level];
        }

        public override int GetUpgradeCost(int level)
        {
            return UpgradeCost[level];
        }

        public override int GetUpgradeLevelCount()
        {
            return UpgradeCost.Count;
        }

        public override ResourceData GetUpgradeResource(int level)
        {
            return ObjectManager.DataTables.GetResourceByName(UpgradeResource[level]);
        }

        public override int GetUpgradeTime(int level)
        {
            return UpgradeTimeH[level] * 3600;
        }

        #endregion Public Methods
    }
}