/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class UnitProductionComponent : Component
    {
        #region Public Constructors

        public UnitProductionComponent(GameObject go) : base(go)
        {
            m_vUnits = new List<DataSlot>();
            SetUnitType(go);
            m_vTimer = null;
            m_vIsWaitingForSpace = false;
        }

        #endregion Public Constructors

        #region Public Properties

        public override int Type => 3;

        #endregion Public Properties

        #region Private Fields

        readonly List<DataSlot> m_vUnits;
        bool m_vIsSpellForge;
        bool m_vIsWaitingForSpace;
        Timer m_vTimer;

        #endregion Private Fields

        #region Public Methods

        public void AddUnitToProductionQueue(CombatItemData cd)
        {
            if (CanAddUnitToQueue(cd))
            {
                for (var i = 0; i < GetSlotCount(); i++)
                {
                    if ((CombatItemData) m_vUnits[i].Data == cd)
                    {
                        m_vUnits[i].Value++;
                        return;
                    }
                }
                var ds = new DataSlot(cd, 1);
                m_vUnits.Add(ds);
                if (m_vTimer == null)
                {
                    var ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                    m_vTimer = new Timer();
                    var trainingTime = cd.GetTrainingTime(ca.GetUnitUpgradeLevel(cd));
                    m_vTimer.StartTimer(trainingTime, GetParent().GetLevel().GetTime());
                }
            }
        }

        public bool CanAddUnitToQueue(CombatItemData cd) => GetMaxTrainCount() >= GetTotalCount() + cd.GetHousingSpace();

        public CombatItemData GetCurrentlyTrainedUnit()
        {
            CombatItemData cd = null;
            if (m_vUnits.Count >= 1)
                cd = (CombatItemData) m_vUnits[0].Data;
            return cd;
        }

        public int GetMaxTrainCount()
        {
            var b = (Building) GetParent();
            var bd = b.GetBuildingData();
            return bd.GetUnitProduction(b.GetUpgradeLevel());
        }

        public int GetSlotCount() => m_vUnits.Count;

        public int GetTotalCount()
        {
            var count = 0;
            if (GetSlotCount() >= 1)
            {
                for (var i = 0; i < GetSlotCount(); i++)
                {
                    var cnt = m_vUnits[i].Value;
                    var housingSpace = ((CombatItemData) m_vUnits[i].Data).GetHousingSpace();
                    count += cnt * housingSpace;
                }
            }
            if (m_vIsSpellForge)
            {
                count += GetParent().GetLevel().GetComponentManager().GetTotalUsedHousing(true);
            }
            return count;
        }

        public int GetTotalRemainingSeconds()
        {
            var result = 0;
            var firstUnit = true;
            if (m_vUnits.Count > 0)
            {
                foreach (var ds in m_vUnits)
                {
                    var cd = (CombatItemData) ds.Data;
                    if (cd != null)
                    {
                        var count = ds.Value;
                        if (count >= 1)
                        {
                            if (firstUnit)
                            {
                                if (m_vTimer != null)
                                    result += m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime());
                                count--;
                                firstUnit = false;
                            }
                            var ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                            result += count * cd.GetTrainingTime(ca.GetUnitUpgradeLevel(cd));
                        }
                    }
                }
            }
            return result;
        }

        public int GetTrainingCount(int index) => m_vUnits[index].Value;

        public CombatItemData GetUnit(int index) => (CombatItemData)m_vUnits[index].Data;

        public bool HasHousingSpaceForSpeedUp()
        {
            var totalRoom = 0;
            if (m_vUnits.Count >= 1)
            {
                foreach (var ds in m_vUnits)
                {
                    var cd = (CombatItemData) ds.Data;
                    totalRoom += cd.GetHousingSpace() * ds.Value;
                }
            }
            var cm = GetParent().GetLevel().GetComponentManager();
            var usedHousing = cm.GetTotalUsedHousing(m_vIsSpellForge);
            var maxHousing = cm.GetTotalMaxHousing(m_vIsSpellForge);
            return totalRoom <= maxHousing - usedHousing;
        }

        public bool IsSpellForge() => m_vIsSpellForge;

        public bool IsWaitingForSpace()
        {
            var result = false;
            if (m_vUnits.Count > 0)
            {
                if (m_vTimer != null)
                {
                    if (m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()) == 0)
                    {
                        result = m_vIsWaitingForSpace;
                    }
                }
            }
            return result;
        }

        public override void Load(JObject jsonObject)
        {
            var unitProdObject = (JObject) jsonObject["unit_prod"];
            m_vIsSpellForge = unitProdObject["unit_type"].ToObject<int>() == 1;
            var timeToken = unitProdObject["t"];
            if (timeToken != null)
            {
                m_vTimer = new Timer();
                var remainingTime = timeToken.ToObject<int>();
                m_vTimer.StartTimer(remainingTime, GetParent().GetLevel().GetTime());
            }
            var unitJsonArray = (JArray) unitProdObject["slots"];
            if (unitJsonArray != null)
            {
                foreach (JObject unitJsonObject in unitJsonArray)
                {
                    var id = unitJsonObject["id"].ToObject<int>();
                    var cnt = unitJsonObject["cnt"].ToObject<int>();
                    m_vUnits.Add(new DataSlot(ObjectManager.DataTables.GetDataById(id), cnt));
                }
            }
        }

        public bool ProductionCompleted()
        {
            var result = false;
            var cf = new ComponentFilter(0);
            var x = GetParent().X;
            var y = GetParent().Y;
            var cm = GetParent().GetLevel().GetComponentManager();
            var c = cm.GetClosestComponent(x, y, cf);

            while (c != null)
            {
                Data d = null;
                if (m_vUnits.Count > 0)
                    d = m_vUnits[0].Data;
                if (!((UnitStorageComponent) c).CanAddUnit((CombatItemData) d))
                {
                    cf.AddIgnoreObject(c.GetParent());
                    c = cm.GetClosestComponent(x, y, cf);
                }
                else
                    break;
            }

            if (c != null)
            {
                var cd = (CombatItemData) m_vUnits[0].Data;
                ((UnitStorageComponent) c).AddUnit(cd);
                StartProducingNextUnit();
                result = true;
            }
            else
            {
                m_vIsWaitingForSpace = true;
            }
            return result;
        }

        public void RemoveUnit(CombatItemData cd)
        {
            var index = -1;
            if (GetSlotCount() >= 1)
            {
                for (var i = 0; i < GetSlotCount(); i++)
                {
                    if (m_vUnits[i].Data == cd)
                        index = i;
                }
            }
            if (index != -1)
            {
                if (m_vUnits[index].Value >= 1)
                {
                    m_vUnits[index].Value--;
                    if (m_vUnits[index].Value == 0)
                    {
                        m_vUnits.RemoveAt(index);
                        if (GetSlotCount() >= 1)
                        {
                            var ds = m_vUnits[0];
                            var newcd = (CombatItemData) m_vUnits[0].Data;
                            var ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                            m_vTimer = new Timer();
                            var trainingTime = newcd.GetTrainingTime(ca.GetUnitUpgradeLevel(newcd));
                            m_vTimer.StartTimer(trainingTime, GetParent().GetLevel().GetTime());
                        }
                    }
                }
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            var unitProdObject = new JObject();
            if (m_vIsSpellForge)
                unitProdObject.Add("unit_type", 1);
            else
                unitProdObject.Add("unit_type", 0);

            if (m_vTimer != null)
            {
                unitProdObject.Add("t", m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()));
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
        }

        public void SetUnitType(GameObject go)
        {
            var b = (Building) GetParent();
            var bd = b.GetBuildingData();
            m_vIsSpellForge = bd.IsSpellForge();
        }

        public void SpeedUp()
        {
            while (m_vUnits.Count >= 1 && ProductionCompleted())
            {
            }
        }

        public void StartProducingNextUnit()
        {
            m_vTimer = null;
            if (GetSlotCount() >= 1)
            {
                RemoveUnit((CombatItemData) m_vUnits[0].Data);
            }
        }

        public override void Tick()
        {
            if (m_vTimer != null)
            {
                if (m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()) <= 0)
                {
                    ProductionCompleted();
                }
            }
        }

        #endregion Public Methods
    }
}
