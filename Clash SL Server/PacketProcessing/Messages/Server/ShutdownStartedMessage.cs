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
    //Packet 20161
    internal class ShutdownStartedMessage : Message
    {
        #region Private Fields

        int m_vCode;

        #endregion Private Fields

        #region Public Constructors

        public ShutdownStartedMessage(PacketProcessing.Client client)
            : base(client)
        {
            SetMessageType(20161);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(m_vCode);
            Encrypt(data.ToArray());
        }

        public void SetCode(int code)
        {
            m_vCode = code;
        }

        #endregion Public Methods
    }
}