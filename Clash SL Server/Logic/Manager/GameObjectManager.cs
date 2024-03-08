using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic.Manager
{
    internal class GameObjectManager
    {
        public GameObjectManager(Level l)
        {
            m_vLevel                = l;
            m_vGameObjects          = new List<List<GameObject>>();
            m_vGameObjectRemoveList = new List<GameObject>();
            m_vGameObjectsIndex     = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                m_vGameObjects.Add(new List<GameObject>());
                m_vGameObjectsIndex.Add(0);
            }
            m_vComponentManager     = new ComponentManager(m_vLevel);
			//m_vObstacleManager      = new ObstacleManager(m_vLevel);
		}

        readonly ComponentManager m_vComponentManager;
        readonly List<GameObject> m_vGameObjectRemoveList;
        readonly List<List<GameObject>> m_vGameObjects;
        readonly List<int> m_vGameObjectsIndex;
        readonly Level m_vLevel;
	    //readonly ObstacleManager m_vObstacleManager;

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

		//public ObstacleManager GetObstacleManager() => m_vObstacleManager;

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
                var bd = (BuildingData)CSVManager.DataTables.GetDataById(jsonBuilding["data"].ToObject<int>());
                var b = new Building(bd, m_vLevel);
                AddGameObject(b);
                b.Load(jsonBuilding);
            }

            var jsonTraps = (JArray) jsonObject["traps"];
            foreach (JObject jsonTrap in jsonTraps)
            {
                var td = (TrapData)CSVManager.DataTables.GetDataById(jsonTrap["data"].ToObject<int>());
                var t = new Trap(td, m_vLevel);
                AddGameObject(t);
                t.Load(jsonTrap);
            }

            var jsonDecos = (JArray) jsonObject["decos"];

            foreach (JObject jsonDeco in jsonDecos)
            {
                var dd = (DecoData)CSVManager.DataTables.GetDataById(jsonDeco["data"].ToObject<int>());
                var d = new Deco(dd, m_vLevel);
                AddGameObject(d);
                d.Load(jsonDeco);
            }

			/*var jsonObstacles = (JArray)jsonObject["obstacles"];
			foreach (JObject jsonObstacle in jsonObstacles)
			{
				var dd = (ObstacleData)CSVManager.DataTables.GetDataById(jsonObstacle["data"].ToObject<int>());
				var d = new Obstacle(dd, m_vLevel);
				AddGameObject(d);
				d.Load(jsonObstacle);
			}

			m_vObstacleManager.Load(jsonObject); */
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
            ClientAvatar pl = m_vLevel.Avatar;
            var jsonData = new JObject();
            jsonData.Add("exp_ver", 1);
            jsonData.Add("android_client", pl.m_vAndroid);
            jsonData.Add("active_layout", pl.m_vActiveLayout);
            jsonData.Add("war_layout", pl.m_vActiveLayout);
            jsonData.Add("layout_state", new JArray { 0, 0, 0, 0, 0, 0 });

            JArray JBuildings = new JArray();
            int c = 0;
            foreach (GameObject go in new List<GameObject>(m_vGameObjects[0]))
            {
                Building b = (Building)go;
                JObject j = new JObject();
                j.Add("data", b.GetBuildingData().GetGlobalID());
                j.Add("id", 500000000 + c);
                b.Save(j);
                JBuildings.Add(j);
                c++;
            }
            jsonData.Add("buildings", JBuildings);

            JArray JTraps = new JArray();
            int u = 0;
            foreach (GameObject go in new List<GameObject>(m_vGameObjects[4]))
            {
                Trap t = (Trap)go;
                JObject j = new JObject();
                j.Add("data", t.GetTrapData().GetGlobalID());
                j.Add("id", 504000000 + u);
                t.Save(j);
                JTraps.Add(j);
                u++;
            }
            jsonData.Add("traps", JTraps);

            JArray JDecos = new JArray();
            int e = 0;
            foreach (GameObject go in new List<GameObject>(m_vGameObjects[6]))
            {
                Deco d = (Deco)go;
                JObject j = new JObject();
                j.Add("data", d.GetDecoData().GetGlobalID());
                j.Add("id", 506000000 + e);
                d.Save(j);
                JDecos.Add(j);
                e++;
            }
            jsonData.Add("decos", JDecos);

            /*JArray JObstacles = new JArray();
            int o = 0;
            foreach (GameObject go in new List<GameObject>(m_vGameObjects[3]))
            {
                Obstacle d = (Obstacle)go;
                JObject j = new JObject();
                j.Add("data", d.GetObstacleData().GetGlobalID());
                j.Add("id", 503000000 + o);
                d.Save(j);
                JObstacles.Add(j);
                o++;
            }
            jsonData.Add("obstacles", JObstacles);

            m_vObstacleManager.Save(jsonData); */

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
            jsonData.Add("troop_req_msg", pl.TroopRequestMessage);
            jsonData.Add("last_league_rank", pl.m_vLeagueId);
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
    }
}
