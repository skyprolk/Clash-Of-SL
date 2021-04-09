using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.DataSlots;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14343
    internal class AddToBookmarkMessage : Message
    {
        public AddToBookmarkMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        private long id;

        internal override void Decode()
        {
            this.id = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            this.Device.Player.Avatar.BookmarkedClan.Add(new BookmarkSlot(id));;
            new BookmarkAddAllianceMessage(Device).Send();
        }
    }
}