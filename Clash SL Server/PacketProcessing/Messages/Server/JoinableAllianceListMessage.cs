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
    //Packet 24304
    internal class JoinableAllianceListMessage : Message
    {
        #region Private Fields

        List<Alliance> m_vAlliances;

        #endregion Private Fields

        #region Public Constructors

        public JoinableAllianceListMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24304);
            m_vAlliances = new List<Alliance>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(m_vAlliances.Count);
            foreach (var alliance in m_vAlliances)
            {
                pack.AddRange(alliance.EncodeFullEntry());
            }

            Encrypt(pack.ToArray());
        }

        public void SetJoinableAlliances(List<Alliance> alliances)
        {
            m_vAlliances = alliances;
        }

        #endregion Public Methods
    }
}