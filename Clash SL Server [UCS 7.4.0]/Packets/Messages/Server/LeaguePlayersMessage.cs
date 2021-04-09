using System.Collections.Generic;
using System.Linq;
using CSS.Core;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24503
    internal class LeaguePlayersMessage : Message
    {
        public LeaguePlayersMessage(Device client) : base(client)
        {
            this.Identifier = 24503;
        }

        internal override async void Encode()
        {
            List<byte> packet1 = new List<byte>();

            int i = 1;
            foreach (Level player in  ResourcesManager.m_vOnlinePlayers.Where(t => t.Avatar.m_vLeagueId == this.Device.Player.Avatar.m_vLeagueId) .OrderByDescending(t => t.Avatar.GetScore()))
            {
                if (i >= 51)
                    break;

                ClientAvatar pl = player.Avatar;
                if (pl.AvatarName != null)
                {
                    packet1.AddLong(pl.UserId);
                    packet1.AddString(pl.AvatarName);
                    packet1.AddInt(i);
                    packet1.AddInt(pl.GetScore());
                    packet1.AddInt(i);
                    packet1.AddInt(pl.m_vAvatarLevel);
                    packet1.AddInt(200);
                    packet1.AddInt(i);
                    packet1.AddInt(100);
                    packet1.AddInt(1);
                    packet1.AddLong(pl.AllianceId);
                    packet1.AddInt(1);
                    packet1.AddInt(1);
                    if (pl.AllianceId > 0)
                    {
                        packet1.Add(1);
                        packet1.AddLong(pl.AllianceId);
                        Alliance _Alliance = ObjectManager.GetAlliance(pl.AllianceId);
                        packet1.AddString(_Alliance.m_vAllianceName);
                        packet1.AddInt(_Alliance.m_vAllianceBadgeData);
                        packet1.AddLong(i);
                    }
                    else
                        packet1.Add(0);
                    i++;
                }
            }
            this.Data.AddInt(9000); //Season End
            this.Data.AddInt(i - 1);
            this.Data.AddRange(packet1);

        }
    }
}