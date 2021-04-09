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
    //Packet 24111
    internal class AvailableServerCommandMessage : Message
    {
        #region Public Constructors

        public AvailableServerCommandMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24111);
        }

        #endregion Public Constructors

        #region Private Fields

        Command m_vCommand;
        int m_vServerCommandId;

        #endregion Private Fields

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(m_vServerCommandId);
            pack.AddRange(m_vCommand.Encode());
            Encrypt(pack.ToArray());
        }

        public void SetCommand(Command c)
        {
            m_vCommand = c;
        }

        public void SetCommandId(int id)
        {
            m_vServerCommandId = id;
        }

        #endregion Public Methods
    }
}