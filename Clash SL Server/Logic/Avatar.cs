using System;
using System.Collections.Generic;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class Avatar
    {
        public Avatar()
        {
            m_vResources         = new List<DataSlot>();
            m_vResourceCaps      = new List<DataSlot>();
            m_vUnitCount         = new List<DataSlot>();
            m_vUnitUpgradeLevel  = new List<DataSlot>();
            m_vHeroHealth        = new List<DataSlot>();
            m_vHeroUpgradeLevel  = new List<DataSlot>();
            m_vHeroState         = new List<DataSlot>();
            m_vSpellCount        = new List<DataSlot>();
            m_vSpellUpgradeLevel = new List<DataSlot>();
        }

        protected List<DataSlot> m_vHeroHealth;
        protected List<DataSlot> m_vHeroState;
        protected List<DataSlot> m_vHeroUpgradeLevel;
        protected List<DataSlot> m_vResourceCaps;
        protected List<DataSlot> m_vResources;
        protected List<DataSlot> m_vSpellCount;
        protected List<DataSlot> m_vSpellUpgradeLevel;
        protected List<DataSlot> m_vUnitCount;
        protected List<DataSlot> m_vUnitUpgradeLevel;

        int m_vCastleLevel = -1;
        int m_vCastleTotalCapacity;
        int m_vCastleUsedCapacity;
        internal int m_vTownHallLevel;

        public static int GetDataIndex(List<DataSlot> dsl, Data d) => dsl.FindIndex(ds => ds.Data == d);

        public void CommodityCountChangeHelper(int commodityType, Data data, int count)
        {
            if (data.GetDataType() == 2)
            {
                if (commodityType == 0)
                {
                    int resourceCount = GetResourceCount((ResourceData) data);
                    int newResourceValue = Math.Max(resourceCount + count, 0);
                    if (count >= 1)
                    {
                        int resourceCap = GetResourceCap((ResourceData) data);
                        if (resourceCount < resourceCap)
                        {
                            if (newResourceValue > resourceCap)
                            {
                                newResourceValue = resourceCap;
                            }
                        }
                    }
                    SetResourceCount((ResourceData) data, newResourceValue);
                }
            }
        }

        public int GetAllianceCastleLevel() => m_vCastleLevel;

        public int GetAllianceCastleTotalCapacity() => m_vCastleTotalCapacity;

        public int GetAllianceCastleUsedCapacity() => m_vCastleUsedCapacity;

        public int GetResourceCap(ResourceData rd)
        {
            int index = GetDataIndex(m_vResourceCaps, rd);
            int count = 0;
            if (index != -1)
            {
                count = m_vResourceCaps[index].Value;
            }
            return count;
        }

        public List<DataSlot> GetResourceCaps() => m_vResourceCaps;

        public int GetResourceCount(ResourceData rd)
        {
            int index = GetDataIndex(m_vResources, rd);
            int count = 0;
            if (index != -1)
            {
                count = m_vResources[index].Value;
            }
            return count;
        }

        public List<DataSlot> GetResources() => m_vResources;

        public List<DataSlot> GetSpells() => m_vSpellCount;

        public int GetUnitCount(CombatItemData cd)
        {
            int result = 0;
            if (cd.GetCombatItemType() == 1)
            {
                int index = GetDataIndex(m_vSpellCount, cd);
                if (index != -1)
                {
                    result = m_vSpellCount[index].Value;
                }
            }
            else
            {
                int index = GetDataIndex(m_vUnitCount, cd);
                if (index != -1)
                {
                    result = m_vUnitCount[index].Value;
                }
            }
            return result;
        }

        public List<DataSlot> GetUnits() => m_vUnitCount;

        public int GetUnitUpgradeLevel(CombatItemData cd)
        {
            int result = 0;
            switch (cd.GetCombatItemType())
            {
                case 2:
                    {
                        int index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                        {
                            result = m_vHeroUpgradeLevel[index].Value;
                        }
                        break;
                    }
                case 1:
                    {
                        int index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                        {
                            result = m_vSpellUpgradeLevel[index].Value;
                        }
                        break;
                    }

                default:
                    {
                        int index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                        {
                            result = m_vUnitUpgradeLevel[index].Value;
                        }
                        break;
                    }
            }
            return result;
        }

        public int GetUnusedResourceCap(ResourceData rd)
        {
            int resourceCount = GetResourceCount(rd);
            int resourceCap   = GetResourceCap(rd);
            return Math.Max(resourceCap - resourceCount, 0);
        }

        public void SetAllianceCastleLevel(int level)
        {
            m_vCastleLevel = level;
        }

        public void IncrementAllianceCastleLevel()
        {
            m_vCastleLevel++;
        }
        public void DeIncrementAllianceCastleLevel()
        {
            m_vCastleLevel--;
        }

        public void SetTownHallLevel(int level)
        {
            m_vTownHallLevel = level;
        }

        public void IncrementTownHallLevel()
        {
            m_vTownHallLevel++;
        }

        public void DeIncrementTownHallLevel()
        {
            m_vTownHallLevel--;
        }

        public void SetAllianceCastleTotalCapacity(int totalCapacity)
        {
            m_vCastleTotalCapacity = totalCapacity;
        }

        public void SetAllianceCastleUsedCapacity(int usedCapacity)
        {
            m_vCastleUsedCapacity = usedCapacity;
        }

        public void SetHeroHealth(HeroData hd, int health)
        {
            int index = GetDataIndex(m_vHeroHealth, hd);
            if (index == -1)
            {
                DataSlot ds = new DataSlot(hd, health);
                m_vHeroHealth.Add(ds);
            }
            else
            {
                m_vHeroHealth[index].Value = health;
            }
        }

        public void SetHeroState(HeroData hd, int state)
        {
            int index = GetDataIndex(m_vHeroState, hd);
            if (index == -1)
            {
                DataSlot ds = new DataSlot(hd, state);
                m_vHeroState.Add(ds);
            }
            else
            {
                m_vHeroState[index].Value = state;
            }
        }

        public void SetResourceCap(ResourceData rd, int value)
        {
            int index = GetDataIndex(m_vResourceCaps, rd);
            if (index == -1)
            {
                DataSlot ds = new DataSlot(rd, value);
                m_vResourceCaps.Add(ds);
            }
            else
            {
                m_vResourceCaps[index].Value = value;
            }
        }

        public void SetResourceCount(ResourceData rd, int value)
        {
            int index = GetDataIndex(m_vResources, rd);
            if (index == -1)
            {
                DataSlot ds = new DataSlot(rd, value);
                m_vResources.Add(ds);
            }
            else
            {
                m_vResources[index].Value = value;
            }
        }

        public void SetUnitCount(CombatItemData cd, int count)
        {
            switch (cd.GetCombatItemType())
            {
                case 1:
                    {
                        int index = GetDataIndex(m_vSpellCount, cd);
                        if (index != -1)
                        {
                            m_vSpellCount[index].Value = count;
                        }
                        else
                        {
                            DataSlot ds = new DataSlot(cd, count);
                            m_vSpellCount.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        int index = GetDataIndex(m_vUnitCount, cd);
                        if (index != -1)
                        {
                            m_vUnitCount[index].Value = count;
                        }
                        else
                        {
                            DataSlot ds = new DataSlot(cd, count);
                            m_vUnitCount.Add(ds);
                        }
                        break;
                    }
            }
        }

        public void SetUnitUpgradeLevel(CombatItemData cd, int level)
        {
            switch (cd.GetCombatItemType())
            {
                case 2:
                    {
                        int index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                        {
                            m_vHeroUpgradeLevel[index].Value = level;
                        }
                        else
                        {
                            DataSlot ds = new DataSlot(cd, level);
                            m_vHeroUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                case 1:
                    {
                        int index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                        {
                            m_vSpellUpgradeLevel[index].Value = level;
                        }
                        else
                        {
                            DataSlot ds = new DataSlot(cd, level);
                            m_vSpellUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        int index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                        {
                            m_vUnitUpgradeLevel[index].Value = level;
                        }
                        else
                        {
                            DataSlot ds = new DataSlot(cd, level);
                            m_vUnitUpgradeLevel.Add(ds);
                        }
                        break;
                    }
            }
        }
    }
}
