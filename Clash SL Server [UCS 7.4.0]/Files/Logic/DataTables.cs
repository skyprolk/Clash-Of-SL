using System.Collections.Generic;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class DataTables
    {
        readonly List<DataTable> m_vDataTables;

        public DataTables()
        {
            m_vDataTables = new List<DataTable>();
            for (var i = 0; i < 41; i++)
                m_vDataTables.Add(new DataTable());
        }

        public CharacterData GetCharacterByName(string name)
        {
            var dt = m_vDataTables[3];
            return (CharacterData) dt.GetDataByName(name);
        }

        public Data GetDataById(int id)
        {
            var classId = GlobalID.GetClassID(id) - 1;
            var dt = m_vDataTables[classId];
            return dt.GetItemById(id);
        }

        public Globals GetGlobals() => (Globals)m_vDataTables[13];

        public HeroData GetHeroByName(string name)
        {
            var dt = m_vDataTables[27];
            return (HeroData) dt.GetDataByName(name);
        }

        public ResourceData GetResourceByName(string name)
        {
            var dt = m_vDataTables[2];
            return (ResourceData) dt.GetDataByName(name);
        }

        public DataTable GetTable(int i) => m_vDataTables[i];

        public void InitDataTable(CSVTable t, int index)
        {
            if (index == 13)
                m_vDataTables[index] = new Globals(t, index);
            else
                m_vDataTables[index] = new DataTable(t, index);
        }
    }
}
