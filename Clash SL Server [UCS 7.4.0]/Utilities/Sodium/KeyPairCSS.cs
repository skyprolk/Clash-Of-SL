namespace CSS.Utilities.Sodium
{
    public class KeyPairCSS
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="KeyPairGL"/> class.
        /// </summary>
        /// <param name="_PublicKey">The public key.</param>
        /// <param name="_PrivateKey">The private key.</param>
        public KeyPairCSS(byte[] _PublicKey, byte[] _PrivateKey)
        {
            this.PublicKey = _PublicKey;
            this.PrivateKey = _PrivateKey;
        }

        public byte[] PrivateKey
        {
            get;
            set;
        }

        public byte[] PublicKey
        {
            get;
            set;
        }
    }
}