using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Logic.StreamEntry;
using System.Threading.Tasks;
using CSS.Helpers.List;

namespace CSS.Logic
{
    internal class Alliance
    {
        const int m_vMaxAllianceMembers    = 50;
        const int m_vMaxChatMessagesNumber = 30;
        internal readonly Dictionary<long, AllianceMemberEntry> m_vAllianceMembers;
        internal readonly List<StreamEntry.StreamEntry> m_vChatMessages;
        internal int m_vAllianceBadgeData;
        internal string m_vAllianceDescription;
        internal int m_vAllianceExperience;
        internal long m_vAllianceId;
        internal int m_vAllianceLevel;
        internal string m_vAllianceName;
        internal int m_vAllianceOrigin;
        internal int m_vAllianceType;
        internal int m_vDrawWars;
        internal int m_vLostWars;
        internal int m_vRequiredScore;
        internal int m_vScore;
        internal int m_vWarFrequency;
        internal byte m_vWarLogPublic;
        internal int m_vWonWars;
        internal byte m_vFriendlyWar;

        public Alliance()
        {
            m_vChatMessages    = new List<StreamEntry.StreamEntry>();
            m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public Alliance(long id)
        {
            m_vAllianceId          = id;
            m_vAllianceName        = "Default";
            m_vAllianceDescription = "Default";
            m_vAllianceBadgeData   = 0;
            m_vAllianceType        = 0;
            m_vRequiredScore       = 0;
            m_vWarFrequency        = 0;
            m_vAllianceOrigin      = 32000001;
            m_vScore               = 1;
            m_vAllianceExperience  = 1;
            m_vAllianceLevel       = 1;
            m_vWonWars             = 0;
            m_vLostWars            = 0;
            m_vDrawWars            = 0;
            m_vChatMessages        = new List<StreamEntry.StreamEntry>();
            m_vAllianceMembers     = new Dictionary<long, AllianceMemberEntry>();
        }

        public void AddAllianceMember(AllianceMemberEntry entry) => m_vAllianceMembers.Add(entry.AvatarId, entry);

        public void AddChatMessage(StreamEntry.StreamEntry message)
        {
            while (m_vChatMessages.Count >= m_vMaxChatMessagesNumber)
            {
                m_vChatMessages.RemoveAt(0);
            }
            m_vChatMessages.Add(message);
        }

        public byte[] EncodeFullEntry()
        {
            List<byte> data = new List<byte>();
            data.AddLong(this.m_vAllianceId);
            data.AddString(this.m_vAllianceName);
            data.AddInt(this.m_vAllianceBadgeData);
            data.AddInt(this.m_vAllianceType);
            data.AddInt(this.m_vAllianceMembers.Count);
            data.AddInt(this.m_vScore);
            data.AddInt(this.m_vRequiredScore);
            data.AddInt(this.m_vWonWars);
            data.AddInt(this.m_vLostWars);
            data.AddInt(this.m_vDrawWars);
            data.AddInt(20000001);
            data.AddInt(this.m_vWarFrequency);
            data.AddInt(this.m_vAllianceOrigin);
            data.AddInt(this.m_vAllianceExperience);
            data.AddInt(this.m_vAllianceLevel);
            data.AddInt(0);
            data.AddInt(0);
            data.Add(this.m_vWarLogPublic);
            data.Add(this.m_vFriendlyWar);
            return data.ToArray();
        }

        public byte[] EncodeHeader()
        {
            List<byte> data = new List<byte>();
            data.AddLong(this.m_vAllianceId);
            data.AddString(this.m_vAllianceName);
            data.AddInt(this.m_vAllianceBadgeData);
            data.Add(0);
            data.AddInt(this.m_vAllianceLevel);
            data.AddInt(1);
            data.AddInt(-1);
            return data.ToArray();
        }

        public List<AllianceMemberEntry> GetAllianceMembers() => m_vAllianceMembers.Values.ToList();

        public bool IsAllianceFull() => m_vAllianceMembers.Count >= m_vMaxAllianceMembers;

        public async void LoadFromJSON(string jsonString)
        {
            try
            {
                JObject jsonObject = JObject.Parse(jsonString);
                m_vAllianceId = jsonObject["alliance_id"].ToObject<long>();
                m_vAllianceName = jsonObject["alliance_name"].ToObject<string>();
                m_vAllianceBadgeData = jsonObject["alliance_badge"].ToObject<int>();
                m_vAllianceType = jsonObject["alliance_type"].ToObject<int>();
                m_vRequiredScore = jsonObject["required_score"].ToObject<int>();
                m_vAllianceDescription = jsonObject["description"].ToObject<string>();
                m_vAllianceExperience = jsonObject["alliance_experience"].ToObject<int>();
                m_vAllianceLevel = jsonObject["alliance_level"].ToObject<int>();
                m_vWarLogPublic = jsonObject["war_log_public"].ToObject<byte>();
                m_vFriendlyWar = jsonObject["friendly_war"].ToObject<byte>();
                m_vWonWars = jsonObject["won_wars"].ToObject<int>();
                m_vLostWars = jsonObject["lost_wars"].ToObject<int>();
                m_vDrawWars = jsonObject["draw_wars"].ToObject<int>();
                m_vWarFrequency = jsonObject["war_frequency"].ToObject<int>();
                m_vAllianceOrigin = jsonObject["alliance_origin"].ToObject<int>();
                JArray jsonMembers = (JArray)jsonObject["members"];
                foreach (JToken jToken in jsonMembers)
                {
                    JObject jsonMember = (JObject)jToken;
                    long id = jsonMember["avatar_id"].ToObject<long>();
                    Level pl = await ResourcesManager.GetPlayer(id);
                    AllianceMemberEntry member = new AllianceMemberEntry(id);
                    m_vScore = m_vScore + pl.Avatar.GetScore();
                    member.Load(jsonMember);
                    m_vAllianceMembers.Add(id, member);
                }
                m_vScore = m_vScore / 2;
                JArray jsonMessages = (JArray)jsonObject["chatMessages"];
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
                        else { }
                        se.Load(jsonMessage);
                        m_vChatMessages.Add(se);
                    }
                }
            }
            catch (Exception) { }
        }

        public void RemoveMember(long avatarId) => m_vAllianceMembers.Remove(avatarId);

        public string SaveToJSON()
        {
            JObject jsonData = new JObject();
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
            jsonData.Add("friendly_war", m_vFriendlyWar);
            jsonData.Add("won_wars", m_vWonWars);
            jsonData.Add("lost_wars", m_vLostWars);
            jsonData.Add("draw_wars", m_vDrawWars);
            jsonData.Add("war_frequency", m_vWarFrequency);
            jsonData.Add("alliance_origin", m_vAllianceOrigin);
            JArray jsonMembersArray = new JArray();
            foreach (AllianceMemberEntry member in m_vAllianceMembers.Values)
            {
                JObject jsonObject = new JObject();
                member.Save(jsonObject);
                jsonMembersArray.Add(jsonObject);
            }
            jsonData.Add("members", jsonMembersArray);
            JArray jsonMessageArray = new JArray();
            foreach (StreamEntry.StreamEntry message in m_vChatMessages)
            {
                JObject jsonObject = new JObject();
                message.Save(jsonObject);
                jsonMessageArray.Add(jsonObject);
            }
            jsonData.Add("chatMessages", jsonMessageArray);
            return JsonConvert.SerializeObject(jsonData);
        }

        public void SetWarAndFriendlytStatus(byte status)
        {
            switch (status)
            {
                case 1:
                    m_vWarLogPublic = 1;
                    break;
                case 2:
                    m_vWarLogPublic = 1;
                    break;
                case 3:
                    m_vWarLogPublic = 1;
                    m_vFriendlyWar = 1;
                    break;
                case 0:
                    m_vWarLogPublic = 0;
                    m_vFriendlyWar = 0;
                    break;
                default:
                    break;
            }
        }
    }
}
