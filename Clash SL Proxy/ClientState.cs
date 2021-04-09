using Sodium;

namespace CSP
{
    public class ClientState : State
    {
        public KeyPair clientKey;
        public byte[] serverKey, nonce;
        public ServerState serverState;
    }
}