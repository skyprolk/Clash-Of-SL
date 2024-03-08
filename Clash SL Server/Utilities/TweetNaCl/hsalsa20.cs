namespace CSS.Utilities.TweetNaCl
{
    public class hsalsa20
    {
        internal const int ROUNDS = 20;

        public static int crypto_core(byte[] outv, byte[] inv, byte[] k, byte[] c)
        {
            int x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15;
            int j0, j1, j2, j3, j4, j5, j6, j7, j8, j9, j10, j11, j12, j13, j14, j15;
            int i;

            j0 = x0 = hsalsa20.load_littleendian(c, 0);
            j1 = x1 = hsalsa20.load_littleendian(k, 0);
            j2 = x2 = hsalsa20.load_littleendian(k, 4);
            j3 = x3 = hsalsa20.load_littleendian(k, 8);
            j4 = x4 = hsalsa20.load_littleendian(k, 12);
            j5 = x5 = hsalsa20.load_littleendian(c, 4);

            if (inv != null)
            {
                j6 = x6 = hsalsa20.load_littleendian(inv, 0);
                j7 = x7 = hsalsa20.load_littleendian(inv, 4);
                j8 = x8 = hsalsa20.load_littleendian(inv, 8);
                j9 = x9 = hsalsa20.load_littleendian(inv, 12);
            }
            else
            {
                j6 = x6 = j7 = x7 = j8 = x8 = j9 = x9 = 0;
            }

            j10 = x10 = hsalsa20.load_littleendian(c, 8);
            j11 = x11 = hsalsa20.load_littleendian(k, 16);
            j12 = x12 = hsalsa20.load_littleendian(k, 20);
            j13 = x13 = hsalsa20.load_littleendian(k, 24);
            j14 = x14 = hsalsa20.load_littleendian(k, 28);
            j15 = x15 = hsalsa20.load_littleendian(c, 12);

            for (i = hsalsa20.ROUNDS; i > 0; i -= 2)
            {
                x4 ^= hsalsa20.rotate(x0 + x12, 7);
                x8 ^= hsalsa20.rotate(x4 + x0, 9);
                x12 ^= hsalsa20.rotate(x8 + x4, 13);
                x0 ^= hsalsa20.rotate(x12 + x8, 18);
                x9 ^= hsalsa20.rotate(x5 + x1, 7);
                x13 ^= hsalsa20.rotate(x9 + x5, 9);
                x1 ^= hsalsa20.rotate(x13 + x9, 13);
                x5 ^= hsalsa20.rotate(x1 + x13, 18);
                x14 ^= hsalsa20.rotate(x10 + x6, 7);
                x2 ^= hsalsa20.rotate(x14 + x10, 9);
                x6 ^= hsalsa20.rotate(x2 + x14, 13);
                x10 ^= hsalsa20.rotate(x6 + x2, 18);
                x3 ^= hsalsa20.rotate(x15 + x11, 7);
                x7 ^= hsalsa20.rotate(x3 + x15, 9);
                x11 ^= hsalsa20.rotate(x7 + x3, 13);
                x15 ^= hsalsa20.rotate(x11 + x7, 18);
                x1 ^= hsalsa20.rotate(x0 + x3, 7);
                x2 ^= hsalsa20.rotate(x1 + x0, 9);
                x3 ^= hsalsa20.rotate(x2 + x1, 13);
                x0 ^= hsalsa20.rotate(x3 + x2, 18);
                x6 ^= hsalsa20.rotate(x5 + x4, 7);
                x7 ^= hsalsa20.rotate(x6 + x5, 9);
                x4 ^= hsalsa20.rotate(x7 + x6, 13);
                x5 ^= hsalsa20.rotate(x4 + x7, 18);
                x11 ^= hsalsa20.rotate(x10 + x9, 7);
                x8 ^= hsalsa20.rotate(x11 + x10, 9);
                x9 ^= hsalsa20.rotate(x8 + x11, 13);
                x10 ^= hsalsa20.rotate(x9 + x8, 18);
                x12 ^= hsalsa20.rotate(x15 + x14, 7);
                x13 ^= hsalsa20.rotate(x12 + x15, 9);
                x14 ^= hsalsa20.rotate(x13 + x12, 13);
                x15 ^= hsalsa20.rotate(x14 + x13, 18);
            }

            x0 += j0;
            x1 += j1;
            x2 += j2;
            x3 += j3;
            x4 += j4;
            x5 += j5;
            x6 += j6;
            x7 += j7;
            x8 += j8;
            x9 += j9;
            x10 += j10;
            x11 += j11;
            x12 += j12;
            x13 += j13;
            x14 += j14;
            x15 += j15;

            x0 -= hsalsa20.load_littleendian(c, 0);
            x5 -= hsalsa20.load_littleendian(c, 4);
            x10 -= hsalsa20.load_littleendian(c, 8);
            x15 -= hsalsa20.load_littleendian(c, 12);

            if (inv != null)
            {
                x6 -= hsalsa20.load_littleendian(inv, 0);
                x7 -= hsalsa20.load_littleendian(inv, 4);
                x8 -= hsalsa20.load_littleendian(inv, 8);
                x9 -= hsalsa20.load_littleendian(inv, 12);
            }

            hsalsa20.store_littleendian(outv, 0, x0);
            hsalsa20.store_littleendian(outv, 4, x5);
            hsalsa20.store_littleendian(outv, 8, x10);
            hsalsa20.store_littleendian(outv, 12, x15);
            hsalsa20.store_littleendian(outv, 16, x6);
            hsalsa20.store_littleendian(outv, 20, x7);
            hsalsa20.store_littleendian(outv, 24, x8);
            hsalsa20.store_littleendian(outv, 28, x9);

            return 0;
        }

        internal static int load_littleendian(byte[] x, int offset)
        {
            return (x[offset] & 0xff) | ((x[offset + 1] & 0xff) << 8) | ((x[offset + 2] & 0xff) << 16) | ((x[offset + 3] & 0xff) << 24);
        }

        internal static int rotate(int u, int c)
        {
            return (u << c) | (int) ((uint) u >> (32 - c));
        }

        internal static void store_littleendian(byte[] x, int offset, int u)
        {
            x[offset] = (byte) u;
            u = (int) ((uint) u >> 8);
            x[offset + 1] = (byte) u;
            u = (int) ((uint) u >> 8);
            x[offset + 2] = (byte) u;
            u = (int) ((uint) u >> 8);
            x[offset + 3] = (byte) u;
        }
    }
}