using System;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24113
    internal class VisitedHomeDataMessage : Message
    {
        public VisitedHomeDataMessage(Device client, Level ownerLevel, Level visitorLevel) : base(client)
        {
            this.Identifier = 24113;
            m_vOwnerLevel = ownerLevel;
            m_vVisitorLevel = visitorLevel;
            this.Device.PlayerState = Logic.Enums.State.VISIT;
        }

        internal override async void Encode()
        {
            try
            {
                ClientHome ownerHome = new ClientHome
                {
                    Id = m_vOwnerLevel.Avatar.UserId,
                    ShieldTime = m_vOwnerLevel.Avatar.m_vShieldTime,
                    ProtectionTime = m_vOwnerLevel.Avatar.m_vProtectionTime,
                    Village = m_vOwnerLevel.SaveToJSON()
                };

                this.Data.AddInt(-1);
                this.Data.AddInt((int)TimeSpan.FromSeconds(100).TotalSeconds);
                this.Data.AddRange(ownerHome.Encode);
                this.Data.AddRange(await this.m_vOwnerLevel.Avatar.Encode());
                this.Data.AddInt(0);
                this.Data.Add(1);
                this.Data.AddRange(await this.m_vVisitorLevel.Avatar.Encode());
            }
            catch (Exception)
            {
            }
        }

        readonly Level m_vOwnerLevel;
        readonly Level m_vVisitorLevel;
    }
}
