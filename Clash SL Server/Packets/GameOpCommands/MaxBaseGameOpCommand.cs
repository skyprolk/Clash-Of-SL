using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.GameOpCommands
{
    class MaxBaseGameOpCommand : GameOpCommand
    {
        public MaxBaseGameOpCommand(string[] Args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.Avatar.AccountPrivileges >= GetRequiredAccountPrivileges())
            {
                string Home;

                using (StreamReader sr = new StreamReader(@"Gamefiles/level/PVP/Base55.json"))
                {
                    Home = sr.ReadToEnd();
                    ResourcesManager.SetGameObject(level, Home);
                    Processor.Send(new OutOfSyncMessage(level.Client));
                }
            }
            else
                SendCommandFailedMessage(level.Client);
        }
    }
}
