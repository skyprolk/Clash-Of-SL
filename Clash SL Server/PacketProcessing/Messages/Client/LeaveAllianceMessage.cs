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
using System.Linq;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Messages.Client
{
    class LeaveAllianceMessage : Message
    {
        #region Public Fields

        public static bool done;

        #endregion Public Fields

        #region Public Constructors

        public LeaveAllianceMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {

        }

        #endregion Public Constructors

        #region Public Methods

        public override void Decode()
        {

        }

        public override void Process(Level level)
        {
            var avatar = level.GetPlayerAvatar();
            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());

            if (avatar.GetAllianceRole() == 2 && alliance.GetAllianceMembers().Count > 1)
            {
                var members = alliance.GetAllianceMembers();
                foreach (AllianceMemberEntry player in members.Where(player => player.GetRole() >= 3))
                {
                    player.SetRole(2);
                    done = true;
                    break;
                }
                if (!done)
                {
                    var count = alliance.GetAllianceMembers().Count;
                    var rnd = new Random();
                    var id = rnd.Next(1, count);
                    while (id != level.GetPlayerAvatar().GetId())
                        id = rnd.Next(1, count);
                    var loop = 0;
                    foreach (AllianceMemberEntry player in members)
                    {
                        loop++;
                        if (loop == id)
                        {
                            player.SetRole(2);
                            break;
                        }
                    }
                }
            }

            alliance.RemoveMember(avatar.GetId());
            avatar.SetAllianceId(0);

            if (alliance.GetAllianceMembers().Count > 0)
            {
                var eventStreamEntry = new AllianceEventStreamEntry();
                eventStreamEntry.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                eventStreamEntry.SetAvatar(avatar);
                eventStreamEntry.SetEventType(4);
                eventStreamEntry.SetAvatarId(avatar.GetId());
                eventStreamEntry.SetAvatarName(avatar.GetAvatarName());
                alliance.AddChatMessage(eventStreamEntry);
                foreach (Level onlinePlayer in ResourcesManager.GetOnlinePlayers())
                    if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == alliance.GetAllianceId())
                    {
                        AllianceStreamEntryMessage p = new AllianceStreamEntryMessage(onlinePlayer.GetClient());
                        p.SetStreamEntry(eventStreamEntry);
                        PacketManager.ProcessOutgoingPacket(p);
                    }
            }
            else
            {
                DatabaseManager.Singelton.RemoveAlliance(alliance);
            }
            PacketManager.ProcessOutgoingPacket(new LeaveAllianceOkMessage(Client, alliance));
        }
        #endregion Public Methods
    }
}
