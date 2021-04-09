using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CSS.Helpers;
using CSS.Helpers.List;

namespace CSS.Logic.StreamEntry
{
    internal class AllianceEventStreamEntry : StreamEntry
    {
        long m_vAvatarId;
        internal string m_vAvatarName;
        internal int EventType;

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddInt(EventType);
            data.AddLong(m_vAvatarId);
            data.AddString(m_vAvatarName);
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 4;
       
        //event id's
        // 1 = kicked from clan
        // 2 = accecpted to clan
        // 3 = join clan
        // 4 = leave clan
        // 5 = promote
        // 6 = demote
        // 7 = start clan war search
        // 8 = cancel clan war search
        // 9 = clan war oponnent not found
        // 10 = update clan setting

        public override void Load(JObject jsonObject)
        {
            m_vAvatarName = jsonObject["avatar_name"].ToObject<string>();
            EventType  = jsonObject["event_type"].ToObject<int>();
            m_vAvatarId   = jsonObject["avatar_id"].ToObject<long>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject.Add("avatar_name", m_vAvatarName);
            jsonObject.Add("event_type", EventType);
            jsonObject.Add("avatar_id", m_vAvatarId);
            return jsonObject;
        }
    }
}
