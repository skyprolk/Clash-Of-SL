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
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class DataTable
    {
        #region Protected Fields

        protected List<Data> m_vData;
        protected int m_vIndex;

        #endregion Protected Fields

        #region Public Constructors

        public DataTable()
        {
            m_vIndex = 0;
            m_vData = new List<Data>();
        }

        public DataTable(CSVTable table, int index)
        {
            m_vIndex = index;
            m_vData = new List<Data>();

            for (var i = 0; i < table.GetRowCount(); i++)
            {
                var row = table.GetRowAt(i);
                var data = CreateItem(row);
                m_vData.Add(data);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public Data CreateItem(CSVRow row)
        {
            var d = new Data(row, this);
            switch (m_vIndex)
            {
                case 0:
                    d = new BuildingData(row, this);
                    break;

                case 2:
                    d = new ResourceData(row, this);
                    break;

                case 3:
                    d = new CharacterData(row, this);
                    break;

                case 7:
                    d = new ObstacleData(row, this);
                    break;

                case 10:
                    d = new ExperienceLevelData(row, this);
                    break;

                case 11:
                    d = new TrapData(row, this);
                    break;

                case 12:
                    d = new LeagueData(row, this);
                    break;

                case 13:
                    d = new GlobalData(row, this);
                    break;

                case 14:
                    d = new TownhallLevelData(row, this);
                    break;

                case 16:
                    d = new NpcData(row, this);
                    break;

                case 17:
                    d = new DecoData(row, this);
                    break;

                case 19:
                    d = new ShieldData(row, this);
                    break;

                case 22:
                    d = new AchievementData(row, this);
                    break;

                case 23:
                    d = new Data(row, this);
                    break;

                case 24:
                    d = new Data(row, this);
                    break;

                case 25:
                    d = new SpellData(row, this);
                    break;

                case 27:
                    d = new HeroData(row, this);
                    break;

                case 28:
                    d = new WarData(row, this);
                    break;

                case 30:
                    d = new AllianceBadgeLayersData(row, this);
                    break;

                case 31:
                    d = new AllianceBadgesData(row, this);
                    break;

                case 32:
                    d = new AllianceLevelsData(row, this);
                    break;

                case 33:
                    d = new AlliancePortalData(row, this);
                    break;

                case 34:
                    d = new BuildingClassesData(row, this);
                    break;

                case 35:
                    d = new EffectsData(row, this);
                    break;

                case 36:
                    d = new LocalesData(row, this);
                    break;

                case 37:
                    d = new MissionsData(row, this);
                    break;

                case 38:
                    d = new ProjectilesData(row, this);
                    break;

                case 39:
                    d = new RegionsData(row, this);
                    break;

                case 40:
                    d = new VariablesData(row, this);
                    break;

                default:
                    break;
            }
            return d;
        }

        public Data GetDataByName(string name)
        {
            return m_vData.Find(d => d.GetName() == name);
        }

        public Data GetItemAt(int index)
        {
            return m_vData[index];
        }

        public Data GetItemById(int id)
        {
            var instanceId = GlobalID.GetInstanceID(id);
            return m_vData[instanceId];
        }

        public int GetItemCount()
        {
            return m_vData.Count;
        }

        public int GetTableIndex()
        {
            return m_vIndex;
        }

        #endregion Public Methods
    }
}