using System;
using System.Collections.Generic;
using CSS.Core;
using CSS.Helpers;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    internal class PreviousGlobalPlayersMessage : Message
    {
        public PreviousGlobalPlayersMessage(Device client) : base(client)
        {
            this.Identifier = 24405;
        }

        internal override async void Encode()
        {
            try
            {
                List<byte> packet1 = new List<byte>();

                int i = 1;
                foreach (var player in ResourcesManager.m_vOnlinePlayers)
                {
                    try
                    {
                        ClientAvatar pl = player.Avatar;
                        packet1.AddLong(pl.UserId);
                        packet1.AddString(pl.AvatarName);
                        packet1.AddInt(i);
                        packet1.AddInt(pl.GetScore());
                        packet1.AddInt(i);
                        packet1.AddInt(pl.m_vAvatarLevel);
                        packet1.AddInt(100);
                        packet1.AddInt(1);
                        packet1.AddInt(100);
                        packet1.AddInt(1);
                        packet1.AddInt(pl.m_vLeagueId);
                        packet1.AddString("EN");
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
                        if (i >= 101)
                            break;
                        i++;
                    }
                    catch (Exception) { }
                }
                this.Data.AddInt(i - 1);
                this.Data.AddRange(packet1);
                this.Data.AddInt(DateTime.Now.Month - 1);
                this.Data.AddInt(DateTime.Now.Year);
            }
            catch (Exception) { }
        }
    }
}
