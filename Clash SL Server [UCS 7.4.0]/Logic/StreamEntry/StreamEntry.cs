using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CSS.Helpers;
using CSS.Logic.DataSlots;
using CSS.Files.Logic;
using CSS.Core;
using CSS.Helpers.List;

namespace CSS.Logic.StreamEntry
{
    internal class StreamEntry
    {
        public StreamEntry()
        {
            m_vMessageTime = DateTime.UtcNow;
        }

        public List<DonationSlot> m_vUnitDonation;
        public List<BookmarkSlot> m_vDonatorList;
        internal long m_vHomeId;
        internal long SenderID;
        internal int ID;
        internal int m_vSenderLeagueId;
        internal int m_vSenderLevel;
        internal int m_vSenderRole;
        internal int m_vType = -1;
        internal string m_vSenderName;
        internal DateTime m_vMessageTime;
        internal string Message;
        internal int m_vMaxTroop;
        internal int m_vDonatedTroop = 0;
        internal int m_vDonatedSpell = 0;
        internal int m_vState = 1; // 3 Refused - 2 Accepted - 1 Waiting
        internal string m_vJudge;

        public virtual byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddInt(GetStreamEntryType());
            data.AddInt(0);
            data.AddInt(ID);
            data.Add(3);
            data.AddLong(SenderID);
            data.AddLong(m_vHomeId);
            data.AddString(m_vSenderName);
            data.AddInt(m_vSenderLevel);
            data.AddInt(m_vSenderLeagueId);
            data.AddInt(m_vSenderRole);
            data.AddInt(GetAgeSeconds());
            return data.ToArray();
        }

        public int GetAgeSeconds() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds -
        (int)m_vMessageTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public virtual int GetStreamEntryType() => m_vType;

        public virtual void Load(JObject jsonObject)
        {
            m_vType = jsonObject["type"].ToObject<int>();
            ID = jsonObject["id"].ToObject<int>();
            SenderID = jsonObject["sender_id"].ToObject<long>();
            m_vHomeId = jsonObject["home_id"].ToObject<long>();
            m_vSenderLevel = jsonObject["sender_level"].ToObject<int>();
            m_vSenderName = jsonObject["sender_name"].ToObject<string>();
            m_vSenderLeagueId = jsonObject["sender_leagueId"].ToObject<int>();
            m_vSenderRole = jsonObject["sender_role"].ToObject<int>();
            m_vMessageTime = jsonObject["message_time"].ToObject<DateTime>();
        }

        public virtual JObject Save(JObject jsonObject)
        {
            jsonObject.Add("type", GetStreamEntryType());
            jsonObject.Add("id", ID);
            jsonObject.Add("sender_id", SenderID);
            jsonObject.Add("home_id", m_vHomeId);
            jsonObject.Add("sender_level", m_vSenderLevel);
            jsonObject.Add("sender_name", m_vSenderName);
            jsonObject.Add("sender_leagueId", m_vSenderLeagueId);
            jsonObject.Add("sender_role", m_vSenderRole);
            jsonObject.Add("message_time", m_vMessageTime);

            return jsonObject;
        }

        internal async void SetSender(ClientAvatar avatar)
        {
            try
            {
                SenderID = avatar.UserId;
                m_vHomeId = avatar.UserId;
                m_vSenderName = avatar.AvatarName;
                m_vSenderLeagueId = avatar.m_vLeagueId;
                m_vSenderLevel = avatar.m_vAvatarLevel;
                m_vSenderRole = await avatar.GetAllianceRole();
            }
            catch(Exception){}
        }

        public void SetState(int status) => m_vState = status;

        public void SetJudgeName(string name) => m_vJudge = name;

        public void SetType(int type) => m_vType = type;

        public void SetMaxTroop(int size) => m_vMaxTroop = size;

        public void AddDonatedTroop(long did, int id, int value, int level)
        {
            DonationSlot e = m_vUnitDonation.Find(t => t.ID == id && t.UnitLevel == level);
            if (e != null)
            {
                int i = m_vUnitDonation.IndexOf(e);
                e.Count = e.Count + value;
                m_vUnitDonation[i] = e;
            }
            else
            {
                DonationSlot ds = new DonationSlot(did, id, value, level);
                m_vUnitDonation.Add(ds);
            }
        }

        public void AddUsedCapicity(int amount) => m_vDonatedTroop = m_vDonatedTroop + amount;
    }
}
