using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.Logic.StreamEntry;
using CSS.Packets.Messages.Server;
using CSS.Packets.Commands.Server;
using System.Threading.Tasks;
using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 543
    internal class KickAllianceMemberCommand : Command
    {
        public KickAllianceMemberCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vAvatarId = this.Reader.ReadInt64();
            this.Reader.ReadByte();
            this.m_vMessage = this.Reader.ReadString();
            this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            try
            {
                var targetAccount = await ResourcesManager.GetPlayer(m_vAvatarId);
                if (targetAccount != null)
                {
                    var targetAvatar = targetAccount.Avatar;
                    var targetAllianceId = targetAvatar.AllianceId;
                    var requesterAvatar = this.Device.Player.Avatar;
                    var requesterAllianceId = requesterAvatar.AllianceId;
                    if (requesterAllianceId > 0 && targetAllianceId == requesterAllianceId)
                    {
                        var alliance = ObjectManager.GetAlliance(requesterAllianceId);
                        var requesterMember = alliance.m_vAllianceMembers[requesterAvatar.UserId];
                        var targetMember = alliance.m_vAllianceMembers[m_vAvatarId];
                        if (targetMember.HasLowerRoleThan(requesterMember.Role))
                        {
                            targetAvatar.AllianceId = 0;
                            alliance.RemoveMember(m_vAvatarId);
                            if (ResourcesManager.IsPlayerOnline(targetAccount))
                            {
                                var leaveAllianceCommand = new LeavedAllianceCommand(this.Device);
                                leaveAllianceCommand.SetAlliance(alliance);
                                leaveAllianceCommand.SetReason(2); //Kick
                                new AvailableServerCommandMessage(targetAccount.Client, leaveAllianceCommand.Handle()).Send();

                                var kickOutStreamEntry = new AllianceKickOutStreamEntry();
                                kickOutStreamEntry.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                                kickOutStreamEntry.SetSender(requesterAvatar);
                                kickOutStreamEntry.IsNew = 2;
                                kickOutStreamEntry.SetAllianceId(alliance.m_vAllianceId);
                                kickOutStreamEntry.SetAllianceBadgeData(alliance.m_vAllianceBadgeData);
                                kickOutStreamEntry.SetAllianceName(alliance.m_vAllianceName);
                                kickOutStreamEntry.SetMessage(m_vMessage);

                                var p = new AvatarStreamEntryMessage(targetAccount.Client);
                                p.SetAvatarStreamEntry(kickOutStreamEntry);
                                p.Send();
                            }

                            var eventStreamEntry = new AllianceEventStreamEntry();
                            eventStreamEntry.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                            eventStreamEntry.SetSender(targetAvatar);
                            eventStreamEntry.m_vAvatarName = this.Device.Player.Avatar.AvatarName;
                            eventStreamEntry.EventType = 1;
                            alliance.AddChatMessage(eventStreamEntry);

                            foreach (AllianceMemberEntry op in alliance.GetAllianceMembers())
                            {
                                Level alliancemembers = await ResourcesManager.GetPlayer(op.AvatarId);
                                if (alliancemembers.Client != null)
                                {
                                    new AllianceStreamEntryMessage(alliancemembers.Client) {StreamEntry = eventStreamEntry}.Send();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        internal long m_vAvatarId;
        internal string m_vMessage;
    }
}
