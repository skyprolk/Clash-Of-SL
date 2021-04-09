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
    internal class SpeedUpHeroUpgradeCommand : Command
    {
        #region Public Constructors

        public SpeedUpHeroUpgradeCommand(CoCSharpPacketReader br)
        {
            m_vBuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            m_vUnknown1 = br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var ca = level.GetPlayerAvatar();
            var go = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);

            if (go != null)
            {
                var b = (Building) go;
                var hbc = b.GetHeroBaseComponent();
                if (hbc != null)
                    hbc.SpeedUpUpgrade();
            }
        }

        #endregion Public Methods

        #region Private Fields

        readonly int m_vBuildingId;
        int m_vUnknown1;

        #endregion Private Fields
    }
}