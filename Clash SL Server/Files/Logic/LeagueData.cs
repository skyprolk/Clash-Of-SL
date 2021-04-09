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
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class LeagueData : Data
    {
        #region Public Constructors

        public LeagueData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> BucketPlacementHardLimit { get; set; }
        public List<int> BucketPlacementRangeHigh { get; set; }
        public List<int> BucketPlacementRangeLow { get; set; }
        public List<int> BucketPlacementSoftLimit { get; set; }
        public int DarkElixirReward { get; set; }
        public bool DemoteEnabled { get; set; }
        public int DemoteLimit { get; set; }
        public int ElixirReward { get; set; }
        public int GoldReward { get; set; }
        public string IconExportName { get; set; }
        public string IconSWF { get; set; }
        public bool IgnoredByServer { get; set; }
        public string LeagueBannerIcon { get; set; }
        public string LeagueBannerIconNum { get; set; }
        public int PlacementLimitHigh { get; set; }
        public int PlacementLimitLow { get; set; }
        public bool PromoteEnabled { get; set; }
        public int PromoteLimit { get; set; }
        public string TID { get; set; }
        public string TIDShort { get; set; }

        #endregion Public Properties
    }
}