using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class ExperienceLevelData : Data
    {
        public ExperienceLevelData(CSVRow row, DataTable dt): base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int ExpPoints { get; set; }
    }
}
