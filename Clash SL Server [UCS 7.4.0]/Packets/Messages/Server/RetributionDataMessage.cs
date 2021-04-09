using System;
using CSS.Core;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packet 24133
    internal class RetributionDataMessage : Message
    {
        public RetributionDataMessage(Device client, Level level, int id) : base(client)
        {
            this.Identifier = 24133; // New one needed
            this.Player = level;
            this.LevelId = id;
            this.JsonBase = ObjectManager.NpcLevels[LevelId];
            this.Device.PlayerState = Logic.Enums.State.IN_BATTLE;
        }

        internal override async void Encode()
        {
            try
            {
                this.Data.AddInt(0);
                this.Data.AddInt((int)Player.Avatar.LastTickSaved.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                this.Data.AddRange(new ClientHome { Id = this.Player.Avatar.UserId, ShieldTime = this.Player.Avatar.m_vShieldTime, ProtectionTime = this.Player.Avatar.m_vProtectionTime, Village = this.JsonBase }.Encode);
                this.Data.AddRange(await Player.Avatar.Encode());
                this.Data.AddInt(LevelId);
            }
            catch(Exception){ }
        }

        public string JsonBase;
        public int LevelId;
        public Level Player;
    }
}