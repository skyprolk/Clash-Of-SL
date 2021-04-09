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

using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class CombatItemData : Data
    {
        #region Public Constructors

        public CombatItemData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public virtual int GetCombatItemType()
        {
            return -1;
        }

        public virtual int GetHousingSpace()
        {
            return -1;
        }

        public virtual int GetRequiredLaboratoryLevel(int level)
        {
            return -1;
        }

        public virtual int GetRequiredProductionHouseLevel()
        {
            return -1;
        }

        public virtual int GetTrainingCost(int level)
        {
            return -1;
        }

        public virtual ResourceData GetTrainingResource()
        {
            return null;
        }

        public virtual int GetTrainingTime(int level)
        {
            return -1;
        }

        public virtual int GetUpgradeCost(int level)
        {
            return -1;
        }

        public virtual int GetUpgradeLevelCount()
        {
            return -1;
        }

        public virtual ResourceData GetUpgradeResource(int level)
        {
            return null;
        }

        public virtual int GetUpgradeTime(int level)
        {
            return -1;
        }

        #endregion Public Methods
    }
}