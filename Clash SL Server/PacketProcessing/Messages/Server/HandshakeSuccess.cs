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
using CSS.Core.Crypto.Blake2b;
using CSS.Helpers;
using CSS.PacketProcessing.Messages.Client;

namespace CSS.PacketProcessing.Messages.Server
{
 //Packet 20100
    internal class HandshakeSuccess : Message
{
    private byte[] _sessionKey;
    private static readonly Hasher Blake = Blake2B.Create(new Blake2BConfig { OutputSizeInBytes = 24 });

    public HandshakeSuccess(PacketProcessing.Client client, HandshakeRequest cka) : base(client)
    {
        SetMessageType(20100);
        Blake.Init();
        Blake.Update(Key.Crypto.PublicKey);
        _sessionKey = Blake.Finish();
    }

    public override void Encode()
    {
        var pack = new List<byte>();
        pack.AddInt32(_sessionKey.Length);
        pack.AddRange(_sessionKey);
        SetData(pack.ToArray());
    }
}
}