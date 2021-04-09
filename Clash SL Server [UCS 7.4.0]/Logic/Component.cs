using Newtonsoft.Json.Linq;

namespace CSS.Logic
{
    internal class Component
    {

        public virtual int Type => -1;

        readonly GameObject m_vParentGameObject;
        bool m_vIsEnabled;

        public Component()
        {
        }

        public Component(GameObject go)
        {
            m_vIsEnabled = true;
            m_vParentGameObject = go;
        }

        public GameObject GetParent() => m_vParentGameObject;

        public bool IsEnabled() => m_vIsEnabled;

        public virtual void Load(JObject jsonObject)
        {
        }

        public virtual JObject Save(JObject jsonObject) => jsonObject;

        public void SetEnabled(bool status)
        {
            m_vIsEnabled = status;
        }

        public virtual void Tick()
        {
        }

    }
}