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

using Sodium;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using CSS.Logic;
using CSS.Helpers;

namespace CSS.PacketProcessing
{
    class Client
    {
        readonly long m_vSocketHandle;
        Level m_vLevel;

        public Client(Socket so)
        {
            Socket = so;
            m_vSocketHandle = so.Handle.ToInt64();
            DataStream = new List<byte>();
            CState = 0;
        }

        public string CIPAddress { get; set; }
        public byte[] CPublicKey { get; set; }
        public byte[] CRNonce { get; set; }
        public byte[] CSessionKey { get; set; }
        public byte[] CSharedKey { get; set; }
        public byte[] CSNonce { get; set; }
        public int CState { get; set; }
        public List<byte> DataStream { get; set; }
        public Socket Socket { get; set; }
        public Level GetLevel() => m_vLevel;
        public long GetSocketHandle() => m_vSocketHandle;

        public bool IsClientSocketConnected()
        {
            try
            {
                return !((Socket.Poll(1000, SelectMode.SelectRead) && (Socket.Available == 0)) || !Socket.Connected);
            }
            catch
            {
                return false;
            }
        }

        public void SetLevel(Level l) => m_vLevel = l;

        public bool TryGetPacket(out Message p)
        {
            p = null;
            bool result = false;
            if (DataStream.Count >= 5)
            {
                int length = (0x00 << 24) | (DataStream[2] << 16) | (DataStream[3] << 8) | DataStream[4];
                ushort type = (ushort)((DataStream[0] << 8) | DataStream[1]);
                if (DataStream.Count - 7 >= length)
                {
                    object obj;
                    byte[] packet = DataStream.Take(7 + length).ToArray();
                    using (CoCSharpPacketReader br = new CoCSharpPacketReader(new MemoryStream(packet)))
                        obj = MessageFactory.Read(this, br, type);
                    if (obj != null)
                    {
                        p = (Message)obj;
                        result = true;
                    }
                    DataStream.RemoveRange(0, 7 + length);
                }
            }
            return result;
        }
    }
}
