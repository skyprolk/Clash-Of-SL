
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Core.Checker;
using CSS.Core.Network;
using CSS.Core.Threading;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14715
    internal class SendGlobalChatLineMessage : Message
    {
        public SendGlobalChatLineMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        public string Message { get; set; }

        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
        }

        internal override async void Process()
        {
            if (Message.Length > 0 && Message.Length < 200)
            {
                if (Message[0] == '/')
                {
                    object obj = GameOpCommandFactory.Parse(Message);
                    if (obj != null)
                    {
                        string player = "";
                        if (this.Device.Player != null)
                            player += " (" + this.Device.Player.Avatar.UserId + ", " +
                                      this.Device.Player.Avatar.AvatarName + ")";
                        ((GameOpCommand) obj).Execute(this.Device.Player);
                    }
                }
                else
                {
                    long senderId = this.Device.Player.Avatar.UserId;
                    string senderName = this.Device.Player.Avatar.AvatarName;

                    bool badword = DirectoryChecker.badwords.Any(s => Message.Contains(s));

                    try
                    {
                        if (badword)
                        {
                            foreach (Level pl in ResourcesManager.m_vOnlinePlayers)
                            {
                                if (pl.Avatar.Region == this.Device.Player.Avatar.Region)
                                {
                                    string NewMessage = "";

                                    for (int i = 0; i < Message.Length; i++){NewMessage += "*";}

                                    GlobalChatLineMessage p = new GlobalChatLineMessage(pl.Client)
                                    {
                                        PlayerName = senderName,
                                        Message = NewMessage,
                                        HomeId = senderId,
                                        CurrentHomeId = senderId,
                                        LeagueId = this.Device.Player.Avatar.m_vLeagueId
                                    };

                                    p.SetAlliance(ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId));
                                    p.Send();
                                }
                            }
                        }
                        else
                        {
                            foreach (Level onlinePlayer in ResourcesManager.m_vOnlinePlayers)
                            {
                                if (onlinePlayer.Avatar.Region == this.Device.Player.Avatar.Region)
                                {
                                    GlobalChatLineMessage p = new GlobalChatLineMessage(onlinePlayer.Client)
                                    {
                                        PlayerName = senderName,
                                        Message = this.Message,
                                        HomeId = senderId,
                                        CurrentHomeId = senderId,
                                        LeagueId = this.Device.Player.Avatar.m_vLeagueId
                                    };
                                    p.SetAlliance(ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId));
                                    p.Send();
                                    Logger.Write($"Chat Message: '{Message}' from '{senderName}':'{senderId}'");
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
    }
}