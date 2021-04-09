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
    internal class HeroBaseComponent : Component
    {
        #region Public Constructors

        public HeroBaseComponent(GameObject go, HeroData hd) : base(go)
        {
            m_vHeroData = hd;
        }

        #endregion Public Constructors

        #region Public Properties

        public override int Type => 10;

        #endregion Public Properties

        #region Private Fields

        readonly HeroData m_vHeroData;
        Timer m_vTimer;
        int m_vUpgradeLevelInProgress;

        #endregion Private Fields

        #region Public Methods

        public void CancelUpgrade()
        {
            if (m_vTimer != null)
            {
                var ca = GetParent().GetLevel().GetPlayerAvatar();
                var currentLevel = ca.GetUnitUpgradeLevel(m_vHeroData);
                var rd = m_vHeroData.GetUpgradeResource(currentLevel);
                var cost = m_vHeroData.GetUpgradeCost(currentLevel);
                var multiplier =
                    ObjectManager.DataTables.GetGlobals().GetGlobalData("HERO_UPGRADE_CANCEL_MULTIPLIER").NumberValue;
                var resourceCount = (int) ((cost * multiplier * (long) 1374389535) >> 32);
                resourceCount = Math.Max((resourceCount >> 5) + (resourceCount >> 31), 0);
                ca.CommodityCountChangeHelper(0, rd, resourceCount);
                GetParent().GetLevel().WorkerManager.DeallocateWorker(GetParent());
                m_vTimer = null;
            }
        }

        public bool CanStartUpgrading()
        {
            var result = false;
            if (m_vTimer == null)
            {
                var currentLevel = GetParent().GetLevel().GetPlayerAvatar().GetUnitUpgradeLevel(m_vHeroData);
                if (!IsMaxLevel())
                {
                    var requiredThLevel = m_vHeroData.GetRequiredTownHallLevel(currentLevel + 1);
                    result = GetParent().GetLevel().GetPlayerAvatar().GetTownHallLevel() >= requiredThLevel;
                }
            }
            return result;
        }

        public void FinishUpgrading()
        {
            var ca = GetParent().GetLevel().GetPlayerAvatar();
            var currentLevel = ca.GetUnitUpgradeLevel(m_vHeroData);
            ca.SetUnitUpgradeLevel(m_vHeroData, currentLevel + 1);
            GetParent().GetLevel().WorkerManager.DeallocateWorker(GetParent());
            m_vTimer = null;
        }

        public int GetRemainingUpgradeSeconds() => m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime());

        public int GetTotalSeconds() => m_vHeroData.GetUpgradeTime(GetParent().GetLevel().GetPlayerAvatar().GetUnitUpgradeLevel(m_vHeroData));

        public bool IsMaxLevel()
        {
            var ca = GetParent().GetLevel().GetPlayerAvatar();
            var currentLevel = ca.GetUnitUpgradeLevel(m_vHeroData);
            var maxLevel = m_vHeroData.GetUpgradeLevelCount() - 1;
            return currentLevel >= maxLevel;
        }

        public bool IsUpgrading() => m_vTimer != null;

        public override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject) jsonObject["hero_upg"];
            if (unitUpgradeObject != null)
            {
                m_vTimer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                m_vTimer.StartTimer(remainingTime, GetParent().GetLevel().GetTime());
                m_vUpgradeLevelInProgress = unitUpgradeObject["level"].ToObject<int>();
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            if (m_vTimer != null)
            {
                var unitUpgradeObject = new JObject();
                unitUpgradeObject.Add("level", m_vUpgradeLevelInProgress);
                unitUpgradeObject.Add("t", m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()));
                jsonObject.Add("hero_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        public void SpeedUpUpgrade()
        {
            var remainingSeconds = 0;
            if (IsUpgrading())
            {
                remainingSeconds = m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime());
            }
            var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
            var ca = GetParent().GetLevel().GetPlayerAvatar();
            if (ca.HasEnoughDiamonds(cost))
            {
                ca.UseDiamonds(cost);
                FinishUpgrading();
            }
        }

        public void StartUpgrading()
        {
            if (CanStartUpgrading())
            {
                GetParent().GetLevel().WorkerManager.AllocateWorker(GetParent());
                m_vTimer = new Timer();
                m_vTimer.StartTimer(GetTotalSeconds(), GetParent().GetLevel().GetTime());
                m_vUpgradeLevelInProgress = GetParent().GetLevel().GetPlayerAvatar().GetUnitUpgradeLevel(m_vHeroData) + 1;
                //SetHeroState v27(v24, v26, 1);
                //Not 100% done
            }
        }

        public override void Tick()
        {
            if (m_vTimer != null)
            {
                if (m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()) <= 0)
                {
                    FinishUpgrading();
                }
            }
        }

        #endregion Public Methods
    }
}
