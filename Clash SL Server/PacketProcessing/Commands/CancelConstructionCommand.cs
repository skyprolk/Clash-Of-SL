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
    internal class CancelConstructionCommand : Command
    {
        #region Public Constructors

        public CancelConstructionCommand(CoCSharpPacketReader br)
        {
            BuildingId = br.ReadInt32WithEndian(); 
            Unknown1 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                if (go.ClassId == 0 || go.ClassId == 4)
                {
                    var constructionItem = (ConstructionItem) go;
                    if (constructionItem.IsConstructing())
                    {
                        constructionItem.CancelConstruction();
                    }
                }
                else if (go.ClassId == 3)
                {
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        public int BuildingId { get; set; }
        public uint Unknown1 { get; set; }

        #endregion Public Properties
    }
}
