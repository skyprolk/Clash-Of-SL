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
    internal class AnswerJoinRequestAllianceMessage : Message
    {
        #region Public Constructors

        public AnswerJoinRequestAllianceMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24317);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();

            Encrypt(pack.ToArray());
        }

        #endregion Public Methods

        #region Private Fields

        readonly int m_vServerCommandType;
        string m_vAvatarName;

        #endregion Private Fields
    }
}