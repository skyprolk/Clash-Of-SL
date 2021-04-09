using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSS.Files;
using CSS.Logic;
using Timer = System.Threading.Timer;
using static CSS.Core.Logger;
using CSS.Logic.Enums;

namespace CSS.Core
{
    internal class ObjectManager : IDisposable
    {
        private static long m_vAllianceSeed;
        private static long m_vAvatarSeed;
        public static int m_vDonationSeed;
        private static int m_vRandomBaseAmount;
        private static DatabaseManager m_vDatabase;
        private static string m_vHomeDefault;
        public static bool m_vTimerCanceled;
        public static Timer TimerReferenceRedis;
        public static Timer TimerReferenceMysql;
        public static Dictionary<int, string> NpcLevels;
        public static Dictionary<int, string> m_vRandomBases;
        public static FingerPrint FingerPrint;
        static int MaxPlayerID;
        static int MaxAllianceID;

        public ObjectManager()
        {
            m_vTimerCanceled       = false;

            m_vDatabase            = new DatabaseManager();

            NpcLevels              = new Dictionary<int, string>();
            m_vRandomBases         = new Dictionary<int, string>();
            FingerPrint            = new FingerPrint();

            MaxPlayerID            = Convert.ToInt32(m_vDatabase.GetMaxPlayerId() + 1);
            MaxAllianceID          = Convert.ToInt32(m_vDatabase.GetMaxAllianceId() + 1);

            m_vAvatarSeed          = MaxPlayerID;
            m_vAllianceSeed        = MaxAllianceID;

            using (StreamReader sr = new StreamReader(@"Gamefiles/starting_home.json"))
            {
                m_vHomeDefault     = sr.ReadToEnd();
            }

            LoadNpcLevels();
            //LoadRandomBase(); // Useless atm

            TimerReferenceRedis = new Timer(SaveRedis, null, 10000, 40000);
            TimerReferenceMysql = new Timer(SaveMysql, null, 40000, 27000);
            Say($"CSS Database has been succesfully loaded. ({Convert.ToInt32(MaxAllianceID + MaxPlayerID)} Tables)");
        }

        private static void SaveRedis(object state)
        {
            m_vDatabase.Save(ResourcesManager.m_vInMemoryLevels.Values.ToList(), Save.Redis);
            m_vDatabase.Save(ResourcesManager.GetInMemoryAlliances(), Save.Redis);
        }
        private static async void SaveMysql(object state)
        {
            m_vDatabase.Save(ResourcesManager.m_vInMemoryLevels.Values.ToList(), Save.Mysql).Wait();
            m_vDatabase.Save(ResourcesManager.GetInMemoryAlliances(), Save.Mysql).Wait();
        }

        public static Alliance CreateAlliance(long seed)
        {
            Alliance alliance;
            if (seed == 0)
            {
                seed = m_vAllianceSeed;
            }
            alliance = new Alliance(seed);
            m_vAllianceSeed++;
            m_vDatabase.CreateAlliance(alliance);
            ResourcesManager.AddAllianceInMemory(alliance);
            return alliance;
        }

        public static Level CreateAvatar(long seed, string token)
        {
            Level pl;
            if (seed == 0)
            {
                seed = m_vAvatarSeed;
            }
            pl = new Level(seed, token);
            m_vAvatarSeed++;
            pl.LoadFromJSON(m_vHomeDefault);
            m_vDatabase.CreateAccount(pl);
            return pl;
        }

        /*public static void LoadAllAlliancesFromDB()
        {
            ResourcesManager.AddAllianceInMemory(m_vDatabase.GetAllAlliances());
        }*/

        public static Alliance GetAlliance(long allianceId)
        {
            Alliance alliance;
            if (ResourcesManager.InMemoryAlliancesContain(allianceId))
            {
                return ResourcesManager.GetInMemoryAlliance(allianceId);
            }
            else
            {
                alliance = m_vDatabase.GetAlliance(allianceId);
                if (alliance != null)
                    ResourcesManager.AddAllianceInMemory(alliance);
                else
                    return null;
                return alliance;
            }
        }

        public static List<Alliance> GetInMemoryAlliances() => ResourcesManager.GetInMemoryAlliances();

        public static Level GetRandomOnlinePlayer()
        {
            int index = new Random().Next(0, ResourcesManager.m_vInMemoryLevels.Count);
            return ResourcesManager.m_vInMemoryLevels.Values.ToList().ElementAt(index);
        }

        public static void LoadNpcLevels()
        {
            int Count = 0;
            NpcLevels.Add(17000000, new StreamReader(@"Gamefiles/level/NPC/tutorial_npc.json").ReadToEnd());
            NpcLevels.Add(17000001, new StreamReader(@"Gamefiles/level/NPC/tutorial_npc2.json").ReadToEnd());

            for (int i = 2; i < 50; i++)
            {
                using (StreamReader sr = new StreamReader(@"Gamefiles/level/NPC/level" + (i + 1) + ".json"))
                {
                    NpcLevels.Add(i + 17000000, sr.ReadToEnd());
                }
                Count++;
            }

            Say($"NPC Levels  have been succesfully loaded. ({Count})");
        }

        /*public static void LoadRandomBase()
        {
            m_vRandomBaseAmount = Directory.GetFiles(@"Gamefiles/level/PVP", "Base*.json").Count();
            for (int i = 0; i < m_vRandomBaseAmount; i++)
                using (StreamReader sr2 = new StreamReader(@"Gamefiles/level/PVP/Base" + (i + 1) + ".json"))
                    m_vRandomBases.Add(i, sr2.ReadToEnd());
            Say("PVP Levels  have been succesfully loaded.");
        }*/

        public static void RemoveInMemoryAlliance(long id)
        {
            ResourcesManager.RemoveAllianceFromMemory(id);
        }

        public static int GetMaxAllianceID() => MaxAllianceID;

        public static int GetMaxPlayerID() => MaxPlayerID;

        public static int RandomBaseCount() => m_vRandomBaseAmount;

        public void Dispose()
        {
            if (TimerReferenceRedis != null && TimerReferenceMysql != null)
            {
                TimerReferenceRedis.Dispose();
                TimerReferenceMysql.Dispose();
                TimerReferenceRedis = null;
                TimerReferenceMysql = null;
            }
        }

    }
}
