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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Messages.Client
{
    internal class SearchAlliancesMessage : Message
    {
        #region Public Constructors

        public SearchAlliancesMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Private Fields

        const int m_vAllianceLimit = 60;
        int m_vAllianceOrigin;
        int m_vAllianceScore;
        int m_vMaximumAllianceMembers;
        int m_vMinimumAllianceLevel;
        int m_vMinimumAllianceMembers;
        string m_vSearchString;
        byte m_vShowOnlyJoinableAlliances;
        int m_vWarFrequency;

        #endregion Private Fields

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vSearchString = br.ReadScString();
                m_vWarFrequency = br.ReadInt32WithEndian();
                m_vAllianceOrigin = br.ReadInt32WithEndian();
                m_vMinimumAllianceMembers = br.ReadInt32WithEndian();
                m_vMaximumAllianceMembers = br.ReadInt32WithEndian();
                m_vAllianceScore = br.ReadInt32WithEndian();
                m_vShowOnlyJoinableAlliances = br.ReadByte();
                br.ReadInt32WithEndian();
                m_vMinimumAllianceLevel = br.ReadInt32WithEndian();
            }
        }

        public override void Process(Level level)
        {
            var alliances = ObjectManager.GetInMemoryAlliances();
            var joinableAlliances = new List<Alliance>();
            var i = 0;
            var j = 0;
            while (j < m_vAllianceLimit && i < alliances.Count)
            {
                if (alliances[i].GetAllianceMembers().Count != 0
                    && alliances[i].GetAllianceName().Contains(m_vSearchString))
                {
                    joinableAlliances.Add(alliances[i]);
                    j++;
                }
                i++;
            }
            joinableAlliances = joinableAlliances.ToList();

            var p = new AllianceListMessage(Client);
            p.SetAlliances(joinableAlliances);
            p.SetSearchString(m_vSearchString);
            PacketManager.ProcessOutgoingPacket(p);
        }

        #endregion Public Methods
    }
}