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

using System.Collections.Generic;
using CSS.Logic;

namespace CSS.PacketProcessing
{
    internal class Command
    {
        #region Public Fields

        public const int MaxEmbeddedDepth = 10;

        #endregion Public Fields

        #region Internal Properties

        internal int Depth { get; set; }

        #endregion Internal Properties

        #region Public Methods

        public virtual byte[] Encode() => new List<byte>().ToArray();

        public virtual void Execute(Level level)
        {
        }

        #endregion Public Methods
    }
}
