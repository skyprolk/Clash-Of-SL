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
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Helpers;
using CSS.Logic.StreamEntry;

namespace CSS.Logic
{
    internal class Alliance
    {
        #region Private Fields


        const int m_vMaxAllianceMembers = 50;
        const int m_vMaxChatMessagesNumber = 30;
        readonly Dictionary<long, AllianceMemberEntry> m_vAllianceMembers;
        readonly List<StreamEntry.StreamEntry> m_vChatMessages;
        int m_vAllianceBadgeData;
        string m_vAllianceDescription;
        int m_vAllianceExperience;
        long m_vAllianceId;
        int m_vAllianceLevel;
        string m_vAllianceName;
        int m_vAllianceOrigin;
        int m_vAllianceType;
        int m_vDrawWars;
        int m_vLostWars;
        int m_vRequiredScore;
        int m_vScore;
        int m_vWarFrequency;
        byte m_vWarLogPublic;
        int m_vWonWars;

        #endregion Private Fields

        #region Public Constructors

        public Alliance()
        {
            m_vChatMessages = new List<StreamEntry.StreamEntry>();
            m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public Alliance(long id)
        {
            var r = new Random();
            m_vAllianceId = id;
            m_vAllianceName = "Default";
            m_vAllianceDescription = "Default";
            m_vAllianceBadgeData = 0;
            m_vAllianceType = 0;
            m_vRequiredScore = 0;
            m_vWarFrequency = 0;
            m_vAllianceOrigin = 32000001;
            m_vScore = 0;
            m_vAllianceExperience = r.Next(100, 5000);
            m_vAllianceLevel = r.Next(6, 10);
            m_vWonWars = r.Next(200, 500);
            m_vLostWars = r.Next(100, 300);
            m_vDrawWars = r.Next(100, 800);
            m_vChatMessages = new List<StreamEntry.StreamEntry>();
            m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddAllianceMember(AllianceMemberEntry entry) => m_vAllianceMembers.Add(entry.GetAvatarId(), entry);

        public void AddChatMessage(StreamEntry.StreamEntry message)
        {
            while (m_vChatMessages.Count >= m_vMaxChatMessagesNumber)
                m_vChatMessages.RemoveAt(0);
            m_vChatMessages.Add(message);
        }

        public byte[] EncodeFullEntry()
        {
            var data = new List<byte>();
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.AddInt32(m_vAllianceType);
            data.AddInt32(m_vAllianceMembers.Count);
            data.AddInt32(m_vScore);
            data.AddInt32(m_vRequiredScore);
            data.AddInt32(m_vWonWars);
            data.AddInt32(m_vLostWars);
            data.AddInt32(m_vDrawWars);
            data.AddInt32(0x001E8481);
            data.AddInt32(m_vWarFrequency);
            data.AddInt32(m_vAllianceOrigin);
            data.AddInt32(m_vAllianceExperience);
            data.AddInt32(m_vAllianceLevel);
            data.AddInt32(0);
            data.Add(m_vWarLogPublic); 
            return data.ToArray();
        }

        public byte[] EncodeHeader()
        {
            List<byte> data = new List<byte>();
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.Add(0);
            data.AddInt32(m_vAllianceLevel);
            data.AddInt32(1);
            data.AddInt32(-1);
            return data.ToArray();
        }

        public int GetAllianceBadgeData() => m_vAllianceBadgeData;

        public string GetAllianceDescription() => m_vAllianceDescription;

        public int GetAllianceExperience() => m_vAllianceExperience;

        public long GetAllianceId() => m_vAllianceId;

        public int GetAllianceLevel() => m_vAllianceLevel;

        public AllianceMemberEntry GetAllianceMember(long avatarId) => m_vAllianceMembers[avatarId];

        public List<AllianceMemberEntry> GetAllianceMembers() => m_vAllianceMembers.Values.ToList();

        public string GetAllianceName() => m_vAllianceName;

        public int GetAllianceOrigin() => m_vAllianceOrigin;

        public int GetAllianceType() => m_vAllianceType;

        public List<StreamEntry.StreamEntry> GetChatMessages() => m_vChatMessages;

        public int GetRequiredScore() => m_vRequiredScore;

        public int GetScore() => m_vScore;

        public int GetWarFrequency() => m_vWarFrequency;

        public int GetWarScore() => m_vWonWars;

        public byte GetWarLogPublic() => m_vWarLogPublic;

        public bool IsAllianceFull() => m_vAllianceMembers.Count >= m_vMaxAllianceMembers;

        public void LoadFromJSON(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            m_vAllianceId = jsonObject["alliance_id"].ToObject<long>();
            m_vAllianceName = jsonObject["alliance_name"].ToObject<string>();
            m_vAllianceBadgeData = jsonObject["alliance_badge"].ToObject<int>();
            m_vAllianceType = jsonObject["alliance_type"].ToObject<int>();
            m_vRequiredScore = jsonObject["required_score"].ToObject<int>();
            m_vAllianceDescription = jsonObject["description"].ToObject<string>();
            m_vAllianceExperience = jsonObject["alliance_experience"].ToObject<int>();
            m_vAllianceLevel = jsonObject["alliance_level"].ToObject<int>();
            m_vWarLogPublic = jsonObject["war_log_public"].ToObject<byte>();
            m_vWonWars = jsonObject["won_wars"].ToObject<int>();
            m_vLostWars = jsonObject["lost_wars"].ToObject<int>();
            m_vDrawWars = jsonObject["draw_wars"].ToObject<int>();
            m_vWarFrequency = jsonObject["war_frequency"].ToObject<int>();
            m_vAllianceOrigin = jsonObject["alliance_origin"].ToObject<int>();
            var jsonMembers = (JArray)jsonObject["members"];
            foreach (JToken jToken in jsonMembers)
            {
                var jsonMember = (JObject)jToken;
                long id = jsonMember["avatar_id"].ToObject<long>();
                var pl = ResourcesManager.GetPlayer(id);
                var member = new AllianceMemberEntry(id);
                m_vScore = m_vScore + pl.GetPlayerAvatar().GetScore();
                member.Load(jsonMember);
                m_vAllianceMembers.Add(id, member);
            }
            m_vScore = m_vScore / 2;
            var jsonMessages = (JArray)jsonObject["chatMessages"];
            if (jsonMessages != null)
            {
                foreach (JToken jToken in jsonMessages)
                {
                    JObject jsonMessage = (JObject)jToken;
                    StreamEntry.StreamEntry se = new StreamEntry.StreamEntry();
                    if (jsonMessage["type"].ToObject<int>() == 1)
                        se = new TroopRequestStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 2)
                        se = new ChatStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 3)
                        se = new InvitationStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 4)
                        se = new AllianceEventStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 5)
                        se = new ShareStreamEntry();
                    se.Load(jsonMessage);
                    m_vChatMessages.Add(se);
                }
            }
        }

        public void RemoveMember(long avatarId) => m_vAllianceMembers.Remove(avatarId);

        public string SaveToJSON()
        {
            var jsonData = new JObject();
            jsonData.Add("alliance_id", m_vAllianceId);
            jsonData.Add("alliance_name", m_vAllianceName);
            jsonData.Add("alliance_badge", m_vAllianceBadgeData);
            jsonData.Add("alliance_type", m_vAllianceType);
            jsonData.Add("score", m_vScore);
            jsonData.Add("required_score", m_vRequiredScore);
            jsonData.Add("description", m_vAllianceDescription);
            jsonData.Add("alliance_experience", m_vAllianceExperience);
            jsonData.Add("alliance_level", m_vAllianceLevel);
            jsonData.Add("war_log_public", m_vWarLogPublic);
            jsonData.Add("won_wars", m_vWonWars);
            jsonData.Add("lost_wars", m_vLostWars);
            jsonData.Add("draw_wars", m_vDrawWars);
            jsonData.Add("war_frequency", m_vWarFrequency);
            jsonData.Add("alliance_origin", m_vAllianceOrigin);
            var jsonMembersArray = new JArray();
            foreach (AllianceMemberEntry member in m_vAllianceMembers.Values)
            {
                var jsonObject = new JObject();
                member.Save(jsonObject);
                jsonMembersArray.Add(jsonObject);
            }
            jsonData.Add("members", jsonMembersArray);
            var jsonMessageArray = new JArray();
            foreach (StreamEntry.StreamEntry message in m_vChatMessages)
            {
                var jsonObject = new JObject();
                message.Save(jsonObject);
                jsonMessageArray.Add(jsonObject);
            }
            jsonData.Add("chatMessages", jsonMessageArray);
            return JsonConvert.SerializeObject(jsonData);
        }

        public void SetAllianceBadgeData(int data) => m_vAllianceBadgeData = data;

        public void SetAllianceDescription(string description) => m_vAllianceDescription = description;

        public void SetAllianceLevel(int level) => m_vAllianceLevel = level;

        public void SetAllianceName(string name) => m_vAllianceName = name;

        public void SetAllianceOrigin(int origin) => m_vAllianceOrigin = origin;

        public void SetAllianceType(int status) => m_vAllianceType = status;

        public void SetRequiredScore(int score) => m_vRequiredScore = score;

        public void SetWarFrequency(int frequency) => m_vWarFrequency = frequency;

        public void SetWarPublicStatus(byte log) => m_vWarLogPublic = log;
        
        #endregion Public Methods
    }
}
