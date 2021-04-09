using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.Packets.Commands.Server;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14305
    internal class JoinAllianceMessage : Message
    {
        long m_vAllianceId;

        public JoinAllianceMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.m_vAllianceId = this.Reader.ReadInt64();
        }

        internal override async void Process()
        {
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(m_vAllianceId);
                if (alliance != null)
                {
                    if (!alliance.IsAllianceFull())
                    {
                        this.Device.Player.Avatar.AllianceId = alliance.m_vAllianceId;
                        AllianceMemberEntry member = new AllianceMemberEntry(this.Device.Player.Avatar.UserId);
                        member.Role = 1;
                        alliance.AddAllianceMember(member);

                        JoinedAllianceCommand b = new JoinedAllianceCommand(this.Device);
                        b.SetAlliance(alliance);

                        AllianceRoleUpdateCommand c = new AllianceRoleUpdateCommand(this.Device);
                        c.SetAlliance(alliance);
                        c.SetRole(1);
                        c.Tick(this.Device.Player);

                        AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                        eventStreamEntry.ID = alliance.m_vChatMessages.Count + 1;
                        eventStreamEntry.SetSender(this.Device.Player.Avatar);
                        eventStreamEntry.EventType = 3;
                        alliance.AddChatMessage(eventStreamEntry);

                        new AvailableServerCommandMessage(this.Device, b.Handle()).Send();

                        new AvailableServerCommandMessage(this.Device, c.Handle()).Send();

                        new AllianceStreamMessage(Device, alliance).Send();

                        foreach (AllianceMemberEntry a in alliance.GetAllianceMembers())
                        {
                            Level l = await ResourcesManager.GetPlayer(a.AvatarId);
                            new AllianceStreamEntryMessage(l.Client) { StreamEntry = eventStreamEntry }.Send();
                        }
                    }
                }
            } catch (Exception) { }
        }
    }
}
