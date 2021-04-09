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
using System.Collections.Generic;
using System.Linq;
using CSS.Core;
using CSS.Helpers;

namespace CSS.PacketProcessing.Messages.Server
{
    internal class GlobalPlayersMessage : Message
    {
        #region Public Constructors

        public GlobalPlayersMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24403);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();
            var packet1 = new List<byte>();

            var i = 1;

            foreach (var player in ResourcesManager.GetInMemoryLevels().OrderByDescending(t => t.GetPlayerAvatar().GetScore()))
            {
                var pl = player.GetPlayerAvatar();
                packet1.AddInt64(pl.GetId()); // The ID of the player
                packet1.AddString(pl.GetAvatarName() + " #" + pl.GetId()); // THe Name of the Player
                packet1.AddInt32(i); // Rank of the player
                packet1.AddInt32(pl.GetScore()); // Number of Trophies of the player
                packet1.AddInt32(i); // Up/Down from previous rank -> (int - 1)
                packet1.AddInt32(pl.GetAvatarLevel()); // The Level of the player
                packet1.AddInt32(100); // The number of successed attack
                packet1.AddInt32(i); // The number of failed attack
                packet1.AddInt32(100); // Number of successed defenses
                packet1.AddInt32(1); // Activation of successed attacks
                packet1.AddInt32(pl.GetLeagueId()); // League of the player
                packet1.AddString(pl.GetUserRegion().ToUpper()); // Locales
                packet1.AddInt64(pl.GetId()); // Clan ID
                packet1.AddInt32(1); // Unknown2
                packet1.AddInt32(1); // Unknown3
                if (pl.GetAllianceId() > 0)
                {
                    packet1.Add(1); // 1 = Have an alliance | 0 = No alliance
                    packet1.AddInt64(pl.GetAllianceId()); // Alliance ID
                    packet1.AddString(ObjectManager.GetAlliance(pl.GetAllianceId()).GetAllianceName()); // Alliance Name
                    packet1.AddInt32(ObjectManager.GetAlliance(pl.GetAllianceId()).GetAllianceBadgeData()); // Unknown4
                }
                else
                    packet1.Add(0);
                if (i >= 101)
                    break;
                i++;
            }
            data.AddInt32(i - 1);
            data.AddRange(packet1);
            data.AddInt32(i - 1);
            data.AddRange(packet1);

            data.AddInt32((int) TimeSpan.FromDays(7).TotalSeconds);
            data.AddInt32(DateTime.Now.Year);
            data.AddInt32(DateTime.Now.Month);
            data.AddInt32(DateTime.Now.Year);
            data.AddInt32(DateTime.Now.Month - 1);
            Encrypt(data.ToArray());
        }

        #endregion Public Methods
    }
}
