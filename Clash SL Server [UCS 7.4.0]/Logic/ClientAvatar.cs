using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;
using static System.Convert;
using static System.Configuration.ConfigurationManager;
using CSS.Logic.DataSlots;
using System.Threading.Tasks;
using CSS.Helpers.List;
using System.Diagnostics;

namespace CSS.Logic
{
    internal class ClientAvatar : Avatar
    {
        // Long
        internal long AllianceId            = 0;
        internal long CurrentHomeId;
        internal long UserId;

        // Int
        internal int HighID;
        internal int LowID;
        internal int m_vAvatarLevel;
        internal int m_vCurrentGems;
        internal int m_vCurrentGold;
        internal int m_vCurrentElixir;
        internal int m_vCurrentDarkElixir;
        internal int m_vExperience;
        internal int m_vLeagueId;
        internal int m_vTrophy;
        internal int m_vDonatedUnits;
        internal int m_vRecievedUnits;
        internal int m_vActiveLayout;
        internal int m_vShieldTime;
        internal int m_vProtectionTime;
        internal int ReportedTimes          = 0;
        internal int m_vDonated;
        internal int m_vReceived;

        // UInt
        internal uint TutorialStepsCount    = 0x0A;

        // Byte
        internal byte m_vNameChangingLeft   = 0x02;
        internal byte m_vnameChosenByUser   = 0x00;
        internal byte AccountPrivileges     = 0x00;
        // String
        internal string AvatarName;
        internal string UserToken;
        internal string Region;
        internal string FacebookId;
        internal string FacebookToken;
        internal string GoogleId;
        internal string GoogleToken;
        internal string IPAddress;
        internal string TroopRequestMessage;

        // Boolean
        internal bool m_vPremium           = false;
        internal bool m_vAndroid;
        internal bool AccountBanned        = false;

        //Datetime
        internal DateTime m_vAccountCreationDate;
        internal DateTime LastTickSaved;

        public ClientAvatar()
        {
            Achievements         = new List<DataSlot>();
            AchievementsUnlocked = new List<DataSlot>();
            AllianceUnits        = new List<DonationSlot>();
            NpcStars             = new List<DataSlot>();
            NpcLootedGold        = new List<DataSlot>();
            NpcLootedElixir      = new List<DataSlot>();
            BookmarkedClan       = new List<BookmarkSlot>();
            QuickTrain1          = new List<DataSlot>();
            QuickTrain2          = new List<DataSlot>();
            QuickTrain3          = new List<DataSlot>();
        }

        public ClientAvatar(long id, string token) : this()
        {
            Random rnd               = new Random();
            this.LastUpdate          = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            this.Login               = id.ToString() + (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            this.UserId              = id;
            this.HighID              = (int)(id >> 32);
            this.LowID               = (int)(id & 0xffffffffL);
            this.UserToken           = token;
            this.CurrentHomeId       = id;
            this.m_vAvatarLevel      = ToInt32(AppSettings["startingLevel"]);
            this.EndShieldTime       = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            this.m_vCurrentGems      = ToInt32(AppSettings["startingGems"]);
            this.m_vCurrentGold    = ToInt32(AppSettings["startingGold"]);
            this.m_vCurrentElixir  = ToInt32(AppSettings["startingElixir"]);
            this.m_vCurrentDarkElixir = ToInt32(AppSettings["startingDarkElixir"]);
            this.m_vExperience = ToInt32(AppSettings["startingLevel"]);
            this.m_vTrophy            = AppSettings["startingTrophies"] == "random" ? rnd.Next(1500, 4999) : ToInt32(AppSettings["startingTrophies"]);

            this.AvatarName          = "NoNameYet";

            SetResourceCount(CSVManager.DataTables.GetResourceByName("Gold"), ToInt32(AppSettings["startingGold"]));
            SetResourceCount(CSVManager.DataTables.GetResourceByName("Elixir"), ToInt32(AppSettings["startingElixir"]));
            SetResourceCount(CSVManager.DataTables.GetResourceByName("DarkElixir"), ToInt32(AppSettings["startingDarkElixir"]));
            SetResourceCount(CSVManager.DataTables.GetResourceByName("Diamonds"), ToInt32(AppSettings["startingGems"]));
        }

        public List<DataSlot> Achievements { get; set; }
        public List<DataSlot> AchievementsUnlocked { get; set; }
        public List<DonationSlot> AllianceUnits { get; set; }
        public int EndShieldTime { get; set; }
        public int LastUpdate { get; set; }
        public string Login { get; set; }
        public List<DataSlot> NpcLootedElixir { get; set; }
        public List<DataSlot> NpcLootedGold { get; set; }
        public List<DataSlot> NpcStars { get; set; }
        public List<BookmarkSlot> BookmarkedClan { get; set; }
        public List<DataSlot> QuickTrain1 { get; set; }
        public List<DataSlot> QuickTrain2 { get; set; }
        public List<DataSlot> QuickTrain3 { get; set; }
        public object State { get; internal set; }

        private void updateLeague()
        {
            DataTable table = CSVManager.DataTables.GetTable(12);
            int i = 0;
            bool found = false;
            while (!found)
            {
                var league = (LeagueData)table.GetItemAt(i);
                if (m_vTrophy <= league.BucketPlacementRangeHigh[league.BucketPlacementRangeHigh.Count - 1] &&
                    m_vTrophy >= league.BucketPlacementRangeLow[0])
                {
                    found = true;
                    m_vLeagueId = i;
                }
                i++;
            }
        }

        public void AddDiamonds(int diamondCount)
        {
            this.m_vCurrentGems += diamondCount;
        }

        public int GetDiamonds()
        {
            return this.m_vCurrentGems;
        }

        public void AddExperience(int exp)
        {
            m_vExperience += exp;
            var experienceCap =
                ((ExperienceLevelData)CSVManager.DataTables.GetTable(10).GetDataByName(m_vAvatarLevel.ToString()))
                    .ExpPoints;
            if (m_vExperience >= experienceCap)
                if (CSVManager.DataTables.GetTable(10).GetItemCount() > m_vAvatarLevel + 1)
                {
                    m_vAvatarLevel += 1;
                    m_vExperience = m_vExperience - experienceCap;
                }
                else
                    m_vExperience = 0;
        }

        public async Task<byte[]> Encode()
        {
            try
            {
                Random rnd = new Random();
                List<byte> data = new List<byte>();
                data.AddLong(this.UserId);
                data.AddLong(this.CurrentHomeId);
                if (this.AllianceId != 0)
                {
                    data.Add(1);
                    data.AddLong(this.AllianceId);
                    Alliance alliance = ObjectManager.GetAlliance(this.AllianceId);
                    data.AddString(alliance.m_vAllianceName);
                    data.AddInt(alliance.m_vAllianceBadgeData);
                    data.AddInt(alliance.m_vAllianceMembers[this.UserId].Role);
                    data.AddInt(alliance.m_vAllianceLevel);
                }
                data.Add(0);

                if (m_vLeagueId == 22)
                {
                    data.AddInt(m_vTrophy / 12);
                    data.AddInt(1);
                    int month = DateTime.Now.Month;
                    data.AddInt(month);
                    data.AddInt(DateTime.Now.Year);
                    data.AddInt(rnd.Next(1, 10));
                    data.AddInt(this.m_vTrophy);
                    data.AddInt(1);
                    if (month == 1)
                    {
                        data.AddInt(12);
                        data.AddInt(DateTime.Now.Year - 1);
                    }
                    else
                    {
                        data.AddInt(month - 1);
                        data.AddInt(DateTime.Now.Year);
                    }
                    data.AddInt(rnd.Next(1, 10));
                    data.AddInt(this.m_vTrophy / 2);
                }
                else
                {
                    data.AddInt(0); //1
                    data.AddInt(0); //2
                    data.AddInt(0); //3
                    data.AddInt(0); //4
                    data.AddInt(0); //5
                    data.AddInt(0); //6
                    data.AddInt(0); //7
                    data.AddInt(0); //8
                    data.AddInt(0); //9
                    data.AddInt(0); //10
                    data.AddInt(0); //11
                }

                data.AddInt(this.m_vLeagueId);
                data.AddInt(GetAllianceCastleLevel());
                data.AddInt(GetAllianceCastleTotalCapacity());
                data.AddInt(GetAllianceCastleUsedCapacity());
                data.AddInt(0);
                data.AddInt(-1);
                data.AddInt(m_vTownHallLevel);
                data.AddString(this.AvatarName);
                data.AddString(this.FacebookId);
                data.AddInt(this.m_vAvatarLevel);
                data.AddInt(this.m_vExperience);
                data.AddInt(this.m_vCurrentGems);
                data.AddInt(this.m_vCurrentGems);
                data.AddInt(1200);
                data.AddInt(60);
                data.AddInt(m_vTrophy);
                data.AddInt(200); // Attack Wins
                data.AddInt(m_vDonated);
                data.AddInt(100); // Attack Loses
                data.AddInt(m_vReceived);
                data.AddInt(this.m_vCurrentGold);
                data.AddInt(this.m_vCurrentElixir);
                data.AddInt(this.m_vCurrentDarkElixir);
                data.AddInt(0);
                data.Add(1);
                data.AddLong(946720861000);

                data.Add(this.m_vnameChosenByUser);

                data.AddInt(0);
                data.AddInt(0);
                data.AddInt(0);
                data.AddInt(1);

                data.AddInt(0);
                data.AddInt(0);
                data.Add(0);
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
                foreach (DonationSlot u in AllianceUnits)
                {
                    data.AddInt(u.ID);
                    data.AddInt(u.Count);
                    data.AddInt(u.UnitLevel);
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
                    data.AddRange(BitConverter.GetBytes(0).Reverse());
                }

                data.AddRange(BitConverter.GetBytes(ObjectManager.NpcLevels.Count).Reverse());
                {
                    for (var i = 17000000; i < 17000050; i++)
                    {
                        data.AddRange(BitConverter.GetBytes(i).Reverse());
                        data.AddRange(BitConverter.GetBytes(rnd.Next(3, 3)).Reverse());
                    }
                }

                data.AddDataSlots(NpcLootedGold);
                data.AddDataSlots(NpcLootedElixir);
                data.AddDataSlots(new List<DataSlot>());
                data.AddDataSlots(new List<DataSlot>());
                data.AddDataSlots(new List<DataSlot>());
                data.AddDataSlots(new List<DataSlot>());

                data.AddInt(QuickTrain1.Count);
                foreach (var i in QuickTrain1)
                {
                    data.AddInt(i.Data.GetGlobalID());
                    data.AddInt(i.Value);
                }

                data.AddInt(QuickTrain2.Count);
                foreach (var i in QuickTrain2)
                {
                    data.AddInt(i.Data.GetGlobalID());
                    data.AddInt(i.Value);
                }
                data.AddInt(QuickTrain3.Count);
                foreach (var i in QuickTrain3)
                {
                    data.AddInt(i.Data.GetGlobalID());
                    data.AddInt(i.Value);
                }
                data.AddInt(QuickTrain1.Count);
                foreach (var i in QuickTrain1)
                {
                    data.AddInt(i.Data.GetGlobalID());
                    data.AddInt(i.Value);
                }
                data.AddDataSlots(new List<DataSlot>());
                return data.ToArray();
            } catch (Exception) { return null; }
        }

        public async Task<AllianceMemberEntry> GetAllianceMemberEntry()
        {
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(this.AllianceId);
                return alliance?.m_vAllianceMembers[this.UserId];
            } catch (Exception) { return null; }
        }

        public async Task<int> GetAllianceRole()
        {
            try
            {
                var ame = await GetAllianceMemberEntry();
                if (ame != null)
                    return ame.Role;
                return -1;
            } catch (Exception) { return 1; }
        }

        public int GetScore()
        {
            updateLeague();
            return m_vTrophy;
        }

        public int GetSecondsFromLastUpdate() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - LastUpdate;

        public bool HasEnoughDiamonds(int diamondCount) => m_vCurrentGems >= diamondCount;

        public bool HasEnoughResources(ResourceData rd, int buildCost) => GetResourceCount(rd) >= buildCost;

        public void LoadFromJSON(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            Console.WriteLine(jsonObject);
            this.UserId = jsonObject["avatar_id"].ToObject<long>();
            this.HighID = jsonObject["id_high_int"].ToObject<int>();
            this.LowID = jsonObject["id_low_int"].ToObject<int>();
            this.UserToken = jsonObject["token"].ToObject<string>();
            this.Region = jsonObject["region"].ToObject<string>();
            this.IPAddress = jsonObject["IPAddress"].ToObject<string>();
            this.m_vAccountCreationDate = jsonObject["avatar_creation_date"].ToObject<DateTime>();
            this.AccountPrivileges = jsonObject["avatar_privilages"].ToObject<byte>();
            this.AccountBanned = jsonObject["avatar_banned"].ToObject<bool>();
            this.m_vActiveLayout = jsonObject["active_layout"].ToObject<int>();
            this.LastTickSaved = jsonObject["last_tick_save"].ToObject<DateTime>();
            this.m_vAndroid = jsonObject["android"].ToObject<bool>();
            this.CurrentHomeId = jsonObject["current_home_id"].ToObject<long>();
            this.AllianceId = jsonObject["alliance_id"].ToObject<long>();
            SetAllianceCastleLevel(jsonObject["alliance_castle_level"].ToObject<int>());
            SetAllianceCastleTotalCapacity(jsonObject["alliance_castle_total_capacity"].ToObject<int>());
            SetAllianceCastleUsedCapacity(jsonObject["alliance_castle_used_capacity"].ToObject<int>());
            SetTownHallLevel(jsonObject["townhall_level"].ToObject<int>());
            this.AvatarName = jsonObject["avatar_name"].ToObject<string>();
            this.m_vAvatarLevel = jsonObject["avatar_level"].ToObject<int>();
            this.m_vExperience = jsonObject["experience"].ToObject<int>();
            this.m_vCurrentGems = jsonObject["current_gems"].ToObject<int>();
            this.m_vCurrentGold = jsonObject["current_gold"].ToObject<int>();
            this.m_vCurrentElixir = jsonObject["current_elixir"].ToObject<int>();
            this.m_vCurrentDarkElixir = jsonObject["current_dark_elixir"].ToObject<int>();
            SetScore(jsonObject["score"].ToObject<int>());
            this.m_vNameChangingLeft = jsonObject["nameChangesLeft"].ToObject<byte>();
            this.m_vnameChosenByUser = jsonObject["nameChosenByUser"].ToObject<byte>();
            this.m_vShieldTime = jsonObject["shield_time"].ToObject<int>();
            this.m_vProtectionTime = jsonObject["protection_time"].ToObject<int>();
            this.FacebookId = jsonObject["fb_id"].ToObject<string>();
            this.FacebookToken = jsonObject["fb_token"].ToObject<string>();
            this.GoogleId = jsonObject["gg_id"].ToObject<string>();
            this.m_vReceived = jsonObject["troops_received"].ToObject<int>();
            this.m_vDonated = jsonObject["troops_donated"].ToObject<int>();
            this.GoogleToken = jsonObject["gg_token"].ToObject<string>();
            this.TroopRequestMessage = jsonObject["rq_message"].ToObject<string>();
            JArray jsonBookmarkedClan = (JArray)jsonObject["bookmark"];
            foreach (JObject jobject in jsonBookmarkedClan)
            {
                JObject data = (JObject)jobject;
                BookmarkSlot ds = new BookmarkSlot(0);
                ds.Load(data);
                BookmarkedClan.Add(ds);
            }

            JArray jsonResources = (JArray) jsonObject["resources"];
            foreach (JObject resource in jsonResources)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(resource);
                GetResources().Add(ds);
            }

            JArray jsonUnits = (JArray) jsonObject["units"];
            foreach (JObject unit in jsonUnits)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(unit);
                m_vUnitCount.Add(ds);
            }

            JArray jsonSpells = (JArray) jsonObject["spells"];
            foreach (JObject spell in jsonSpells)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(spell);
                m_vSpellCount.Add(ds);
            }

            JArray jsonUnitLevels = (JArray) jsonObject["unit_upgrade_levels"];
            foreach (JObject unitLevel in jsonUnitLevels)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(unitLevel);
                m_vUnitUpgradeLevel.Add(ds);
            }

            JArray jsonSpellLevels = (JArray) jsonObject["spell_upgrade_levels"];
            foreach (JObject data in jsonSpellLevels)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vSpellUpgradeLevel.Add(ds);
            }

            JArray jsonHeroLevels = (JArray) jsonObject["hero_upgrade_levels"];
            foreach (JObject data in jsonHeroLevels)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroUpgradeLevel.Add(ds);
            }

            JArray jsonHeroHealth = (JArray) jsonObject["hero_health"];
            foreach (JObject data in jsonHeroHealth)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroHealth.Add(ds);
            }

            JArray jsonHeroState = (JArray) jsonObject["hero_state"];
            foreach (JObject data in jsonHeroState)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroState.Add(ds);
            }

            JArray jsonAllianceUnits = (JArray) jsonObject["alliance_units"];
            foreach (JObject data in jsonAllianceUnits)
            {
                DonationSlot ds = new DonationSlot(0, 0, 0, 0);
                ds.Load(data);
                AllianceUnits.Add(ds);
            }
            TutorialStepsCount = jsonObject["tutorial_step"].ToObject<uint>();

            JArray jsonAchievementsProgress = (JArray) jsonObject["achievements_progress"];
            foreach (JObject data in jsonAchievementsProgress)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                Achievements.Add(ds);
            }

            JArray jsonNpcStars = (JArray) jsonObject["npc_stars"];
            foreach (JObject data in jsonNpcStars)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcStars.Add(ds);
            }

            JArray jsonNpcLootedGold = (JArray) jsonObject["npc_looted_gold"];
            foreach (JObject data in jsonNpcLootedGold)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcLootedGold.Add(ds);
            }

            JArray jsonNpcLootedElixir = (JArray) jsonObject["npc_looted_elixir"];
            foreach (JObject data in jsonNpcLootedElixir)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcLootedElixir.Add(ds);
            }
            JArray jsonQuickTrain1 = (JArray)jsonObject["quick_train_1"];
            foreach (JObject data in jsonQuickTrain1)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                QuickTrain1.Add(ds);
            }
            JArray jsonQuickTrain2 = (JArray)jsonObject["quick_train_2"];
            foreach (JObject data in jsonQuickTrain2)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                QuickTrain2.Add(ds);
            }
            JArray jsonQuickTrain3 = (JArray)jsonObject["quick_train_3"];
            foreach (JObject data in jsonQuickTrain3)
            {
                DataSlot ds = new DataSlot(null, 0);
                ds.Load(data);
                QuickTrain3.Add(ds);
            }
            m_vPremium = jsonObject["Premium"].ToObject<bool>();
        }

        public string SaveToJSON()
        {
            #region Foreach Stuff
            JArray jsonBookmarkClan = new JArray();
            foreach (BookmarkSlot clan in BookmarkedClan)
                jsonBookmarkClan.Add(clan.Save(new JObject()));

            JArray jsonResourcesArray = new JArray();
            foreach (DataSlot resource in GetResources())
                jsonResourcesArray.Add(resource.Save(new JObject()));

            JArray jsonUnitsArray = new JArray();
            foreach (DataSlot unit in GetUnits())
                jsonUnitsArray.Add(unit.Save(new JObject()));

            JArray jsonSpellsArray = new JArray();
            foreach (DataSlot spell in GetSpells())
                jsonSpellsArray.Add(spell.Save(new JObject()));

            JArray jsonUnitUpgradeLevelsArray = new JArray();
            foreach (DataSlot unitUpgradeLevel in m_vUnitUpgradeLevel)
                jsonUnitUpgradeLevelsArray.Add(unitUpgradeLevel.Save(new JObject()));


            JArray jsonSpellUpgradeLevelsArray = new JArray();
            foreach (DataSlot spellUpgradeLevel in m_vSpellUpgradeLevel)
                jsonSpellUpgradeLevelsArray.Add(spellUpgradeLevel.Save(new JObject()));

            JArray jsonHeroUpgradeLevelsArray = new JArray();
            foreach (DataSlot heroUpgradeLevel in m_vHeroUpgradeLevel)
                jsonHeroUpgradeLevelsArray.Add(heroUpgradeLevel.Save(new JObject()));

            JArray jsonHeroHealthArray = new JArray();
            foreach (DataSlot heroHealth in m_vHeroHealth)
                jsonHeroHealthArray.Add(heroHealth.Save(new JObject()));

            JArray jsonHeroStateArray = new JArray();
            foreach (DataSlot heroState in m_vHeroState)
                jsonHeroStateArray.Add(heroState.Save(new JObject()));

             JArray jsonAllianceUnitsArray = new JArray();
            foreach (DonationSlot allianceUnit in AllianceUnits)
                jsonAllianceUnitsArray.Add(allianceUnit.Save(new JObject()));

            JArray jsonAchievementsProgressArray = new JArray();
            foreach (DataSlot achievement in Achievements)
                jsonAchievementsProgressArray.Add(achievement.Save(new JObject()));

            JArray jsonNpcStarsArray = new JArray();
            foreach (DataSlot npcLevel in NpcStars)
                jsonNpcStarsArray.Add(npcLevel.Save(new JObject()));

            JArray jsonNpcLootedGoldArray = new JArray();
            foreach (DataSlot npcLevel in NpcLootedGold)
                jsonNpcLootedGoldArray.Add(npcLevel.Save(new JObject()));
  
            JArray jsonNpcLootedElixirArray = new JArray();
            foreach (DataSlot npcLevel in NpcLootedElixir)
                jsonNpcLootedElixirArray.Add(npcLevel.Save(new JObject()));

            JArray jsonQuickTrain1Array = new JArray();
            foreach (DataSlot quicktrain1 in QuickTrain1)
                jsonQuickTrain1Array.Add(quicktrain1.Save(new JObject()));

            JArray jsonQuickTrain2Array = new JArray();
            foreach (DataSlot quicktrain2 in QuickTrain2)
                jsonQuickTrain1Array.Add(quicktrain2.Save(new JObject()));

            JArray jsonQuickTrain3Array = new JArray();
            foreach (DataSlot quicktrain3 in QuickTrain3)
                jsonQuickTrain3Array.Add(quicktrain3.Save(new JObject()));
        #endregion

            JObject jsonData = new JObject
            {
                {"avatar_id", this.UserId},
                {"id_high_int", this.HighID},
                {"id_low_int", this.LowID},
                {"token", this.UserToken},
                {"region", this.Region},
                {"IPAddress", this.IPAddress},
                {"avatar_creation_date", this.m_vAccountCreationDate},
                {"avatar_privilages", this.AccountPrivileges},
                {"avatar_banned", this.AccountBanned},
                {"active_layout", this.m_vActiveLayout},
                {"last_tick_save", this.LastTickSaved},
                {"android", this.m_vAndroid},
                {"current_home_id", this.CurrentHomeId},
                {"alliance_id", this.AllianceId},
                {"alliance_castle_level", GetAllianceCastleLevel()},
                {"alliance_castle_total_capacity", GetAllianceCastleTotalCapacity()},
                {"alliance_castle_used_capacity", GetAllianceCastleUsedCapacity()},
                {"townhall_level", m_vTownHallLevel},
                {"avatar_name", this.AvatarName},
                {"avatar_level", this.m_vAvatarLevel},
                {"experience", this.m_vExperience},
                {"current_gems", this.m_vCurrentGems},
                {"current_gold", this.m_vCurrentGold},
                {"current_elixir", this.m_vCurrentElixir},
                {"current_dark_elixir", this.m_vCurrentDarkElixir},
                {"score", GetScore()},
                {"nameChangesLeft", this.m_vNameChangingLeft},
                {"nameChosenByUser", (ushort) m_vnameChosenByUser},
                {"shield_time", this.m_vShieldTime},
                {"protection_time", this.m_vProtectionTime},
                {"fb_id", this.FacebookId},
                {"fb_token", this.FacebookToken},
                {"gg_id", this.GoogleId},
                {"troops_received", this.m_vReceived},
                {"troops_donated", this.m_vDonated},
                {"gg_token", this.GoogleToken},
                {"rq_message", this.TroopRequestMessage},
                {"bookmark", jsonBookmarkClan},
                {"resources", jsonResourcesArray},
                {"units", jsonUnitsArray},
                {"spells", jsonSpellsArray},
                {"unit_upgrade_levels", jsonUnitUpgradeLevelsArray},
                {"spell_upgrade_levels", jsonSpellUpgradeLevelsArray},
                {"hero_upgrade_levels", jsonHeroUpgradeLevelsArray},
                {"hero_health", jsonHeroHealthArray},
                {"hero_state", jsonHeroStateArray},
                {"alliance_units", jsonAllianceUnitsArray},
                {"tutorial_step", this.TutorialStepsCount},
                {"achievements_progress", jsonAchievementsProgressArray},
                {"npc_stars", jsonNpcStarsArray},
                {"npc_looted_gold", jsonNpcLootedGoldArray},
                {"npc_looted_elixir", jsonNpcLootedElixirArray},
                {"quick_train_1", jsonQuickTrain1Array},
                {"quick_train_2", jsonQuickTrain2Array},
                {"quick_train_3", jsonQuickTrain3Array},
                {"Premium", this.m_vPremium}
            };

            return JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        }

        public void InitializeAccountCreationDate() => m_vAccountCreationDate = DateTime.Now;

        public void AddAllianceTroop(long did, int id, int value, int level)
        {
            DonationSlot e = AllianceUnits.Find(t => t.ID == id && t.DonatorID == did && t.UnitLevel == level);
            if (e != null)
            {
                int i = AllianceUnits.IndexOf(e);
                e.Count = e.Count + value;
                AllianceUnits[i] = e;
            }
            else
            {
                DonationSlot ds = new DonationSlot(did, id, value, level);
                AllianceUnits.Add(ds);
            }
        }

        public void SetAchievment(AchievementData ad, bool finished)
        {
            int index = GetDataIndex(Achievements, ad);
            int value = finished ? 1 : 0;
            if (index != -1)
                Achievements[index].Value = value;
            else
            {
                DataSlot ds = new DataSlot(ad, value);
                Achievements.Add(ds);
            }
        }

        public async void SetAllianceRole(int a)
        {
            try
            {
                AllianceMemberEntry ame = await GetAllianceMemberEntry();
                if (ame != null)
                    ame.Role = a;
            }
            catch (Exception){}
        }

        public void SetName(string name)
        {
            this.AvatarName = name;
            if (m_vnameChosenByUser == 0x01)
            {
                m_vNameChangingLeft = 0x01;
            }
            else
            {
                m_vNameChangingLeft = 0x02;
            }
            TutorialStepsCount = 0x0D;
        }

        public void SetScore(int newScore)
        {
            m_vTrophy = newScore;
            updateLeague();
        }

        public void UseDiamonds(int diamondCount) => m_vCurrentGems -= diamondCount;
    }
}
