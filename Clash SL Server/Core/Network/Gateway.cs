using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UCS.Core.Checker;
using UCS.Core.Settings;
using UCS.Core.Threading;
using UCS.Packets;
using static UCS.Core.Logger;
using static UCS.Core.Settings.UCSControl;

namespace UCS.Core.Network
{
	internal class Gateway
    {
        public static ManualResetEvent AllDone = new ManualResetEvent(false);

        public Gateway()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress.ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]));
                Socket _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _Socket.Bind(localEndPoint);
                _Socket.Listen(200);

                Say();
                Say("UCS has been started at " + ipAddress + ":" + localEndPoint.Port + " in " + Program._Stopwatch.ElapsedMilliseconds + " Milliseconds.");
                Program._Stopwatch.Stop();

                while (true)
                {
                    AllDone.Reset();

                    _Socket.BeginAccept(this.AcceptCallback, _Socket);

                    AllDone.WaitOne();
                }
            }
            catch (Exception)
            {
                Error("Gateway failed to start. Restarting...");
                Thread.Sleep(5000);
                UCSControl.UCSRestart();
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {        
            try
            {
				AllDone.Set();

				Socket _Listener = (Socket)ar.AsyncState;
                Socket _Handler = _Listener.EndAccept(ar);

                Logger.Write("New TCP Client connected -> " + ((IPEndPoint)_Handler.RemoteEndPoint).Address);

                if (!ConnectionBlocker.IsAddressBanned(((IPEndPoint)_Handler.RemoteEndPoint).Address.ToString()))
                {
                    ResourcesManager.AddClient(_Handler);
                    new Reader(_Handler, this.ProcessPacket);
                }
                else
                {
                    Disconnect(_Handler);
                    ResourcesManager.DropClient(_Handler.Handle);
                }
            }
            catch (Exception)
            {
            }
        }

		private void ProcessPacket(Reader _Reader, byte[] _Data)
		{
			try
			{
				Client _Client = ResourcesManager.GetClient(_Reader.Socket.Handle);
				_Client.DataStream.AddRange(_Data);
				Message _Message;
                while (_Client.TryGetPacket(out _Message))
                {
                    PacketProcessor.Receive(_Message);
                }
			}
			catch
            {
                Disconnect(_Reader.Socket);
                ResourcesManager.DropClient(_Reader.Socket.Handle);
            }
		}

		public static void Disconnect(Socket _Socket)
		{
			try
			{
				_Socket.Shutdown(SocketShutdown.Both);
				_Socket.Close();
			}
			catch (Exception)
			{
			}
		}
	}	
}