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

using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.PacketProcessing.Commands;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Messages.Client
{
    class CreateAllianceMessage : Message
    {
        #region Public Constructors
        public CreateAllianceMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {

        }
        #endregion Public Constructors
      
        #region Private Fields
        int m_vAllianceBadgeData;
        string m_vAllianceDescription;
        string m_vAllianceName;
        int m_vAllianceOrigin;
        int m_vAllianceType;
        int m_vRequiredScore;
        int m_vWarFrequency;
        byte m_vWarLogPublic;
        #endregion Private Fields

        #region Public Methods
        public override void Decode()
        {
            using (CoCSharpPacketReader br = new CoCSharpPacketReader(new MemoryStream(GetData())))
            {
                m_vAllianceName = br.ReadString();
                m_vAllianceDescription = br.ReadString();
                m_vAllianceBadgeData = br.ReadInt32WithEndian();
                m_vAllianceType = br.ReadInt32WithEndian();
                m_vRequiredScore = br.ReadInt32WithEndian();
                m_vWarFrequency = br.ReadInt32WithEndian();
                m_vAllianceOrigin = br.ReadInt32WithEndian();
                m_vWarLogPublic = br.ReadByte();
            }
        }

        public override void Process(Level level)
        {
            var alliance = ObjectManager.CreateAlliance(0);
            alliance.SetAllianceName(m_vAllianceName);
            alliance.SetAllianceDescription(m_vAllianceDescription);
            alliance.SetAllianceType(m_vAllianceType);
            alliance.SetRequiredScore(m_vRequiredScore);
            alliance.SetAllianceBadgeData(m_vAllianceBadgeData);
            alliance.SetAllianceOrigin(m_vAllianceOrigin);
            alliance.SetWarFrequency(m_vWarFrequency);
            alliance.SetWarPublicStatus(m_vWarLogPublic);
            level.GetPlayerAvatar().SetAllianceId(alliance.GetAllianceId());
            var member = new AllianceMemberEntry(level.GetPlayerAvatar().GetId());
            member.SetRole(2);
            alliance.AddAllianceMember(member);
            var b = new JoinAllianceCommand();
            b.SetAlliance(alliance);
            var a = new AvailableServerCommandMessage(Client);
            a.SetCommandId(1);
            a.SetCommand(b);
            PacketManager.ProcessOutgoingPacket(a);
            PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(Client, level));
            PacketManager.ProcessOutgoingPacket (new AllianceStreamMessage(Client, alliance));
            PacketManager.ProcessOutgoingPacket(new AllianceFullEntryMessage(Client, alliance));
        }
        #endregion Public Methods
    }
}
