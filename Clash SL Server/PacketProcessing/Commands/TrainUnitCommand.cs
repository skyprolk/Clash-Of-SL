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

namespace CSS.PacketProcessing
{
    class TrainUnitCommand : Command
    {
        #region Public Constructors

        public TrainUnitCommand(CoCSharpPacketReader br)
        {
            BuildingId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
            UnitType = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
            Unknown3 = br.ReadUInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Properties

        public int BuildingId { get; set; }
        public int Count { get; set; }
        public int UnitType { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown3 { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Execute(Level level)
        {
            var go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            var b = (Building)go;
            var c = b.GetUnitProductionComponent();
            var cid = (CombatItemData)ObjectManager.DataTables.GetDataById(UnitType);
            var co = level.GetHomeOwnerAvatar();
            var trainingResource = cid.GetTrainingResource();
            while (Count > 0)
            {
                if (!c.CanAddUnitToQueue(cid))
                    break;
                int trainingCost = cid.GetTrainingCost(co.GetUnitUpgradeLevel(cid));
                if (!co.HasEnoughResources(trainingResource, trainingCost))
                    break;
                co.SetResourceCount(trainingResource, co.GetResourceCount(trainingResource) - trainingCost);
                c.AddUnitToProductionQueue(cid);
                Count--;
            }
        }
    }
    #endregion Public Methods
}