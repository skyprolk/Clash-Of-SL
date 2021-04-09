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
using System.Configuration;
using System.Threading;
using CSS.Core.Network;
using CSS.Core;

namespace CSS.Core.Threading
{
    internal class NetworkThread
    {
        #region Private Properties

        static Thread T { get; set; }

        #endregion Private Properties

        #region Public Methods

        public static void Start()
        {
            T = new Thread(() =>
            {
                //new VersionChecker();
                new PacketManager().Start();
                new MessageManager();
                new ResourcesManager();
                new ObjectManager();
                new Gateway().Start();
            });
            T.Start();
        }

        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }

        #endregion Public Methods
    }
}
