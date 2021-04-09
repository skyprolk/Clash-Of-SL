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
    internal class ResourceData : Data
    {
        #region Public Constructors

        public ResourceData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public string CapFullTID { get; set; }
        public string CollectEffect { get; set; }
        public string HudInstanceName { get; set; }
        public bool PremiumCurrency { get; set; }
        public string ResourceIconExportName { get; set; }
        public string StealEffect { get; set; }
        public string SWF { get; set; }
        public int TextBlue { get; set; }
        public int TextGreen { get; set; }
        public int TextRed { get; set; }
        public string TID { get; set; }
        public string WarRefResource { get; set; }

        #endregion Public Properties
    }
}