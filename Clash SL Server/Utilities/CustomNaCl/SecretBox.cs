using System;
using UCS.Utilities.TweetNaCl;

namespace UCS.Utilities.CustomNaCl
{
    public class SecretBox
    {
        const int SHAREDKEYLENGTH = 32;
        byte[] KnownSharedKey = new byte[SHAREDKEYLENGTH];

        public SecretBox(byte[] s)
        {
            this.KnownSharedKey = s;
        }

        public byte[] create(byte[] plain, byte[] nonce)
        {
            int plainLength = plain.Length;
            var paddedMessage = new byte[plainLength + SHAREDKEYLENGTH];
            Array.Copy(plain, 0, paddedMessage, SHAREDKEYLENGTH, plainLength);

            var buffer = new byte[paddedMessage.Length];

            if (xsalsa20poly1305.crypto_secretbox(buffer, paddedMessage, paddedMessage.Length, nonce, KnownSharedKey) != 0)
                throw new Exception("SecretBox Encryption failed");

            return buffer;
        }

        public byte[] open(byte[] cipher, byte[] nonce)
        {
            int cipherLength = cipher.Length;
            var buffer = new byte[cipherLength];

            if (xsalsa20poly1305.crypto_secretbox_open(buffer, cipher, cipherLength, nonce, KnownSharedKey) != 0)
                throw new Exception("SecretBox Decryption failed");

            var final = new byte[buffer.Length - SHAREDKEYLENGTH];
            Array.Copy(buffer, SHAREDKEYLENGTH, final, 0, buffer.Length - SHAREDKEYLENGTH);
            return final;
        }
    }
}