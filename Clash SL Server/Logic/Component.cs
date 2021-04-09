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

using Newtonsoft.Json.Linq;

namespace CSS.Logic
{
    internal class Component
    {

        public virtual int Type
        {
            get { return -1; }
        }

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

        public GameObject GetParent()
        {
            return m_vParentGameObject;
        }

        public bool IsEnabled()
        {
            return m_vIsEnabled;
        }

        public virtual void Load(JObject jsonObject)
        {
        }

        public virtual JObject Save(JObject jsonObject)
        {
            return jsonObject;
        }

        public void SetEnabled(bool status)
        {
            m_vIsEnabled = status;
        }

        public virtual void Tick()
        {
        }

    }
}