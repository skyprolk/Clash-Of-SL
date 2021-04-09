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
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class BuyBuildingCommand : Command
    {
        #region Public Constructors

        public BuyBuildingCommand(CoCSharpPacketReader br)
        {
            X = br.ReadInt32WithEndian();
            Y = br.ReadInt32WithEndian();
            BuildingId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var ca = level.GetPlayerAvatar();

            var bd = (BuildingData) ObjectManager.DataTables.GetDataById(BuildingId);
            var b = new Building(bd, level);

            if (ca.HasEnoughResources(bd.GetBuildResource(0), bd.GetBuildCost(0)))
            {
                if (bd.IsWorkerBuilding() || level.HasFreeWorkers())
                {
                    var rd = bd.GetBuildResource(0);
                    ca.CommodityCountChangeHelper(0, rd, -bd.GetBuildCost(0));

                    b.StartConstructing(X, Y);
                    level.GameObjectManager.AddGameObject(b);
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        public int BuildingId { get; set; }
        public uint Unknown1 { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        #endregion Public Properties
    }
}