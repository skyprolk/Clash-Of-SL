using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json.Linq;
using CSS.Files.Logic;
using System.Threading.Tasks;
using CSS.Logic.Enums;
using System;

namespace CSS.Logic
{
    internal class GameObject
    {
        public GameObject(Data data, Level level)
        {
            m_vLevel      = level;
            m_vData       = data;
            m_vComponents = new List<Component>();
            for (var i = 0; i < 11; i++)
            {
                m_vComponents.Add(new Component());
            }
        }

        readonly List<Component> m_vComponents;
        readonly Data m_vData;
        readonly Level m_vLevel;

        public virtual int ClassId => -1;

        public int GlobalId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        /*public int L1X { get; set; }

        public int L1Y { get; set; }

        public int L2X { get; set; }

        public int L2Y { get; set; }

        public int L3X { get; set; }

        public int L3Y { get; set; }*/

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
            {
                result = m_vComponents[index];
            }
            return result;
        }

        public Data GetData() => m_vData;

        public Level Avatar => m_vLevel;

        public Vector GetPosition() => new Vector(X, Y);

        public virtual bool IsHero() => false;

        public int TownHallLevel() => Avatar.Avatar.m_vTownHallLevel;

        public int LayoutID() => Avatar.Avatar.m_vActiveLayout;

        public void Load(JObject jsonObject)
        {
            X = jsonObject["x"].ToObject<int>();
            Y = jsonObject["y"].ToObject<int>();

           /* if (TownHallLevel() >= 4)
            {
                L1X = jsonObject["l1x"].ToObject<int>();
                L1Y = jsonObject["l1y"].ToObject<int>();

                L2X = jsonObject["l2x"].ToObject<int>();
                L2Y = jsonObject["l2y"].ToObject<int>();

                if (TownHallLevel() >= 6)
                {
                    L3X = jsonObject["l3x"].ToObject<int>();
                    L3Y = jsonObject["l3y"].ToObject<int>();
                }
            } */

            foreach(Component c in m_vComponents)
            {
                c.Load(jsonObject);
            }
        }    

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);

            /*if (TownHallLevel() >= 4)
            {
                if (LayoutID() == 2)
                {
                    jsonObject.Add("lmx", L2X);
                    jsonObject.Add("lmy", L2Y);
                }
                else if (LayoutID() == 3)
                {
                    jsonObject.Add("lmx", L3X);
                    jsonObject.Add("lmy", L3Y);
                }

                jsonObject.Add("l1x", L1X);
                jsonObject.Add("l1y", L1Y);

                jsonObject.Add("l2x", L2X);
                jsonObject.Add("l2y", L2Y);

                if (TownHallLevel() >= 6)
                {
                    jsonObject.Add("l3x", L3X);
                    jsonObject.Add("l3y", L3Y);
                }  
            }*/

            foreach(Component c in m_vComponents)
            {
                c.Save(jsonObject);
            }

            return jsonObject;
        }

        public void SetPositionXY(int newX, int newY, int Layout)
        {
            /*if (Layout == LayoutID())
            {*/
                X = newX;
                Y = newY;
            //}
            /*if (Layout == Convert.ToInt32(Layouts.Layout.WarLayout1))
            {
                L1X = newX;
                L1Y = newY;
            }
            else if (Layout == Convert.ToInt32(Layouts.Layout.Layout2))
            {
                L2X = newX;
                L2Y = newY;
            }
            else if (Layout == Convert.ToInt32(Layouts.Layout.Layout3))
            {
                L3X = newX;
                L3Y = newY;
            } */
        }

        public virtual void Tick()
        {
            foreach(Component comp in m_vComponents)
            {
                if (comp.IsEnabled())
                    comp.Tick();
            }
        }
    }
}