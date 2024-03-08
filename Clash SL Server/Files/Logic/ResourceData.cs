using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class ResourceData : Data
    {
        public ResourceData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string CapFullTID { get; set; }
        public string CollectEffect { get; set; }
        public string HudInstanceName { get; set; }
        public bool PremiumCurrency { get; set; }
        public string ResourceIconExportName { get; set; }
        public string StealEffect { get; set; }
        public string SWF { get; set; }
        public int TextBlue { get; set; }
        public int TextGreen { get; set; }
        public int TextRed { get; set; }
        public string TID { get; set; }
        public string WarRefResource { get; set; }
    }
}
