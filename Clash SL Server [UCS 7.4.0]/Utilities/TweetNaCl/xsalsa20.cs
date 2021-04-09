namespace CSS.Utilities.TweetNaCl
{
    public class xsalsa20
    {
        public static readonly byte[] sigma =
        {
            (byte) 'e', (byte) 'x', (byte) 'p', (byte) 'a', (byte) 'n', (byte) 'd', (byte) ' ', (byte) '3', (byte) '2', (byte) '-', (byte) 'b', (byte) 'y', (byte) 't', (byte) 'e', (byte) ' ', (byte) 'k'
        };

        internal readonly int crypto_stream_xsalsa20_ref_KEYBYTES = 32;
        internal readonly int crypto_stream_xsalsa20_ref_NONCEBYTES = 24;

        public static int crypto_stream(byte[] c, int clen, byte[] n, byte[] k)
        {
            byte[] subkey = new byte[32];

            hsalsa20.crypto_core(subkey, n, k, xsalsa20.sigma);
            return salsa20.crypto_stream(c, clen, n, 16, subkey);
        }

        public static int crypto_stream_xor(byte[] c, byte[] m, long mlen, byte[] n, byte[] k)
        {
            byte[] subkey = new byte[32];

            hsalsa20.crypto_core(subkey, n, k, xsalsa20.sigma);
            return salsa20.crypto_stream_xor(c, m, (int) mlen, n, 16, subkey);
        }
    }
}