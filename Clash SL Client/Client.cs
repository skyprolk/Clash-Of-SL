using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSC
{
    class Client
    {
        public static Socket _Socket = null;

        public Client()
        {
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string _HostName, int _Port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(_HostName);
            IPAddress _IpAddress = ipHostInfo.AddressList[0];
            IPEndPoint _IPEndPoint = new IPEndPoint(_IpAddress, _Port);
            _Socket.Bind(_IPEndPoint);

            //Crypto...
        }
    }
}
