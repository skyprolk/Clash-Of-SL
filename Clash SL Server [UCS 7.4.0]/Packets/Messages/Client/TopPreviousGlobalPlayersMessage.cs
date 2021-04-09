using System.IO;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14406
    internal class TopPreviousGlobalPlayersMessage : Message
    {
        public TopPreviousGlobalPlayersMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
            new PreviousGlobalPlayersMessage(Device).Send();
        }
    }
}