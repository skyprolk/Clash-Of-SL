using System.IO;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 10113
    internal class GetDeviceTokenMessage : Message
    {
        public GetDeviceTokenMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
            new SetDeviceTokenMessage(Device).Send();
        }
    }
}