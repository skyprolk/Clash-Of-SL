using System.IO;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Messages.Client
{
    // Packet 14322
    internal class AllianceInviteMessage : Message
    {
        public AllianceInviteMessage(Device device, Reader reader) : base(device, reader)
        {
        }

    }
}