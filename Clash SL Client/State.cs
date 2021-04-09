using Sodium;

namespace Ultrapowa_Client
{
    internal class State
    {
        public KeyPair _ClientKey;
        public byte[] _ServerKey, nonce;
    }
}
