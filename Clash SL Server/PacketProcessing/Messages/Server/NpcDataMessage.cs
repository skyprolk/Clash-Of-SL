/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System.Collections.Generic;
using System.Text;
using CSS.Core;
using CSS.Helpers;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Client;

namespace CSS.PacketProcessing.Messages.Server
{
    //Packet 24133
    internal class NpcDataMessage : Message
    {
        #region Public Constructors

        public NpcDataMessage(PacketProcessing.Client client, Level level, AttackNpcMessage cnam) : base(client)
        {
            SetMessageType(24133);
            Player = level;
            LevelId = cnam.LevelId;
            JsonBase = ObjectManager.NpcLevels[LevelId];
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();

            data.AddInt32(0);
            data.AddInt32(JsonBase.Length);
            data.AddRange(Encoding.ASCII.GetBytes(JsonBase));
            data.AddRange(Player.GetPlayerAvatar().Encode());
            data.AddInt32(0);
            data.AddInt32(LevelId);

            Encrypt(data.ToArray());
        }

        #endregion Public Methods

        #region Public Properties

        public string JsonBase { get; set; }

        public int LevelId { get; set; }

        public Level Player { get; set; }

        #endregion Public Properties
    }
}