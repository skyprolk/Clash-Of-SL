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

using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class Building : ConstructionItem
    {
        #region Public Constructors

        public Building(Data data, Level level) : base(data, level)
        {
            Locked = GetBuildingData().Locked;
            AddComponent(new HitpointComponent());
            if (GetBuildingData().IsHeroBarrack)
            {
                var hd = ObjectManager.DataTables.GetHeroByName(GetBuildingData().HeroType);
                AddComponent(new HeroBaseComponent(this, hd));
            }
            if (GetBuildingData().UpgradesUnits)
                AddComponent(new UnitUpgradeComponent(this));
            if (GetBuildingData().UnitProduction[0] > 0)
                AddComponent(new UnitProductionComponent(this));
            if (GetBuildingData().HousingSpace[0] > 0)
            {
                if (GetBuildingData().Bunker)
                    AddComponent(new BunkerComponent());
                else
                    AddComponent(new UnitStorageComponent(this, 0));
            }
            if (GetBuildingData().Damage[0] > 0 || GetBuildingData().BuildingClass == "Defense")
                AddComponent(new CombatComponent(this, level));
            if (GetBuildingData().ProducesResource != null && GetBuildingData().ProducesResource != string.Empty)
            {
                var s = GetBuildingData().ProducesResource;
                AddComponent(new ResourceProductionComponent(this, level));
            }
            if (GetBuildingData().MaxStoredGold[0] > 0 ||
                GetBuildingData().MaxStoredElixir[0] > 0 ||
                GetBuildingData().MaxStoredDarkElixir[0] > 0 ||
                GetBuildingData().MaxStoredWarGold[0] > 0 ||
                GetBuildingData().MaxStoredWarElixir[0] > 0 ||
                GetBuildingData().MaxStoredWarDarkElixir[0] > 0)
                AddComponent(new ResourceStorageComponent(this));
        }

        #endregion Public Constructors

        #region Public Properties

        public override int ClassId
        {
            get { return 0; }
        }

        #endregion Public Properties

        #region Public Methods

        public BuildingData GetBuildingData()
        {
            return (BuildingData) GetData();
        }

        #endregion Public Methods
    }
}