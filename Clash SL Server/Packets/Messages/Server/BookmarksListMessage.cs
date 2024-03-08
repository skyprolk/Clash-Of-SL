using System.Collections.Generic;
using CSS.Core;
using CSS.Helpers.List;
using CSS.Logic;
using CSS.Logic.DataSlots;

namespace CSS.Packets.Messages.Server
{
    // Packet 24341
    internal class BookmarkListMessage : Message
    {
        public ClientAvatar Player;
        public int I;

        public BookmarkListMessage(Device client) : base(client)
        {
            this.Identifier = 24341;
            this.Player = client.Player.Avatar;
            I = 0;
        }

        internal override async void Encode()
        {
            List<byte> list = new List<byte>();
            List<BookmarkSlot> rem = new List<BookmarkSlot>();

            foreach (var p in Player.BookmarkedClan)
            {
                Alliance a = ObjectManager.GetAlliance(p.Value);
                if (a != null)
                {
                    list.AddRange(a.EncodeFullEntry());
                    I++;
                }
                else
                {
                    rem.Add(p);
                    if (I > 0)
                        I--;
                }
            }

            this.Data.AddInt(I);
            this.Data.AddRange(list);

            foreach (BookmarkSlot im in rem)
            {
                Player.BookmarkedClan.RemoveAll(t => t == im);
            }
        }
    }
}
