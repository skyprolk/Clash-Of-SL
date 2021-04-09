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
    internal class SpellData : CombatItemData
    {
        #region Public Constructors

        public SpellData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        public override int GetCombatItemType()
        {
            return 1;
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
            return SpellForgeLevel;
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