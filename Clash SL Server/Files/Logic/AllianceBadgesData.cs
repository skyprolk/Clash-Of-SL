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
    internal class AllianceBadgesData : Data
    {
        #region Public Constructors

        public AllianceBadgesData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public string IconExportName { get; set; }
        public string IconLayer0 { get; set; }
        public string IconLayer1 { get; set; }
        public string IconLayer2 { get; set; }
        public string IconSWF { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }
}