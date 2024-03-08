using System.IO;
using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 24224
    internal class ReplayData : Message
    {
        public ReplayData(Device client) : base(client)
        {
            this.Identifier = 24114;
        }

        internal override void Encode()
        {
            this.Data.AddCompressed(File.ReadAllText("replay-json.txt"), false);
        }
    }
}