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
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic.Manager
{
    internal class GameObjectManager
    {
        #region Public Constructors

        public GameObjectManager(Level l)
        {
            m_vLevel = l;
            m_vGameObjects = new List<List<GameObject>>();
            m_vGameObjectRemoveList = new List<GameObject>();
            m_vGameObjectsIndex = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                m_vGameObjects.Add(new List<GameObject>());
                m_vGameObjectsIndex.Add(0);
            }
            m_vComponentManager = new ComponentManager(m_vLevel);
        }

        #endregion Public Constructors

        #region Private Fields

        readonly ComponentManager m_vComponentManager;
        readonly List<GameObject> m_vGameObjectRemoveList;
        readonly List<List<GameObject>> m_vGameObjects;
        readonly List<int> m_vGameObjectsIndex;
        readonly Level m_vLevel;

        #endregion Private Fields

        #region Public Methods

        public void AddGameObject(GameObject go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                    m_vLevel.WorkerManager.IncreaseWorkerCount();
            }
            m_vGameObjects[go.ClassId].Add(go);
        }

        public List<List<GameObject>> GetAllGameObjects() => m_vGameObjects;

        public ComponentManager GetComponentManager() => m_vComponentManager;

        public GameObject GetGameObjectByID(int id)
        {
            var classId = GlobalID.GetClassID(id) - 500;
            if (m_vGameObjects.Capacity < classId)
            return null;
            return m_vGameObjects[classId].Find(g => g.GlobalId == id);
        }

        public List<GameObject> GetGameObjects(int id) => m_vGameObjects[id];

        public void Load(JObject jsonObject)
        {
            var jsonBuildings = (JArray) jsonObject["buildings"];
            foreach (JObject jsonBuilding in jsonBuildings)
            {
                var bd = (BuildingData) ObjectManager.DataTables.GetDataById(jsonBuilding["data"].ToObject<int>());
                var b = new Building(bd, m_vLevel);
                AddGameObject(b);
                b.Load(jsonBuilding);
            }

            var jsonTraps = (JArray) jsonObject["traps"];
            foreach (JObject jsonTrap in jsonTraps)
            {
                var td = (TrapData) ObjectManager.DataTables.GetDataById(jsonTrap["data"].ToObject<int>());
                var t = new Trap(td, m_vLevel);
                AddGameObject(t);
                t.Load(jsonTrap);
            }

            var jsonDecos = (JArray) jsonObject["decos"];

            foreach (JObject jsonDeco in jsonDecos)
            {
                var dd = (DecoData) ObjectManager.DataTables.GetDataById(jsonDeco["data"].ToObject<int>());
                var d = new Deco(dd, m_vLevel);
                AddGameObject(d);
                d.Load(jsonDeco);
            }
        }

        public void RemoveGameObject(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                {
                    m_vLevel.WorkerManager.DecreaseWorkerCount();
                }
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            m_vComponentManager.RemoveGameObjectReferences(go);
        }

        public JObject Save()
        {
            var jsonData = new JObject();
            jsonData.Add("exp_ver", 1);
            jsonData.Add("android_client", true);
            jsonData.Add("active_layout", 0);
            jsonData.Add("layout_state", new JArray { 0, 0, 0, 0, 0, 0 });

            var jsonBuildingsArray = new JArray();
            foreach (var go in new List<GameObject>(m_vGameObjects[0]))
            {
                var b = (Building) go;
                var jsonObject = new JObject();
                jsonObject.Add("data", b.GetBuildingData().GetGlobalID());
                b.Save(jsonObject);
                jsonBuildingsArray.Add(jsonObject);
            }
            jsonData.Add("buildings", jsonBuildingsArray);

            var jsonTrapsArray = new JArray();
            foreach (var go in new List<GameObject>(m_vGameObjects[4]))
            {
                var t = (Trap) go;
                var jsonObject = new JObject();
                jsonObject.Add("data", t.GetTrapData().GetGlobalID());
                t.Save(jsonObject);
                jsonTrapsArray.Add(jsonObject);
            }
            jsonData.Add("traps", jsonTrapsArray);

            var jsonDecosArray = new JArray();
            foreach (var go in new List<GameObject>(m_vGameObjects[6]))
            {
                var d = (Deco) go;
                var jsonObject = new JObject();
                jsonObject.Add("data", d.GetDecoData().GetGlobalID());
                d.Save(jsonObject);
                jsonDecosArray.Add(jsonObject);
            }
            jsonData.Add("decos", jsonDecosArray);

            var cooldowns = new JArray();
            jsonData.Add("cooldowns", cooldowns);
            var newShopBuildings = new JArray
            {
                4,
                0,
                7,
                4,
                7,
                4,
                4,
                1,
                7,
                8,
                275,
                5,
                4,
                4,
                1,
                5,
                0,
                0,
                0,
                4,
                1,
                4,
                1,
                3,
                1,
                1,
                2,
                2,
                2,
                1,
                1,
                1
            };
            jsonData.Add("newShopBuildings", newShopBuildings);
            var newShopTraps = new JArray { 6, 6, 5, 0, 0, 5, 5, 0, 3 };
            jsonData.Add("newShopTraps", newShopTraps);
            var newShopDecos = new JArray
            {
                1,
                4,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };
            jsonData.Add("newShopDecos", newShopDecos);
            jsonData.Add("troop_req_msg", "css Developement Team");
            jsonData.Add("last_league_rank", m_vLevel.GetPlayerAvatar().GetLeagueId());
            jsonData.Add("last_league_shuffle", 1);
            jsonData.Add("last_season_seen", 1);
            jsonData.Add("last_news_seen", 999);
            jsonData.Add("edit_mode_shown", true);
            jsonData.Add("war_tutorials_seen", 1);
            jsonData.Add("war_base", true);
            jsonData.Add("help_opened", true);
            jsonData.Add("bool_layout_edit_shown_erase", false);

            return jsonData;
        }

        public void Tick()
        {
            m_vComponentManager.Tick();
            foreach (var l in m_vGameObjects)
            {
                foreach (var go in l)
                    go.Tick();
            }
            foreach (var g in new List<GameObject>(m_vGameObjectRemoveList))
            {
                RemoveGameObjectTotally(g);
                m_vGameObjectRemoveList.Remove(g);
            }
        }

        #endregion Public Methods

        #region Private Methods

        int GenerateGameObjectGlobalId(GameObject go)
        {
            var index = m_vGameObjectsIndex[go.ClassId];
            m_vGameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index);
        }

        void RemoveGameObjectTotally(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                    m_vLevel.WorkerManager.DecreaseWorkerCount();
            }
            RemoveGameObjectReferences(go);
        }

        #endregion Private Methods
    }
}
