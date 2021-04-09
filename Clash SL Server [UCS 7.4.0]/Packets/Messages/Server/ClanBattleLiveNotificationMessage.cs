using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    internal class ClanBattleLiveNotificationMessage : Message
    {
        public ClanBattleLiveNotificationMessage(Device _Device) : base(_Device)
        {
            this.Identifier = 25006;
        }

        internal override void Encode()
        {
        }
    }
}
