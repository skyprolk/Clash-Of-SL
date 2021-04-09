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

namespace CSS.PacketProcessing.Messages.Server
{
    internal class BookmarkRemoveAllianceMessage : Message
    {
        #region Public Constructors

        public BookmarkRemoveAllianceMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24344);
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