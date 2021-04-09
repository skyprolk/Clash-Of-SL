using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.GameOpCommands
{
    internal class MaxRessourcesCommand : GameOpCommand
    {
        public MaxRessourcesCommand(string[] Args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.Avatar.AccountPrivileges >= GetRequiredAccountPrivileges())
            {
                var p = level.Avatar;
                p.SetResourceCount(CSVManager.DataTables.GetResourceByName("Gold"), 999999999);
                p.SetResourceCount(CSVManager.DataTables.GetResourceByName("Elixir"), 999999999);
                p.SetResourceCount(CSVManager.DataTables.GetResourceByName("DarkElixir"), 999999999);
                p.m_vCurrentGems = 999999999;
                Processor.Send(new OwnHomeDataMessage(level.Client, level));
            }
            else
                SendCommandFailedMessage(level.Client);
        }
    }
}
