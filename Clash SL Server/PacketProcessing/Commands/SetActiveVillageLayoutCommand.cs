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
    internal class SetActiveVillageLayoutCommand : Command
    {
        #region Public Constructors

        public SetActiveVillageLayoutCommand(CoCSharpPacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
            // Not done
        }

        #endregion Public Constructors
    }
}
