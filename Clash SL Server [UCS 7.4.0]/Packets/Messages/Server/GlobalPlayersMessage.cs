using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Helpers;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24403
    internal class GlobalPlayersMessage : Message
    {
        public GlobalPlayersMessage(Device client) : base(client)
        {
            this.Identifier = 24403;
        }

        internal override async void Encode()
        {
            List<byte> packet1 = new List<byte>();
            int i = 0;

            foreach (var player in ResourcesManager.m_vInMemoryLevels.Values.ToList().OrderByDescending(t => t.Avatar.GetScore()))
            {
                try
                {
                    if (player.Avatar.m_vAvatarLevel >= 70)
                    {
                        var pl = player.Avatar;
                        if (i >= 100)
                            break;
                        packet1.AddLong(pl.UserId);
                        packet1.AddString(pl.AvatarName);
                        packet1.AddInt(i + 1);
                        packet1.AddInt(pl.GetScore());
                        packet1.AddInt(i + 1);
                        packet1.AddInt(pl.m_vAvatarLevel);
                        packet1.AddInt(100);
                        packet1.AddInt(i);
                        packet1.AddInt(100);
                        packet1.AddInt(1);
                        packet1.AddInt(pl.m_vLeagueId);
                        packet1.AddString(pl.Region.ToUpper());
                        packet1.AddLong(pl.UserId);
                        packet1.AddInt(1);
                        packet1.AddInt(1);
                        if (pl.AllianceId > 0)
                        {
                            packet1.Add(1); // 1 = Have an alliance | 0 = No alliance
                            packet1.AddLong(pl.AllianceId);
                            Alliance _Alliance = ObjectManager.GetAlliance(pl.AllianceId);
                            packet1.AddString(_Alliance.m_vAllianceName);
                            packet1.AddInt(_Alliance.m_vAllianceBadgeData);
                        }
                        else
                            packet1.Add(0);
                        i++;
                    }
                }
                catch (Exception) { }
            }

            this.Data.AddInt(i);
            this.Data.AddRange(packet1);
            this.Data.AddInt(i);
            this.Data.AddRange(packet1);

            this.Data.AddInt((int) TimeSpan.FromDays(7).TotalSeconds);
            this.Data.AddInt(DateTime.Now.Year);
            this.Data.AddInt(DateTime.Now.Month);
            this.Data.AddInt(DateTime.Now.Year);
            this.Data.AddInt(DateTime.Now.Month - 1);
        }
    }
}