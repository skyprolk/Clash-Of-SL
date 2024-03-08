using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core.Network;
using UCS.Helpers.Binary;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    internal class AllianceWarVisitMessage : Message
    {
        public AllianceWarVisitMessage(Device client, Reader _Reader) : base(client, _Reader)
        {
        }

        internal override void Process()
        {
            new OwnHomeDataMessage(this.Device, this.Device.Player).Send();
        }
    }
}
