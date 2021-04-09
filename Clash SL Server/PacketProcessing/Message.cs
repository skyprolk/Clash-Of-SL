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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSS.Helpers;
using CSS.Logic;
using CSS.Core.Crypto.CustomNaCl;
using CSS.Core.Crypto.Blake2b;
using static System.Console;

namespace CSS.PacketProcessing
{
    internal class Message
    {

        byte[] m_vData;
        int m_vLength;
        ushort m_vMessageVersion;
        ushort m_vType;
        // Constants
        const int KeyLength = 32, NonceLength = 24, SessionLength = 24;

        // A custom keypair used for en/decryption 
        static KeyPair CustomKeyPair = new KeyPair();

        static Hasher Blake = Blake2B.Create(new Blake2BConfig() { OutputSizeInBytes = 24 });

        public Message()
        {

        }

        public Message(Client c)
        {
            Client = c;
            m_vType = 0;
            m_vLength = -1;
            m_vMessageVersion = 0;
            m_vData = null;
        }

        public Message(Client c, BinaryReader br)
        {
            Client = c;
            m_vType = br.ReadUInt16WithEndian();
            var tempLength = br.ReadBytes(3);
            m_vLength = (0x00 << 24) | (tempLength[0] << 16) | (tempLength[1] << 8) | tempLength[2];
            m_vMessageVersion = br.ReadUInt16WithEndian();
            m_vData = br.ReadBytes(m_vLength);
        }

        public int Broadcasting { get; set; }

        public Client Client { get; set; }

        public virtual void Decode()
        {

        }

        public void Decrypt()
        {
            try
            {
                if (m_vType == 10101)
                {
                    byte[] cipherText = m_vData;
                    Client.CPublicKey = cipherText.Take(32).ToArray();
                    Hasher b = Blake2B.Create(new Blake2BConfig
                    {
                        OutputSizeInBytes = 24
                    });
                    b.Init();
                    b.Update(Client.CPublicKey);
                    b.Update(Key.Crypto.PublicKey);
                    Client.CRNonce = b.Finish();
                    cipherText = CustomNaCl.OpenPublicBox(cipherText.Skip(32).ToArray(), Client.CRNonce, Key.Crypto.PrivateKey, Client.CPublicKey);
                    Client.CSharedKey = Client.CPublicKey;
                    Client.CSessionKey = cipherText.Take(24).ToArray();
                    Client.CSNonce = cipherText.Skip(24).Take(24).ToArray();
                    Client.CState = 1;
                    SetData(cipherText.Skip(48).ToArray());
                }
                else if (m_vType != 10100)
                {
                    Client.CSNonce.Increment();
                    SetData(CustomNaCl.OpenSecretBox(new byte[16].Concat(m_vData).ToArray(), Client.CSNonce, Client.CSharedKey));
                }
            }
            catch (Exception ex)
            {
                Client.CState = 0;
            }
        }

        public virtual void Encode()
        {

        }

        public void Encrypt(byte[] plainText)
        {
            try
            {
                 if (GetMessageType() == 20104 || GetMessageType() == 20103)
                {
                    Blake.Init();
                    Blake.Update(Client.CSNonce);
                    Blake.Update(Client.CPublicKey);
                    Blake.Update(Key.Crypto.PublicKey);
                    var tmpNonce = Blake.Finish();
                    plainText = Client.CRNonce.Concat(Client.CSharedKey).Concat(plainText).ToArray();
                    SetData(CustomNaCl.CreatePublicBox(plainText, tmpNonce, Key.Crypto.PrivateKey, Client.CPublicKey));
                    if (GetMessageType() == 20104)
                        Client.CState = 2;
                }
                else
                {
                    Client.CRNonce.Increment();
                    SetData(CustomNaCl.CreateSecretBox(plainText, Client.CRNonce, Client.CSharedKey).Skip(16).ToArray());
                }
            }
            catch (Exception ex)
            {
                Client.CState = 0;
            }
        }

        public byte[] GetData() => m_vData;

        public int GetLength() => m_vLength;

        public ushort GetMessageType() => m_vType;

        public ushort GetMessageVersion() => m_vMessageVersion;

        public byte[] GetRawData()
        {
            var encodedMessage = new List<byte>();
            encodedMessage.AddRange(BitConverter.GetBytes(m_vType).Reverse());
            encodedMessage.AddRange(BitConverter.GetBytes(m_vLength).Reverse().Skip(1));
            encodedMessage.AddRange(BitConverter.GetBytes(m_vMessageVersion).Reverse());
            encodedMessage.AddRange(m_vData);
            return encodedMessage.ToArray();
        }

        public virtual void Process(Level level)
        {

        }

        public void SetData(byte[] data)
        {
            m_vData = data;
            m_vLength = data.Length;
        }

        public void SetMessageType(ushort type)
        {
            m_vType = type;
        }

        public void SetMessageVersion(ushort v)
        {
            m_vMessageVersion = v;
        }

        public string ToHexString()
        {
            var hex = BitConverter.ToString(m_vData);
            return hex.Replace("-", " ");
        }

        public override string ToString() => Encoding.UTF8.GetString(m_vData, 0, m_vLength);
    }
}
