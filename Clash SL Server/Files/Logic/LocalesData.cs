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
    internal class LocalesData : Data
    {
        #region Public Constructors

        public LocalesData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public string Description { get; set; }
        public bool HasEvenSpaceCharacters { get; set; }
        public string HelpshiftSDKLanguage { get; set; }
        public string HelpshiftSDKLanguageAndroid { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string TestExcludes { get; set; }
        public bool TestLanguage { get; set; }
        public string UsedSystemFont { get; set; }

        #endregion Public Properties
    }
}