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
using CSS.Core;
using CSS.Helpers;
using CSS.Logic.AvatarStreamEntry;

namespace CSS.PacketProcessing.Messages.Server
{
    //Packet 24411
    internal class AvatarStreamMessage : Message
    {
        #region Private Fields

        AvatarStreamEntry m_vAvatarStreamEntry;

        #endregion Private Fields

        #region Public Constructors

        public AvatarStreamMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24411);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var pl = Client.GetLevel().GetPlayerAvatar();
            var pack = new List<byte>();
            pack.AddInt32(2);
            pack.AddInt64(pl.GetId());
            pack.Add(0);
            pack.AddInt64(pl.GetAllianceId());
            pack.AddString(ObjectManager.GetAlliance(pl.GetAllianceId()).GetAllianceName());
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.Add(0);
            pack.AddString("Win");
            pack.Add(0);
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.AddInt32(0);
            Encrypt(pack.ToArray());
        }

        #endregion Public Methods
    }
}