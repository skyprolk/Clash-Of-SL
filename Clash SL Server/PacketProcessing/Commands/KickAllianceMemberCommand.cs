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

using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.Logic.StreamEntry;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Commands
{
    internal class KickAllianceMemberCommand : Command
    {
        #region Public Constructors

        public KickAllianceMemberCommand(CoCSharpPacketReader br)
        {
            m_vAvatarId = br.ReadInt64WithEndian();
            br.ReadByte();
            m_vMessage = br.ReadScString();
            br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var targetAccount = ResourcesManager.GetPlayer(m_vAvatarId, true);
            if (targetAccount != null)
            {
                var targetAvatar = targetAccount.GetPlayerAvatar();
                var targetAllianceId = targetAvatar.GetAllianceId();
                var requesterAvatar = level.GetPlayerAvatar();
                var requesterAllianceId = requesterAvatar.GetAllianceId();
                if (requesterAllianceId > 0 && targetAllianceId == requesterAllianceId)
                {
                    var alliance = ObjectManager.GetAlliance(requesterAllianceId);
                    var requesterMember = alliance.GetAllianceMember(requesterAvatar.GetId());
                    var targetMember = alliance.GetAllianceMember(m_vAvatarId);
                    if (targetMember.HasLowerRoleThan(requesterMember.GetRole()))
                    {
                        targetAvatar.SetAllianceId(0);
                        alliance.RemoveMember(m_vAvatarId);
                        if (ResourcesManager.IsPlayerOnline(targetAccount))
                        {
                            var leaveAllianceCommand = new LeaveAllianceCommand();
                            leaveAllianceCommand.SetAlliance(alliance);
                            leaveAllianceCommand.SetReason(2); //Kick
                            var availableServerCommandMessage =
                                new AvailableServerCommandMessage(targetAccount.GetClient());
                            availableServerCommandMessage.SetCommandId(2);
                            availableServerCommandMessage.SetCommand(leaveAllianceCommand);
                            PacketManager.ProcessOutgoingPacket(availableServerCommandMessage);

                            var kickOutStreamEntry = new AllianceKickOutStreamEntry();
                            kickOutStreamEntry.SetId(
                                (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                            kickOutStreamEntry.SetAvatar(requesterAvatar);
                            kickOutStreamEntry.SetIsNew(0);
                            kickOutStreamEntry.SetAllianceId(alliance.GetAllianceId());
                            kickOutStreamEntry.SetAllianceBadgeData(alliance.GetAllianceBadgeData());
                            kickOutStreamEntry.SetAllianceName(alliance.GetAllianceName());
                            kickOutStreamEntry.SetMessage(m_vMessage);
                            var p = new AvatarStreamEntryMessage(targetAccount.GetClient());
                            p.SetAvatarStreamEntry(kickOutStreamEntry);
                            PacketManager.ProcessOutgoingPacket(p);
                        }

                        var eventStreamEntry = new AllianceEventStreamEntry();
                        eventStreamEntry.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        eventStreamEntry.SetAvatar(targetAvatar);
                        eventStreamEntry.SetEventType(1);
                        eventStreamEntry.SetAvatarId(requesterAvatar.GetId());
                        eventStreamEntry.SetAvatarName(requesterAvatar.GetAvatarName());
                        alliance.AddChatMessage(eventStreamEntry);

                        foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                            if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == requesterAllianceId)
                            {
                                var p = new AllianceStreamEntryMessage(onlinePlayer.GetClient());
                                p.SetStreamEntry(eventStreamEntry);
                                PacketManager.ProcessOutgoingPacket(p);
                            }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Fields

        readonly long m_vAvatarId;
        readonly string m_vMessage;

        #endregion Private Fields
    }
}