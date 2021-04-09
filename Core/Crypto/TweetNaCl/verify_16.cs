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
    public class verify_16
    {
        internal readonly int crypto_verify_16_ref_BYTES = 16;

        public static int crypto_verify(byte[] x, int xoffset, byte[] y)
        {
            int differentbits = 0;

            for (int i = 0; i < 15; i++)
            {
                differentbits |= ((int) (x[xoffset + i] ^ y[i])) & 0xff;
            }

            return (1 & ((int) ((uint) ((int) differentbits - 1) >> 8))) - 1;
        }
    }
}