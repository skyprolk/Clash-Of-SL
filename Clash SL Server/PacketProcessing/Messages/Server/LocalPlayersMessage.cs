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

using System.Collections.Generic;
using System.Linq;
using CSS.Core;
using CSS.Helpers;

namespace CSS.PacketProcessing.Messages.Server
{
    internal class LocalPlayersMessage : Message
    {
        #region Public Constructors

        public LocalPlayersMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24404);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var packet = new List<byte>();
            var data = new List<byte>();

            var i = 1;
            foreach (var player in ResourcesManager.GetInMemoryLevels().OrderByDescending(t => t.GetPlayerAvatar().GetScore()))
            {
                var pl = player.GetPlayerAvatar();
                var id = pl.GetAllianceId();
                data.AddInt64(pl.GetId()); // The ID of the player
                data.AddString(pl.GetAvatarName() + " #" + pl.GetId()); // The Name of the Player
                data.AddInt32(i); // Rank of the player
                data.AddInt32(pl.GetScore()); // Number of Trophies of the player
                data.AddInt32(i); // Up/Down from previous rank -> (int - 1)
                data.AddInt32(pl.GetAvatarLevel()); // The score of the player
                data.AddInt32(100); // The number of successed attack
                data.AddInt32(1); // Unknown1 Int32
                data.AddInt32(100); // Number of successed defenses
                data.AddInt32(1); // Activation of successed attacks
                data.AddInt32(pl.GetLeagueId()); // League of the player
                data.AddString(pl.GetUserRegion().ToUpper()); // Locales
                data.AddInt64(pl.GetAllianceId()); // Clan ID
                data.AddInt32(1); // Unknown2
                data.AddInt32(1); // Unknown3
                if (pl.GetAllianceId() > 0)
                {
                    data.Add(1); // 1 = Have an alliance | 0 = No alliance
                    data.AddInt64(pl.GetAllianceId()); // Alliance ID
                    data.AddString(ObjectManager.GetAlliance(id).GetAllianceName()); // Alliance Name
                    data.AddInt32(ObjectManager.GetAlliance(id).GetAllianceBadgeData()); // Alliance Badge
                }
                else
                    data.Add(0);
                if (i >= 101)
                    break;
                i++;
            }

            packet.AddInt32(i - 1);
            packet.AddRange(data.ToArray());
            Encrypt(packet.ToArray());
        }

        #endregion Public Methods
    }
}
