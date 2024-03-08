using System;
using System.IO;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Messages.Client
{
    // Packet 10905
    internal class NewsSeenMessage : Message
    {
        public NewsSeenMessage(Device device, Reader reader) : base(device, reader)
        {

        }
    }
}
