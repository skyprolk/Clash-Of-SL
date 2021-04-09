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
    internal class AlliancePortalData : Data
    {
        #region Public Constructors

        public AlliancePortalData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public string ExportName { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string SWF { get; set; }
        public string TID { get; set; }
        public int Width { get; set; }

        #endregion Public Properties
    }
}