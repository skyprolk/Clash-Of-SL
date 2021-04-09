using Sodium;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using UCS.Logic;
using UCS.Helpers;
using System;
using UCS.Core.Crypto;
using UCS.Core.Settings;
using System.Threading.Tasks;
using UCS.Core.Network;

namespace UCS.Packets
{
    internal class Client
    {
        readonly IntPtr m_vSocketHandle;
        Level m_vLevel;

        public Client(Socket so)
        {
            Socket = so;
            m_vSocketHandle = so.Handle;
            DataStream = new List<byte>();
            State = ClientState.Exception;
            IncomingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, IncomingPacketsKey, Key._RC4_EndecryptKey.Length);
            OutgoingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, OutgoingPacketsKey, Key._RC4_EndecryptKey.Length);
        }

        public string CIPAddress { get; set; }
        public byte[] CPublicKey { get; set; }
        public byte[] CRNonce { get; set; }
        public byte[] CSessionKey { get; set; }
        public byte[] CSharedKey { get; set; }
        public byte[] CSNonce { get; set; }
        public ClientState State { get; set; }
        public List<byte> DataStream { get; set; }
        public Socket Socket { get; set; }
        public Level GetLevel() => m_vLevel;
        public IntPtr GetSocketHandle() => m_vSocketHandle;
        public uint ClientSeed { get; set; }
        public byte[] IncomingPacketsKey { get; set; }
        public byte[] OutgoingPacketsKey { get; set; }

        public enum ClientState : int
        {
            Exception    = 0,
            Login        = 1,
            LoginSuccess = 2,
        }

        public static void TransformSessionKey(int clientSeed, byte[] sessionKey)
        {
            int[] buffer = new int[624];
            initialize_generator(clientSeed, buffer);
            int byte100 = 0;
            for (int i = 0; i < 100; i++)
            {
                byte100 = extract_number(buffer, i);
            }

            for (int i = 0; i < sessionKey.Length; i++)
            {
                sessionKey[i] ^= (byte)(extract_number(buffer, i + 100) & byte100);
            }
        }

        // Initialize the generator from a seed
        public static void initialize_generator(int seed, int[] buffer)
        {
            buffer[0] = seed;
            for (int i = 1; i < 624; ++i)
            {
                buffer[i] = (int)(1812433253 * ((buffer[i - 1] ^ (buffer[i - 1] >> 30)) + 1));
            }
        }
        // Extract a tempered pseudorandom number based on the index-th value,
        // calling generate_numbers() every 624 numbers

        public static int extract_number(int[] buffer, int ix)
        {
            if (ix == 0)
            {
                generate_numbers(buffer);
            }

            int y = buffer[ix];
            y ^= (y >> 11);
            y ^= (int)(y << 7 & (2636928640)); // 0x9d2c5680
            y ^= (int)(y << 15 & (4022730752)); // 0xefc60000
            y ^= (y >> 18);

            if ((y & (1 << 31)) != 0)
            {
                y = ~y + 1;
            }

            ix = (ix + 1) % 624;
            return y % 256;
        }

        public static void generate_numbers(int[] buffer)
        {
            for (int i = 0; i < 624; i++)
            {
                int y = (int)((buffer[i] & 0x80000000) + (buffer[(i + 1) % 624] & 0x7fffffff));
                buffer[i] = (int)(buffer[(i + 397) % 624] ^ (y >> 1));
                if ((y % 2) != 0)
                {
                    buffer[i] = (int)(buffer[i] ^ (2567483615));
                }
            }
        }

        public unsafe void UpdateKey(byte[] sessionKey)
        {
            TransformSessionKey((int)ClientSeed, sessionKey);

            byte[] newKey = new byte[264];
            byte[] clientKey = sessionKey;
            int v7 = Key._RC4_PrivateKey.Length;
            int v9 = Key._RC4_PrivateKey.Length + sessionKey.Length;
            byte[] completeSessionKey = new byte[Key._RC4_PrivateKey.Length + sessionKey.Length];
            Array.Copy(Key._RC4_PrivateKey, 0, completeSessionKey, 0, v7); //memcpy(v10, v8, v7);
            Array.Copy(clientKey, 0, completeSessionKey, v7, sessionKey.Length); //memcpy(v10 + v7, clientKey, sessionKeySize);
            uint v11 = 0;
            uint v16;
            uint v12;//attention type
            byte v13;//attention type
            uint v14;
            byte* v15;
            uint v17;
            uint v18;
            byte v19;
            byte* v20;
            byte v21;
            uint v22;
            byte* v23;

            fixed (byte* v5 = newKey, v8 = Key._RC4_PrivateKey, v10 = completeSessionKey)
            {
                do
                {
                    *(byte*)(v5 + v11 + 8) = (byte)v11;
                    ++v11;
                }
                while (v11 != 256);
                *v5 = 0;
                *(v5 + 4) = 0;
                while (true)
                {
                    v16 = *v5;

                    //if (v16 == 255)//if ( *v5 > 255 )
                    //    break;
                    v12 = *((byte*)v10 + v16 % v9) + *(uint*)(v5 + 4);
                    *(uint*)v5 = v16 + 1;
                    v13 = *(byte*)(v5 + v16 + 8);
                    v14 = (byte)(v12 + *(byte*)(v5 + v16 + 8));
                    *(uint*)(v5 + 4) = v14;
                    v15 = v5 + v14;
                    *(byte*)(v5 + v16 + 8) = *(byte*)(v15 + 8);
                    *(byte*)(v15 + 8) = v13;
                    if (v16 == 255)//if ( *v5 > 255 )
                        break;
                }
                v17 = 0;
                *v5 = 0;
                *(v5 + 4) = 0;
                while (v17 < v9)
                {
                    ++v17;
                    v18 = *(uint*)(v5 + 4);
                    v19 = (byte)(*(uint*)v5 + 1);
                    *(uint*)v5 = v19;
                    v20 = v5 + v19;
                    v21 = *(byte*)(v20 + 8);
                    v22 = (byte)(v18 + v21);
                    *(uint*)(v5 + 4) = v22;
                    v23 = v5 + v22;
                    *(byte*)(v20 + 8) = *(byte*)(v23 + 8);
                    *(byte*)(v23 + 8) = v21;
                }
            }
            Array.Copy(newKey, IncomingPacketsKey, newKey.Length);
            Array.Copy(newKey, OutgoingPacketsKey, newKey.Length);
        }

        public void EnDecrypt(Byte[] key, Byte[] data)
        {
            int dataLen;

            if (data != null)
            {
                dataLen = data.Length;

                if (dataLen >= 1)
                {
                    do
                    {
                        dataLen--;
                        byte index = (byte)(key[0] + 1);
                        key[0] = index;
                        byte num2 = (byte)(key[4] + key[index + 8]);
                        key[4] = num2;
                        byte num3 = key[index + 8];
                        key[index + 8] = key[num2 + 8];
                        key[key[4] + 8] = num3;
                        byte num4 = key[((byte)(key[key[4] + 8] + key[key[0] + 8])) + 8];
                        data[(data.Length - dataLen) - 1] = (byte)(data[(data.Length - dataLen) - 1] ^ num4);
                    }
                    while (dataLen > 0);
                }
            }
        }

        public void Decrypt(Byte[] data)
        {
            EnDecrypt(this.IncomingPacketsKey, data);
        }

        public void Encrypt(Byte[] data)
        {
            EnDecrypt(this.OutgoingPacketsKey, data);
        }

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

        public bool TryGetPacket(out Message _Message)
        {
            _Message = null;
            bool result = false;
            if (DataStream.Count >= 5)
            {
                int length = (DataStream[2] << 16) | (DataStream[3] << 8) | DataStream[4];
                ushort type = (ushort)((DataStream[0] << 8) | DataStream[1]);
                if (DataStream.Count - 7 >= length)
                {
                    object obj;
                    byte[] packet = DataStream.Take(7 + length).ToArray();
                    using (PacketReader br = new PacketReader(new MemoryStream(packet)))
                    {
                        obj = MessageFactory.Read(this, br, type);
                    }
                    if (obj != null)
                    {
                        _Message = (Message)obj;
                        result = true;
                    }
                    else
                    {
                        if (Constants.IsRc4)
                        {
                            Decrypt(DataStream.Skip(7).Take(length).ToArray());
                        }
                        else
                        {
                            CSNonce.Increment();                           
                        }    
                    }
                    DataStream.RemoveRange(0, 7 + length);
                }
                else
                {
                    DataStream.RemoveRange(0, 7 + length);
                    this.Socket.Close();
                }
            }
            return result;
        }
    }
}
