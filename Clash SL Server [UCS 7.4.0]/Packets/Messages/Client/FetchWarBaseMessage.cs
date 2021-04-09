using System.IO;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Messages.Client
{
    // Packet ?
    internal class FetchWarBaseMessage : Message
    {
        public FetchWarBaseMessage(Device device, Reader reader) : base(device, reader)
        {
        }
    }
}