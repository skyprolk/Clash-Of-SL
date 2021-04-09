using System.Collections.Generic;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class LeagueData : Data
    {
        public LeagueData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

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
    }
}
