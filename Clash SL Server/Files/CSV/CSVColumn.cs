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

namespace CSS.Files.CSV
{
    internal class CSVColumn
    {
        #region Private Fields

        readonly List<string> m_vValues;

        #endregion Private Fields

        #region Public Constructors

        public CSVColumn()
        {
            m_vValues = new List<string>();
        }

        #endregion Public Constructors

        #region Public Methods

        public static int GetArraySize(int currentOffset, int nextOffset) => nextOffset - currentOffset;

        public void Add(string value)
        {
            m_vValues.Add(value);
        }

        public string Get(int row) => m_vValues[row];

        public int GetSize() => m_vValues.Count;

        #endregion Public Methods
    }
}
