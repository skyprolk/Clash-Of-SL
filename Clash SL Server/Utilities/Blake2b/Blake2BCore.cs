namespace CSS.Utilities.Blake2B
{
    #region Usings

    using System;

    #endregion

    public sealed partial class Blake2BCore
    {
        private const int BlockSizeInBytes = 128;

        private const ulong IV0 = 0x6A09E667F3BCC908UL;
        private const ulong IV1 = 0xBB67AE8584CAA73BUL;
        private const ulong IV2 = 0x3C6EF372FE94F82BUL;
        private const ulong IV3 = 0xA54FF53A5F1D36F1UL;
        private const ulong IV4 = 0x510E527FADE682D1UL;
        private const ulong IV5 = 0x9B05688C2B3E6C1FUL;
        private const ulong IV6 = 0x1F83D9ABFB41BD6BUL;
        private const ulong IV7 = 0x5BE0CD19137E2179UL;

        private readonly byte[] _buf = new byte[128];

        private int _bufferFilled;
        private ulong _counter0;
        private ulong _counter1;
        private ulong _finalizationFlag0;
        private ulong _finalizationFlag1;
        private readonly ulong[] _h = new ulong[8];
        private bool _isInitialized;

        private readonly ulong[] _m = new ulong[16];

        public void HashCore(byte[] array, int start, int count)
        {
            int offset = start;
            int bufferRemaining = Blake2BCore.BlockSizeInBytes - this._bufferFilled;

            if (this._bufferFilled > 0 && count > bufferRemaining)
            {
                Array.Copy(array, offset, this._buf, this._bufferFilled, bufferRemaining);
                this._counter0 += Blake2BCore.BlockSizeInBytes;
                if (this._counter0 == 0)
                {
                    this._counter1++;
                }

                this.Compress(this._buf, 0);
                offset += bufferRemaining;
                count -= bufferRemaining;
                this._bufferFilled = 0;
            }

            while (count > Blake2BCore.BlockSizeInBytes)
            {
                this._counter0 += Blake2BCore.BlockSizeInBytes;
                if (this._counter0 == 0)
                {
                    this._counter1++;
                }

                this.Compress(array, offset);
                offset += Blake2BCore.BlockSizeInBytes;
                count -= Blake2BCore.BlockSizeInBytes;
            }

            if (count > 0)
            {
                Array.Copy(array, offset, this._buf, this._bufferFilled, count);
                this._bufferFilled += count;
            }
        }

        public byte[] HashFinal()
        {
            return this.HashFinal(false);
        }

        public byte[] HashFinal(bool isEndOfLayer)
        {
            if (!this._isInitialized)
            {
                throw new InvalidOperationException("Not initialized");
            }

            this._isInitialized = false;

            this._counter0 += (uint) this._bufferFilled;
            this._finalizationFlag0 = ulong.MaxValue;
            if (isEndOfLayer)
            {
                this._finalizationFlag1 = ulong.MaxValue;
            }

            for (int i = this._bufferFilled; i < this._buf.Length; i++)
            {
                this._buf[i] = 0;
            }

            this.Compress(this._buf, 0);

            // Output
            byte[] hash = new byte[64];
            for (int i = 0; i < 8; ++i)
            {
                Blake2BCore.UInt64ToBytes(this._h[i], hash, i << 3);
            }

            return hash;
        }

        public void Initialize(ulong[] config)
        {
            this._isInitialized = true;

            this._h[0] = Blake2BCore.IV0;
            this._h[1] = Blake2BCore.IV1;
            this._h[2] = Blake2BCore.IV2;
            this._h[3] = Blake2BCore.IV3;
            this._h[4] = Blake2BCore.IV4;
            this._h[5] = Blake2BCore.IV5;
            this._h[6] = Blake2BCore.IV6;
            this._h[7] = Blake2BCore.IV7;

            this._counter0 = 0;
            this._counter1 = 0;
            this._finalizationFlag0 = 0;
            this._finalizationFlag1 = 0;

            this._bufferFilled = 0;

            Array.Clear(this._buf, 0, this._buf.Length);

            for (int i = 0; i < 8; i++)
            {
                this._h[i] ^= config[i];
            }
        }

        internal static ulong BytesToUInt64(byte[] buf, int offset)
        {
            return ((ulong) buf[offset + 7] << (7 * 8)) | ((ulong) buf[offset + 6] << (6 * 8)) | ((ulong) buf[offset + 5] << (5 * 8)) | ((ulong) buf[offset + 4] << (4 * 8)) | ((ulong) buf[offset + 3] << (3 * 8)) | ((ulong) buf[offset + 2] << (2 * 8)) | ((ulong) buf[offset + 1] << (1 * 8)) | buf[offset];
        }

        private static void UInt64ToBytes(ulong value, byte[] buf, int offset)
        {
            buf[offset + 7] = (byte) (value >> (7 * 8));
            buf[offset + 6] = (byte) (value >> (6 * 8));
            buf[offset + 5] = (byte) (value >> (5 * 8));
            buf[offset + 4] = (byte) (value >> (4 * 8));
            buf[offset + 3] = (byte) (value >> (3 * 8));
            buf[offset + 2] = (byte) (value >> (2 * 8));
            buf[offset + 1] = (byte) (value >> (1 * 8));
            buf[offset] = (byte) value;
        }

        partial void Compress(byte[] block, int start);
    }
}