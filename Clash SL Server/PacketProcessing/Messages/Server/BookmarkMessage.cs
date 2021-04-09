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

namespace CSS.PacketProcessing.Messages.Server
{
    internal class BookmarkMessage : Message
    {
        #region Public Constructors

        public BookmarkMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24340);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt64(1);
            data.AddInt64(2);
            Encrypt(data.ToArray());
        }

        #endregion Public Methods
    }
}