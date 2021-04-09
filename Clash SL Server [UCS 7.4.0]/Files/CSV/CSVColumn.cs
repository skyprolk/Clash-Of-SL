using System.Collections.Generic;

namespace CSS.Files.CSV
{
    internal class CSVColumn
    {
        readonly List<string> m_vValues;

        public CSVColumn()
        {
            m_vValues = new List<string>();
        }

        public static int GetArraySize(int currentOffset, int nextOffset) => nextOffset - currentOffset;

        public void Add(string value)
        {
            m_vValues.Add(value);
        }

        public string Get(int row) => m_vValues[row];

        public int GetSize() => m_vValues.Count;
    }
}
