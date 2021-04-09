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

using CSS.Core.Threading;

namespace CSS
{
    internal class Program
    {
        #region Public Methods

        public static void Main(string[] args)
        {
            ConsoleThread.Start();
        }

        #endregion Public Methods
    }
}