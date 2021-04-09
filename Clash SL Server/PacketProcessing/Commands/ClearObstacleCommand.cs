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
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class ClearObstacleCommand : Command
    {
        #region Public Constructors

        public ClearObstacleCommand(CoCSharpPacketReader br)
        {
            ObstacleId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
        }

        #endregion Public Methods

        #region Public Properties

        public int ObstacleId { get; set; }
        public uint Unknown1 { get; set; }

        #endregion Public Properties
    }
}
