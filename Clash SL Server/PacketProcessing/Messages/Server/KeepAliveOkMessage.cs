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
using CSS.PacketProcessing.Messages.Client;

namespace CSS.PacketProcessing.Messages.Server
{
    //Packet 20108
    internal class KeepAliveOkMessage : Message
    {
        #region Public Constructors

        public KeepAliveOkMessage(PacketProcessing.Client client, KeepAliveMessage cka) : base(client)
        {
            SetMessageType(20108);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();
            Encrypt(data.ToArray());
        }

        #endregion Public Methods
    }
}