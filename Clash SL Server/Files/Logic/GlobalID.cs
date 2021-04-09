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

namespace CSS.Files.Logic
{
    public static class GlobalID
    {
        #region Public Methods

        public static int CreateGlobalID(int index, int count)
        {
            return count + 1000000 * index;
        }

        public static int GetClassID(int commandType)
        {
            /*
             * Resource:
             * commandeType: 3000000 (2DC6C0)
             * commandType: 786432
             * return 3 + 0
             */

            var r1 = 1125899907;
            commandType = (int) ((r1 * (long) commandType) >> 32);
            return (commandType >> 18) + (commandType >> 31);
        }

        public static int GetInstanceID(int globalID)
        {
            /*
             * Resource:
             * globalID: 3000000 (2DC6C0)
             * r1: 1125899907
             * r1: 786432
             * return 3000000 - 1000000 * (3 + 0)
             */

            var r1 = 1125899907;
            r1 = (int) ((r1 * (long) globalID) >> 32);
            return globalID - 1000000 * ((r1 >> 18) + (r1 >> 31));
        }

        #endregion Public Methods
    }
}