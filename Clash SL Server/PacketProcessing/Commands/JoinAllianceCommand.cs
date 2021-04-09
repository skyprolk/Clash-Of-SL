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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class JoinAllianceCommand : Command
    {
        #region Private Fields

        Alliance m_vAlliance;

        #endregion Private Fields

        #region Public Constructors

        public JoinAllianceCommand()
        {
        }

        public JoinAllianceCommand(CoCSharpPacketReader br)
        {
            br.ReadInt64();
            br.ReadString();
            br.ReadInt32();
            br.ReadByte();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
        }

        #endregion Public Constructors

        #region Public Methods

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange(m_vAlliance.EncodeHeader());
            return data.ToArray();
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

        #endregion Public Methods
    }
}