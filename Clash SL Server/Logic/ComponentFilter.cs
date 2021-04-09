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

namespace CSS.Logic
{
    internal class ComponentFilter : GameObjectFilter
    {
        #region Public Fields

        public int Type;

        #endregion Public Fields

        #region Public Constructors

        public ComponentFilter(int type)
        {
            Type = type;
        }

        #endregion Public Constructors

        #region Public Methods

        public override bool IsComponentFilter()
        {
            return true;
        }

        public bool TestComponent(Component c)
        {
            var go = c.GetParent();
            return TestGameObject(go);
        }

        public new bool TestGameObject(GameObject go)
        {
            var result = false;
            var c = go.GetComponent(Type, true);
            if (c != null)
                result = base.TestGameObject(go);
            return result;
        }

        #endregion Public Methods
    }
}