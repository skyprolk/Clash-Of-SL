using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using UCS.Logic;
using UCS.Packets;
using UCS.Packets.Messages.Server;

namespace UCS.Core.Network
{
    class PacketProcessor
    {
        /// <summary>
        /// Process the Message 
        /// </summary>
        /// <param name="_Message"></param>
        public static void Receive(Message _Message)
        {
            _Message.Decrypt();
            _Message.Decode();
            _Message.Process(_Message.Device.Player);
        }               

        /// <summary>
        /// Process and Send the Message
        /// </summary>
        /// <param name="_Message"></param>
        public static void Send(Message _Message)
        {
            try
            {
                _Message.Encode();
                if (_Message.GetMessageType() == 20000)
                {
                    byte[] sessionKey = ((RC4SessionKey)_Message).Key;
                    _Message.Device.UpdateKey(sessionKey);
                }
                _Message.Process(_Message.Device.Player);
                _Message.Device.Socket.BeginSend(_Message.GetRawData(), 0, _Message.GetRawData().Length, SocketFlags.None, SendCallback, (object)null);
            }
            catch (Exception)
            {
                Gateway.Disconnect(_Message.Device.Socket);
                ResourcesManager.DropClient(_Message.Device.SocketHandle);
            }
        }

        private static void SendCallback(IAsyncResult _Ar)
        {
            Message _Message = _Ar.AsyncState as Message;
        }
    }
}
