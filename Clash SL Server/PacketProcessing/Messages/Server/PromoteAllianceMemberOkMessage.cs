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
using CSS.PacketProcessing.Messages.Client;

namespace CSS.PacketProcessing.Messages.Server
{
    //Packet 24340
    internal class PromoteAllianceMemberOkMessage : Message
    {
        #region Public Constructors

        public PromoteAllianceMemberOkMessage(PacketProcessing.Client client, Level level, PromoteAllianceMemberMessage pam)
            : base(client)
        {
            SetMessageType(24306);
            m_vId = pam.m_vId;
            m_vRole = pam.m_vRole;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt64(m_vId);
            pack.AddInt32(m_vRole);
            Encrypt(pack.ToArray());
        }

        #endregion Public Methods

        #region Private Fields

        readonly long m_vId;
        readonly int m_vRole;

        #endregion Private Fields
    }
}