using System;
using System.Collections.Generic;
using System.Linq;
using CSS.Core;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24402
    internal class LocalAlliancesMessage : Message
    {
        public LocalAlliancesMessage(Device client) : base(client)
        {
            this.Identifier = 24402;
        }

        internal override void Encode()
        {
            List<byte> packet1 = new List<byte>();
            int i = 0;

            foreach(Alliance alliance in ObjectManager.GetInMemoryAlliances().OrderByDescending(t => t.m_vScore))
            {
                if (i >= 100)
                    break;
                packet1.AddLong(alliance.m_vAllianceId);
                packet1.AddString(alliance.m_vAllianceName);
                packet1.AddInt(i + 1);
                packet1.AddInt(alliance.m_vScore);
                packet1.AddInt(i + 1);
                packet1.AddInt(alliance.m_vAllianceBadgeData);
                packet1.AddInt(alliance.GetAllianceMembers().Count);
                packet1.AddInt(0);
                packet1.AddInt(alliance.m_vAllianceLevel);
                i++;
            }
            this.Data.AddInt(i);       
            this.Data.AddRange(packet1.ToArray());
        }
    }
}
