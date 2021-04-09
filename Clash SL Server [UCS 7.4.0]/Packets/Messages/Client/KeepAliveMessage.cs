using System.IO;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 10108
    internal class KeepAliveMessage : Message
    {
        public KeepAliveMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
           new KeepAliveOkMessage(Device, this).Send();
        }
    }
}