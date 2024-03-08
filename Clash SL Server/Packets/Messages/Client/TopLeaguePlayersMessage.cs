using System.IO;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14503
    internal class TopLeaguePlayersMessage : Message
    {
        public TopLeaguePlayersMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
          new LeaguePlayersMessage(Device).Send();
        }
    }
}