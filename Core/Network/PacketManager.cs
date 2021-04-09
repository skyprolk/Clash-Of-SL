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
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using CSS.Logic;
using CSS.PacketProcessing;
using CSS.Core;

namespace CSS.Core.Network
{
    internal class PacketManager : IDisposable
    {
        private static readonly EventWaitHandle m_vIncomingWaitHandle = (EventWaitHandle)new AutoResetEvent(false);
        private static readonly EventWaitHandle m_vOutgoingWaitHandle = (EventWaitHandle)new AutoResetEvent(false);
        private static ConcurrentQueue<Message> m_vIncomingPackets;
        private static ConcurrentQueue<Message> m_vOutgoingPackets;

        public PacketManager()
        {
            PacketManager.m_vIncomingPackets = new ConcurrentQueue<Message>();
            PacketManager.m_vOutgoingPackets = new ConcurrentQueue<Message>();
        }

        public void Dispose()
        {
            m_vIncomingWaitHandle.Dispose();
            m_vOutgoingWaitHandle.Dispose();
            GC.SuppressFinalize((object)this);
        }

        public static void ProcessIncomingPacket(Message p)
        {
            m_vIncomingPackets.Enqueue(p);
            m_vIncomingWaitHandle.Set();
        }

        public static void ProcessOutgoingPacket(Message p)
        {
            try
            {
                p.Encode();
                p.Process(p.Client.GetLevel());
                m_vOutgoingPackets.Enqueue(p);
                m_vOutgoingWaitHandle.Set();
            }
            catch (Exception)
            {
            }
        }

        public void Start()
        {
            IncomingProcessingDelegate incomingProcessing = IncomingProcessing;
            OutgoingProcessingDelegate outgoingProcessing = OutgoingProcessing;
            incomingProcessing.BeginInvoke(null, null);
            outgoingProcessing.BeginInvoke(null, null);
            _Logger.Print("     Packet Manager started successfully",Types.INFO);
        }

        private void IncomingProcessing()
        {
            while (true)
            {
                m_vIncomingWaitHandle.WaitOne();
                Message result;
                while (PacketManager.m_vIncomingPackets.TryDequeue(out result))
                {
                    result.GetData();
                    result.Decrypt();
                    MessageManager.ProcessPacket(result);
                }
            }
        }

        private void OutgoingProcessing()
        {
            while (true)
            {
                m_vOutgoingWaitHandle.WaitOne();
                Message result;
                while (m_vOutgoingPackets.TryDequeue(out result))
                {
                    try
                    {
                        if (result.Client.Socket != null)
                            result.Client.Socket.Send(result.GetRawData());
                        else
                            ResourcesManager.DropClient(result.Client.GetSocketHandle());
                    }
                    catch (Exception ex1)
                    {
                        try
                        {
                            ResourcesManager.DropClient(result.Client.GetSocketHandle());
                            result.Client.Socket.Shutdown(SocketShutdown.Both);
                            result.Client.Socket.Close();
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                }
            }
        }

        delegate void IncomingProcessingDelegate();

        delegate void OutgoingProcessingDelegate();
    }
}
