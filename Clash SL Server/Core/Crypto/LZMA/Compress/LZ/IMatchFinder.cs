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
// IMatchFinder.cs

using System;

namespace CSS.Core.Crypto.LZMA.Compress.LZ
{
    internal interface IInWindowStream
    {
        void SetStream(System.IO.Stream inStream);

        void Init();

        void ReleaseStream();

        Byte GetIndexByte(Int32 index);

        UInt32 GetMatchLen(Int32 index, UInt32 distance, UInt32 limit);

        UInt32 GetNumAvailableBytes();
    }

    internal interface IMatchFinder : IInWindowStream
    {
        void Create(UInt32 historySize, UInt32 keepAddBufferBefore,
                UInt32 matchMaxLen, UInt32 keepAddBufferAfter);

        UInt32 GetMatches(UInt32[] distances);

        void Skip(UInt32 num);
    }
}