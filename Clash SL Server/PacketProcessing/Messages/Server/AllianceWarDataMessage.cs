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
    internal class AllianceWarDataMessage : Message
    {
        #region Public Constructors

        public AllianceWarDataMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24331);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt64(1); // Team ID
            data.AddString("DARK");
            data.AddInt32(0);
            data.AddInt32(1);
            data.Add(0);
            data.AddRange(new List<byte> { 1, 2, 3, 4 });
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt64(1); // Team ID
            data.AddString("DARK");
            data.AddInt32(0);
            data.AddInt32(1);
            data.Add(0);
            data.AddRange(new List<byte> { 1, 2, 3, 4 });
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt64(11);

            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt32(1);
            data.AddInt32(3600);
            data.AddInt64(1);
            data.AddInt64(1);
            data.AddInt64(2);
            data.AddInt64(2);

            data.AddString("DARK");
            data.AddString("");

            data.AddInt32(2);
            data.AddInt32(1);
            data.AddInt32(50);

            data.AddInt32(0);

            data.AddInt32(8);
            data.AddInt32(0);
            data.AddInt32(0);
            data.Add(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);

            Encrypt(data.ToArray());
        }

        #endregion Public Methods
    }
}
