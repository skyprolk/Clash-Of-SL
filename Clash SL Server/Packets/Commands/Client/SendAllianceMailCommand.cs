using System;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Commands.Client
{
    // Packet 537
    internal class SendAllianceMailCommand : Command
    {
        internal string m_vMailContent;

        public SendAllianceMailCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vMailContent = this.Reader.ReadString();
            this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            try
            {
                var avatar = this.Device.Player.Avatar;
                var allianceId = avatar.AllianceId;
                if (allianceId > 0)
                {
                    var alliance = ObjectManager.GetAlliance(allianceId);
                    if (alliance != null)
                    {
                        var mail = new AllianceMailStreamEntry();
                        mail.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        mail.SetSender(avatar);
                        mail.IsNew = 2;
                        mail.SenderId = avatar.UserId;
                        mail.AllianceId = allianceId;
                        mail.AllianceBadgeData = alliance.m_vAllianceBadgeData;
                        mail.AllianceName = alliance.m_vAllianceName;
                        mail.Message = m_vMailContent;

                        foreach (var onlinePlayer in ResourcesManager.m_vOnlinePlayers)
                        {
                            if (onlinePlayer.Avatar.AllianceId == allianceId)
                            {
                                var p = new AvatarStreamEntryMessage(onlinePlayer.Client);
                                p.SetAvatarStreamEntry(mail);
                                p.Send();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}