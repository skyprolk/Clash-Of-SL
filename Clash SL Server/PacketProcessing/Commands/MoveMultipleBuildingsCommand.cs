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
    internal class BuildingToMove
    {
        #region Public Properties

        public int GameObjectId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        #endregion Public Properties
    }

    internal class MoveMultipleBuildingsCommand : Command
    {
        #region Private Fields

        readonly List<BuildingToMove> m_vBuildingsToMove;

        #endregion Private Fields

        #region Public Constructors

        public MoveMultipleBuildingsCommand(CoCSharpPacketReader br)
        {
            m_vBuildingsToMove = new List<BuildingToMove>();
            var buildingCount = br.ReadInt32WithEndian();
            for (var i = 0; i < buildingCount; i++)
            {
                var buildingToMove = new BuildingToMove();
                buildingToMove.X = br.ReadInt32WithEndian();
                buildingToMove.Y = br.ReadInt32WithEndian();
                buildingToMove.GameObjectId = br.ReadInt32WithEndian();
                m_vBuildingsToMove.Add(buildingToMove);
            }
            br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            foreach (var buildingToMove in m_vBuildingsToMove)
            {
                var go = level.GameObjectManager.GetGameObjectByID(buildingToMove.GameObjectId);
                go.SetPositionXY(buildingToMove.X, buildingToMove.Y);
            }
        }

        #endregion Public Methods
    }
}