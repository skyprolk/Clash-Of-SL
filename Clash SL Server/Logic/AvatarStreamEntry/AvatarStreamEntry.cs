using System;
using System.Collections.Generic;
using CSS.Helpers;
using CSS.Helpers.List;

namespace CSS.Logic.AvatarStreamEntry
{
    internal class AvatarStreamEntry
    {
        public AvatarStreamEntry()
        {
            m_vCreationTime = DateTime.UtcNow;
        }

        internal DateTime m_vCreationTime;
        internal int ID;
        internal byte IsNew;
        internal long m_vSenderId;
        internal int m_vSenderLeagueId;
        internal int m_vSenderLevel;
        internal string m_vSenderName;

        public virtual byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddInt(GetStreamEntryType());
            data.AddLong(ID);
            data.Add(1);
            data.AddLong(m_vSenderId);
            data.AddString(m_vSenderName);
            data.AddInt(m_vSenderLevel);
            data.AddInt(m_vSenderLeagueId);
            data.AddInt(10);
            data.Add(IsNew);
            return data.ToArray();
        }

        public int GetAgeSeconds() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - (int)m_vCreationTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public virtual int GetStreamEntryType() => -1;

        public void SetSender(ClientAvatar avatar)
        {
            m_vSenderId = avatar.UserId;
            m_vSenderName = avatar.AvatarName;
            m_vSenderLevel = avatar.m_vAvatarLevel;
            m_vSenderLeagueId = avatar.m_vLeagueId;
        }
    }
}
