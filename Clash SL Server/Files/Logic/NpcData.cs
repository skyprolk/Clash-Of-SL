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

using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class NpcData : Data
    {
        #region Public Constructors

        public NpcData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public bool AlwaysUnlocked { get; set; }
        public int Elixir { get; set; }
        public int ExpLevel { get; set; }
        public int Gold { get; set; }
        public string LevelFile { get; set; }
        public string MapDependencies { get; set; }
        public string MapInstanceName { get; set; }
        public string TID { get; set; }
        public int UnitCount { get; set; }
        public string UnitType { get; set; }

        #endregion Public Properties
    }
}