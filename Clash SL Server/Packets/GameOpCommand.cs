using System;
using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets
{
    internal class GameOpCommand
    {
        byte m_vRequiredAccountPrivileges;

        public static void SendCommandFailedMessage(Device c)
        {
            Console.WriteLine("GameOp command failed. Insufficient privileges. Requster ID -> " + c.Player.Avatar.UserId);
            var p = new GlobalChatLineMessage(c)
            {
                Message = "GameOp command failed. Insufficient privileges.",
                HomeId = 0,
                CurrentHomeId = 0,
                LeagueId = 22,
                PlayerName = "Clash SL Server AI"
            };
            p.Send();
        }

        public virtual void Execute(Level level)
        {
        }

        public byte GetRequiredAccountPrivileges() => m_vRequiredAccountPrivileges;

        public void SetRequiredAccountPrivileges(byte level)
        {
            m_vRequiredAccountPrivileges = level;
        }
    }
}