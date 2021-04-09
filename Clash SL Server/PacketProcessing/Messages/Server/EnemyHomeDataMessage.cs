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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Messages.Server
{
    //Packet 24107
    internal class EnemyHomeDataMessage : Message
    {
        #region Public Constructors

        public EnemyHomeDataMessage(PacketProcessing.Client client, Level ownerLevel, Level visitorLevel) : base(client)
        {
            SetMessageType(24107);
            m_vOwnerLevel = ownerLevel;
            m_vVisitorLevel = visitorLevel;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddRange(new byte[]
            {
                0x00, 0x00, 0x00, 0xF0,
                0xFF, 0xFF, 0xFF, 0xFF,
                0x54, 0xCE, 0x5C, 0x4A
            });
            var ch = new ClientHome(m_vOwnerLevel.GetPlayerAvatar().GetId());
            ch.SetShieldDurationSeconds(m_vOwnerLevel.GetPlayerAvatar().RemainingShieldTime);
            ch.SetHomeJSON(m_vOwnerLevel.SaveToJSON());
            data.AddRange(ch.Encode());
            data.AddRange(m_vOwnerLevel.GetPlayerAvatar().Encode());
            data.AddRange(m_vVisitorLevel.GetPlayerAvatar().Encode());
            data.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x03, 0x00 });
            data.AddInt32(200);
            data.AddInt32(100);
            data.AddInt32(0);
            data.AddInt32(0);
            data.Add(0);
            Encrypt(data.ToArray());
        }

        #endregion Public Methods

        #region Private Fields

        readonly Level m_vOwnerLevel;
        readonly Level m_vVisitorLevel;

        #endregion Private Fields
    }
}