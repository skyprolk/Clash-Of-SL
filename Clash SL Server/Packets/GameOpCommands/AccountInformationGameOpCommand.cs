using System;
using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.GameOpCommands
{
    internal class AccountInformationGameOpCommand : GameOpCommand
    {
        private readonly string[] m_vArgs;
        private string Message;
        public AccountInformationGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(5);
        }
        public override async void Execute(Level level)
        {
            if (level.Avatar.AccountPrivileges >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 2)
                {
                    try
                    {
                        long id = Convert.ToInt64(m_vArgs[1]);
                        Level l = await ResourcesManager.GetPlayer(id);
                        if (l != null)
                        {
                            ClientAvatar acc = l.Avatar;
                            Message = "Player Info : \n\n" + "ID = " + id + "\nName = " + acc.AvatarName +
                                      "\nCreation Date : " + acc.m_vAccountCreationDate + "\nRegion : " + acc.Region +
                                      "\nIP Address : " + l.Avatar.IPAddress;
                            if (acc.AllianceId != 0)
                            {
                                Alliance a = ObjectManager.GetAlliance(acc.AllianceId);
                                Message = Message + "\nClan Name : " + a.m_vAllianceName;
                                switch (await acc.GetAllianceRole())
                                {
                                    case 1:
                                        Message = Message + "\nClan Role : Member";
                                        break;

                                    case 2:
                                        Message = Message + "\nClan Role : Leader";
                                        break;

                                    case 3:
                                        Message = Message + "\nClan Role : Elder";
                                        break;

                                    case 4:
                                        Message = Message + "\nClan Role : Co-Leader";
                                        break;

                                    default:
                                        Message = Message + "\nClan Role : Unknown";
                                        break;
                                }
                            }
                            Message = Message + "\nLevel : " + acc.m_vAvatarLevel + "\nTrophies : " + acc.GetScore() +
                                      "\nTown Hall Level : " + (acc.m_vTownHallLevel + 1) +
                                      "\nAlliance Castle Level : " + (acc.GetAllianceCastleLevel() + 1);

                            var avatar = level.Avatar;
                            AllianceMailStreamEntry mail = new AllianceMailStreamEntry();
                            mail.SenderId = avatar.UserId;
                            mail.m_vSenderName = avatar.AvatarName;
                            mail.IsNew = 2;
                            mail.AllianceId = 0;
                            mail.AllianceBadgeData = 1526735450;
                            mail.AllianceName = "CSS Server Information";
                            mail.Message = Message;
                            mail.m_vSenderLevel = avatar.m_vAvatarLevel;
                            mail.m_vSenderLeagueId = avatar.m_vLeagueId;

                            AvatarStreamEntryMessage p = new AvatarStreamEntryMessage(level.Client);
                            p.SetAvatarStreamEntry(mail);
                            Processor.Send(p);
                        }
                    }
                    catch (Exception)
                    {
                        GlobalChatLineMessage c = new GlobalChatLineMessage(level.Client)
                        {
                            Message = "Command Failed, Wrong Format Or User Doesn't Exist (/accinfo id).",
                            HomeId = level.Avatar.UserId,
                            CurrentHomeId = level.Avatar.UserId,
                            LeagueId = 22,
                            PlayerName = "Clash SL Server AI"
                        };

                        Processor.Send(c);
                        return;
                    }
                }
                else
                {
                    GlobalChatLineMessage b = new GlobalChatLineMessage(level.Client)
                    {
                        Message = "Command Failed, Wrong Format (/accinfo id).",
                        HomeId = level.Avatar.UserId,
                        CurrentHomeId = level.Avatar.UserId,
                        LeagueId = 22,
                        PlayerName = "Clash SL Server AI"
                    };
                    Processor.Send(b);
                }
            }
        }
    }
}


