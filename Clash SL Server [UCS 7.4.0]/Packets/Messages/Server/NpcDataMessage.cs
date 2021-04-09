using System;
using CSS.Core;
using CSS.Helpers.List;
using CSS.Logic;
using CSS.Logic.Enums;
using CSS.Packets.Messages.Client;

namespace CSS.Packets.Messages.Server
{
    // Packet 24133
    internal class NpcDataMessage : Message
    {
        public NpcDataMessage(Device client, Level level, AttackNpcMessage cnam) : base(client)
        {
            this.Identifier = 24133;
            this.Player = level;
            this.LevelId = cnam.LevelId;
            this.JsonBase = ObjectManager.NpcLevels[LevelId];
            this.Device.PlayerState = State.IN_BATTLE;
        }

        internal override async void Encode()
        {
            try
            {
                this.Data.AddInt(0);
                this.Data.AddInt((int)Player.Avatar.LastTickSaved.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                this.Data.AddRange(new ClientHome { Id = Player.Avatar.UserId, ShieldTime = this.Player.Avatar.m_vShieldTime, ProtectionTime = this.Player.Avatar.m_vProtectionTime, Village = this.JsonBase }.Encode);
                this.Data.AddRange(await this.Player.Avatar.Encode());
                this.Data.AddInt(this.LevelId);
            }
            catch (Exception)
            {
            }
        }

        public string JsonBase;
        public int LevelId;
        public Level Player;
    }
}