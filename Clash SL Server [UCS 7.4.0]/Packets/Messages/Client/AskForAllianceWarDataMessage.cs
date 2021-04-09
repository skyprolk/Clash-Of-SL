using System.IO;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14331
    internal class AskForAllianceWarDataMessage : Message
    {
        public AskForAllianceWarDataMessage(Device client, Reader reader) : base(client, reader)
        {
        }


        internal override void Process()
        {
            new AllianceWarDataMessage(this.Device).Send();
        }
    }
}