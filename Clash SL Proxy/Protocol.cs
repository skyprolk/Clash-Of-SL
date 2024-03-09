using System;
using System.Net.Sockets;
using System.Threading;

namespace CSP
{
    public class Protocol
    {
        protected static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                State state       = (State)ar.AsyncState;
                Socket socket     = state.socket;
                int bytesReceived = socket.EndReceive(ar);
                PacketReceiver.receive(bytesReceived, socket, state);
                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[COSS]      Client has been disconnected!");
                Console.ResetColor();
            }
        }

        protected static void SendCallback(IAsyncResult ar)
        {
            try
            {
                State state   = (State)ar.AsyncState;
                Socket socket = state.socket;
                int bytesSent = socket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
