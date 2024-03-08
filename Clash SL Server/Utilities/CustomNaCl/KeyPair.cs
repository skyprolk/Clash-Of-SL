using UCS.Utilities.TweetNaCl;

namespace UCS.Utilities.CustomNaCl
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