using System;
using System.Net;
using System.Net.Sockets;

namespace CSP
{
    public class Client : ClientCrypto
    {
        public ClientState state = new ClientState();

        public Client(ServerState serverstate)
        {
            state.serverState = serverstate;
            state.clientKey   = clientKey;
            state.serverKey   = serverKey;
        }

        public void StartClient()
        {
            try
            {
                IPHostEntry ipHostInfo    = Dns.GetHostEntry(CSP.Proxy.hostname);
                IPAddress ipAddress       = ipHostInfo.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, CSP.Proxy.port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                state.socket  = socket;
                socket.Connect(remoteEndPoint);
                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, ReceiveCallback, state);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[COC]");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("[HOST]");
                Console.ResetColor();
                Console.WriteLine("  The proxy is succefully linked to {0} ...", socket.RemoteEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}