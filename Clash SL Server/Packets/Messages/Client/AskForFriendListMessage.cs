using System.IO;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 10105
    internal class AskForFriendListMessage : Message
    {
        public AskForFriendListMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
            new FriendListMessage(this.Device).Send();
        }
    }
}