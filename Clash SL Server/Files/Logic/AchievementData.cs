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
    internal class AchievementData : Data
    {
        #region Public Constructors

        public AchievementData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties
    }
}