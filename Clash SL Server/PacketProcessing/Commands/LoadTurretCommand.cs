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
using CSS.Core;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class LoadTurretCommand : Command
    {
        #region Public Constructors

        public LoadTurretCommand(CoCSharpPacketReader br)
        {
            m_vUnknown1 = br.ReadUInt32WithEndian();
            m_vBuildingId = br.ReadInt32WithEndian();
            m_vUnknown2 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var go = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (go != null)
                if (go.GetComponent(1, true) != null)
                    ((CombatComponent) go.GetComponent(1, true)).FillAmmo();
        }

        #endregion Public Methods

        #region Public Properties

        public int m_vBuildingId { get; set; }

        public uint m_vUnknown1 { get; set; }

        public uint m_vUnknown2 { get; set; }

        #endregion Public Properties
    }
}