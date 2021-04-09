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
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Commands
{
    //Commande 0x219
    internal class SendAllianceMailCommand : Command
    {
        #region Private Fields

        readonly string m_vMailContent;

        #endregion Private Fields

        #region Public Constructors

        public SendAllianceMailCommand(CoCSharpPacketReader br)
        {
            m_vMailContent = br.ReadScString();
            br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var avatar = level.GetPlayerAvatar();
            var allianceId = avatar.GetAllianceId();
            if (allianceId > 0)
            {
                var alliance = ObjectManager.GetAlliance(allianceId);
                if (alliance != null)
                {
                    var mail = new AllianceMailStreamEntry();
                    mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetAvatar(avatar);
                    mail.SetIsNew(2);
                    mail.SetSenderId(avatar.GetId());
                    mail.SetAllianceId(allianceId);
                    mail.SetAllianceBadgeData(alliance.GetAllianceBadgeData());
                    mail.SetAllianceName(alliance.GetAllianceName());
                    mail.SetMessage(m_vMailContent);

                    foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                        if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == allianceId)
                        {
                            var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                            p.SetAvatarStreamEntry(mail);
                            PacketManager.ProcessOutgoingPacket(p);
                        }
                }
            }
        }

        #endregion Public Methods
    }
}