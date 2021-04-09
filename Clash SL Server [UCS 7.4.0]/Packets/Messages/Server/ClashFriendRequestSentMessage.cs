using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Helpers.Binary;
using UCS.Helpers.List;

namespace UCS.Packets.Messages.Server
{
    internal class ClashFriendRequestSentMessage : Message
    {
        public ClashFriendRequestSentMessage(Device client) :base(client)
        {
            this.Identifier = 20106;
        }

        internal override  void Encode()
        {
            this.Data.AddLong(1);
        }
    }
}
