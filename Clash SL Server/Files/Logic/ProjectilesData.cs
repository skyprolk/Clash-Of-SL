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
    internal class ProjectilesData : Data
    {
        #region Public Constructors

        public ProjectilesData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public string Effect { get; set; }
        public string ExportName { get; set; }
        public bool IsBallistic { get; set; }
        public string Name { get; set; }
        public string ParticleEmitter { get; set; }
        public bool PlayOnce { get; set; }
        public bool RandomHitPosition { get; set; }
        public int Scale { get; set; }
        public string ShadowExportName { get; set; }
        public string ShadowSWF { get; set; }
        public int Speed { get; set; }
        public int StartHeight { get; set; }
        public int StartOffset { get; set; }
        public string SWF { get; set; }
        public bool UseRotate { get; set; }
        public bool UseTopLayer { get; set; }

        #endregion Public Properties
    }
}