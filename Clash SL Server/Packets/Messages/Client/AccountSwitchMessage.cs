using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Messages.Client
{
    class AccountSwitchMessage : Message
    {
        public AccountSwitchMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
        }

        internal override async void Process()
        {
        }
    }
}
