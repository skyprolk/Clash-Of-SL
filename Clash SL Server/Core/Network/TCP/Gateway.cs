/*namespace CSS.Core.Network.TCP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CSS.Core.Settings;
    using CSS.Packets;
    using System.Net;
    using System.Net.Sockets;
    using CSS.Helpers;

    internal class Gateway : IDisposable
    {
        private bool _disposed;
        private bool _started;

        private volatile bool _stopAccept;
        private readonly List<Tuple<Socket, EndPoint>> _listeners;

        private readonly Pool<SocketAsyncEventArgs> _argsPool;

        internal SocketAsyncEventArgsPool ReadPool;
        internal SocketAsyncEventArgsPool WritePool;
        internal Gateway()
        {
            this.ReadPool = new SocketAsyncEventArgsPool();
            this.WritePool = new SocketAsyncEventArgsPool();

            this._listeners = new List<Tuple<Socket, EndPoint>>();
            this._argsPool = new Pool<SocketAsyncEventArgs>(() =>
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += OnAcceptCompleted;
                return args;
            });
            this.Initialize();

            this.Start(new IPEndPoint(IPAddress.Any, Utils.ParseConfigInt("ServerPort")));
        }
        public void Start(params EndPoint[] endPoints)
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Can't access disposed Listener object.");
            if (_started)
                throw new InvalidOperationException("Listener instance has already been started.");

            if (endPoints == null || endPoints.Length == 0)
                throw new ArgumentNullException(nameof(endPoints));

            foreach (EndPoint t in endPoints)
            {
                var startEx = (Exception)null;
                var endPoint = t;
                var listener = StartListener(endPoint, ref startEx);
                if (listener == null)
                    throw new InvalidOperationException("Failed to start listener; check inner exception", startEx);

                _listeners.Add(Tuple.Create(listener, endPoint));
            }

            _started = true;
        }

        internal void Initialize()
        {
            for (int Index = 0; Index < Constants.MaxOnlinePlayers + 1; Index++)
            {
                SocketAsyncEventArgs ReadEvent = new SocketAsyncEventArgs();
                ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                ReadEvent.Completed += this.OnReceiveCompleted;
                this.ReadPool.Enqueue(ReadEvent);

                SocketAsyncEventArgs WriterEvent = new SocketAsyncEventArgs();
                WriterEvent.Completed += this.OnSendCompleted;
                this.WritePool.Enqueue(WriterEvent);
            }
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _stopAccept = true;

                // Dispose our list of listeners.
                foreach (var t in _listeners)
                {
                    var listener = t.Item1;

                    try
                    {
                        listener.Close();
                    }
                    catch
                    {
                        
                    }
                    try
                    {
                        listener.Dispose();
                    }
                    catch
                    {
                       
                    }
                }

                // Dispose our pool of SocketAsyncEventArgs objects.
                for (int i = 0; i < _argsPool.Count; i++)
                {
                    var e = _argsPool.Get();

                    try
                    {
                        e.Dispose();
                    }
                    catch
                    {
                        
                    }
                }
                lock (this.ReadPool.Gate)
                {
                    lock (this.WritePool.Gate)
                    {
                        this.ReadPool.Dispose();
                        this.WritePool.Dispose();
                    }
                }
            }

            _disposed = true;
        }

        internal void OnAcceptCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessAccept(AsyncEvent);
        }

        internal void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessReceive(AsyncEvent);
        }

        private Socket StartListener(EndPoint endPoint, ref Exception ex)
        {
            try
            {

                var listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveBufferSize = Constants.ReceiveBuffer,
                    SendBufferSize = Constants.SendBuffer,
                    Blocking = false,
                    NoDelay = true,
                };
                listener.Bind(endPoint);
                listener.Listen(200);

                var args = _argsPool.Get();
                args.UserToken = listener;
                StartAccept(args);

                Logger.Say();
                Program._Stopwatch.Stop();
                Logger.Say($"CSS has been started on {endPoint} in {Program._Stopwatch.ElapsedMilliseconds / 1000} Seconds !");
                return listener;
            }
            catch (Exception iex)
            {

                Logger.Say("Server failed to stat listening socket on " + iex);
                ex = iex;
                return null;
            }
        }
        private void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                var listener = (Socket)e.UserToken;

                try
                {
                    if (!listener.AcceptAsync(e))
                        ProcessAccept(e);
                }
                catch //(Exception ex)
                {
                    if (_disposed)
                        return;

                    lock (_listeners)
                    {
                        for (int i = 0; i < _listeners.Count; i++)
                        {
                            var tmpListener = _listeners[i].Item1;
                            if (tmpListener != listener)
                                break;

                            var tmpEndPoint = _listeners[i].Item2;

                            var startEx = (Exception)null;
                            var newListener = StartListener(tmpEndPoint, ref startEx);
                            _listeners[i] = Tuple.Create(newListener, tmpEndPoint);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred > 0 && AsyncEvent.SocketError == SocketError.Success)
            {
                Token token = AsyncEvent.UserToken as Token;

                if (token != null)
                {
                    token.SetData();

                    try
                    {
                        if (token.Device.Socket.Available == 0)
                        {
                            token.Process();

                            if (!token.Aborting)
                            {
                                if (!token.Device.Socket.ReceiveAsync(AsyncEvent))
                                {
                                    this.ProcessReceive(AsyncEvent);
                                }
                            }
                        }
                        else
                        {
                            if (!token.Device.Socket.ReceiveAsync(AsyncEvent))
                            {
                                this.ProcessReceive(AsyncEvent);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.Disconnect(AsyncEvent);
                    }
                }
            }
            else
            {
                this.Disconnect(AsyncEvent);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs AsyncEvent)
        {
            var Socket = AsyncEvent.AcceptSocket;
            AsyncEvent.AcceptSocket = null;
            try
            {
                if (Socket.Connected && AsyncEvent.SocketError == SocketError.Success)
                {
                    Logger.Write($"New client connected -> {((IPEndPoint)Socket.RemoteEndPoint).Address}");
                    if (Constants.Local)
                    {
                        if (!Constants.AuthorizedIP.Contains(Socket.RemoteEndPoint.ToString().Split(':')[0]))
                        {
                            Socket.Close();

                            AsyncEvent.AcceptSocket = null;
                            this.StartAccept(AsyncEvent);
                            return;
                        }
                    }

                    SocketAsyncEventArgs ReadEvent = this.ReadPool.Dequeue();

                    if (ReadEvent == null)
                    {
                        ReadEvent = new SocketAsyncEventArgs();
                        ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                        ReadEvent.Completed += this.OnReceiveCompleted;
                    }

                    // Accept only if the listener haven't been stopped.
                    if (!_stopAccept)
                    {
                        var device = new Device(Socket);
                        var token = new Token(ReadEvent, device);


                        try
                        {
                            if (!Socket.ReceiveAsync(ReadEvent))
                            {
                                this.ProcessReceive(ReadEvent);
                            }
                        }
                        catch (Exception)
                        {
                            this.Disconnect(AsyncEvent);
                        }
                    }
                    else
                    {
                        this.Disconnect(AsyncEvent);
                    }
                }
                else
                {
                    Logger.Write("Client seems to be disconnected at ProcessAccept. Probably an attack to the serevr");
                    this.Disconnect(AsyncEvent);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(" processing of accept failed: {0}", ex);
#endif
                this.Disconnect(AsyncEvent);
            }
            this.StartAccept(AsyncEvent);
        }

        internal void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            var token = AsyncEvent.UserToken as Token;

            if (token != null)
            {
                token.Aborting = true;

                if (token.Device.Player != null)
                {
                    Resources.DatabaseManager.Save(token.Device.Player);
                    ResourcesManager.DropClient(token.Device);
                }
                else if (token.Device.Connected)
                {
                    try
                    {
                        token.Device.Socket.Shutdown(SocketShutdown.Send);
                    }
                    catch (Exception)
                    {
                        // Already Closed.
                    }

                    token.Device.Socket.Close();
                    token.Device.Socket.Dispose();
                }
            }

            this.ReadPool.Enqueue(AsyncEvent);
        }

        internal void Send(Message Message)
        {
            SocketAsyncEventArgs WriteEvent = this.WritePool.Dequeue();

            if (this.WritePool.Dequeue() == null)
            {
                WriteEvent = new SocketAsyncEventArgs();
            }

            WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

            WriteEvent.AcceptSocket = Message.Device.Socket;
            WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;

            if (!Message.Device.Socket.SendAsync(WriteEvent))
            {
                this.ProcessSend(Message, WriteEvent);
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        internal void Send(Device Device, byte[] Buffer)
        {
            SocketAsyncEventArgs writeEvent = this.WritePool.Dequeue();

            if (this.WritePool.Dequeue() == null)
            {
                writeEvent = new SocketAsyncEventArgs();
            }

            writeEvent.SetBuffer(Buffer, 0, Buffer.Length);

            writeEvent.AcceptSocket = Device.Socket;
            writeEvent.RemoteEndPoint = Device.Socket.RemoteEndPoint;

            if (!Device.Socket.SendAsync(writeEvent))
            {
                // Rip
            }
        }

        internal void ProcessSend(Message Message, SocketAsyncEventArgs Args)
        {
            while (true)
            {
                Message.Offset += Args.BytesTransferred;

                if (Message.Length + 7 > Message.Offset)
                {
                    if (Message.Device.Connected)
                    {
                        Args.SetBuffer(Message.Offset, Message.Length + 7 - Message.Offset);

                        if (!Message.Device.Socket.SendAsync(Args))
                        {
                            continue;
                        }
                    }
                }
                break;
            }
        }

        internal void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.WritePool.Enqueue(AsyncEvent);
        }
    }
}*/

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient.Properties;
using CSS.Core.Settings;
using CSS.Packets;
using System.Configuration;
using CSS.Helpers;
using CSS.Core.Checker;

namespace CSS.Core.Network.TCP
{
    internal class Gateway
    {

        internal SocketAsyncEventArgsPool ReadPool;
        internal SocketAsyncEventArgsPool WritePool;
        internal Socket Listener;
        internal Mutex Mutex;

        internal int ConnectedSockets;

        internal Gateway()
        {
            this.ReadPool = new SocketAsyncEventArgsPool();
            this.WritePool = new SocketAsyncEventArgsPool();

            this.Initialize();

            this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveBufferSize = Constants.ReceiveBuffer,
                SendBufferSize = Constants.SendBuffer,
                Blocking = false,
                NoDelay = true
            };
            this.Listener.Bind(new IPEndPoint(IPAddress.Any, Utils.ParseConfigInt("ServerPort")));
            this.Listener.Listen(200);

            Logger.Say();
            Logger.Say($"CSS has been started on {this.Listener.LocalEndPoint} in {Program._Stopwatch.ElapsedMilliseconds} Milliseconds !");
            Program._Stopwatch.Stop();

            SocketAsyncEventArgs AcceptEvent = new SocketAsyncEventArgs();
            AcceptEvent.Completed += this.OnAcceptCompleted;

            this.StartAccept(AcceptEvent);
        }

        internal void Initialize()
        {
            for (int Index = 0; Index < Constants.MaxOnlinePlayers + 1; Index++)
            {
                SocketAsyncEventArgs ReadEvent = new SocketAsyncEventArgs();
                ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                ReadEvent.Completed += this.OnReceiveCompleted;
                this.ReadPool.Enqueue(ReadEvent);

                SocketAsyncEventArgs WriterEvent = new SocketAsyncEventArgs();
                WriterEvent.Completed += this.OnSendCompleted;
                this.WritePool.Enqueue(WriterEvent);
            }
        }

        internal void StartAccept(SocketAsyncEventArgs AcceptEvent)
        {
            AcceptEvent.AcceptSocket = null;

            if (!this.Listener.AcceptAsync(AcceptEvent))
            {
                this.ProcessAccept(AcceptEvent);
            }
        }

        internal void ProcessAccept(SocketAsyncEventArgs AsyncEvent)
        {
            Socket Socket = AsyncEvent.AcceptSocket;

            if (Socket.Connected && AsyncEvent.SocketError == SocketError.Success)
            {
                if (!ConnectionBlocker.Banned_IPs.Contains(((IPEndPoint)Socket.RemoteEndPoint).Address.ToString()))
                {
                    Logger.Write($"New client connected -> {((IPEndPoint)Socket.RemoteEndPoint).Address}");

                    SocketAsyncEventArgs ReadEvent = this.ReadPool.Dequeue();

                    if (ReadEvent != null)
                    {
                        Device device = new Device(Socket)
                        {
                            IPAddress = ((IPEndPoint)Socket.RemoteEndPoint).Address.ToString()
                        };

                        Token Token = new Token(ReadEvent, device);
                        Interlocked.Increment(ref this.ConnectedSockets);
                        ResourcesManager.AddClient(device);

                        Task.Run(() =>
                        {
                            try
                            {
                                if (!Socket.ReceiveAsync(ReadEvent))
                                {
                                    this.ProcessReceive(ReadEvent);
                                }
                            }
                            catch (Exception)
                            {
                                this.Disconnect(ReadEvent);
                            }
                        });
                    }
                }
            }
            else
            {
                Logger.Write("Not connected or error at ProcessAccept.");
                Socket.Close(5);
            }

            this.StartAccept(AsyncEvent);
        }

        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred > 0 && AsyncEvent.SocketError == SocketError.Success)
            {
                Token Token = AsyncEvent.UserToken as Token;

                Token.SetData();

                try
                {
                    if (Token.Device.Socket.Available == 0)
                    {
                        Token.Process();

                        if (!Token.Aborting)
                        {
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                            {
                                this.ProcessReceive(AsyncEvent);
                            }
                        }
                    }
                    else
                    {
                        if (!Token.Aborting)
                        {
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                            {
                                this.ProcessReceive(AsyncEvent);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    this.Disconnect(AsyncEvent);
                }
            }
            else
            {
                this.Disconnect(AsyncEvent);
            }
        }

        internal void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessReceive(AsyncEvent);
        }

        internal void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            Token Token = AsyncEvent.UserToken as Token;
            ResourcesManager.DropClient(Token.Device);
            this.ReadPool.Enqueue(AsyncEvent);
        }

        internal void OnAcceptCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessAccept(AsyncEvent);
        }

        internal void Send(Message Message)
        {
            SocketAsyncEventArgs WriteEvent = this.WritePool.Dequeue();

            if (WriteEvent != null)
            {
                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;

                if (!Message.Device.Socket.SendAsync(WriteEvent))
                {
                    this.ProcessSend(Message, WriteEvent);
                }
            }
            else
            {
                WriteEvent = new SocketAsyncEventArgs();

                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;

                if (!Message.Device.Socket.SendAsync(WriteEvent))
                {
                    this.ProcessSend(Message, WriteEvent);
                }
            }
        }

        internal void ProcessSend(Message Message, SocketAsyncEventArgs Args)
        {
            Message.Offset += Args.BytesTransferred;

            if (Message.Length + 7 > Message.Offset)
            {
                if (Message.Device.Connected)
                {
                    Args.SetBuffer(Message.Offset, Message.Length + 7 - Message.Offset);

                    if (!Message.Device.Socket.SendAsync(Args))
                    {
                        this.ProcessSend(Message, Args);
                    }
                }
            }
        }

        internal void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.WritePool.Enqueue(AsyncEvent);
        }
    }
}
