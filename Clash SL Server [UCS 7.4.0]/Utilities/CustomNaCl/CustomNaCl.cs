namespace UCS.Utilities.CustomNaCl
{
    /// <summary>
    /// Custom written NaCl version
    /// </summary>
    internal class CustomNaCl
    {
        /// <summary>
        /// Decrypts a public-box ciphertext
        /// </summary>
        /// <param name="c">Ciphertext to decrypt</param>
        /// <param name="n">24-byte Nonce</param>
        /// <param name="sk">32-byte SecretKey</param>
        /// <param name="pk">32-byte PublicKey</param>
        /// <returns>Decrypted plaintext</returns>
        public static byte[] OpenPublicBox(byte[] c, byte[] n, byte[] sk, byte[] pk)
        {
            return new PublicBox(sk, pk).open(c, n);
        }

        /// <summary>
        /// Encrypts a public-box plaintext
        /// </summary>
        /// <param name="p">Plaintext to encrypt</param>
        /// <param name="n">24-byte Nonce</param>
        /// <param name="sk">32-byte SecretKey</param>
        /// <param name="pk">32-byte PublicKey</param>
        /// <returns>Encrypted ciphertext</returns>
        public static byte[] CreatePublicBox(byte[] p, byte[] n, byte[] sk, byte[] pk)
        {
            return new PublicBox(sk, pk).create(p, n);
        }

        /// <summary>
        /// Decrypts a secret-box ciphertext
        /// </summary>
        /// <param name="c">Ciphertext to decrypt</param>
        /// <param name="n">24-byte Nonce</param>
        /// <param name="s">32-byte SharedKey</param>
        /// <returns>Decrypted plaintext</returns>
        public static byte[] OpenSecretBox(byte[] c, byte[] n, byte[] s)
        {
            return new SecretBox(s).open(c, n);
        }

        /// <summary>
        /// Encrypts a secret-box plaintext
        /// </summary>
        /// <param name="p">Plaintext to encrypt</param>
        /// <param name="n">24-byte Nonce</param>
        /// <param name="s">32-byte SharedKey</param>
        /// <returns>Encrypted ciphertext</returns>
        public static byte[] CreateSecretBox(byte[] p, byte[] n, byte[] s)
        {
            return new SecretBox(s).create(p, n);
        }
    }
}