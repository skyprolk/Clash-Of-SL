using System.Collections.Generic;

namespace CSS.Logic
{
    internal class GameObjectFilter
    {
        List<int> m_vIgnoredObjects;

        public void AddIgnoreObject(GameObject go)
        {
            if (m_vIgnoredObjects == null)
            {
                m_vIgnoredObjects = new List<int>();
            }
            m_vIgnoredObjects.Add(go.GlobalId);
        }

        public virtual bool IsComponentFilter() => false;

        public void RemoveAllIgnoreObjects()
        {
            if (m_vIgnoredObjects != null)
            {
                m_vIgnoredObjects.Clear();
                m_vIgnoredObjects = null;
            }
        }

        public bool TestGameObject(GameObject go)
        {
            bool result = true;
            if (m_vIgnoredObjects != null)
            {
                result = m_vIgnoredObjects.IndexOf(go.GlobalId) == -1;
            }
            return result;
        }
    }
}