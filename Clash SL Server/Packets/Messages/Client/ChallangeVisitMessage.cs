using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    internal class ChallangeVisitMessage : Message
    {
        public ChallangeVisitMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        public long AvatarID { get; set; }

        internal override void Decode()
        {
            this.AvatarID = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            new OwnHomeDataMessage(Device, this.Device.Player).Send();
            //var defender = ResourcesManager.GetPlayer(AvatarID); // TODO: FIX BUGS		
            //PacketManager.ProcessOutgoingPacket(new VisitedHomeDataMessage(Client, defender, level)); 
        }
    }
}
