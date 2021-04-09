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
using System.IO;
using CSS.Helpers;

namespace CSS.Logic
{
    internal class Base
    {
        #region Private Fields

        int m_vUnknown1;

        #endregion Private Fields

        #region Public Constructors

        public Base(int unknown1)
        {
            m_vUnknown1 = unknown1;
        }

        #endregion Public Constructors

        #region Public Methods

        public virtual void Decode(byte[] baseData)
        {
            using (var br = new CoCSharpPacketReader(new MemoryStream(baseData)))
            {
                m_vUnknown1 = br.ReadInt32WithEndian();
            }
        }

        public virtual byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt32(m_vUnknown1);
            return data.ToArray();
        }

        #endregion Public Methods
    }
}
