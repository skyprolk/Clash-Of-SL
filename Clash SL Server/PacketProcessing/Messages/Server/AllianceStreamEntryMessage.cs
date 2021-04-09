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
using CSS.Logic.StreamEntry;

namespace CSS.PacketProcessing.Messages.Server
{
    internal class AllianceStreamEntryMessage : Message
    {
        #region Private Fields

        StreamEntry m_vStreamEntry;

        #endregion Private Fields

        #region Public Constructors

        public AllianceStreamEntryMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24312);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddRange(m_vStreamEntry.Encode());
            Encrypt(pack.ToArray());
        }

        public void SetStreamEntry(StreamEntry entry)
        {
            m_vStreamEntry = entry;
        }

        #endregion Public Methods
    }
}