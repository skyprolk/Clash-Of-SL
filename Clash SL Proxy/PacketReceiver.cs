using System;
using System.Linq;
using System.Net.Sockets;

namespace CSP
{
    internal class PacketReceiver
    {
        public static void receive(int bytesReceived, Socket socket, State state)
        {
            int bytesRead = 0;
            int payloadLength, bytesAvailable, bytesNeeded;

            while (bytesRead < bytesReceived)
            {
                bytesAvailable = bytesReceived - bytesRead;
                if (bytesReceived > 0)
                {
                    if (state.packet.Length >= 7)
                    {
                        payloadLength = BitConverter.ToInt32(new byte[1].Concat(state.packet.Skip(2).Take(3)).Reverse().ToArray(), 0);
                        bytesNeeded   = payloadLength - (state.packet.Length - 7);

                        if (bytesAvailable >= bytesNeeded)
                        {
                            state.packet    = state.packet.Concat(state.buffer.Skip(bytesRead).Take(bytesNeeded)).ToArray();
                            bytesRead      += bytesNeeded;
                            bytesAvailable -= bytesNeeded;

                            if (state.GetType() == typeof(ClientState))
                            {
                                ClientCrypto.DecryptPacket(socket, (ClientState)state, state.packet);
                            }
                            else if (state.GetType() == typeof(ServerState))
                            {
                                ServerCrypto.DecryptPacket(socket, (ServerState)state, state.packet);
                            }

                            state.packet = new byte[0];
                        }
                        else
                        {
                            state.packet   = state.packet.Concat(state.buffer.Skip(bytesRead).Take(bytesAvailable)).ToArray();
                            bytesRead      = bytesReceived;
                            bytesAvailable = 0;
                        }
                    }
                    else if (bytesAvailable >= 7)
                    {
                        state.packet    = state.packet.Concat(state.buffer.Skip(bytesRead).Take(7)).ToArray();
                        bytesRead      += 7;
                        bytesAvailable -= 7;
                    }
                    else
                    {
                        state.packet   = state.packet.Concat(state.buffer.Skip(bytesRead).Take(bytesAvailable)).ToArray();
                        bytesRead      = bytesReceived;
                        bytesAvailable = 0;
                    }
                }
            }
        }
    }
}