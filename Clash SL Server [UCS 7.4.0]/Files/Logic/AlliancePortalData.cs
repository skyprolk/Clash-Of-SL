using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class AlliancePortalData : Data
    {
        public AlliancePortalData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string ExportName { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string SWF { get; set; }
        public string TID { get; set; }
        public int Width { get; set; }
    }
}
