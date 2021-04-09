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
    internal class ClientServerTickCommand : Command
    {
        #region Public Constructors

        public ClientServerTickCommand(CoCSharpPacketReader br)
        {
            Unknown1 = br.ReadInt32();
            Tick = br.ReadInt32();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
        }

        #endregion Public Methods

        #region Public Properties

        public static int Tick { get; set; }
        public static int Unknown1 { get; set; }

        #endregion Public Properties
    }
}