using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UCS.Core.Network
{
	internal class Reader
	{
		public delegate void IncomingReadHandler(Reader read, byte[] data);
        public const int BufferSize = 2048;
        private readonly byte[] _buffer = new byte[BufferSize];
		private readonly IncomingReadHandler _readHandler;
		public Socket Socket;

		public Reader(Socket _Socket, IncomingReadHandler readHandler)
		{
            this.Socket = _Socket;
            this._readHandler = readHandler;
            this.Socket.BeginReceive(this._buffer, 0, BufferSize, 0, this.OnReceive, this);
        }

		private void OnReceive(IAsyncResult _Ar)
		{
			try
			{
				SocketError tmp;
				int bytesRead = this.Socket.EndReceive(_Ar, out tmp);
				if (tmp == SocketError.Success && bytesRead > 0)
				{
					byte[] read = new byte[bytesRead];
					Array.Copy(this._buffer, 0, read, 0, bytesRead);
					_readHandler(this, read);
					Socket.BeginReceive(this._buffer, 0, BufferSize, 0, OnReceive, this);
				}
			}
			catch (Exception)
			{
                Gateway.Disconnect(Socket);
                ResourcesManager.DropClient(Socket.Handle);
			}
		}
	}
}
