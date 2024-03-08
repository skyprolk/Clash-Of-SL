using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.Packets.Commands.Server;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14308
    internal class LeaveAllianceMessage : Message
    {
        public static bool done;

        public LeaveAllianceMessage(Device device, Reader reader) : base(device, reader)
        {

        }

        internal override async void Process()
        {
            try
            {
                ClientAvatar avatar = this.Device.Player.Avatar;
                Alliance alliance = ObjectManager.GetAlliance(avatar.AllianceId);

                if (await avatar.GetAllianceRole() == 2 && alliance.GetAllianceMembers().Count > 1)
                {
                    List<AllianceMemberEntry> members = alliance.GetAllianceMembers();
                    foreach (AllianceMemberEntry player in members.Where(player => player.Role >= 3))
                    {
                        player.Role = 2;

                        if (ResourcesManager.IsPlayerOnline(await ResourcesManager.GetPlayer(player.AvatarId)))
                        {

                            Level l = await ResourcesManager.GetPlayer(player.AvatarId);

                            AllianceRoleUpdateCommand c = new AllianceRoleUpdateCommand(l.Client);
                            c.SetAlliance(alliance);
                            c.SetRole(2);
                            c.Tick(l);

                             new AvailableServerCommandMessage(l.Client, c.Handle()).Send();
                        }
                        done = true;
                        break;
                    }
                    if (!done)
                    {
                        int count = alliance.GetAllianceMembers().Count;
                        Random rnd = new Random();
                        int id = rnd.Next(1, count);
                        while (id != this.Device.Player.Avatar.UserId)
                            id = rnd.Next(1, count);
                        int loop = 0;
                        foreach (AllianceMemberEntry player in members)
                        {
                            loop++;
                            if (loop == id)
                            {
                                player.Role = 2;
                                if (ResourcesManager.IsPlayerOnline(await ResourcesManager.GetPlayer(player.AvatarId)))
                                {
                                    Level l2 = await ResourcesManager.GetPlayer(player.AvatarId);
                                    AllianceRoleUpdateCommand e = new AllianceRoleUpdateCommand(l2.Client);
                                    e.SetAlliance(alliance);
                                    e.SetRole(2);
                                    e.Tick(l2);

                                    new AvailableServerCommandMessage(l2.Client, e.Handle()).Send();
                                }
                                break;
                            }
                        }
                    }
                }
                LeavedAllianceCommand a = new LeavedAllianceCommand(this.Device);
                a.SetAlliance(alliance);
                a.SetReason(1);

                new AvailableServerCommandMessage(Device, a.Handle()).Send();

                alliance.RemoveMember(avatar.UserId);
                avatar.AllianceId = 0;

                if (alliance.GetAllianceMembers().Count > 0)
                {
                    AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                    eventStreamEntry.ID = alliance.m_vChatMessages.Count + 1;
                    eventStreamEntry.SetSender(avatar);
                    eventStreamEntry.EventType = 4;
                    alliance.AddChatMessage(eventStreamEntry);
                    foreach (Level onlinePlayer in ResourcesManager.m_vOnlinePlayers)
                    {
                        if (onlinePlayer.Avatar.AllianceId == alliance.m_vAllianceId)
                        {
                            new AllianceStreamEntryMessage(onlinePlayer.Client) { StreamEntry = eventStreamEntry }.Send();
                        }
                    }
                }
                else
                {
                    Resources.DatabaseManager.RemoveAlliance(alliance);
                }
                new LeaveAllianceOkMessage(Device, alliance).Send();
            } catch (Exception) { }
        }
    }
}
