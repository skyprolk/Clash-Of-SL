using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    internal class ChallangeWatchLiveMessage : Message
    {
        public ChallangeWatchLiveMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
            new OwnHomeDataMessage(Device, this.Device.Player).Send();
        }
    }
}
