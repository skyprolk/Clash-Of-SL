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
    internal class SpeedUpHeroHealthCommand : Command
    {
        #region Private Fields

        int m_vBuildingId;

        #endregion Private Fields

        #region Public Constructors

        public SpeedUpHeroHealthCommand(CoCSharpPacketReader br)
        {
            /*
            m_vBuildingId = br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
            */
        }

        #endregion Public Constructors
    }
}