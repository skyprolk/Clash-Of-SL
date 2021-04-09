namespace CSS.Utilities.Blake2B
{
    #region Usings

    using System.Security.Cryptography;

    #endregion Usings

    public abstract class Blake2BBase
    {
        public HashAlgorithm AsHashAlgorithm()
        {
            return new HashAlgorithmAdapter(this);
        }

        public abstract byte[] Finish();

        /// <summary>
        /// Initialize the Blake2B Hasher.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Update the Blake2B Hasher configuration with the specified data.
        /// </summary>
        /// <param name="_Data">The data.</param>
        /// <param name="start">The start.</param>
        /// <param name="count">The count.</param>
        public abstract void Update(byte[] _Data, int start, int count);

        /// <summary>
        /// Update the Blake2B Hasher using the specified data.
        /// </summary>
        /// <param name="_Data">The data.</param>
        public void Update(byte[] _Data)
        {
            this.Update(_Data, 0, _Data.Length);
        }

        internal class HashAlgorithmAdapter : HashAlgorithm
        {
            private readonly Blake2BBase _hasher;

            public HashAlgorithmAdapter(Blake2BBase hasher)
            {
                this._hasher = hasher;
            }

            public override void Initialize()
            {
                this._hasher.Init();
            }

            protected override void HashCore(byte[] array, int ibStart, int cbSize)
            {
                this._hasher.Update(array, ibStart, cbSize);
            }

            protected override byte[] HashFinal()
            {
                return this._hasher.Finish();
            }
        }
    }
}