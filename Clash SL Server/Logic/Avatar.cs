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

using System;
using System.Collections.Generic;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class Avatar
    {
        #region Public Constructors

        public Avatar()
        {
            m_vResources = new List<DataSlot>();
            m_vResourceCaps = new List<DataSlot>();
            m_vUnitCount = new List<DataSlot>();
            m_vUnitUpgradeLevel = new List<DataSlot>();
            m_vHeroHealth = new List<DataSlot>();
            m_vHeroUpgradeLevel = new List<DataSlot>();
            m_vHeroState = new List<DataSlot>();
            m_vSpellCount = new List<DataSlot>();
            m_vSpellUpgradeLevel = new List<DataSlot>();
        }

        #endregion Public Constructors

        #region Protected Fields

        protected List<DataSlot> m_vHeroHealth;
        protected List<DataSlot> m_vHeroState;
        protected List<DataSlot> m_vHeroUpgradeLevel;
        protected List<DataSlot> m_vResourceCaps;
        protected List<DataSlot> m_vResources;
        protected List<DataSlot> m_vSpellCount;
        protected List<DataSlot> m_vSpellUpgradeLevel;
        protected List<DataSlot> m_vUnitCount;
        protected List<DataSlot> m_vUnitUpgradeLevel;

        #endregion Protected Fields

        #region Private Fields

        int m_vCastleLevel;
        int m_vCastleTotalCapacity;
        int m_vCastleUsedCapacity;
        int m_vTownHallLevel;

        #endregion Private Fields

        #region Public Methods

        public static int GetDataIndex(List<DataSlot> dsl, Data d) => dsl.FindIndex(ds => ds.Data == d);

        public void CommodityCountChangeHelper(int commodityType, Data data, int count)
        {
            if (data.GetDataType() == 2)
            {
                if (commodityType == 0)
                {
                    var resourceCount = GetResourceCount((ResourceData) data);
                    var newResourceValue = Math.Max(resourceCount + count, 0);
                    if (count >= 1)
                    {
                        var resourceCap = GetResourceCap((ResourceData) data);
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
            var index = GetDataIndex(m_vResourceCaps, rd);
            var count = 0;
            if (index != -1)
                count = m_vResourceCaps[index].Value;
            return count;
        }

        public List<DataSlot> GetResourceCaps() => m_vResourceCaps;

        public int GetResourceCount(ResourceData rd)
        {
            var index = GetDataIndex(m_vResources, rd);
            var count = 0;
            if (index != -1)
                count = m_vResources[index].Value;
            return count;
        }

        public List<DataSlot> GetResources() => m_vResources;

        public List<DataSlot> GetSpells() => m_vSpellCount;

        public int GetTownHallLevel() => m_vTownHallLevel;

        public int GetUnitCount(CombatItemData cd)
        {
            var result = 0;
            if (cd.GetCombatItemType() == 1)
            {
                var index = GetDataIndex(m_vSpellCount, cd);
                if (index != -1)
                    result = m_vSpellCount[index].Value;
            }
            else
            {
                var index = GetDataIndex(m_vUnitCount, cd);
                if (index != -1)
                    result = m_vUnitCount[index].Value;
            }
            return result;
        }

        public List<DataSlot> GetUnits() => m_vUnitCount;

        public int GetUnitUpgradeLevel(CombatItemData cd)
        {
            var result = 0;
            switch (cd.GetCombatItemType())
            {
                case 2:
                    {
                        var index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vHeroUpgradeLevel[index].Value;
                        break;
                    }
                case 1:
                    {
                        var index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vSpellUpgradeLevel[index].Value;
                        break;
                    }

                default:
                    {
                        var index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vUnitUpgradeLevel[index].Value;
                        break;
                    }
            }
            return result;
        }

        public int GetUnusedResourceCap(ResourceData rd)
        {
            var resourceCount = GetResourceCount(rd);
            var resourceCap = GetResourceCap(rd);
            return Math.Max(resourceCap - resourceCount, 0);
        }

        public void SetAllianceCastleLevel(int level)
        {
            m_vCastleLevel = level;
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
            var index = GetDataIndex(m_vHeroHealth, hd);
            if (index == -1)
            {
                var ds = new DataSlot(hd, health);
                m_vHeroHealth.Add(ds);
            }
            else
            {
                m_vHeroHealth[index].Value = health;
            }
        }

        public void SetHeroState(HeroData hd, int state)
        {
            var index = GetDataIndex(m_vHeroState, hd);
            if (index == -1)
            {
                var ds = new DataSlot(hd, state);
                m_vHeroState.Add(ds);
            }
            else
            {
                m_vHeroState[index].Value = state;
            }
        }

        public void SetResourceCap(ResourceData rd, int value)
        {
            var index = GetDataIndex(m_vResourceCaps, rd);
            if (index == -1)
            {
                var ds = new DataSlot(rd, value);
                m_vResourceCaps.Add(ds);
            }
            else
            {
                m_vResourceCaps[index].Value = value;
            }
        }

        public void SetResourceCount(ResourceData rd, int value)
        {
            var index = GetDataIndex(m_vResources, rd);
            if (index == -1)
            {
                var ds = new DataSlot(rd, value);
                m_vResources.Add(ds);
            }
            else
            {
                m_vResources[index].Value = value;
            }
        }

        public void SetTownHallLevel(int level)
        {
            m_vTownHallLevel = level;
        }

        public void SetUnitCount(CombatItemData cd, int count)
        {
            switch (cd.GetCombatItemType())
            {
                case 1:
                    {
                        var index = GetDataIndex(m_vSpellCount, cd);
                        if (index != -1)
                            m_vSpellCount[index].Value = count;
                        else
                        {
                            var ds = new DataSlot(cd, count);
                            m_vSpellCount.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        var index = GetDataIndex(m_vUnitCount, cd);
                        if (index != -1)
                            m_vUnitCount[index].Value = count;
                        else
                        {
                            var ds = new DataSlot(cd, count);
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
                        var index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                            m_vHeroUpgradeLevel[index].Value = level;
                        else
                        {
                            var ds = new DataSlot(cd, level);
                            m_vHeroUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                case 1:
                    {
                        var index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                            m_vSpellUpgradeLevel[index].Value = level;
                        else
                        {
                            var ds = new DataSlot(cd, level);
                            m_vSpellUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        var index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                            m_vUnitUpgradeLevel[index].Value = level;
                        else
                        {
                            var ds = new DataSlot(cd, level);
                            m_vUnitUpgradeLevel.Add(ds);
                        }
                        break;
                    }
            }
        }

        #endregion Public Methods
    }
}
