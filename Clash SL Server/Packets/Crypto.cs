using System;

namespace CSS.Packets
{
    internal class Crypto : IDisposable
    {
        internal byte[] SNonce;
        internal byte[] RNonce;
        internal byte[] PublicKey;

        internal Crypto()
        {
            this.PublicKey = new byte[32];
            this.SNonce = new byte[24];
            this.RNonce = new byte[24];
        }

        void IDisposable.Dispose()
        {
            this.SNonce = null;
            this.RNonce = null;
            this.PublicKey = null;
        }
    }
}
