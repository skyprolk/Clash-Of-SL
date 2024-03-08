using System;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    internal class ChallangeAttackDataMessage : Message
    {
        internal readonly Level OwnerLevel;
        internal readonly Level VisitorLevel;

        public ChallangeAttackDataMessage(Device client, Level level) : base(client)
        {
            this.Identifier = 24107;
            this.OwnerLevel = level;
            this.VisitorLevel = client.Player;
        }

        internal override async void Encode()
        {
            try
            {
                ClientHome ch = new ClientHome
                {
                    Id = this.OwnerLevel.Avatar.UserId,
                    Village = this.OwnerLevel.SaveToJSON()
                };

                this.Data.AddRange(new byte[]{0x00, 0x00, 0x00, 0xF0,0xFF, 0xFF, 0xFF, 0xFF,0x54, 0xCE, 0x5C, 0x4A});
                this.Data.AddRange(ch.Encode);
                this.Data.AddRange(await this.OwnerLevel.Avatar.Encode());
                this.Data.AddRange(await this.OwnerLevel.Avatar.Encode());
                this.Data.AddRange(new byte[]{0x00, 0x00, 0x00, 0x03, 0x00});
                this.Data.AddInt(0);
                this.Data.AddInt(0);
                this.Data.AddLong(0);
                this.Data.AddLong(0);
                this.Data.AddLong(0);
            }
            catch (Exception) { }
        }
    }
}

