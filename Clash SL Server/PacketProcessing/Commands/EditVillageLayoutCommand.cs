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
    internal class EditVillageLayoutCommand : Command
    {
        #region Public Constructors

        public EditVillageLayoutCommand(CoCSharpPacketReader br)
        {
            br.ReadInt32(); 
            br.ReadInt32(); 
            br.ReadInt32(); 
            br.ReadInt32(); 
            br.ReadInt32();
            // Not 100% Done
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
        }

        #endregion Public Methods
    }
}
