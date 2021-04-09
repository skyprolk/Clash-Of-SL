/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CSS.PacketProcessing;
using CSS.Core;

namespace CSS.Core.Network
{
    internal class Gateway
    {
        public static Socket Socket { get; set; }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void Start()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            {
                var ip = IPAddress.Any;
                var port = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);
                try
                {
                    Socket.Bind(new IPEndPoint(ip, port));
                    Socket.Listen(1000);
                    _Logger.Print("     Gateway started on " + ip+":"+port, Types.INFO);
                    _Logger.Print("     Server started succesfully! Let's Play Clash of SL!",Types.INFO);
                    while (true)
                    {
                        allDone.Reset();
                        Socket.BeginAccept(new AsyncCallback(OnClientConnect), Socket);
                        allDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    _Logger.Print("     Exception when attempting to host the server : " + e,Types.ERROR);
                    Socket = null;
                }
            }
        }

        static void OnReceive(SocketRead read, byte[] data)
        {
            try
            {
                Client c = ResourcesManager.GetClient(read.Socket.Handle.ToInt64());
                c.DataStream.AddRange(data);
                Message p;
                while (c.TryGetPacket(out p))
                    PacketManager.ProcessIncomingPacket(p);
            }
            catch (Exception ex)
            {
            }
        }

        static void OnReceiveError(SocketRead read, Exception exception)
        {
        }

        void OnClientConnect(IAsyncResult ar)
        {
            allDone.Set();
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                Socket clientSocket = listener.EndAccept(ar);
                _Logger.Print("[CSS]    Player connected -> " + ((IPEndPoint) clientSocket.RemoteEndPoint).Address + "", Types.ERROR);
                ResourcesManager.AddClient(new Client(clientSocket), ((IPEndPoint) clientSocket.RemoteEndPoint).Address.ToString());
                SocketRead.Begin(clientSocket, OnReceive, OnReceiveError);
            }
            catch (Exception e)
            {
            }
        }
    }

    public class SocketRead
    {
        SocketRead(Socket socket, IncomingReadHandler readHandler, IncomingReadErrorHandler errorHandler = null)
        {
            Socket = socket;
            _readHandler = readHandler;
            _errorHandler = errorHandler;
            BeginReceive();
        }

        public Socket Socket { get; }

        public static SocketRead Begin(Socket socket, IncomingReadHandler readHandler, IncomingReadErrorHandler errorHandler = null) 
            => new SocketRead(socket, readHandler, errorHandler);

        readonly byte[] _buffer = new byte[1024];

        readonly IncomingReadErrorHandler _errorHandler;

        readonly IncomingReadHandler _readHandler;

        public delegate void IncomingReadErrorHandler(SocketRead read, Exception exception);

        public delegate void IncomingReadHandler(SocketRead read, byte[] data);

        void BeginReceive()
        {
            Socket.BeginReceive(_buffer, 0, 1024, SocketFlags.None, OnReceive, this);
        }

        void OnReceive(IAsyncResult result)
        {
            try
            {
                if (result.IsCompleted)
                {
                    var bytesRead = Socket.EndReceive(result);
                    if (bytesRead > 0)
                    {
                        var read = new byte[bytesRead];
                        Array.Copy(_buffer, 0, read, 0, bytesRead);

                        _readHandler(this, read);
                        Begin(Socket, _readHandler, _errorHandler);
                    }
                }
            }
            catch (Exception e)
            {
                _errorHandler?.Invoke(this, e);
            }
        }
    }
}
