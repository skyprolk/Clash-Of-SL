using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Commands.Client;
using CSS.Packets.Messages.Server;
using CSS.Packets.Commands.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14301
    internal class CreateAllianceMessage : Message
    {
        public CreateAllianceMessage(Device device, Reader reader) : base(device, reader)
        {

        }

        int m_vAllianceBadgeData;
        string m_vAllianceDescription;
        string m_vAllianceName;
        int m_vAllianceOrigin;
        int m_vAllianceType;
        int m_vRequiredScore;
        int m_vWarFrequency;
        byte m_vWarAndFriendlyStatus;

        internal override void Decode()
        {
            this.m_vAllianceName = this.Reader.ReadString();
            this.m_vAllianceDescription = this.Reader.ReadString();
            this.m_vAllianceBadgeData = this.Reader.ReadInt32();
            this.m_vAllianceType = this.Reader.ReadInt32();
            this.m_vRequiredScore = this.Reader.ReadInt32();
            this.m_vWarFrequency = this.Reader.ReadInt32();
            this.m_vAllianceOrigin = this.Reader.ReadInt32();
            this.m_vWarAndFriendlyStatus = this.Reader.ReadByte();

        }

        internal override void Process()
        {
            if (m_vAllianceName == null)
                m_vAllianceName = "Clan";

            if (m_vAllianceName.Length < 16 || m_vAllianceName.Length < 1)
            {
                if (m_vAllianceDescription.Length < 259 || m_vAllianceDescription.Length < 0)
                {

                    Alliance alliance = ObjectManager.CreateAlliance(0);
                    alliance.m_vAllianceName = m_vAllianceName;
                    alliance.m_vAllianceDescription = m_vAllianceDescription;
                    alliance.m_vAllianceType = m_vAllianceType;
                    alliance.m_vRequiredScore = m_vRequiredScore;
                    alliance.m_vAllianceBadgeData = m_vAllianceBadgeData;
                    alliance.m_vAllianceOrigin = m_vAllianceOrigin;
                    alliance.m_vWarFrequency = m_vWarFrequency;
                    alliance.SetWarAndFriendlytStatus(m_vWarAndFriendlyStatus);
                    this.Device.Player.Avatar.AllianceId = alliance.m_vAllianceId;

                    AllianceMemberEntry member = new AllianceMemberEntry(this.Device.Player.Avatar.UserId) {Role = 2};
                    alliance.AddAllianceMember(member);

                    JoinedAllianceCommand b = new JoinedAllianceCommand(this.Device);
                    b.SetAlliance(alliance);

                    AllianceRoleUpdateCommand d = new AllianceRoleUpdateCommand(this.Device);
                    d.SetAlliance(alliance);
                    d.SetRole(2);
                    d.Tick(this.Device.Player);

                    new AvailableServerCommandMessage(this.Device, b.Handle()).Send();

                    new AvailableServerCommandMessage(this.Device, d.Handle()).Send();

                }
                else
                {
                    ResourcesManager.DisconnectClient(Device);
                }
            }
            else
            {
                ResourcesManager.DisconnectClient(Device);
            }
        }
    }
}
