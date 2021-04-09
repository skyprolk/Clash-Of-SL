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
    //Packet 20113
    internal class SetDeviceTokenMessage : Message
    {
        readonly Level level;

        #region Public Constructors

        public SetDeviceTokenMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(20113);
            level = client.GetLevel();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddString(level.GetPlayerAvatar().GetUserToken());
            Encrypt(pack.ToArray());
        }

        #endregion Public Methods
    }
}