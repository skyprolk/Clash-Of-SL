namespace CSS.Utilities.TweetNaCl
{
    #region Usings

    using System;

    #endregion Usings

    public class curve25519xsalsa20poly1305
    {
        public const int crypto_secretbox_BEFORENMBYTES = 32;
        public const int crypto_secretbox_BOXZEROBYTES = 16;
        public const int crypto_secretbox_NONCEBYTES = 24;
        public const int crypto_secretbox_PUBLICKEYBYTES = 32;
        public const int crypto_secretbox_SECRETKEYBYTES = 32;
        public const int crypto_secretbox_ZEROBYTES = 32;

        public static int crypto_box(byte[] c, byte[] m, long mlen, byte[] n, byte[] pk, byte[] sk)
        {
            byte[] k = new byte[curve25519xsalsa20poly1305.crypto_secretbox_BEFORENMBYTES];
            byte[] kp = k;

            curve25519xsalsa20poly1305.crypto_box_beforenm(kp, pk, sk);
            return curve25519xsalsa20poly1305.crypto_box_afternm(c, m, mlen, n, kp);
        }

        public static int crypto_box(byte[] c, byte[] m, byte[] n, byte[] pk, byte[] sk)
        {
            byte[] cp = c, mp = m, np = n, pkp = pk, skp = sk;
            return curve25519xsalsa20poly1305.crypto_box(cp, mp, m.Length, np, pkp, skp);
        }

        public static int crypto_box_afternm(byte[] c, byte[] m, long mlen, byte[] n, byte[] k)
        {
            return xsalsa20poly1305.crypto_secretbox(c, m, mlen, n, k);
        }

        public static int crypto_box_afternm(byte[] c, byte[] m, byte[] n, byte[] k)
        {
            byte[] cp = c, mp = m, np = n, kp = k;
            return curve25519xsalsa20poly1305.crypto_box_afternm(cp, mp, m.Length, np, kp);
        }

        public static int crypto_box_beforenm(byte[] k, byte[] pk, byte[] sk)
        {
            byte[] s = new byte[32];
            byte[] sp = s, sigmap = xsalsa20.sigma;

            curve25519.crypto_scalarmult(sp, sk, pk);
            return hsalsa20.crypto_core(k, null, sp, sigmap);
        }

        public static int crypto_box_getpublickey(byte[] pk, byte[] sk)
        {
            return curve25519.crypto_scalarmult_base(pk, sk);
        }

        public static int crypto_box_keypair(byte[] pk, byte[] sk)
        {
            new Random().NextBytes(sk);
            return curve25519.crypto_scalarmult_base(pk, sk);
        }

        public static int crypto_box_open(byte[] m, byte[] c, long clen, byte[] n, byte[] pk, byte[] sk)
        {
            byte[] k = new byte[curve25519xsalsa20poly1305.crypto_secretbox_BEFORENMBYTES];
            byte[] kp = k;

            curve25519xsalsa20poly1305.crypto_box_beforenm(kp, pk, sk);
            return curve25519xsalsa20poly1305.crypto_box_open_afternm(m, c, clen, n, kp);
        }

        public static int crypto_box_open(byte[] m, byte[] c, byte[] n, byte[] pk, byte[] sk)
        {
            byte[] cp = c, mp = m, np = n, pkp = pk, skp = sk;
            return curve25519xsalsa20poly1305.crypto_box_open(mp, cp, c.Length, np, pkp, skp);
        }

        public static int crypto_box_open_afternm(byte[] m, byte[] c, long clen, byte[] n, byte[] k)
        {
            return xsalsa20poly1305.crypto_secretbox_open(m, c, clen, n, k);
        }

        public static int crypto_box_open_afternm(byte[] m, byte[] c, byte[] n, byte[] k)
        {
            byte[] cp = c, mp = m, np = n, kp = k;
            return curve25519xsalsa20poly1305.crypto_box_open_afternm(mp, cp, c.Length, np, kp);
        }
    }
}