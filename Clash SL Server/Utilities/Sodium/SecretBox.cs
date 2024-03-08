namespace CSS.Utilities.Sodium
{
    using System;
    using CSS.Utilities.TweetNaCl;

    public class SecretBox
    {
        private const int SHAREDKEYLENGTH = 32;
        private readonly byte[] KnownSharedKey;

        public SecretBox(byte[] _S)
        {
            this.KnownSharedKey = _S;
        }

        public byte[] Decrypt(byte[] _Cipher, byte[] _Nonce)
        {
            int cipherLength = _Cipher.Length;
            byte[] buffer = new byte[cipherLength];

            if (xsalsa20poly1305.crypto_secretbox_open(buffer, _Cipher, cipherLength, _Nonce, this.KnownSharedKey) != 0)
            {
                return null;
            }

            byte[] final = new byte[buffer.Length - SecretBox.SHAREDKEYLENGTH];
            Array.Copy(buffer, SecretBox.SHAREDKEYLENGTH, final, 0, buffer.Length - SecretBox.SHAREDKEYLENGTH);
            return final;
        }

        public byte[] Encrypt(byte[] _Plain, byte[] _Nonce)
        {
            int plainLength = _Plain.Length;
            byte[] paddedMessage = new byte[plainLength + SecretBox.SHAREDKEYLENGTH];
            Array.Copy(_Plain, 0, paddedMessage, SecretBox.SHAREDKEYLENGTH, plainLength);

            byte[] buffer = new byte[paddedMessage.Length];

            if (xsalsa20poly1305.crypto_secretbox(buffer, paddedMessage, paddedMessage.Length, _Nonce, this.KnownSharedKey) != 0)
            {
                return null;
            }

            return buffer;
        }
    }
}