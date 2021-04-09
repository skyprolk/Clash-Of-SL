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
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CSS.Core.Network;
using CSS.Core;
using CSS.Files;
using CSS.Files.CSV;
using CSS.Files.Logic;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;
using Timer = System.Threading.Timer;

namespace CSS.Core

{
    internal class ObjectManager : IDisposable
    {

        /// <summary>
        ///     Loader of the ObjectManager class, which is neccessary for all css functionality.
        /// </summary>
        public ObjectManager()
        {
            m_vTimerCanceled = false;
            m_vDatabase = new DatabaseManager();
            NpcLevels = new Dictionary<int, string>();
            DataTables = new DataTables();
            m_vAlliances = new Dictionary<long, Alliance>();
            LoadFingerPrint();
            using (var sr = new StreamReader(@"Gamefiles/starting_home.json"))
                m_vHomeDefault = sr.ReadToEnd();
            m_vDatabase.CheckConnection();
            m_vAvatarSeed = m_vDatabase.GetMaxPlayerId() + 1;
            m_vAllianceSeed = m_vDatabase.GetMaxAllianceId() + 1;
            LoadGameFiles();
            LoadNpcLevels();
            GetAllAlliancesFromDB();
            var TimerItem = new Timer(Save, null, 30000, 30000);
            new Timer(Restart, null, 10800000, 0);
            TimerReference = TimerItem;
            m_vRandomSeed = new Random();
            _Logger.Print("     Database Sync started successfully",Types.INFO);
            
        }

        public static void Stop()
        {
            foreach (Level op in ResourcesManager.GetOnlinePlayers())
            {
                ResourcesManager.DropClient(op.GetClient().GetSocketHandle());
            }
                
            m_vDatabase = new DatabaseManager();
            NpcLevels = new Dictionary<int, string>();
            DataTables = new DataTables();
            m_vAlliances = new Dictionary<long, Alliance>();
            m_vHomeDefault = "";
            m_vAvatarSeed = 0;
            m_vAllianceSeed = 0;
            _Logger.Print("     Object Manager unloaded successfully",Types.INFO);
        }

        /// <summary>
        ///     This function save someuser (Need to implement).
        /// </summary>
        /// <param name="state"></param>
        void Save(object state)
        {
            m_vDatabase.Save(ResourcesManager.GetInMemoryLevels());
            m_vDatabase.Save(m_vAlliances.Values.ToList());
            if (m_vTimerCanceled)
                TimerReference.Dispose();
        }

        void Restart(object state)
        {
            Restart();
        }

        public static void Restart()
        {
            //DatabaseManager.Singelton.Save(ResourcesManager.GetInMemoryLevels());
            //DatabaseManager.Singelton.Save(ObjectManager.GetInMemoryAlliances());
            //foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            //{
            //    var p = new ShutdownStartedMessage(onlinePlayer.GetClient());
            //    p.SetCode(5);
            //    PacketManager.ProcessOutgoingPacket(p);
            // }
            //Console.WriteLine("Wait 10 seconds, your css will be restarted.");
            //Thread.Sleep(10000);
            
            //Process.Start("restarter.bat");
        }

        public bool m_vTimerCanceled;
        public Timer TimerReference;
        static readonly object m_vDatabaseLock = new object();
        static Dictionary<long, Alliance> m_vAlliances;
        static long m_vAllianceSeed;
        static long m_vAvatarSeed;
        static DatabaseManager m_vDatabase;
        static string m_vHomeDefault;
        static Random m_vRandomSeed;
        public static DataTables DataTables { get; set; }
        public static FingerPrint FingerPrint { get; set; }
        public static Dictionary<int, string> NpcLevels { get; set; }

        /// <summary>
        ///     This function store a new alliance in the database.
        /// </summary>
        /// <param name="seed">The seed of the client.</param>
        /// <returns>The alliance data.</returns>
        /// <seealso cref="Alliance" />
        public static Alliance CreateAlliance(long seed)
        {
            Alliance alliance;
            lock (m_vDatabase)
            {
                if (seed == 0)
                    seed = m_vAllianceSeed;
                alliance = new Alliance(seed);
                m_vAllianceSeed++;
            }
            m_vDatabase.CreateAlliance(alliance);
            m_vAlliances.Add(alliance.GetAllianceId(), alliance);
            return alliance;
        }

        /// <summary>
        ///     This function create a new player in the database.
        /// </summary>
        /// <param name="seed">The seed of the client.</param>
        /// <returns>The level() of the player.</returns>
        /// <seealso cref="Level" />
        public static Level CreateAvatar(long seed, string token)
        {
            Level pl;
            lock (m_vDatabase)
            {
                if (seed == 0)
                    seed = m_vAvatarSeed;
                pl = new Level(seed, token);
                m_vAvatarSeed++;
            }
            pl.LoadFromJSON(m_vHomeDefault);
            m_vDatabase.CreateAccount(pl);
            return pl;
        }

        /// <summary>
        ///     This function store all alliances in the database, in a list<> variable, named
        ///     as m_vAlliances.
        /// </summary>
        public static void GetAllAlliancesFromDB()
        {
            var alliances = m_vDatabase.GetAllAlliances();
            foreach (Alliance t in alliances)
            {
                if (!m_vAlliances.ContainsKey(t.GetAllianceId()))
                    m_vAlliances.Add(t.GetAllianceId(), t);
            }
        }

        

        /// <summary>
        ///     This function get the info of an alliance.
        /// </summary>
        /// <param name="allianceId">The (Int64) ID of the alliance.</param>
        /// <returns>Return data about the alliance.</returns>
        public static Alliance GetAlliance(long allianceId)
        {
            Alliance alliance = null;
            if (m_vAlliances.ContainsKey(allianceId))
            {
                alliance = m_vAlliances[allianceId];
            }
            else
            {
                alliance = m_vDatabase.GetAlliance(allianceId);
                if (alliance != null)
                    m_vAlliances.Add(alliance.GetAllianceId(), alliance);
            }
            return alliance;
        }

        /// <summary>
        ///     This function return all in-memory alliances.
        /// </summary>
        /// <returns>All alliances in-memory</returns>
        public static List<Alliance> GetInMemoryAlliances()
        {
            var alliances = new List<Alliance>();
            alliances.AddRange(m_vAlliances.Values);
            return alliances;
        }

        /// <summary>
        ///     This function return the data of a random player, in memory.
        /// </summary>
        /// <returns>Random player data.</returns>
        public static Level GetRandomOnlinePlayer()
        {
            var index = m_vRandomSeed.Next(0, ResourcesManager.GetInMemoryLevels().Count); //accès concurrent KO
            return ResourcesManager.GetInMemoryLevels().ElementAt(index);
        }

        /// <summary>
        ///     This function return the data of a random player in database.
        /// </summary>
        /// <returns>Data of a random player.</returns>
        public static Level GetRandomPlayerFromAll()
        {
            var index = m_vRandomSeed.Next(0, ResourcesManager.GetAllPlayerIds().Count); //accès concurrent KO
            return ResourcesManager.GetPlayer(ResourcesManager.GetAllPlayerIds()[index]);
        }

        /// <summary>
        ///     This function store the content of the fingerprint file in a variable.
        /// </summary>
        public static void LoadFingerPrint()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]))
                FingerPrint = new FingerPrint(@"Gamefiles/fingerprint.json");
        }

        /// <summary>
        ///     This function load all gamefiles in the csv/logic/ folder.
        /// </summary>
        public static void LoadGameFiles()
        {
            try
            {

                var gameFiles = new List<Tuple<string, string, int>>();
                gameFiles.Add(new Tuple<string, string, int>("Achievements", @"Gamefiles/logic/achievements.csv", 22));
                gameFiles.Add(new Tuple<string, string, int>("Buildings", @"Gamefiles/logic/buildings.csv", 0));
                gameFiles.Add(new Tuple<string, string, int>("Characters", @"Gamefiles/logic/characters.csv", 3));
                gameFiles.Add(new Tuple<string, string, int>("Decos", @"Gamefiles/logic/decos.csv", 17));
                gameFiles.Add(new Tuple<string, string, int>("Experience Levels", @"Gamefiles/logic/experience_levels.csv", 10));
                gameFiles.Add(new Tuple<string, string, int>("Globals", @"Gamefiles/logic/globals.csv", 13));
                gameFiles.Add(new Tuple<string, string, int>("Heroes", @"Gamefiles/logic/heroes.csv", 27));
                gameFiles.Add(new Tuple<string, string, int>("Leagues", @"Gamefiles/logic/leagues.csv", 12));
                gameFiles.Add(new Tuple<string, string, int>("NPCs", @"Gamefiles/logic/npcs.csv", 16));
                //gameFiles.Add(new Tuple<string, string, int>("Obstacles", @"Gamefiles/logic/obstacles.csv", 7));
                //gameFiles.Add(new Tuple<string, string, int>("Shields", @"Gamefiles/logic/shields.csv", 19));
                gameFiles.Add(new Tuple<string, string, int>("Spells", @"Gamefiles/logic/spells.csv", 25));
                gameFiles.Add(new Tuple<string, string, int>("Townhall Levels", @"Gamefiles/logic/townhall_levels.csv", 14));
                gameFiles.Add(new Tuple<string, string, int>("Traps", @"Gamefiles/logic/traps.csv", 11));
                gameFiles.Add(new Tuple<string, string, int>("Resources", @"Gamefiles/logic/resources.csv", 2));

                /*
                  
                gameFiles.Add(new Tuple<string, string, int>("Alliance Badge Layers", @"Gamefiles/logic/alliance_badge_layers.csv", 30));
                gameFiles.Add(new Tuple<string, string, int>("Alliance Badges", @"Gamefiles/logic/alliance_badges.csv", 31));
                gameFiles.Add(new Tuple<string, string, int>("Alliance Levels", @"Gamefiles/logic/alliance_levels.csv", 32));
                gameFiles.Add(new Tuple<string, string, int>("Alliance Portal", @"Gamefiles/logic/alliance_portal.csv", 33));
                gameFiles.Add(new Tuple<string, string, int>("Buildings Classes", @"Gamefiles/logic/building_classes.csv", 34));
                gameFiles.Add(new Tuple<string, string, int>("Effects", @"Gamefiles/logic/effects.csv", 35));
                gameFiles.Add(new Tuple<string, string, int>("Locales", @"Gamefiles/logic/locales.csv", 36));
                gameFiles.Add(new Tuple<string, string, int>("Missions", @"Gamefiles/logic/missions.csv", 37));
                gameFiles.Add(new Tuple<string, string, int>("Projectiles", @"Gamefiles/logic/projectiles.csv", 38));
                gameFiles.Add(new Tuple<string, string, int>("Regions", @"Gamefiles/logic/regions.csv", 39));
                gameFiles.Add(new Tuple<string, string, int>("Variables", @"Gamefiles/logic/variables.csv", 40)); 
                gameFiles.Add(new Tuple<string, string, int>("War", @"Gamefiles/logic/war.csv", 28));
                
                */

                _Logger.Print("     Loading server gamefiles & data...",Types.DEBUG);
                for (var i = 0; i < gameFiles.Count; i++)
                {
                    _Logger.Print("             ->  " + gameFiles[i].Item1,Types.DEBUG);
                    DataTables.InitDataTable(new CSVTable(gameFiles[i].Item2), gameFiles[i].Item3);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.ResetColor();
                }

            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(); 
                _Logger.Print("     Error loading Gamefiles. Looks like you have :",Types.ERROR);
                _Logger.Print("     -> Edited the files wrongly", Types.ERROR);
                _Logger.Print("     -> Made mistakes by deleting values", Types.ERROR);
                _Logger.Print("     -> Entered High or low value", Types.ERROR);
                _Logger.Print("     -> Please check to these errors", Types.ERROR);
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(0);

            }
        }

        /// <summary>
        ///     This function store all files content in the folder Gamefiles/pve/level* to a
        ///     list<> variable.
        /// </summary>
        public static void LoadNpcLevels()
        {
            _Logger.Print("\n       Loading Npc levels... ", Types.INFO);
            for (var i = 0; i < 50; i++)
                using (var sr = new StreamReader(@"Gamefiles/pve/level" + (i + 1) + ".json"))
                    NpcLevels.Add(i + 17000000, sr.ReadToEnd());
        }

        /// <summary>
        ///     This function return all in-memory alliances.
        /// </summary>
        /// <returns>All alliances in-memory</returns>
        public static void RemoveInMemoryAlliance(long id) => m_vAlliances.Remove(id);

        /// <summary>
        ///     This function dispose the class.
        /// </summary>
        public void Dispose()
        {
            if (TimerReference != null)
            {
                TimerReference.Dispose();
                TimerReference = null;
            }
        }

    }
}
