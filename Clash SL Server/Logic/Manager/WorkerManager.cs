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

namespace CSS.Logic.Manager
{
    internal class WorkerManager
    {
        readonly List<GameObject> m_vGameObjectReferences;
       
        int m_vWorkerCount;

        public WorkerManager()
        {
            m_vGameObjectReferences = new List<GameObject>();
            m_vWorkerCount = 0;
        }

        public static int GetFinishTaskOfOneWorkerCost() => 0;

        public static void RemoveGameObjectReferences(GameObject go)
        {
        }

        public void AllocateWorker(GameObject go)
        {
            if (m_vGameObjectReferences.IndexOf(go) == -1)
            {
                m_vGameObjectReferences.Add(go);
            }
        }

        public void DeallocateWorker(GameObject go)
        {
            if (m_vGameObjectReferences.IndexOf(go) != -1)
            {
                m_vGameObjectReferences.Remove(go);
            }
        }

        public void DecreaseWorkerCount() => m_vWorkerCount--;

        public void FinishTaskOfOneWorker()
        {
            var go = GetShortestTaskGO();
            if (go != null)
            {
                if (go.ClassId == 3)
                {
                }
                else
                {
                    var b = (ConstructionItem)go;
                    if (b.IsConstructing())
                        b.SpeedUpConstruction();
                    else
                    {
                        var hero = b.GetHeroBaseComponent();
                        if (hero != null)
                            hero.SpeedUpUpgrade();
                    }
                }
            }
        }

        public int GetFreeWorkers() => m_vWorkerCount - m_vGameObjectReferences.Count;

        public GameObject GetShortestTaskGO()
        {
            GameObject shortestTaskGO = null;
            int shortestGOTime = 0;
            int currentGOTime;

            foreach (GameObject go in m_vGameObjectReferences)
            {
                currentGOTime = -1;
                if (go.ClassId == 3)
                {
                }
                else
                {
                    var c = (ConstructionItem)go;
                    if (c.IsConstructing())
                    {
                        currentGOTime = c.GetRemainingConstructionTime();
                    }
                    else
                    {
                        var hero = c.GetHeroBaseComponent();
                        if (hero != null)
                        {
                            if (hero.IsUpgrading())
                            {
                                currentGOTime = hero.GetRemainingUpgradeSeconds();
                            }
                        }
                    }
                }
                if (shortestTaskGO == null)
                {
                    if (currentGOTime > -1)
                    {
                        shortestTaskGO = go;
                        shortestGOTime = currentGOTime;
                    }
                }
                else if (currentGOTime > -1)
                {
                    if (currentGOTime < shortestGOTime)
                    {
                        shortestGOTime = currentGOTime;
                        shortestTaskGO = go;
                    }
                }
            }
            return shortestTaskGO;
        }

        public int GetTotalWorkers() => m_vWorkerCount;

        public void IncreaseWorkerCount() => m_vWorkerCount++;
    }
}
