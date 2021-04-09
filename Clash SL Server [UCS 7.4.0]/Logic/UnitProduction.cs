using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Files.Logic;

namespace UCS.Logic
{
    internal class UnitProduction
    {
        public UnitProduction(Level level, CombatItemData cd, bool IsSpellForge)
        {
            m_vUnits        = new List<DataSlot>();
            Unit            = cd;
            m_vTimer        = null;
            m_vLevel        = level;
            m_vIsSpellForge = IsSpellForge;
        }

        public void AddUnitToQueue(CombatItemData cd, int count)
        {
            for (int i = 0; i < m_vUnits.Count; i++)
            {
                if ((CombatItemData)m_vUnits[i].Data == cd)
                {
                    if (count != 1)
                    {
                        m_vUnits[i].Value += count;
                        return;
                    }
                    else
                    {
                        m_vUnits[i].Value++;
                        return;
                    }
                }
            }

            DataSlot ds = new DataSlot(cd, count);
            m_vUnits.Add(ds);

            if (m_vTimer == null)
            {
                m_vTimer         = new Timer();
                int trainingTime = cd.GetTrainingTime(m_vLevel.GetPlayerAvatar().GetUnitUpgradeLevel(cd));
                m_vTimer.StartTimer(trainingTime, m_vLevel.GetTime());
            }
        }

        public int GetSlotCount() => m_vUnits.Count;

        public int GetTrainingCount(int index) => m_vUnits[index].Value;

        public CombatItemData GetUnit(int index) => (CombatItemData)m_vUnits[index].Data;

        public int GetTotalCount()
        {
            int count = 0;
            if (GetSlotCount() >= 1)
            {
                for (int  i = 0; i < GetSlotCount(); i++)
                {
                    int cnt = m_vUnits[i].Value;
                    int housingSpace = ((CombatItemData)m_vUnits[i].Data).GetHousingSpace();
                    count += cnt * housingSpace;
                }
            }
            if (m_vIsSpellForge)
            {
                count += m_vLevel.GetComponentManager().GetTotalUsedHousing(true);
            }
            return count;
        }

        /*public int GetMaxTrainCount()
        {
            return 0;
        }*/

        //public bool CanAddUnitToQueue(CombatItemData cd) => GetMaxTrainCount() >= GetTotalCount() + cd.GetHousingSpace();

        /*public override void Load(JObject jsonObject)
        {
            JObject unitProdObject = (JObject)jsonObject["unit_prod"];
            m_vIsSpellForge = unitProdObject["unit_type"].ToObject<int>() == 1;
            JToken timeToken = unitProdObject["t"];
            if (timeToken != null)
            {
                m_vTimer = new Timer();
                int remainingTime = timeToken.ToObject<int>();
                m_vTimer.StartTimer(remainingTime, m_vLevel.GetTime());
            }
            JArray unitJsonArray = (JArray)unitProdObject["slots"];
            if (unitJsonArray != null)
            {
                foreach (JObject unitJsonObject in unitJsonArray)
                {
                    int id = unitJsonObject["id"].ToObject<int>();
                    int cnt = unitJsonObject["cnt"].ToObject<int>();
                    m_vUnits.Add(new DataSlot(CSVManager.DataTables.GetDataById(id), cnt));
                }
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            JObject unitProdObject = new JObject();
            if (m_vIsSpellForge)
                unitProdObject.Add("unit_type", 1);
            else
                unitProdObject.Add("unit_type", 0);

            if (m_vTimer != null)
            {
                unitProdObject.Add("t", m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()));
            }

            if (GetSlotCount() >= 1)
            {
                var unitJsonArray = new JArray();
                foreach (var unit in m_vUnits)
                {
                    var unitJsonObject = new JObject();
                    unitJsonObject.Add("id", unit.Data.GetGlobalID());
                    unitJsonObject.Add("cnt", unit.Value);
                    unitJsonArray.Add(unitJsonObject);
                }
                unitProdObject.Add("slots", unitJsonArray);
            }
            jsonObject.Add("unit_prod", unitProdObject);
            return jsonObject;
        }*/

        Timer m_vTimer;

        readonly List<DataSlot> m_vUnits;

        Level m_vLevel;

        bool m_vIsSpellForge;

        public CombatItemData Unit { get; set; }
    }
}
