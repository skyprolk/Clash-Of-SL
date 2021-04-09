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
using System.IO;
using CSS.Utilities.ZLib;

namespace CSS.PacketProcessing.Messages.Server
{
    internal class ReplayData : Message
    {
        #region Public Constructors

        public ReplayData(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24114);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();
            string text = File.ReadAllText("replay-json.txt");
            data.AddRange(ZlibStream.CompressString(text));
            Encrypt(data.ToArray());
        }

        #endregion Public Methods
    }
}