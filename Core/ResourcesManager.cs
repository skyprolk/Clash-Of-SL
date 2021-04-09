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
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.PacketProcessing;
using Timer = System.Threading.Timer;

namespace CSS.Core
{
    internal class ResourcesManager : IDisposable
    {
        #region Public Constructors

        /// <summary>
        ///     This is the loader of the ResourcesManager class, which is neccessary for all css functionality.
        /// </summary>
        public ResourcesManager()
        {
            m_vDatabase = new DatabaseManager();
            m_vClients = new ConcurrentDictionary<long, Client>();
            m_vOnlinePlayers = new List<Level>();
            m_vInMemoryLevels = new ConcurrentDictionary<long, Level>();
            m_vTimerCanceled = false;
            TimerCallback TimerDelegate = ReleaseOrphans;
            var TimerItem = new Timer(TimerDelegate, null, 40000, 30000);
            TimerReference = TimerItem;
        }

        #endregion Public Constructors

        #region Private Fields

        static ConcurrentDictionary<long, Client> m_vClients;
        static DatabaseManager m_vDatabase;
        static ConcurrentDictionary<long, Level> m_vInMemoryLevels;
        static List<Level> m_vOnlinePlayers;
        readonly bool m_vTimerCanceled;
        readonly Timer TimerReference;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        ///     This function add a client in the m_vClients list.
        /// </summary>
        /// <param name="c">The client data.</param>
        /// <param name="IP">The IP of the client.</param>
        public static void AddClient(Client c, string IP)
        {
            var socketHandle = c.Socket.Handle.ToInt64();
            c.CIPAddress = IP;
            if (!m_vClients.ContainsKey(socketHandle))
                m_vClients.TryAdd(socketHandle, c);
        }

        /// <summary>
        ///     This function drop a client from m_vClients list.
        /// </summary>
        /// <param name="socketHandle">The (Int64) SocketHandle ID of the client.</param>
        public static void DropClient(long socketHandle)
        {
            try
            {
                Client c;
                m_vClients.TryRemove(socketHandle, out c);

                if (c.GetLevel() != null)
                    LogPlayerOut(c.GetLevel());
            }
            catch (Exception e)
            {
                _Logger.Print("     Error dropping client: ",Types.ERROR);
            }
        }

        /// <summary>
        ///     This function return all players ids in the database.
        /// </summary>
        /// <returns>List of players.</returns>
        public static List<long> GetAllPlayerIds() => m_vDatabase.GetAllPlayerIds();

        /// <summary>
        ///     This function return the client data.
        /// </summary>
        /// <param name="socketHandle">The (Int64) socket handle ID of the client.</param>
        /// <returns>Client Data.</returns>
        public static Client GetClient(long socketHandle) => m_vClients[socketHandle];

        /// <summary>
        ///     This function return all connected clients.
        /// </summary>
        /// <returns>A list of connected clients.</returns>
        public static List<Client> GetConnectedClients()
        {
            var clients = new List<Client>();
            clients.AddRange(m_vClients.Values);
            return clients;
        }

        public static void GetAllPlayersFromDB()
        {
            var players = m_vDatabase.GetAllPlayers();
            foreach (var t in players)
            {
                if (!m_vInMemoryLevels.ContainsKey(t.Key))
                    m_vInMemoryLevels.TryAdd(t.Key, t.Value);
            }
        }

        /// <summary> This function return all in-memory player. </summary>
        /// <returns>
        ///     A List<> of
        ///     in-memory players.
        /// </returns>
        public static List<Level> GetInMemoryLevels()
        {
            var levels = new List<Level>();
            lock (m_vInMemoryLevels)
                levels.AddRange(m_vInMemoryLevels.Values);
            return levels;
        }

        /// <summary> This function return all online players. </summary>
        /// <returns>
        ///     A List<> of
        ///     online players.
        /// </returns>
        public static List<Level> GetOnlinePlayers()
        {
            var onlinePlayers = new List<Level>();
            lock (m_vOnlinePlayers)
                onlinePlayers = m_vOnlinePlayers.ToList();
            return onlinePlayers;
        }

        /// <summary>
        ///     This function get the data of a certain player.
        /// </summary>
        /// <param name="id">The (int64) ID of the Player.</param>
        /// <param name="persistent">Load the player or no.</param>
        /// <returns>The player data.</returns>
        /// <seealso cref="Level" />
        public static Level GetPlayer(long id, bool persistent = false)
        {
            var result = GetInMemoryPlayer(id);
            if (result == null)
            {
                result = m_vDatabase.GetAccount(id);
                if (persistent)
                    LoadLevel(result);
            }
            return result;
        }

        /// <summary>
        ///     This function check if client is connected or no.
        /// </summary>
        /// <param name="socketHandle">The (Int64) Socket Handle ID of the Client.</param>
        /// <returns>A boolean value.</returns>
        public static bool IsClientConnected(long socketHandle) => m_vClients.ContainsKey(socketHandle);

        /// <summary>
        ///     This function check if player is online.
        /// </summary>
        /// <param name="l">The level() of the player.</param>
        /// <returns>A Boolean value.</returns>
        public static bool IsPlayerOnline(Level l) => m_vOnlinePlayers.Contains(l);

        /// <summary>
        ///     This function load a player by adding him in memory.
        /// </summary>
        /// <param name="level">The level of the client.</param>
        public static void LoadLevel(Level level)
        {
            var id = level.GetPlayerAvatar().GetId();
            if (!m_vInMemoryLevels.ContainsKey(id))
                m_vInMemoryLevels.TryAdd(id, level);
        }

        /// <summary>
        ///     This function log a player in, by ading him to online players.
        /// </summary>
        /// <param name="level">The level of the player.</param>
        /// <param name="client">The client of the player.</param>
        public static void LogPlayerIn(Level level, Client client)
        {
            level.SetClient(client);
            client.SetLevel(level);
            level.SetIPAddress(client.CIPAddress);

            lock (m_vOnlinePlayers)
                if (!m_vOnlinePlayers.Contains(level))
                {
                    m_vOnlinePlayers.Add(level);
                    LoadLevel(level);
                }
        }

        /// <summary>
        ///     This function log a player out, by removing him from online players and saving him.
        /// </summary>
        /// <param name="level">The level of the player.</param>
        public static void LogPlayerOut(Level level)
        {
            lock (m_vOnlinePlayers)
                m_vOnlinePlayers.Remove(level);
            DatabaseManager.Singelton.Save(level);
            lock (m_vInMemoryLevels)
                m_vInMemoryLevels.TryRemove(level.GetPlayerAvatar().GetId());
        }

        /// <summary>
        ///     The Dispose() function launched by class.
        /// </summary>
        public void Dispose()
        {
            if (m_vTimerCanceled)
                TimerReference.Dispose();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     This function return the data of an in-memory player.
        /// </summary>
        /// <param name="id">The (Int64) ID of the player.</param>
        /// <returns>Return the level of the player.</returns>
        static Level GetInMemoryPlayer(long id)
        {
            Level result = null;
            lock (m_vInMemoryLevels)
                if (m_vInMemoryLevels.ContainsKey(id))
                    result = m_vInMemoryLevels[id];
            return result;
        }

        /// <summary>
        ///     This function is running at an interval, and check for dead clients.
        /// </summary>
        /// <param name="state">The state.</param>
        void ReleaseOrphans(object state)
        {
            if (m_vTimerCanceled)
                TimerReference.Dispose();
        }

        #endregion Private Methods
    }
}