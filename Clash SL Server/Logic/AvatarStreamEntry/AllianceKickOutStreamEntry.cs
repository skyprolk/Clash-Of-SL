using System.Collections.Generic;
using CSS.Helpers;
using CSS.Helpers.List;

namespace CSS.Logic.AvatarStreamEntry
{
    internal class AllianceKickOutStreamEntry : AvatarStreamEntry
    {
        int m_vAllianceBadgeData;
        long m_vAllianceId;
        string m_vAllianceName;
        string m_vMessage;

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.Encode());
            data.AddString(m_vMessage);
            data.AddLong(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt(m_vAllianceBadgeData);
            data.Add(1);
            data.AddInt(0x29);
            data.AddInt(0x0084E879);

            return data.ToArray();
        }

        public override int GetStreamEntryType() => 5;

        public void SetAllianceBadgeData(int data) => m_vAllianceBadgeData = data;

        public void SetAllianceId(long id) => m_vAllianceId = id;

        public void SetAllianceName(string name) => m_vAllianceName = name;

        public void SetMessage(string message) => m_vMessage = message;
    }
}