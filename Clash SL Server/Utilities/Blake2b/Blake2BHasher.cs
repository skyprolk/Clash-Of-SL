namespace CSS.Utilities.Blake2B
{
    #region Usings

    using System;

    #endregion Usings

    internal class Blake2BHasher : Blake2BBase
    {
        private readonly Blake2BConfig _Config = new Blake2BConfig();
        private readonly Blake2BCore _Core = new Blake2BCore();
        private readonly byte[] _Key;
        private readonly int _OutputSize;
        private readonly ulong[] _RawConfig;

        /// <summary>
        /// Initialize a new instance of the <see cref="Blake2BHasher"/> class.
        /// </summary>
        public Blake2BHasher()
        {
            this._RawConfig = Blake2Builder.ConfigB(this._Config, null);

            if (this._Config.Key != null && this._Config.Key.Length != 0)
            {
                this._Key = new byte[24];
                Array.Copy(this._Config.Key, this._Key, this._Config.Key.Length);
            }

            this._OutputSize = this._Config.OutputSize;

            this.Init();
        }

        /// <summary>
        /// Finish this instance.
        /// </summary>
        /// <returns>The nonce.</returns>
        public override byte[] Finish()
        {
            byte[] _FResult = this._Core.HashFinal();

            if (this._OutputSize != _FResult.Length)
            {
                byte[] _Result = new byte[this._OutputSize];
                Array.Copy(_FResult, _Result, _Result.Length);
                return _Result;
            }

            return _FResult;
        }

        /// <summary>
        /// Initialize the Blake2B Hasher.
        /// </summary>
        public override sealed void Init()
        {
            this._Core.Initialize(this._RawConfig);

            if (this._Key != null)
            {
                this._Core.HashCore(this._Key, 0, this._Key.Length);
            }
        }

        /// <summary>
        /// Update the Blake2B Hasher with the specified data.
        /// </summary>
        /// <param name="_Data">The data.</param>
        /// <param name="_Index">The index.</param>
        /// <param name="_Count">The count.</param>
        public override void Update(byte[] _Data, int _Index, int _Count)
        {
            this._Core.HashCore(_Data, _Index, _Count);
        }
    }
}