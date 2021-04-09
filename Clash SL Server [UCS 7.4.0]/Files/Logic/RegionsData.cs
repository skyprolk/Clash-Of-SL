using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class RegionsData : Data
    {
        public RegionsData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string DisplayName { get; set; }
        public bool IsCountry { get; set; }
        public string Name { get; set; }
        public string TID { get; set; }
    }
}
