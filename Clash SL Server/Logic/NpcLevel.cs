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
    internal class NpcLevel
    {
        #region Private Fields

        const int m_vType = 0x01036640;

        #endregion Private Fields

        #region Public Constructors

        public NpcLevel()
        {
        }

        public NpcLevel(int index)
        {
            Index = index;
            Stars = 0;
            LootedGold = 0;
            LootedElixir = 0;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id
        {
            get { return m_vType + Index; }
        }

        public int Index { get; set; }
        public int LootedElixir { get; set; }
        public int LootedGold { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; }

        #endregion Public Properties
    }
}