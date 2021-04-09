using System.IO;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14114
    internal class ReplayRequestMessage : Message
    {
        public ReplayRequestMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
            this.Device.PlayerState = Logic.Enums.State.REPLAY;
            new ReplayData(this.Device).Send();
        }
    }
}