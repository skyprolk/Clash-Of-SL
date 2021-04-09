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
// BLAKE2 reference source code package - C# implementation

// Written in 2012 by Christian Winnerlein  <codesinchaos@gmail.com>

// To the extent possible under law, the author(s) have dedicated all copyright
// and related and neighboring rights to this software to the public domain
// worldwide. This software is distributed without any warranty.

// You should have received a copy of the CC0 Public Domain Dedication along with
// this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

namespace CSS.Core.Crypto.Blake2b
{
    public abstract class NodeHasher
    {
        public abstract void Init(int depth, long nodeOffset);

        public abstract byte[] Finish(bool isEndOfLayer);

        public abstract void Update(byte[] data, int start, int count);

        public void Update(byte[] data)
        {
            Update(data, 0, data.Length);
        }
    }
}