using System.IO;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14341
    internal class AskForBookmarkMessage : Message
    {
        public AskForBookmarkMessage(Device device, Reader reader) : base(device, reader)
        {
        }


        internal override void Process()
        {
            new BookmarkListMessage(this.Device).Send();
        }
    }
}