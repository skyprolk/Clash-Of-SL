using UCS.Core;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    internal class SetLeagueGameOpCommand : GameOpCommand
    {
        private readonly string[] m_vArgs;

        public SetLeagueGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(5);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 2)
                {
                    long id;
                    if (long.TryParse(m_vArgs[1], out id))
                    {
                        int newLeague;
                        if (int.TryParse(m_vArgs[2], out newLeague))
                        {
                            var l = ResourcesManager.GetPlayer(id);
                            if (l != null)
                            {
                                l.GetPlayerAvatar().SetLeagueId(newLeague);
                            }
                            else
                            {
                                Debugger.WriteLine("SetLeague failed: id " + id + " not found");
                            }
                        }
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }
    }
}
