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
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class RemoveUnitsCommand : Command
    {
        #region Public Constructors

        public RemoveUnitsCommand(CoCSharpPacketReader br)
        {
            Unknown1 = br.ReadUInt32WithEndian();
            UnitTypesCount = br.ReadInt32WithEndian();

            UnitsToRemove = new List<UnitToRemove>();
            for (var i = 0; i < UnitTypesCount; i++)
            {
                var unit = (CharacterData) br.ReadDataReference();
                var count = br.ReadInt32WithEndian();
                var level = br.ReadInt32WithEndian();
                UnitsToRemove.Add(new UnitToRemove { Data = unit, Count = count, Level = level });
            }

            Unknown2 = br.ReadUInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            foreach (var unit in UnitsToRemove)
            {
                List<Component> components = level.GetComponentManager().GetComponents(0);
                for (var i = 0; i < components.Count; i++)
                {
                    UnitStorageComponent c = (UnitStorageComponent)components[i];
                    if (c.GetUnitTypeIndex(unit.Data) != -1)
                    {
                        int storageCount = c.GetUnitCountByData(unit.Data);
                        if (storageCount >= unit.Count)
                        {
                            c.RemoveUnits(unit.Data, unit.Count);
                            break;
                        }
                        c.RemoveUnits(unit.Data, storageCount);
                        unit.Count -= storageCount;
                    }
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        public List<UnitToRemove> UnitsToRemove { get; set; }
        public int UnitTypesCount { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }

        #endregion Public Properties
    }

    internal class UnitToRemove
    {
        #region Public Properties

        public int Count { get; set; }
        public CharacterData Data { get; set; }
        public int Level { get; set; }

        #endregion Public Properties
    }
}
