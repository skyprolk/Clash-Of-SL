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
using CSS.Logic.AvatarStreamEntry;

namespace CSS.PacketProcessing.Messages.Server
{
    //Packet 24412
    internal class AvatarStreamEntryMessage : Message
    {
        #region Private Fields

        AvatarStreamEntry m_vAvatarStreamEntry;

        #endregion Private Fields

        #region Public Constructors

        public AvatarStreamEntryMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24412);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();

            pack.AddRange(m_vAvatarStreamEntry.Encode());

            Encrypt(pack.ToArray());
        }

        public void SetAvatarStreamEntry(AvatarStreamEntry entry)
        {
            m_vAvatarStreamEntry = entry;
        }

        #endregion Public Methods
    }
}