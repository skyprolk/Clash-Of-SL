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
using CSS.Core;

namespace CSS.Logic
{
    internal class ResourceStorageComponent : Component
    {
        #region Public Constructors

        public ResourceStorageComponent(GameObject go) : base(go)
        {
            m_vCurrentResources = new List<int>();
            m_vMaxResources = new List<int>();
            m_vStolenResources = new List<int>();

            var table = ObjectManager.DataTables.GetTable(2);
            var resourceCount = table.GetItemCount();
            for (var i = 0; i < resourceCount; i++)
            {
                m_vCurrentResources.Add(0);
                m_vMaxResources.Add(0);
                m_vStolenResources.Add(0);
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public override int Type => 6;

        #endregion Public Properties

        #region Private Fields

        readonly List<int> m_vCurrentResources;
        readonly List<int> m_vStolenResources;
        List<int> m_vMaxResources;

        #endregion Private Fields

        #region Public Methods

        public int GetCount(int resourceIndex) => m_vCurrentResources[resourceIndex];

        public int GetMax(int resourceIndex) => m_vMaxResources[resourceIndex];

        public void SetMaxArray(List<int> resourceCaps)
        {
            m_vMaxResources = resourceCaps;
            GetParent().GetLevel().GetComponentManager().RefreshResourcesCaps();
        }

        #endregion Public Methods
    }
}
