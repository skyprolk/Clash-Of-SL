namespace CSS.Files.CSV
{
    internal class CSVRow
    {
        public CSVRow(CSVTable table)
        {
            m_vCSVTable = table;
            m_vRowStart = m_vCSVTable.GetColumnRowCount();
            m_vCSVTable.AddRow(this);
        }

        readonly CSVTable m_vCSVTable;
        readonly int m_vRowStart;

        public int GetArraySize(string name)
        {
            var columnIndex = m_vCSVTable.GetColumnIndexByName(name);
            var result = 0;
            if (columnIndex != -1)
                result = m_vCSVTable.GetArraySizeAt(this, columnIndex);
            return result;
        }

        public string GetName() => m_vCSVTable.GetValueAt(0, m_vRowStart);

        public int GetRowOffset() => m_vRowStart;

        public string GetValue(string name, int level) => m_vCSVTable.GetValue(name, level + m_vRowStart);
    }
}

