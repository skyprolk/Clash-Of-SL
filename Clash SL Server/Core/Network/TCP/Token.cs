using System;
using System.Collections.Generic;
using System.Net.Sockets;
using CSS.Core.Settings;
using CSS.Packets;

namespace CSS.Core.Network
{
    internal class Token
    {
        internal Device Device;
        internal SocketAsyncEventArgs Args;
        internal List<byte> Packet;

        internal byte[] Buffer;

        internal int Offset;

        internal bool Aborting;

        internal Token(SocketAsyncEventArgs Args, Device Device)
        {
            this.Device = Device;
            this.Device.Token = this;

            this.Args = Args;
            this.Args.UserToken = this;

            this.Buffer = new byte[Constants.ReceiveBuffer];
            this.Packet = new List<byte>(Constants.ReceiveBuffer);
        }

        internal void SetData()
        {
            byte[] Data = new byte[this.Args.BytesTransferred];
            Array.Copy(this.Args.Buffer, 0, Data, 0, this.Args.BytesTransferred);
            this.Packet.AddRange(Data);
        }

        internal void Process()
        {
            byte[] Data = this.Packet.ToArray();
            this.Device.Process(Data);
        }

        internal void Reset()
        {
            this.Offset = 0;
            this.Packet.Clear();
        }
    }
}