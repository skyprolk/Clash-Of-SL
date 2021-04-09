using System;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24107
    internal class EnemyHomeDataMessage : Message
    {
        public EnemyHomeDataMessage(Device client, Level ownerLevel, Level visitorLevel) : base(client)
        {
            this.Identifier = 24107;
            this.OwnerLevel = ownerLevel;
            this.VisitorLevel = visitorLevel;
        }

        internal override async void Encode()
        {
            try
            {
                ClientHome ch = new ClientHome
                {
                    Id = this.OwnerLevel.Avatar.UserId,
                    ShieldTime = this.OwnerLevel.Avatar.m_vShieldTime,
                    Village = this.OwnerLevel.SaveToJSON(),
                    ProtectionTime = this.OwnerLevel.Avatar.m_vProtectionTime
                };

                this.Data.AddInt((int)TimeSpan.FromSeconds(100).TotalSeconds);
                this.Data.AddInt(-1);
                this.Data.AddInt((int)this.OwnerLevel.Avatar.LastTickSaved.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                this.Data.AddRange(ch.Encode);
                this.Data.AddRange(await this.OwnerLevel.Avatar.Encode());
                this.Data.AddRange(await this.VisitorLevel.Avatar.Encode());
                this.Data.AddInt(3); // 1 : Amical ?       2 : next button disabled       3 : PvP         5 : Amical again ?
                this.Data.AddInt(0);
                this.Data.Add(0);
            }
            catch (Exception)
            {
            }
        }

        internal readonly Level OwnerLevel;
        internal readonly Level VisitorLevel;
    }
}
