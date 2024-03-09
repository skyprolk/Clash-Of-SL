using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSP
{
    public class Server : ServerCrypto
    {
        private static readonly ManualResetEvent allDone = new ManualResetEvent(false);
        private readonly int port;

        public Server(int port)
        {
            this.port = port;
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                Socket socket   = listener.EndAccept(ar);

                ServerState state = new ServerState
                {
                    socket    = socket,
                    serverKey = serverKey
                };

                Console.WriteLine("[PROXY]    Connection from {0} ...", socket.RemoteEndPoint);

                Client client            = new Client(state);
                client.StartClient();
                client.state.serverState = state;
                state.clientState        = client.state;

                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void StartServer()
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

                using (Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    Console.WriteLine("[PROXY]    Started Listener on " + port + " in " + CSP.Proxy._Stopwatch.ElapsedMilliseconds + " Milliseconds!");
                    CSP.Proxy._Stopwatch.Stop();

                    while (true)
                    {
                        allDone.Reset();
                        listener.BeginAccept(AcceptCallback, listener);
                        allDone.WaitOne();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[CSP]    {0}", e.Message);
            }

            Console.WriteLine("\n[CSP]      Press ENTER to continue...");
            Console.Read();
        }
    }
}