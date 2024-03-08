using System;
using System.IO;
using System.Text;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.Packets.Messages.Server;
using  CSS.Packets.Commands.Server;
using System.Threading.Tasks;

namespace CSS.Packets.Messages.Client
{
    // Packet 14316
    internal class EditClanSettingsMessage : Message
    {
        public EditClanSettingsMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        int m_vAllianceBadgeData;
        string m_vAllianceDescription;
        int m_vAllianceOrigin;
        int m_vAllianceType;
        int m_vRequiredScore;
        int m_vWarFrequency;
        byte m_vWarAndFriendlyStatus;

        internal override void Decode()
        {
            this.m_vAllianceDescription = this.Reader.ReadString();
            this.Reader.ReadInt32();
            this.m_vAllianceBadgeData = this.Reader.ReadInt32();
            this.m_vAllianceType = this.Reader.ReadInt32();
            this.m_vRequiredScore = this.Reader.ReadInt32();
            this.m_vWarFrequency = this.Reader.ReadInt32();
            this.m_vAllianceOrigin = this.Reader.ReadInt32();
            this.m_vWarAndFriendlyStatus = this.Reader.ReadByte();
        }

        internal override async void Process()
        {
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId);
                if (alliance != null)
                {
                    if (m_vAllianceDescription.Length < 259 || m_vAllianceDescription.Length < 0)
                    {
                        alliance.m_vAllianceDescription = m_vAllianceDescription;
                        alliance.m_vAllianceBadgeData = m_vAllianceBadgeData;
                        alliance.m_vAllianceType = m_vAllianceType;
                        alliance.m_vRequiredScore = m_vRequiredScore;
                        alliance.m_vWarFrequency = m_vWarFrequency;
                        alliance.m_vAllianceOrigin = m_vAllianceOrigin;
                        alliance.SetWarAndFriendlytStatus(m_vWarAndFriendlyStatus);

                        ClientAvatar avatar = this.Device.Player.Avatar;
                        long allianceId = avatar.AllianceId;
                        AllianceEventStreamEntry eventStreamEntry =
                            new AllianceEventStreamEntry
                            {
                                ID =
                                    (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))
                                        .TotalSeconds
                            };
                        eventStreamEntry.SetSender(avatar);
                        eventStreamEntry.EventType = 10;
                        eventStreamEntry.SetSender(avatar);
                        alliance.AddChatMessage(eventStreamEntry);

                        AllianceSettingChangedCommand edit = new AllianceSettingChangedCommand(this.Device);
                        edit.SetAlliance(alliance);
                        edit.SetPlayer(this.Device.Player);

                        new AvailableServerCommandMessage(this.Device, edit.Handle()).Send();

                        foreach (AllianceMemberEntry op in alliance.GetAllianceMembers())
                        {
                            Level user = await ResourcesManager.GetPlayer(op.AvatarId);
                            if (ResourcesManager.IsPlayerOnline(user))
                            {
                                new AllianceStreamEntryMessage(user.Client) {StreamEntry = eventStreamEntry}.Send();
                            }
                        }

                        Resources.DatabaseManager.Save(alliance);
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
            catch 
            {
                //Exception
            }
        }
    }
}