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
  class Achievement
    {
        #region Private Fields

        const int m_vType = 0x015EF3C0;

        #endregion Private Fields

        #region Public Constructors

        public Achievement()
        {
        }

        public Achievement(int index)
        {
            Index = index;
            Unlocked = false;
            Value = 0;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => m_vType + Index;

        public int Index { get; set; }
        public string Name { get; set; }
        public bool Unlocked { get; set; }
        public int Value { get; set; }

        #endregion Public Properties
    }
}
