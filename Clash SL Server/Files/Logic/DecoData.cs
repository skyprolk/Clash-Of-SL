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

using CSS.Core;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class DecoData : Data
    {
        #region Public Constructors

        public DecoData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        #endregion Public Constructors

        #region Public Properties

        public int BuildCost { get; set; }

        public string BuildResource { get; set; }

        public string ExportName { get; set; }

        public string ExportNameBase { get; set; }

        public string ExportNameBaseNpc { get; set; }

        public string ExportNameBaseWar { get; set; }

        public string ExportNameConstruction { get; set; }

        public int Height { get; set; }

        public string Icon { get; set; }

        public string InfoTID { get; set; }

        public int MaxCount { get; set; }

        public int RequiredExpLevel { get; set; }

        public string SWF { get; set; }

        public string TID { get; set; }

        public int Width { get; set; }

        #endregion Public Properties

        #region Public Methods

        public int GetBuildCost()
        {
            return BuildCost;
        }

        public ResourceData GetBuildResource()
        {
            return ObjectManager.DataTables.GetResourceByName(BuildResource);
        }

        public int GetSellPrice()
        {
            var calculation = (int) ((BuildCost * (long) 1717986919) >> 32);
            return (calculation >> 2) + (calculation >> 31);
        }

        #endregion Public Methods
    }
}