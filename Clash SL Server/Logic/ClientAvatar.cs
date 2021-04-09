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
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;

namespace CSS.Logic
{
    internal class ClientAvatar : Avatar
    {

        void updateLeague()
        {
            var table = ObjectManager.DataTables.GetTable(12);
            var i = 0;
            bool found = false;
            while (!found)
            {
                var league = (LeagueData) table.GetItemAt(i);
                if (m_vScore <= league.BucketPlacementRangeHigh[league.BucketPlacementRangeHigh.Count - 1] &&
                    m_vScore >= league.BucketPlacementRangeLow[0])
                {
                    found = true;
                    SetLeagueId(i);
                }
                i++;
            }
        }

        long m_vAllianceId;
        int m_vAvatarLevel;
        string m_vAvatarName;
        int m_vCurrentGems;
        long m_vCurrentHomeId;
        int m_vExperience;
        long m_vId;
        int m_vLeagueId;
        byte m_vNameChangingLeft;
        byte m_vnameChosenByUser;
        int m_vScore;
        string m_vToken;
        string m_vRegion;

        public ClientAvatar()
        {
            Achievements = new List<DataSlot>();
            AchievementsUnlocked = new List<DataSlot>();
            AllianceUnits = new List<TroopDataSlot>();
            NpcStars = new List<DataSlot>();
            NpcLootedGold = new List<DataSlot>();
            NpcLootedElixir = new List<DataSlot>();
        }

        public ClientAvatar(long id, string token) : this()
        {
            var rnd = new Random();

            LastUpdate = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Login = id.ToString() + (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            m_vId = id;
            m_vToken = token;
            m_vCurrentHomeId = id;
            m_vnameChosenByUser = 0x00;
            m_vNameChangingLeft = 0x02;
            m_vAvatarLevel = 1;
            m_vAllianceId = 0;
            m_vExperience = 0;
            EndShieldTime = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            m_vCurrentGems = Convert.ToInt32(ConfigurationManager.AppSettings["startingGems"]);

            m_vScore = ConfigurationManager.AppSettings["startingTrophies"] == "random"
                ? rnd.Next(1500, 4800)
                : Convert.ToInt32(ConfigurationManager.AppSettings["startingTrophies"]);

            TutorialStepsCount = 0x0A;
            m_vAvatarName = ConfigurationManager.AppSettings["defaultName"];
            SetResourceCount(ObjectManager.DataTables.GetResourceByName("Gold"),
                Convert.ToInt32(ConfigurationManager.AppSettings["startingGold"]));
            SetResourceCount(ObjectManager.DataTables.GetResourceByName("Elixir"),
                Convert.ToInt32(ConfigurationManager.AppSettings["startingElixir"]));
            SetResourceCount(ObjectManager.DataTables.GetResourceByName("DarkElixir"),
                Convert.ToInt32(ConfigurationManager.AppSettings["startingDarkElixir"]));
            SetResourceCount(ObjectManager.DataTables.GetResourceByName("Diamonds"),
                Convert.ToInt32(ConfigurationManager.AppSettings["startingGems"]));
        }

        public List<DataSlot> Achievements { get; set; }
        public List<DataSlot> AchievementsUnlocked { get; set; }
        public List<TroopDataSlot> AllianceUnits { get; set; }
        public int EndShieldTime { get; set; }
        public int LastUpdate { get; set; }
        public string Login { get; set; }
        public List<DataSlot> NpcLootedElixir { get; set; }
        public List<DataSlot> NpcLootedGold { get; set; }
        public List<DataSlot> NpcStars { get; set; }

        public uint Region { get; set; }

        public int RemainingShieldTime
        {
            get
            {
                var rest = EndShieldTime - (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                return rest > 0 ? rest : 0;
            }
        }

        public uint TutorialStepsCount { get; set; }

        public void AddDiamonds(int diamondCount)
        {
            m_vCurrentGems += diamondCount;
        }

        public void AddExperience(int exp)
        {
            m_vExperience += exp;
            var experienceCap =
                ((ExperienceLevelData) ObjectManager.DataTables.GetTable(10).GetDataByName(m_vAvatarLevel.ToString()))
                    .ExpPoints;
            if (m_vExperience >= experienceCap)
                if (ObjectManager.DataTables.GetTable(10).GetItemCount() > m_vAvatarLevel + 1)
                {
                    m_vAvatarLevel += 1;
                    m_vExperience = m_vExperience - experienceCap;
                }
                else
                    m_vExperience = 0;
        }

        public byte[] Encode()
        {
            var rnd = new Random();
            var data = new List<byte>();
            data.AddInt32(0);
            data.AddInt64(m_vId);
            data.AddInt64(m_vCurrentHomeId);
            if (m_vAllianceId != 0)
            {
                data.Add(1);
                data.AddInt64(m_vAllianceId);
                var alliance = ObjectManager.GetAlliance(m_vAllianceId);
                data.AddString(alliance.GetAllianceName());
                data.AddInt32(alliance.GetAllianceBadgeData());
                data.AddInt32(alliance.GetAllianceMember(m_vId).GetRole());
                data.AddInt32(alliance.GetAllianceLevel());
            }
            data.Add(0);

            if (m_vLeagueId == 22)
            {
                data.AddInt32(m_vScore / 12); //Legend Trophies
                data.AddInt32(1); //bestSeasonEnabled
                var month = DateTime.Now.Month;
                data.AddInt32(month); //bestSeasonMonth
                data.AddInt32(DateTime.Now.Year); ///bestSeasonYear
                data.AddInt32(rnd.Next(1, 10)); ///bestSeasonRank
                data.AddInt32(m_vScore); ///bestSeasonTrophies
                data.AddInt32(1); //lastSeasonEnabled
                if (month == 1)
                {
                    data.AddInt32(12); //lastSeasonMonth
                    data.AddInt32(DateTime.Now.Year - 1); //9
                }
                else
                {
                    int pmonth = month - 1;
                    data.AddInt32(pmonth); //lastSeasonMonth
                    data.AddInt32(DateTime.Now.Year); //lastSeasonYear
                }
                data.AddInt32(rnd.Next(1,10)); //lastSeasonRank
                data.AddInt32(m_vScore / 2); //lastSeasonTrophies
            }
            else
            {
                data.AddInt32(0); //1
                data.AddInt32(0); //2
                data.AddInt32(0); //3
                data.AddInt32(0); //4
                data.AddInt32(0); //5
                data.AddInt32(0); //6
                data.AddInt32(0); //7
                data.AddInt32(0); //8
                data.AddInt32(0); //9
                data.AddInt32(0); //10
                data.AddInt32(0); //11
            }

            data.AddInt32(m_vLeagueId);
            data.AddInt32(GetAllianceCastleLevel());
            data.AddInt32(GetAllianceCastleTotalCapacity());
            data.AddInt32(GetAllianceCastleUsedCapacity());
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(GetTownHallLevel());
            data.AddString(m_vAvatarName);
            data.AddInt32(-1);
            data.AddInt32(m_vAvatarLevel);
            data.AddInt32(m_vExperience);
            data.AddInt32(m_vCurrentGems);
            data.AddInt32(m_vCurrentGems);
            data.AddInt32(1200);
            data.AddInt32(60);
            data.AddInt32(m_vScore);
            data.AddInt32(100); //Attack win
            data.AddInt32(1);
            data.AddInt32(100); //Attack loses
            data.AddInt32(0);

            data.AddInt32(2800000); //Castle Gold
            data.AddInt32(2800000); //Castle Elexir
            data.AddInt32(14400); //Castle Dark Elexir
            data.Add(1);
            data.AddInt64(0);

            data.Add(m_vnameChosenByUser);

            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(1);

            data.AddInt32(1);
            data.AddInt32(0);

            data.AddDataSlots(GetResourceCaps());
            data.AddDataSlots(GetResources());
            data.AddDataSlots(GetUnits());
            data.AddDataSlots(GetSpells());
            data.AddDataSlots(m_vUnitUpgradeLevel);
            data.AddDataSlots(m_vSpellUpgradeLevel);
            data.AddDataSlots(m_vHeroUpgradeLevel);
            data.AddDataSlots(m_vHeroHealth);
            data.AddDataSlots(m_vHeroState);

            data.AddRange(BitConverter.GetBytes(AllianceUnits.Count).Reverse());
            foreach (var u in AllianceUnits)
            {
                data.AddRange(BitConverter.GetBytes(u.Data.GetGlobalID()).Reverse());
                data.AddRange(BitConverter.GetBytes(u.Value).Reverse());
                data.AddRange(BitConverter.GetBytes(0).Reverse()); 
            }

            data.AddRange(BitConverter.GetBytes(TutorialStepsCount).Reverse());
            for (uint i = 0; i < TutorialStepsCount; i++)
                data.AddRange(BitConverter.GetBytes(0x01406F40 + i).Reverse());

            data.AddRange(BitConverter.GetBytes(Achievements.Count).Reverse());
            foreach (var a in Achievements)
                data.AddRange(BitConverter.GetBytes(a.Data.GetGlobalID()).Reverse());

            data.AddRange(BitConverter.GetBytes(Achievements.Count).Reverse());
            foreach (var a in Achievements)
            {
                data.AddRange(BitConverter.GetBytes(a.Data.GetGlobalID()).Reverse());
                data.AddRange(BitConverter.GetBytes(0).Reverse()); //A CHANGER
            }

            data.AddRange(BitConverter.GetBytes(ObjectManager.NpcLevels.Count).Reverse());
            {
                for (var i = 17000000; i < 17000050; i++)
                {
                    data.AddRange(BitConverter.GetBytes(i).Reverse());
                    data.AddRange(BitConverter.GetBytes(rnd.Next(3, 3)).Reverse()); //Star
                }
            }

            data.AddDataSlots(NpcLootedGold);
            data.AddDataSlots(NpcLootedElixir);

            data.AddDataSlots(new List<DataSlot>());
            data.AddDataSlots(new List<DataSlot>());
            data.AddDataSlots(new List<DataSlot>());
            data.AddDataSlots(new List<DataSlot>());

            return data.ToArray();
        }

        public long GetAllianceId() => m_vAllianceId;

        public AllianceMemberEntry GetAllianceMemberEntry()
        {
            var alliance = ObjectManager.GetAlliance(m_vAllianceId);
            if (alliance != null)
                return alliance.GetAllianceMember(m_vId);
            return null;
        }

        public int GetAllianceRole()
        {
            var ame = GetAllianceMemberEntry();
            if (ame != null)
                return ame.GetRole();
            return -1;
        }

        public int GetAvatarLevel() => m_vAvatarLevel;

        public string GetAvatarName() => m_vAvatarName;

        public long GetCurrentHomeId() => m_vCurrentHomeId;

        public int GetDiamonds() => m_vCurrentGems;

        public long GetId() => m_vId;

        public int GetLeagueId() => m_vLeagueId;

        public int GetScore()
        {
            updateLeague();
            return m_vScore;
        }

        public int GetSecondsFromLastUpdate() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - LastUpdate;

        public string GetUserToken() => m_vToken;

        public string GetUserRegion() => m_vRegion;

        public bool HasEnoughDiamonds(int diamondCount) => m_vCurrentGems >= diamondCount;

        public bool HasEnoughResources(ResourceData rd, int buildCost) => GetResourceCount(rd) >= buildCost;

        public void LoadFromJSON(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            m_vId = jsonObject["avatar_id"].ToObject<long>();
            m_vToken = jsonObject["token"].ToObject<string>();
            m_vRegion = jsonObject["region"].ToObject<string>();
            m_vCurrentHomeId = jsonObject["current_home_id"].ToObject<long>();
            m_vAllianceId = jsonObject["alliance_id"].ToObject<long>();
            SetAllianceCastleLevel(jsonObject["alliance_castle_level"].ToObject<int>());
            SetAllianceCastleTotalCapacity(jsonObject["alliance_castle_total_capacity"].ToObject<int>());
            SetAllianceCastleUsedCapacity(jsonObject["alliance_castle_used_capacity"].ToObject<int>());
            SetTownHallLevel(jsonObject["townhall_level"].ToObject<int>());
            m_vAvatarName = jsonObject["avatar_name"].ToObject<string>();
            m_vAvatarLevel = jsonObject["avatar_level"].ToObject<int>();
            m_vExperience = jsonObject["experience"].ToObject<int>();
            m_vCurrentGems = jsonObject["current_gems"].ToObject<int>();
            SetScore(jsonObject["score"].ToObject<int>());
            m_vNameChangingLeft = jsonObject["nameChangesLeft"].ToObject<byte>();
            m_vnameChosenByUser = jsonObject["nameChosenByUser"].ToObject<byte>();

            var jsonResources = (JArray) jsonObject["resources"];
            foreach (JObject resource in jsonResources)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(resource);
                GetResources().Add(ds);
            }

            var jsonUnits = (JArray) jsonObject["units"];
            foreach (JObject unit in jsonUnits)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(unit);
                m_vUnitCount.Add(ds);
            }

            var jsonSpells = (JArray) jsonObject["spells"];
            foreach (JObject spell in jsonSpells)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(spell);
                m_vSpellCount.Add(ds);
            }

            var jsonUnitLevels = (JArray) jsonObject["unit_upgrade_levels"];
            foreach (JObject unitLevel in jsonUnitLevels)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(unitLevel);
                m_vUnitUpgradeLevel.Add(ds);
            }

            var jsonSpellLevels = (JArray) jsonObject["spell_upgrade_levels"];
            foreach (JObject data in jsonSpellLevels)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vSpellUpgradeLevel.Add(ds);
            }

            var jsonHeroLevels = (JArray) jsonObject["hero_upgrade_levels"];
            foreach (JObject data in jsonHeroLevels)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroUpgradeLevel.Add(ds);
            }

            var jsonHeroHealth = (JArray) jsonObject["hero_health"];
            foreach (JObject data in jsonHeroHealth)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroHealth.Add(ds);
            }

            var jsonHeroState = (JArray) jsonObject["hero_state"];
            foreach (JObject data in jsonHeroState)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroState.Add(ds);
            }

            var jsonAllianceUnits = (JArray) jsonObject["alliance_units"];
            foreach (JObject data in jsonAllianceUnits)
            {
                var ds = new TroopDataSlot(null, 0, 0);
                ds.Load(data);
                AllianceUnits.Add(ds);
            }
            TutorialStepsCount = jsonObject["tutorial_step"].ToObject<uint>();

            var jsonAchievementsProgress = (JArray) jsonObject["achievements_progress"];
            foreach (JObject data in jsonAchievementsProgress)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                Achievements.Add(ds);
            }

            var jsonNpcStars = (JArray) jsonObject["npc_stars"];
            foreach (JObject data in jsonNpcStars)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcStars.Add(ds);
            }

            var jsonNpcLootedGold = (JArray) jsonObject["npc_looted_gold"];
            foreach (JObject data in jsonNpcLootedGold)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcLootedGold.Add(ds);
            }

            var jsonNpcLootedElixir = (JArray) jsonObject["npc_looted_elixir"];
            foreach (JObject data in jsonNpcLootedElixir)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcLootedElixir.Add(ds);
            }
        }

        public string SaveToJSON()
        {
            var jsonData = new JObject();
            jsonData.Add("avatar_id", m_vId);
            jsonData.Add("token", m_vToken);
            jsonData.Add("region", m_vRegion);
            jsonData.Add("current_home_id", m_vCurrentHomeId);
            jsonData.Add("alliance_id", m_vAllianceId);
            jsonData.Add("alliance_castle_level", GetAllianceCastleLevel());
            jsonData.Add("alliance_castle_total_capacity", GetAllianceCastleTotalCapacity());
            jsonData.Add("alliance_castle_used_capacity", GetAllianceCastleUsedCapacity());
            jsonData.Add("townhall_level", GetTownHallLevel());
            jsonData.Add("avatar_name", m_vAvatarName);
            jsonData.Add("avatar_level", m_vAvatarLevel);
            jsonData.Add("experience", m_vExperience);
            jsonData.Add("current_gems", m_vCurrentGems);
            jsonData.Add("score", GetScore());
            jsonData.Add("nameChangesLeft", m_vNameChangingLeft);
            jsonData.Add("nameChosenByUser", (ushort) m_vnameChosenByUser);

            var jsonResourcesArray = new JArray();
            foreach (var resource in GetResources())
                jsonResourcesArray.Add(resource.Save(new JObject()));
            jsonData.Add("resources", jsonResourcesArray);

            var jsonUnitsArray = new JArray();
            foreach (var unit in GetUnits())
                jsonUnitsArray.Add(unit.Save(new JObject()));
            jsonData.Add("units", jsonUnitsArray);

            var jsonSpellsArray = new JArray();
            foreach (var spell in GetSpells())
                jsonSpellsArray.Add(spell.Save(new JObject()));
            jsonData.Add("spells", jsonSpellsArray);

            var jsonUnitUpgradeLevelsArray = new JArray();
            foreach (var unitUpgradeLevel in m_vUnitUpgradeLevel)
                jsonUnitUpgradeLevelsArray.Add(unitUpgradeLevel.Save(new JObject()));
            jsonData.Add("unit_upgrade_levels", jsonUnitUpgradeLevelsArray);

            var jsonSpellUpgradeLevelsArray = new JArray();
            foreach (var spellUpgradeLevel in m_vSpellUpgradeLevel)
                jsonSpellUpgradeLevelsArray.Add(spellUpgradeLevel.Save(new JObject()));
            jsonData.Add("spell_upgrade_levels", jsonSpellUpgradeLevelsArray);

            var jsonHeroUpgradeLevelsArray = new JArray();
            foreach (var heroUpgradeLevel in m_vHeroUpgradeLevel)
                jsonHeroUpgradeLevelsArray.Add(heroUpgradeLevel.Save(new JObject()));
            jsonData.Add("hero_upgrade_levels", jsonHeroUpgradeLevelsArray);

            var jsonHeroHealthArray = new JArray();
            foreach (var heroHealth in m_vHeroHealth)
                jsonHeroHealthArray.Add(heroHealth.Save(new JObject()));
            jsonData.Add("hero_health", jsonHeroHealthArray);

            var jsonHeroStateArray = new JArray();
            foreach (var heroState in m_vHeroState)
                jsonHeroStateArray.Add(heroState.Save(new JObject()));
            jsonData.Add("hero_state", jsonHeroStateArray);

            var jsonAllianceUnitsArray = new JArray();
            foreach (var allianceUnit in AllianceUnits)
                jsonAllianceUnitsArray.Add(allianceUnit.Save(new JObject()));
            jsonData.Add("alliance_units", jsonAllianceUnitsArray);

            jsonData.Add("tutorial_step", TutorialStepsCount);

            var jsonAchievementsProgressArray = new JArray();
            foreach (var achievement in Achievements)
                jsonAchievementsProgressArray.Add(achievement.Save(new JObject()));
            jsonData.Add("achievements_progress", jsonAchievementsProgressArray);

            var jsonNpcStarsArray = new JArray();
            foreach (var npcLevel in NpcStars)
                jsonNpcStarsArray.Add(npcLevel.Save(new JObject()));
            jsonData.Add("npc_stars", jsonNpcStarsArray);

            var jsonNpcLootedGoldArray = new JArray();
            foreach (var npcLevel in NpcLootedGold)
                jsonNpcLootedGoldArray.Add(npcLevel.Save(new JObject()));
            jsonData.Add("npc_looted_gold", jsonNpcLootedGoldArray);

            var jsonNpcLootedElixirArray = new JArray();
            foreach (var npcLevel in NpcLootedElixir)
                jsonNpcLootedElixirArray.Add(npcLevel.Save(new JObject()));
            jsonData.Add("npc_looted_elixir", jsonNpcLootedElixirArray);

            return JsonConvert.SerializeObject(jsonData);
        }

        public void SetAchievment(AchievementData ad, bool finished)
        {
            var index = GetDataIndex(Achievements, ad);
            var value = finished ? 1 : 0;
            if (index != -1)
                Achievements[index].Value = value;
            else
            {
                var ds = new DataSlot(ad, value);
                Achievements.Add(ds);
            }
        }

        public void SetAllianceId(long id)
        {
            m_vAllianceId = id;
        }

        public void SetAllianceRole(int a)
        {
            var ame = GetAllianceMemberEntry();
            if (ame != null)
                ame.SetRole(a);
        }

        public void SetDiamonds(int count)
        {
            m_vCurrentGems = count;
        }

        public void SetLeagueId(int id)
        {
            m_vLeagueId = id;
        }

        public void SetName(string name)
        {
            m_vAvatarName = name;
            m_vnameChosenByUser = 0x01;
            m_vNameChangingLeft = 0x02;
            TutorialStepsCount = 0x0D;
        }

        public void SetScore(int newScore)
        {
            m_vScore = newScore;
            updateLeague();
        }

        public void SetToken(string token)
        {
            m_vToken = token;
        }

        public void SetRegion(string region)
        {
            m_vRegion = region;
        }
        public void SetAvatarLevel(int newlv)
        {
            m_vAvatarLevel = newlv;
        }

        public void UseDiamonds(int diamondCount)
        {
            m_vCurrentGems -= diamondCount;
        }

    }
}
