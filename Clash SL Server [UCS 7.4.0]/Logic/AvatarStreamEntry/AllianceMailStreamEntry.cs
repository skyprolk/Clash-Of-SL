using System.Collections.Generic;
using CSS.Helpers;
using CSS.Helpers.List;

namespace CSS.Logic.AvatarStreamEntry
{
    internal class AllianceMailStreamEntry : AvatarStreamEntry
    {
        internal int AllianceBadgeData;
        internal long AllianceId;
        internal string AllianceName;
        internal string Message;
        internal long SenderId;

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddString(Message);
            data.Add(1);
            data.AddLong(SenderId);
            data.AddLong(AllianceId);
            data.AddString(AllianceName);
            data.AddInt(AllianceBadgeData);
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 6;
    }
}
