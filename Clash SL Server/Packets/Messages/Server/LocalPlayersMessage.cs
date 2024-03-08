using System;
using System.Collections.Generic;
using System.Linq;
using CSS.Core;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24404
    internal class LocalPlayersMessage : Message
    {
        public LocalPlayersMessage(Device client) : base(client)
        {
            this.Identifier = 24404;
        }

        internal override async void Encode()
        {
            List<byte> data = new List<byte>();
            var i = 0;

            foreach (Level player in ResourcesManager.m_vInMemoryLevels.Values.ToList().OrderByDescending(t => t.Avatar.GetScore()))
            {
                try
                {
                    ClientAvatar pl = player.Avatar;
                    if (i >= 100)
                        break;
                    data.AddLong(pl.UserId);
                    data.AddString(pl.AvatarName);
                    data.AddInt(i + 1);
                    data.AddInt(pl.GetScore());
                    data.AddInt(i + 1);
                    data.AddInt(pl.m_vAvatarLevel);
                    data.AddInt(100);
                    data.AddInt(1);
                    data.AddInt(100);
                    data.AddInt(1);
                    data.AddInt(pl.m_vLeagueId);
                    data.AddString(pl.Region.ToUpper());
                    data.AddLong(pl.AllianceId);
                    data.AddInt(1);
                    data.AddInt(1);
                    if (pl.AllianceId > 0)
                    {
                        data.Add(1); // 1 = Have an alliance | 0 = No alliance
                        data.AddLong(pl.AllianceId);
                        Alliance _Alliance = ObjectManager.GetAlliance(pl.AllianceId);
                        data.AddString(_Alliance.m_vAllianceName);
                        data.AddInt(_Alliance.m_vAllianceBadgeData);
                    }
                    else
                        data.Add(0);
                    i++;
                }
                catch (Exception) { }
            }

            this.Data.AddInt(i);
            this.Data.AddRange(data.ToArray());
        }
    }
}