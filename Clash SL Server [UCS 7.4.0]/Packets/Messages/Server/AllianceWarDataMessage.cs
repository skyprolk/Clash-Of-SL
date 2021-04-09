using System.Collections.Generic;
using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 24331
    internal class AllianceWarDataMessage : Message
    {
        public AllianceWarDataMessage(Device client) : base(client)
        {
            this.Identifier = 24331;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(0);

            this.Data.AddLong(1); // Team ID
            this.Data.AddString("Dark");
            this.Data.AddInt(0);
            this.Data.AddInt(1);
            this.Data.Add(0);
            this.Data.AddRange(new List<byte> { 1, 2, 3, 4 });
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);

            this.Data.AddLong(1); // Team ID
            this.Data.AddString("CSS");
            this.Data.AddInt(0);
            this.Data.AddInt(1);
            this.Data.Add(0);
            this.Data.AddRange(new List<byte> { 1, 2, 3, 4 });
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);

            this.Data.AddLong(11);

            this.Data.AddInt(0);
            this.Data.AddInt(0);

            this.Data.AddInt(1);
            this.Data.AddInt(3600);
            this.Data.AddLong(1);
            this.Data.AddLong(1);
            this.Data.AddLong(2);
            this.Data.AddLong(2);

            this.Data.AddString("Ultra");
            this.Data.AddString("Powa");

            this.Data.AddInt(2);
            this.Data.AddInt(1);
            this.Data.AddInt(50);

            this.Data.AddInt(0);

            this.Data.AddInt(8);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.Add(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
        }
    }
}
