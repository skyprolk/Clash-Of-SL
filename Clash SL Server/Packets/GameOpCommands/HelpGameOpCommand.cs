using System;
using CSS.Core;
using CSS.Core.Network;
using CSS.Core.Threading;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.GameOpCommands
{
    internal class HelpGameOpCommand: GameOpCommand
    {
        readonly string[] m_vArgs;

        public HelpGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.Avatar.AccountPrivileges >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 1)
                {
                    ClientAvatar avatar = level.Avatar;
                    var mail = new AllianceMailStreamEntry();
                    mail.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    mail.SetSender(avatar);
                    mail.IsNew = 2;
                    mail.AllianceId = 0;
                    mail.AllianceBadgeData = 1526735450;
                    mail.AllianceName = "CSS Server Commands Help";
                    mail.Message = @"/help" +
                        "\n/ban" +
                        "\n/kick" +
                        "\n/rename " +
                        "\n/setprivileges" +
                        "\n/shutdown" +
                        "\n/unban" +
                        "\n/visit" +
                        "\n/sysmsg" +
                        "\n/id" +
                        "\n/min" +
                        "\n/max" +
                        "\n/saveacc" +
                        "\n/saveall" +
                        "\n/becomeleader" +
                        "\n/status" +
                        "\n/help"+
                        "\n/accinfo";

                    var p = new AvatarStreamEntryMessage(level.Client);
                    p.SetAvatarStreamEntry(mail);
                    Processor.Send(p);
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }
    }
}
