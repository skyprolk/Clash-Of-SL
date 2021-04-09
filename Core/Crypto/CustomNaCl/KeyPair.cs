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

using CSS.Core.Crypto.TweetNaCl;

namespace CSS.Core.Crypto.CustomNaCl
{
    internal class KeyPair
    {
        byte[] sk = new byte[32], pk = new byte[32];

        /// <summary>
        /// KeyPair constructor
        /// </summary>
        public KeyPair()
        {
            curve25519xsalsa20poly1305.crypto_box_keypair(pk, sk);
        }

        /// <summary>
        /// Returns the randomly generated PublicKey
        /// </summary>
        public byte[] PublicKey
        {
            get
            {
                return pk;
            }
        }

        /// <summary>
        /// Returns the randomly generated SecretKey
        /// </summary>
        public byte[] SecretKey
        {
            get
            {
                return sk;
            }
        }
    }
}