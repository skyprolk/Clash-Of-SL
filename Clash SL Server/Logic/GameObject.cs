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
using System.Windows;
using Newtonsoft.Json.Linq;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class GameObject
    {
        #region Public Constructors

        public GameObject(Data data, Level level)
        {
            m_vLevel = level;
            m_vData = data;
            m_vComponents = new List<Component>();
            for (var i = 0; i < 11; i++)
                m_vComponents.Add(new Component());
        }

        #endregion Public Constructors

        #region Private Fields

        readonly List<Component> m_vComponents;
        readonly Data m_vData;
        readonly Level m_vLevel;

        #endregion Private Fields

        #region Public Properties

        public virtual int ClassId
        {
            get { return -1; }
        }

        public int GlobalId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void AddComponent(Component c)
        {
            if (m_vComponents[c.Type].Type != -1)
            {
            }
            else
            {
                m_vLevel.GetComponentManager().AddComponent(c);
                m_vComponents[c.Type] = c;
            }
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (!test || m_vComponents[index].IsEnabled())
                result = m_vComponents[index];
            return result;
        }

        public Data GetData()
        {
            return m_vData;
        }

        public Level GetLevel()
        {
            return m_vLevel;
        }

        public Vector GetPosition()
        {
            return new Vector(X, Y);
        }

        public virtual bool IsHero()
        {
            return false;
        }

        public void Load(JObject jsonObject)
        {
            X = jsonObject["x"].ToObject<int>();
            Y = jsonObject["y"].ToObject<int>();
            foreach (var c in m_vComponents)
                c.Load(jsonObject);
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);
            foreach (var c in m_vComponents)
                c.Save(jsonObject);
            return jsonObject;
        }

        public void SetPositionXY(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public virtual void Tick()
        {
            foreach (var comp in m_vComponents)
            {
                if (comp.IsEnabled())
                    comp.Tick();
            }
        }

        #endregion Public Methods
    }
}