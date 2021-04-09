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

namespace CSS.Core.Crypto.TweetNaCl
{
    public class xsalsa20
    {
        internal readonly int crypto_stream_xsalsa20_ref_KEYBYTES = 32;
        internal readonly int crypto_stream_xsalsa20_ref_NONCEBYTES = 24;

        public static readonly byte[] sigma = new byte[] {(byte) 'e', (byte) 'x', (byte) 'p', (byte) 'a', (byte) 'n', (byte) 'd', (byte) ' ', (byte) '3', (byte) '2', (byte) '-', (byte) 'b', (byte) 'y', (byte) 't', (byte) 'e', (byte) ' ', (byte) 'k'};

        public static int crypto_stream(byte[] c, int clen, byte[] n, byte[] k)
        {
            byte[] subkey = new byte[32];

            hsalsa20.crypto_core(subkey, n, k, sigma);
            return salsa20.crypto_stream(c, clen, n, 16, subkey);
        }

        public static int crypto_stream_xor(byte[] c, byte[] m, long mlen, byte[] n, byte[] k)
        {
            byte[] subkey = new byte[32];

            hsalsa20.crypto_core(subkey, n, k, sigma);
            return salsa20.crypto_stream_xor(c, m, (int) mlen, n, 16, subkey);
        }
    }
}