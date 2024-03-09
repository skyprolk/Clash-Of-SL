using System.Net.Sockets;

namespace CSP
{
    public class State
    {
        public const int BufferSize = 2048;
        public byte[] buffer = new byte[BufferSize];
        public byte[] packet = new byte[0];
        public Socket socket = null;
    }
}