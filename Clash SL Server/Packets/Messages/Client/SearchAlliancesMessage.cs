using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14324
    internal class SearchAlliancesMessage : Message
    {
        public SearchAlliancesMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        const int m_vAllianceLimit = 40;
        int m_vAllianceOrigin;
        int m_vAllianceScore;
        int m_vMaximumAllianceMembers;
        int m_vMinimumAllianceLevel;
        int m_vMinimumAllianceMembers;
        string m_vSearchString;
        byte m_vShowOnlyJoinableAlliances;
        int m_vWarFrequency;

        internal override void Decode()
        {
            this.m_vWarFrequency = this.Reader.ReadInt32();
            this.m_vAllianceOrigin = this.Reader.ReadInt32();
            this.m_vMinimumAllianceMembers = this.Reader.ReadInt32();
            this.m_vMaximumAllianceMembers = this.Reader.ReadInt32();
            this.m_vAllianceScore = this.Reader.ReadInt32();
            this.m_vShowOnlyJoinableAlliances = this.Reader.ReadByte();
            this.Reader.ReadInt32();
            this.m_vMinimumAllianceLevel = this.Reader.ReadInt32();

        }

        internal override void Process()
        {
            if (m_vSearchString.Length < 15)
            {
                ResourcesManager.DisconnectClient(Device);
            }
            else
            {
                List<Alliance> joinableAlliances = new List<Alliance>();

                foreach (Alliance _Alliance in ResourcesManager.m_vInMemoryAlliances.Values)
                {
                    if (_Alliance.m_vAllianceName.Contains(m_vSearchString, StringComparison.OrdinalIgnoreCase))
                    {
                        joinableAlliances.Add(_Alliance);
                    }
                }

                AllianceListMessage p = new AllianceListMessage(Device);
                p.m_vAlliances = joinableAlliances;
                p.m_vSearchString = m_vSearchString;
                p.Send();
            }
        }
    }
}