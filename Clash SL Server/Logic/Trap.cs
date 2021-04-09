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

using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class Trap : ConstructionItem
    {
        #region Public Constructors

        public Trap(Data data, Level l) : base(data, l)
        {
            AddComponent(new TriggerComponent());
        }

        #endregion Public Constructors

        #region Public Properties

        public override int ClassId
        {
            get { return 4; }
        }

        #endregion Public Properties

        #region Public Methods

        public TrapData GetTrapData() => (TrapData)GetData();

        #endregion Public Methods
    }
}
