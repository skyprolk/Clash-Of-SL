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

using System.IO;
using CSS.Helpers;

namespace CSS.PacketProcessing.Commands
{
    internal class MissionProgressCommand : Command
    {
        #region Public Constructors

        public MissionProgressCommand(CoCSharpPacketReader br)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public uint MissionId { get; set; }
        public uint Unknown1 { get; set; }

        #endregion Public Properties
    }
}