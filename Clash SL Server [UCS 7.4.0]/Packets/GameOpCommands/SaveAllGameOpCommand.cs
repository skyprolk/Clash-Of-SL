using System.Linq;
using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.GameOpCommands
{
    internal class SaveAllGameOpCommand : GameOpCommand
    {
        public SaveAllGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(3);
        }

        public override async void Execute(Level level)
        {
            if (level.Avatar.AccountPrivileges >= GetRequiredAccountPrivileges())
            {
                /* Starting saving of players */
                var pm = new GlobalChatLineMessage(level.Client)
                {
                    Message = "Starting saving process of every player!",
                    HomeId = 0,
                    CurrentHomeId = 0,
                    LeagueId = 22,
                    PlayerName = "CSS Bot"
                };
                Processor.Send(pm);
                Resources.DatabaseManager.Save(ResourcesManager.m_vInMemoryLevels.Values.ToList());
                var p = new GlobalChatLineMessage(level.Client)
                {
                    Message = "All Players are saved!",
                     HomeId = 0,
                    CurrentHomeId = 0,
                    LeagueId = 22,
                    PlayerName = "CSS Bot"
                };
                /* Confirmation */
                Processor.Send(p);
                /* Starting saving of Clans */
                var pmm = new GlobalChatLineMessage(level.Client)
                {
                    Message = "Starting with saving of every Clan!",
                    HomeId = 0,
                    CurrentHomeId = 0,
                    LeagueId = 22,
                    PlayerName = "CSS Bot"
                };
                Processor.Send(pmm);
                /* Confirmation */
                //var clans = Resources.DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances());
                //clans.Wait();
                var pmp = new GlobalChatLineMessage(level.Client)
                {
                    Message = "All Clans are saved!",
                    HomeId = 0,
                    CurrentHomeId = 0,
                    LeagueId = 22,
                    PlayerName = "CSS Bot"
                };
                Processor.Send(pmp);
            }
            else
            {
                var p = new GlobalChatLineMessage(level.Client)
                {
                    Message = "GameOp command failed. Access to Admin GameOP is prohibited.",
                    HomeId = 0,
                    CurrentHomeId = 0,
                    LeagueId = 22,
                    PlayerName = "CSS Bot"
                };

                Processor.Send(p);
            }
        }
        readonly string[] m_vArgs;
    }
}
    