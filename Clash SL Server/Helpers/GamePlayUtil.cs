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

using System;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Helpers
{
    internal static class GamePlayUtil
    {
        #region Public Methods

        public static int CalculateResourceCost(int sup, int inf, int supCost, int infCost, int amount) => 
            (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;

        public static int CalculateSpeedUpCost(int sup, int inf, int supCost, int infCost, int amount) => 
            (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;

        public static int GetResourceDiamondCost(int resourceCount, ResourceData resourceData) => 
            Globals.GetResourceDiamondCost(resourceCount, resourceData);

        public static int GetSpeedUpCost(int seconds) => 
            Globals.GetSpeedUpCost(seconds);

        #endregion Public Methods
    }
}
