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

using System;
using System.Collections.Generic;
using CSS.Helpers;

namespace CSS.PacketProcessing.Messages.Server
{
    internal class AllianceWarHistoryMessage : Message
    {
        #region Public Constructors

        public AllianceWarHistoryMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24338);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();

            data.AddInt64(1); // 1 Alliance ID
            data.AddString("DARK"); // 1 Alliance Name
            data.AddInt32(0); // 1 Alliance Badge
            data.AddInt32(1); // 1 Alliance Level

            data.AddInt64(2); // 2 Alliance ID
            data.AddString("DARK"); // 2 Alliance Name
            data.AddInt32(0); // 2 Alliance Badge
            data.AddInt32(1); // 2 Alliance Level

            data.AddInt32(3); // 1 Stars
            data.AddInt32(0); // 2 Stars

            data.AddInt32(30); // 1 Village Destroyed
            data.AddInt32(10); // 2 Village Destroyed

            data.AddInt32(0); // 1 Unknown
            data.AddInt32(0); // 2 Unknown

            data.AddInt32(40); // Attack Used
            data.AddInt32(4000); // XP Earned

            data.AddInt64(515631654); // War ID
            data.AddInt64(40); // Members Count

            data.AddInt32(1); // War Won Count

            data.AddRange(new byte[] { 0x0A });
            data.AddInt32((int) TimeSpan.FromDays(1).TotalSeconds);
            data.AddInt64((int) (TimeSpan.FromDays(1).TotalSeconds - TimeSpan.FromDays(0.5).TotalSeconds));

            Encrypt(data.ToArray());
        }

        #endregion Public Methods
    }
}
