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
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;

namespace CSS.Logic
{
    internal class ConstructionItem : GameObject
    {
        #region Public Constructors

        public ConstructionItem(Data data, Level level) : base(data, level)
        {
            m_vLevel = level;
            IsBoosted = false;
            m_vBoostEndTime = level.GetTime();
            m_vIsConstructing = false;
            UpgradeLevel = -1;
        }

        #endregion Public Constructors

        #region Protected Fields

        protected bool Locked;
        protected DateTime m_vBoostEndTime;
        protected bool m_vIsConstructing;
        protected Level m_vLevel;
        protected Timer m_vTimer;

        #endregion Protected Fields

        #region Public Properties

        public bool IsBoosted { get; set; }

        public int UpgradeLevel { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void BoostBuilding()
        {
            IsBoosted = true;
            m_vBoostEndTime = GetLevel().GetTime().AddMinutes(GetBoostDuration());
        }

        public void CancelConstruction()
        {
            if (IsConstructing())
            {
                var wasUpgrading = IsUpgrading();
                m_vIsConstructing = false;
                if (wasUpgrading)
                {
                    SetUpgradeLevel(UpgradeLevel);
                }
                var bd = GetConstructionItemData();
                var rd = bd.GetBuildResource(UpgradeLevel + 1);
                var cost = bd.GetBuildCost(UpgradeLevel + 1);
                var multiplier =
                    ObjectManager.DataTables.GetGlobals().GetGlobalData("BUILD_CANCEL_MULTIPLIER").NumberValue;
                var resourceCount = (int) ((cost * multiplier * (long) 1374389535) >> 32);
                resourceCount = Math.Max((resourceCount >> 5) + (resourceCount >> 31), 0);
                GetLevel().GetPlayerAvatar().CommodityCountChangeHelper(0, rd, resourceCount);
                m_vLevel.WorkerManager.DeallocateWorker(this);
                if (UpgradeLevel == -1)
                    m_vLevel.GameObjectManager.RemoveGameObject(this);
            }
        }

        public bool CanUpgrade()
        {
            var result = false;
            if (!IsConstructing())
            {
                if (UpgradeLevel < GetConstructionItemData().GetUpgradeLevelCount() - 1)
                {
                    result = true;
                    if (ClassId == 0 || ClassId == 4)
                    {
                        var currentTownHallLevel = GetLevel().GetPlayerAvatar().GetTownHallLevel();
                        var requiredTownHallLevel = GetRequiredTownHallLevelForUpgrade();
                        if (currentTownHallLevel < requiredTownHallLevel)
                        {
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

        public void FinishConstruction()
        {
            m_vIsConstructing = false;
            m_vLevel.WorkerManager.DeallocateWorker(this);
            SetUpgradeLevel(GetUpgradeLevel() + 1);
            if (GetResourceProductionComponent() != null)
            {
                GetResourceProductionComponent().Reset();
            }

            //Add exp to client avatar
            var constructionTime = GetConstructionItemData().GetConstructionTime(GetUpgradeLevel());
            var exp = (int) Math.Pow(constructionTime, 0.5f);
            GetLevel().GetPlayerAvatar().AddExperience(exp);

            //LogicAchievementManager::refreshStatus(v48)
            //LogicLevel::refreshNewShopUnlocksTH(v50)
            //v28 = v27(v7, 10, 1);//enable
            if (GetHeroBaseComponent(true) != null) //(GetBuildingData().IsHeroBarrack)
            {
                var data = (BuildingData) GetData();
                var hd = ObjectManager.DataTables.GetHeroByName(data.HeroType);
                GetLevel().GetPlayerAvatar().SetUnitUpgradeLevel(hd, 0);
                GetLevel().GetPlayerAvatar().SetHeroHealth(hd, 0);
                GetLevel().GetPlayerAvatar().SetHeroState(hd, 3);
            }
        }

        public int GetBoostDuration()
        {
            if (GetResourceProductionComponent() != null)
            {
                return ObjectManager.DataTables.GetGlobals().GetGlobalData("RESOURCE_PRODUCTION_BOOST_MINS").NumberValue;
            }
            if (GetUnitProductionComponent() != null)
            {
                if (GetUnitProductionComponent().IsSpellForge())
                {
                    return ObjectManager.DataTables.GetGlobals().GetGlobalData("SPELL_FACTORY_BOOST_MINS").NumberValue;
                }
                return ObjectManager.DataTables.GetGlobals().GetGlobalData("BARRACKS_BOOST_MINS").NumberValue;
            }
            if (GetHeroBaseComponent() != null)
            {
                return ObjectManager.DataTables.GetGlobals().GetGlobalData("HERO_REST_BOOST_MINS").NumberValue;
            }

            return 0;
        }

        public DateTime GetBoostEndTime()
        {
            return m_vBoostEndTime;
        }

        public float GetBoostMultipier()
        {
            if (GetResourceProductionComponent() != null)
            {
                return
                    ObjectManager.DataTables.GetGlobals()
                                 .GetGlobalData("RESOURCE_PRODUCTION_BOOST_MULTIPLIER")
                                 .NumberValue;
            }
            if (GetUnitProductionComponent() != null)
            {
                if (GetUnitProductionComponent().IsSpellForge())
                {
                    return
                        ObjectManager.DataTables.GetGlobals()
                                     .GetGlobalData("SPELL_FACTORY_BOOST_MULTIPLIER")
                                     .NumberValue;
                }
                return ObjectManager.DataTables.GetGlobals().GetGlobalData("BARRACKS_BOOST_MULTIPLIER").NumberValue;
            }
            if (GetHeroBaseComponent() != null)
            {
                return ObjectManager.DataTables.GetGlobals().GetGlobalData("HERO_REST_BOOST_MULTIPLIER").NumberValue;
            }

            return 0;
        }

        public ConstructionItemData GetConstructionItemData()
        {
            return (ConstructionItemData) GetData();
        }

        public HeroBaseComponent GetHeroBaseComponent(bool enabled = false)
        {
            var comp = GetComponent(10, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (HeroBaseComponent) comp;
            }
            return null;
        }

        public int GetRemainingConstructionTime()
        {
            return m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
        }

        public int GetRequiredTownHallLevelForUpgrade()
        {
            var upgradeLevel = Math.Min(UpgradeLevel + 1, GetConstructionItemData().GetUpgradeLevelCount() - 1);
            return GetConstructionItemData().GetRequiredTownHallLevel(upgradeLevel);
        }

        public ResourceProductionComponent GetResourceProductionComponent(bool enabled = false)
        {
            var comp = GetComponent(5, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (ResourceProductionComponent) comp;
            }
            return null;
        }

        public ResourceStorageComponent GetResourceStorageComponent(bool enabled = false)
        {
            var comp = GetComponent(6, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (ResourceStorageComponent) comp;
            }
            return null;
        }

        public UnitProductionComponent GetUnitProductionComponent(bool enabled = false)
        {
            var comp = GetComponent(3, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (UnitProductionComponent) comp;
            }
            return null;
        }

        public UnitStorageComponent GetUnitStorageComponent(bool enabled = false)
        {
            var comp = GetComponent(0, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (UnitStorageComponent) comp;
            }
            return null;
        }

        public UnitUpgradeComponent GetUnitUpgradeComponent(bool enabled = false)
        {
            var comp = GetComponent(9, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (UnitUpgradeComponent) comp;
            }
            return null;
        }

        public int GetUpgradeLevel()
        {
            return UpgradeLevel;
        }

        public bool IsConstructing()
        {
            return m_vIsConstructing;
        }

        public bool IsMaxUpgradeLevel()
        {
            return UpgradeLevel >= GetConstructionItemData().GetUpgradeLevelCount() - 1;
        }

        public bool IsUpgrading()
        {
            return m_vIsConstructing && UpgradeLevel >= 0;
        }

        public new void Load(JObject jsonObject)
        {
            UpgradeLevel = jsonObject["lvl"].ToObject<int>();
            m_vLevel.WorkerManager.DeallocateWorker(this);
            var constTimeToken = jsonObject["const_t"];
            if (constTimeToken != null)
            {
                m_vTimer = new Timer();
                m_vIsConstructing = true;
                var remainingConstructionTime = constTimeToken.ToObject<int>();
                m_vTimer.StartTimer(remainingConstructionTime, m_vLevel.GetTime());

                //a changer ici, utilise seconds_from_last_respawn?
                m_vLevel.WorkerManager.AllocateWorker(this);
            }
            Locked = false;
            var lockedToken = jsonObject["locked"];
            if (lockedToken != null)
            {
                Locked = lockedToken.ToObject<bool>();
            }

            var boostToken = jsonObject["boost_endTime"];
            if (boostToken != null)
            {
                m_vBoostEndTime = jsonObject["boost_endTime"].ToObject<DateTime>();
                IsBoosted = true;
            }

            SetUpgradeLevel(UpgradeLevel);
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            jsonObject.Add("lvl", UpgradeLevel);
            if (IsConstructing())
                jsonObject.Add("const_t", m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()));
            if (Locked)
                jsonObject.Add("locked", true);
            if (IsBoosted)
            {
                if ((int) (m_vBoostEndTime - GetLevel().GetTime()).TotalSeconds >= 0)
                {
                    jsonObject.Add("boost_t", (int) (m_vBoostEndTime - GetLevel().GetTime()).TotalSeconds);
                }
                jsonObject.Add("boost_endTime", m_vBoostEndTime);
            }
            base.Save(jsonObject);
            return jsonObject;
        }

        public void SetUpgradeLevel(int level)
        {
            UpgradeLevel = level;
            if (GetConstructionItemData().IsTownHall())
            {
                GetLevel().GetPlayerAvatar().SetTownHallLevel(level);
            }
            if (UpgradeLevel > -1 || IsUpgrading() || !IsConstructing())
            {
                if (GetUnitStorageComponent(true) != null)
                {
                    var data = (BuildingData) GetData();
                    if (data.GetUnitStorageCapacity(level) > 0)
                    {
                        if (!data.Bunker)
                        {
                            GetUnitStorageComponent().SetMaxCapacity(data.GetUnitStorageCapacity(level));
                            GetUnitStorageComponent().SetEnabled(!IsConstructing());
                        }
                    }
                }
                var resourceStorageComponent = GetResourceStorageComponent(true);
                if (resourceStorageComponent != null)
                {
                    var maxStoredResourcesList = ((BuildingData) GetData()).GetMaxStoredResourceCounts(UpgradeLevel);
                    resourceStorageComponent.SetMaxArray(maxStoredResourcesList);
                }
            }
        }

        public void SpeedUpConstruction()
        {
            if (IsConstructing())
            {
                var ca = GetLevel().GetPlayerAvatar();
                var remainingSeconds = m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
                var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
                if (ca.HasEnoughDiamonds(cost))
                {
                    ca.UseDiamonds(cost);
                    FinishConstruction();
                }
            }
        }

        public void StartConstructing(int newX, int newY)
        {
            X = newX;
            Y = newY;
            var constructionTime = GetConstructionItemData().GetConstructionTime(UpgradeLevel + 1);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                m_vTimer = new Timer();
                m_vTimer.StartTimer(constructionTime, m_vLevel.GetTime());
                m_vLevel.WorkerManager.AllocateWorker(this);
                m_vIsConstructing = true;
            }
        }

        public void StartUpgrading()
        {
            var constructionTime = GetConstructionItemData().GetConstructionTime(UpgradeLevel + 1);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                //todo : collecter les ressources avant upgrade si un component de ressources est défini
                m_vIsConstructing = true;
                m_vTimer = new Timer();
                m_vTimer.StartTimer(constructionTime, m_vLevel.GetTime());
                m_vLevel.WorkerManager.AllocateWorker(this);
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (IsConstructing())
            {
                if (m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()) <= 0)
                    FinishConstruction();
            }
        }

        public void Unlock()
        {
            Locked = false;
        }

        #endregion Public Methods
    }
}