using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;

namespace CSS.Logic
{
    internal class UnitUpgradeComponent : Component
    {
        public UnitUpgradeComponent(GameObject go) : base(go)
        {
            m_vTimer = null;
            m_vCurrentlyUpgradedUnit = null;
        }

        public override int Type => 9;

        CombatItemData m_vCurrentlyUpgradedUnit;
        Timer m_vTimer;

        public bool CanStartUpgrading(CombatItemData cid)
        {
            var result = false;
            if (m_vCurrentlyUpgradedUnit == null)
            {
                var b = (Building) GetParent();
                var ca = GetParent().Avatar.Avatar;
                var cm = GetParent().Avatar.GetComponentManager();
                var  maxProductionBuildingLevel = cid.GetCombatItemType() == 1 ? cm.GetMaxSpellForgeLevel() : cm.GetMaxBarrackLevel();
                if (ca.GetUnitUpgradeLevel(cid) < cid.GetUpgradeLevelCount() - 1)
                {
                    if (maxProductionBuildingLevel >= cid.GetRequiredProductionHouseLevel() - 1)
                    {
                        result = b.GetUpgradeLevel() >=
                                 cid.GetRequiredLaboratoryLevel(ca.GetUnitUpgradeLevel(cid) + 1) - 1;
                    }
                }
            }
            return result;
        }

        public void FinishUpgrading()
        {
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var ca = GetParent().Avatar.Avatar;
                var level = ca.GetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit);
                ca.SetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit, level + 1);
            }
            m_vTimer = null;
            m_vCurrentlyUpgradedUnit = null;
        }

        public CombatItemData GetCurrentlyUpgradedUnit() => m_vCurrentlyUpgradedUnit;

        public int GetRemainingSeconds()
        {
            var result = 0;
            if (m_vTimer != null)
            {
                result = m_vTimer.GetRemainingSeconds(GetParent().Avatar.Avatar.LastTickSaved);
            }
            return result;
        }

        public int GetTotalSeconds()
        {
            var result = 0;
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var ca = GetParent().Avatar.Avatar;
                var level = ca.GetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit);
                result = m_vCurrentlyUpgradedUnit.GetUpgradeTime(level);
            }
            return result;
        }

        public override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject) jsonObject["unit_upg"];
            if (unitUpgradeObject != null)
            {
                m_vTimer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                m_vTimer.StartTimer(remainingTime, GetParent().Avatar.Avatar.LastTickSaved);

                var id = unitUpgradeObject["id"].ToObject<int>();
                m_vCurrentlyUpgradedUnit = (CombatItemData)CSVManager.DataTables.GetDataById(id);
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var unitUpgradeObject = new JObject();

                unitUpgradeObject.Add("unit_type", m_vCurrentlyUpgradedUnit.GetCombatItemType());
                unitUpgradeObject.Add("t", m_vTimer.GetRemainingSeconds(GetParent().Avatar.Avatar.LastTickSaved));
                unitUpgradeObject.Add("id", m_vCurrentlyUpgradedUnit.GetGlobalID());
                jsonObject.Add("unit_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        public void SpeedUp()
        {
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var remainingSeconds = 0;
                if (m_vTimer != null)
                {
                    remainingSeconds = m_vTimer.GetRemainingSeconds(GetParent().Avatar.Avatar.LastTickSaved);
                }
                var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
                var ca = GetParent().Avatar.Avatar;
                if (ca.HasEnoughDiamonds(cost))
                {
                    ca.UseDiamonds(cost);
                    FinishUpgrading();
                }
            }
        }

        public void StartUpgrading(CombatItemData cid)
        {
            if (CanStartUpgrading(cid))
            {
                m_vCurrentlyUpgradedUnit = cid;
                m_vTimer = new Timer();
                m_vTimer.StartTimer(GetTotalSeconds(), GetParent().Avatar.Avatar.LastTickSaved);
            }
        }

        public override void Tick()
        {
            if (m_vTimer?.GetRemainingSeconds(GetParent().Avatar.Avatar.LastTickSaved) <= 0)
            {
                FinishUpgrading();
            }
        }
    }
}
