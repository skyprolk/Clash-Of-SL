using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers.List;

namespace UCS.Packets.Messages.Server
{
    class ReportedPlayerMessage : Message
    {
        public ReportedPlayerMessage(Device _Device) : base(_Device)
        {
            this.Identifier = 20117;
        }

        private int ID { get; set; }

        internal override void Encode()
        {
            this.Data.AddInt(ID);
        }

        public void SetID(int _ID) => ID = _ID;
    }
}
