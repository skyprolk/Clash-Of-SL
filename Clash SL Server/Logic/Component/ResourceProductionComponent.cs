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
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class ResourceProductionComponent : Component
    {
        #region Public Constructors

        public ResourceProductionComponent(ConstructionItem ci, Level level) : base(ci)
        {
            m_vTimeSinceLastClick = level.GetTime();
            m_vProductionResourceData =
            ObjectManager.DataTables.GetResourceByName(((BuildingData) ci.GetData()).ProducesResource);
            m_vResourcesPerHour = ((BuildingData) ci.GetData()).ResourcePerHour;
            m_vMaxResources = ((BuildingData) ci.GetData()).ResourceMax;
        }

        #endregion Public Constructors

        #region Public Properties

        public override int Type => 5;

        #endregion Public Properties

        #region Private Fields

        readonly List<int> m_vMaxResources;
        readonly ResourceData m_vProductionResourceData;
        readonly List<int> m_vResourcesPerHour;
        DateTime m_vTimeSinceLastClick;

        #endregion Private Fields

        #region Public Methods

        public void CollectResources()
        {
            var ci = (ConstructionItem) GetParent();
            var span = ci.GetLevel().GetTime() - m_vTimeSinceLastClick;
            float currentResources = 0;
            if (!ci.IsBoosted)
            {
                currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * (float) span.TotalSeconds;
            }
            else
            {
                if (ci.GetBoostEndTime() >= ci.GetLevel().GetTime())
                {
                    currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * (float) span.TotalSeconds;
                    currentResources *= ci.GetBoostMultipier();
                }
                else
                {
                    var boostedTime = (float) span.TotalSeconds - (float) (ci.GetLevel().GetTime() - ci.GetBoostEndTime()).TotalSeconds;
                    var notBoostedTime = (float) span.TotalSeconds - boostedTime;
                    currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * notBoostedTime;
                    currentResources += m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * boostedTime * ci.GetBoostMultipier();
                    ci.IsBoosted = false;
                }
            }

            currentResources = Math.Min(Math.Max(currentResources, 0), m_vMaxResources[ci.UpgradeLevel]);

            if (currentResources >= 1)
            {
                var ca = ci.GetLevel().GetPlayerAvatar();
                if (ca.GetResourceCap(m_vProductionResourceData) >= ca.GetResourceCount(m_vProductionResourceData))
                {
                    if (ca.GetResourceCap(m_vProductionResourceData) - ca.GetResourceCount(m_vProductionResourceData) <
                        currentResources)
                    {
                        var newCurrentResources = ca.GetResourceCap(m_vProductionResourceData) - ca.GetResourceCount(m_vProductionResourceData);
                        m_vTimeSinceLastClick = ci.GetLevel()
                              .GetTime()
                              .AddSeconds(-((currentResources - newCurrentResources) / (m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f))));
                        currentResources = newCurrentResources;
                    }
                    else
                    {
                        m_vTimeSinceLastClick = ci.GetLevel().GetTime();
                    }

                    ca.CommodityCountChangeHelper(0, m_vProductionResourceData, (int) currentResources);
                }
            }
        }

        public override void Load(JObject jsonObject)
        {
            var productionObject = (JObject) jsonObject["production"];
            if (productionObject != null)
            {
                m_vTimeSinceLastClick = productionObject["t_lastClick"].ToObject<DateTime>();
            }
        }

        public void Reset()
        {
            m_vTimeSinceLastClick = GetParent().GetLevel().GetTime();
        }

        public override JObject Save(JObject jsonObject)
        {
            if (((ConstructionItem) GetParent()).GetUpgradeLevel() != -1)
            {
                var productionObject = new JObject();
                productionObject.Add("t_lastClick", m_vTimeSinceLastClick);
                jsonObject.Add("production", productionObject);
                var ci = (ConstructionItem) GetParent();
                var seconds = (float) (GetParent().GetLevel().GetTime() - m_vTimeSinceLastClick).TotalSeconds;
                if (ci.IsBoosted)
                {
                    if (ci.GetBoostEndTime() >= ci.GetLevel().GetTime())
                    {
                        seconds *= ci.GetBoostMultipier();
                    }
                    else
                    {
                        var boostedTime = seconds - (float) (ci.GetLevel().GetTime() - ci.GetBoostEndTime()).TotalSeconds;
                        var notBoostedTime = seconds - boostedTime;
                        seconds = boostedTime * ci.GetBoostMultipier() + notBoostedTime;
                    }
                }
                jsonObject.Add("res_time", (int)
                        (m_vMaxResources[ci.GetUpgradeLevel()] / (float) m_vResourcesPerHour[ci.GetUpgradeLevel()] * 3600f -
                         seconds));
            }
            return jsonObject;
        }

        #endregion Public Methods
    }
}
