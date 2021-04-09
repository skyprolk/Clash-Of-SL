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
using CSS.Core.Network;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.GameOpCommands
{
    internal class GetIdGameopCommand : GameOpCommand
    {
        #region Private Fields

        readonly string[] m_vArgs;

        #endregion Private Fields

        #region Public Constructors

        public GetIdGameopCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(0);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 1)
                {
                    var avatar = level.GetPlayerAvatar();
                    var mail = new AllianceMailStreamEntry();
                    mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetSenderId(avatar.GetId());
                    mail.SetSenderAvatarId(avatar.GetId());
                    mail.SetSenderName(avatar.GetAvatarName());
                    mail.SetIsNew(0);
                    mail.SetAllianceId(1);
                    mail.SetAllianceBadgeData(0);
                    mail.SetAllianceName("css Information");
                    mail.SetMessage("Your Player ID: " + level.GetPlayerAvatar().GetId());
                    mail.SetSenderLevel(avatar.GetAvatarLevel());
                    mail.SetSenderLeagueId(avatar.GetLeagueId());

                    var p = new AvatarStreamEntryMessage(level.GetClient());
                    p.SetAvatarStreamEntry(mail);
                    PacketManager.ProcessOutgoingPacket(p);
                }
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }

        #endregion Public Methods
    }
}