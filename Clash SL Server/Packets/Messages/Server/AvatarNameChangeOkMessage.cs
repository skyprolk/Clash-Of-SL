using System.Collections.Generic;
using CSS.Helpers;
using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    // Packet 24111
    internal class AvatarNameChangeOkMessage : Message
    {
        public AvatarNameChangeOkMessage(Device client) : base(client)
        {
            this.Identifier = 24111;
            AvatarName = "NoNameYet";
        }

        internal string AvatarName;

        internal override void Encode()
        {
            this.Data.AddInt(3);
            this.Data.AddString(this.AvatarName);
            this.Data.AddInt(1);
            this.Data.AddInt(-1);
        }
    }
}