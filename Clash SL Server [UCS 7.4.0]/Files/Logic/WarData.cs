using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class WarData : Data
    {
        public WarData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public bool DisableProduction { get; set; }
        public string Name { get; set; }
        public int PreparationMinutes { get; set; }
        public int TeamSize { get; set; }
        public int WarMinutes { get; set; }
    }
}
