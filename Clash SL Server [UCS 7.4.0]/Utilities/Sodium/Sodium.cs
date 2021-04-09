namespace CSS.Utilities.Sodium
{
    internal class Sodium
    {
        /// <summary>
        /// Encrypt a packet using Sodium Public Box.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        /// <param name="Nonce">The nonce.</param>
        /// <param name="PrivateKey">The private key.</param>
        /// <param name="PublicKey">The public key.</param>
        /// <returns>The encrypted packet in bytes.</returns>
        internal static byte[] Encrypt(byte[] Packet, byte[] Nonce, byte[] PrivateKey, byte[] PublicKey)
        {
            return new PublicBox(PrivateKey, PublicKey).Encrypt(Packet, Nonce);
        }

        /// <summary>
        /// Encrypt a packet using Sodium Secret Box.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        /// <param name="Nonce">The nonce.</param>
        /// <param name="SharedKey">The shared key.</param>
        /// <returns>The encrypted packet in bytes.</returns>
        internal static byte[] Encrypt(byte[] Packet, byte[] Nonce, byte[] SharedKey)
        {
            return new SecretBox(SharedKey).Encrypt(Packet, Nonce);
        }

        /// <summary>
        /// Generate a key pair.
        /// </summary>
        /// <param name="PublicKey">The public key.</param>
        /// <param name="PrivateKey">The private key.</param>
        /// <returns>The generated KeyPair.</returns>
        internal static KeyPairCSS GenerateKeyPair(byte[] PublicKey, byte[] PrivateKey)
        {
            return new KeyPairCSS(PublicKey, PrivateKey);
        }

        /// <summary>
        /// Decrypt a packet using Sodium Public Box.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        /// <param name="Nonce">The nonce.</param>
        /// <param name="PrivateKey">The private key.</param>
        /// <param name="PublicKey">The public key.</param>
        /// <returns>The decrypted packet in bytes.</returns>
        internal static byte[] Decrypt(byte[] Packet, byte[] Nonce, byte[] PrivateKey, byte[] PublicKey)
        {
            return new PublicBox(PrivateKey, PublicKey).Decrypt(Packet, Nonce);
        }

        /// <summary>
        /// Decrypt a packet using Sodium Secret Box.
        /// </summary>
        /// <param name="Packet">The packet.</param>
        /// <param name="Nonce">The nonce.</param>
        /// <param name="SharedKey">The shared key.</param>
        /// <returns>The decrypted packet in bytes.</returns>
        internal static byte[] Decrypt(byte[] Packet, byte[] Nonce, byte[] SharedKey)
        {
            return new SecretBox(SharedKey).Decrypt(Packet, Nonce);
        }
    }
}