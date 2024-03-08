using System.Collections.Generic;
using CSS.Core;

namespace CSS.Logic
{
    internal class ResourceStorageComponent : Component
    {
        public ResourceStorageComponent(GameObject go) : base(go)
        {
            m_vCurrentResources = new List<int>();
            m_vMaxResources = new List<int>();
            m_vStolenResources = new List<int>();

            var table = CSVManager.DataTables.GetTable(2);
            var resourceCount = table.GetItemCount();
            for (var i = 0; i < resourceCount; i++)
            {
                m_vCurrentResources.Add(0);
                m_vMaxResources.Add(0);
                m_vStolenResources.Add(0);
            }
        }

        public override int Type => 6;
    
        readonly List<int> m_vCurrentResources;
        readonly List<int> m_vStolenResources;
        List<int> m_vMaxResources;

        public int GetCount(int resourceIndex) => m_vCurrentResources[resourceIndex];

        public int GetMax(int resourceIndex) => m_vMaxResources[resourceIndex];

        public void SetMaxArray(List<int> resourceCaps)
        {
            m_vMaxResources = resourceCaps;
            GetParent().Avatar.GetComponentManager().RefreshResourcesCaps();
        }
    }
}
