using System.IO;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14402
    internal class TopLocalAlliancesMessage : Message
    {
        public TopLocalAlliancesMessage(Device device, Reader reader) : base(device, reader)
        {
        }


        internal override void Process()
        {
            new LocalAlliancesMessage(Device).Send();
        }
    }
}
