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
    internal class GlobalData : Data
    {
        #region Public Constructors

        public GlobalData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public string AltStringArray { get; set; }
        public bool BooleanValue { get; set; }
        public int NumberArray { get; set; }
        public int NumberValue { get; set; }
        public string StringArray { get; set; }
        public string TextValue { get; set; }

        #endregion Public Properties
    }
}