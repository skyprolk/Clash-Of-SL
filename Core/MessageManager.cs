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
using System.Threading;
using CSS.PacketProcessing;
using CSS.Core;

namespace CSS.Core
{
    internal class MessageManager
    {
        #region Private Fields

        private static BlockingCollection<Message> m_vPackets = new BlockingCollection<Message>(); // new Method
        
        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     The loader of the MessageManager class.
        /// </summary>
        public MessageManager()
        {
            PacketProcessingDelegate packetProcessing = PacketProcessing;
            packetProcessing.BeginInvoke(null, null);
            _Logger.Print("     Message manager has been successfully started !",Types.INFO);
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        ///     This function process packets.
        /// </summary>
        void PacketProcessing()
        {
            while (true)
            {
                var p = m_vPackets.Take();
                ThreadPool.QueueUserWorkItem(state =>
                {
                    var m = (Message)state;
                    var m2 = m.Client.GetLevel();
                    try
                    {
                        m.Decode();
                        m.Process(m2);
                    }
                    catch (Exception e)
                    {
                        _Logger.Print("     " + e,Types.ERROR);
                    }
                }, p);
            }
        }

        #endregion Private Methods

        #region Private Delegates

        private delegate void PacketProcessingDelegate();

        #endregion Private Delegates

        #region Public Methods

        /// <summary>
        ///     This function handle the packet by enqueue him.
        /// </summary>
        /// <param name="p">The message/packet.</param>
        public static void ProcessPacket(Message p)
        {
            m_vPackets.Add(p);
        }
        #endregion Public Methods
    }
}
