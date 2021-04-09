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
    internal class ConstructionItemData : Data
    {
        #region Public Constructors

        public ConstructionItemData(CSVRow row, DataTable dt) : base(row, dt)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public virtual int GetBuildCost(int level) => -1;

        public virtual ResourceData GetBuildResource(int level) => null;

        public virtual int GetConstructionTime(int level) => -1;

        public virtual int GetRequiredTownHallLevel(int level) => -1;

        public virtual int GetUpgradeLevelCount() => -1;

        public virtual bool IsTownHall() => false;

        #endregion Public Methods
    }
}
