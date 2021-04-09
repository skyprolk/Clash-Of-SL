using System;
using CSS.Helpers.List;
using CSS.Logic;
using CSS.Logic.Enums;

namespace CSS.Packets.Messages.Server
{
    // Packet 24101
    internal class OwnHomeDataMessage : Message
    {
        public OwnHomeDataMessage(Device client, Level level) : base(client)
        {
            this.Identifier = 24101;
            this.Player = level;
            this.Player.Tick();
        }

        public Level Player;

        internal override async void Encode()
        {
            try
            {
                var _Home =
                    new ClientHome
                    {
                        Id = this.Player.Avatar.UserId,
                        ShieldTime = this.Player.Avatar.m_vShieldTime,
                        ProtectionTime = this.Player.Avatar.m_vProtectionTime,
                        Village = this.Player.SaveToJSON()
                    };

                this.Data.AddInt(0);
                this.Data.AddInt(-1);
                this.Data.AddInt((int)Player.Avatar.LastTickSaved.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                this.Data.AddRange(_Home.Encode);
                this.Data.AddRange(await this.Player.Avatar.Encode());
                this.Data.AddInt(this.Device.PlayerState == State.WAR_EMODE ? 1 : 0);
                this.Data.AddInt(0);
                this.Data.AddLong(0);
                this.Data.AddLong(0);
                this.Data.AddLong(0);
            }
            catch (Exception)
            {
            }
        }
    }
}