using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using CSS.Database;
using CSS.Logic;

namespace CSS.Core
{
    internal class DatabaseManager
    {
        #region Public Constructors

        public DatabaseManager()
        {
            m_vConnectionString = ConfigurationManager.AppSettings["databaseConnectionName"];
        }

        #endregion Public Constructors

        #region Public Properties

        public static DatabaseManager Singelton
        {
            get
            {
                if (singelton == null)
                    singelton = new DatabaseManager();
                return singelton;
            }
        }

        #endregion Public Properties

        #region Private Fields

        static DatabaseManager singelton;
        readonly string m_vConnectionString;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        ///     This function create a new player in the database, with default parameters.
        /// </summary>
        /// <param name="l">The level of the player.</param>
        public void CreateAccount(Level l)
        {
            try
            {
                using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                {
                    db.player.Add(
                        new player
                        {
                            PlayerId = l.GetPlayerAvatar().GetId(),
                            AccountStatus = l.GetAccountStatus(),
                            AccountPrivileges = l.GetAccountPrivileges(),
                            LastUpdateTime = l.GetTime(),
                            IPAddress = l.GetIPAddress(),
                            Avatar = l.GetPlayerAvatar().SaveToJSON(),
                            GameObjects = l.SaveToJSON()
                        }
                        );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _Logger.Print("     An exception occured during CreateAccount processing :", Types.ERROR);
            }
        }

        /// <summary>
        ///     This function create a new alliance in the database, with default parameters.
        /// </summary>
        /// <param name="a">The alliance data.</param>
        public void CreateAlliance(Alliance a)
        {
            try
            {
                using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                {
                    db.clan.Add(
                        new clan
                        {
                            ClanId = a.GetAllianceId(),
                            LastUpdateTime = DateTime.Now,
                            Data = a.SaveToJSON()
                        }
                        );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _Logger.Print("     An exception occured during CreateAlliance processing :", Types.ERROR);
            }
        }

        /// <summary>
        ///     This function get the player data.
        /// </summary>
        /// <param name="playerId">The (int64) ID of the player.</param>
        /// <returns>The level of the player.</returns>
        public Level GetAccount(long playerId)
        {
            Level account = null;
            try
            {
                using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                {
                    player p = db.player.Find(playerId);

                    if (p != null)
                    {
                        account = new Level();
                        account.SetAccountStatus(p.AccountStatus);
                        account.SetAccountPrivileges(p.AccountPrivileges);
                        account.SetTime(p.LastUpdateTime);
                        account.SetIPAddress(p.IPAddress);
                        account.GetPlayerAvatar().LoadFromJSON(p.Avatar);
                        account.LoadFromJSON(p.GameObjects);
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.Print("     An exception occured during GetAccount processing :", Types.ERROR);
            }
            return account;
        }

        /// <summary>
        ///     This function return all alliances in database, in a list<>.
        /// </summary>
        /// <returns>
        ///     Return a list<> containing all alliances.
        /// </returns>
        public List<Alliance> GetAllAlliances()
        {
            List<Alliance> alliances = new List<Alliance>();
            try
            {
                using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                {
                    DbSet<clan> a = db.clan;
                    int count = 0;
                    foreach (clan c in a)
                    {
                        Alliance alliance = new Alliance();
                        alliance.LoadFromJSON(c.Data);
                        alliances.Add(alliance);
                        if (count++ >= 100)
                            break;
                    }
                    _Logger.Print("     The server loaded " + count + " alliances",Types.DEBUG);
                }
            }
            catch (Exception ex)
            {
                _Logger.Print("      An exception occured during GetAlliance processing:", Types.ERROR);
            }
            return alliances;
        }

        public ConcurrentDictionary<long, Level> GetAllPlayers()
        {
            ConcurrentDictionary<long, Level> players = new ConcurrentDictionary<long, Level>();
            try
            {
                using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                {
                    DbSet<player> a = db.player;
                    int count = 0;
                    foreach (player c in a)
                    {
                        Level pl = new Level();
                        players.TryAdd(pl.GetPlayerAvatar().GetId(), pl);
                        if (count++ >= 100)
                            break;
                    }
                    _Logger.Print("     The server loaded " + count + " players",Types.DEBUG);
                }
            }
            catch (Exception ex)
            {
                _Logger.Print("      An exception occured during GetPlayers processing:", Types.ERROR);
            }
            return players;
        }

        /// <summary>
        ///     This function get the alliance data.
        /// </summary>
        /// <param name="allianceId">The (Int64) ID of the alliance.</param>
        /// <returns>The Alliance of the Clan.</returns>
        public Alliance GetAlliance(long allianceId)
        {
            Alliance alliance = null;
            try
            {
                using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                {
                    clan p = db.clan.Find(allianceId);
                    if (p != null)
                    {
                        alliance = new Alliance();
                        alliance.LoadFromJSON(p.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.Print("      An exception occured during GetAlliance processing :", Types.ERROR);
            }
            return alliance;
        }

        /// <summary>
        ///     This function get all players id in a list.
        /// </summary>
        /// <returns>A list of all players id.</returns>
        public List<long> GetAllPlayerIds()
        {
            List<long> ids = new List<long>();
            using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                ids.AddRange(db.player.Select(p => p.PlayerId));
            return ids;
        }

        /// <summary>
        ///     This function return the highest alliance id stored in database.
        /// </summary>
        /// <returns>An int64 (ID) .</returns>
        public long GetMaxAllianceId()
        {
            long max = 0;
            using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                max = (from alliance in db.clan select (long?) alliance.ClanId ?? 0).DefaultIfEmpty().Max();
            return max;
        }

        /// <summary>
        ///     The function return the highest player id stored in database.
        /// </summary>
        /// <returns>An int64 long ID.</returns>
        public long GetMaxPlayerId()
        {
            long max = 0;
            using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                max = (from ep in db.player select (long?) ep.PlayerId ?? 0).DefaultIfEmpty().Max();
            return max;
        }

        /// <summary>
        ///     This function remove an alliance from database.
        /// </summary>
        /// <param name="alliance">The Alliance of the alliance.</param>
        public void RemoveAlliance(Alliance alliance)
        {
            long id = alliance.GetAllianceId();
            using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
            {
                db.clan.Remove(db.clan.Find((int) id));
                db.SaveChanges();
            }
            ObjectManager.RemoveInMemoryAlliance(id);
        }

        public void Save(Alliance alliance)
        {
            using (cssdbEntities context = new cssdbEntities(m_vConnectionString))
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;
                clan c = context.clan.Find((int) alliance.GetAllianceId());
                if (c != null)
                {
                    c.LastUpdateTime = DateTime.Now;
                    c.Data = alliance.SaveToJSON();
                    context.Entry(c).State = EntityState.Modified;
                }
                else
                {
                    context.clan.Add(
                        new clan
                        {
                            ClanId = alliance.GetAllianceId(),
                            LastUpdateTime = DateTime.Now,
                            Data = alliance.SaveToJSON()
                        }
                        );
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        ///     This function save a specific player in the database.
        /// </summary>
        /// <param name="avatar">The level of the player.</param>
        public void Save(Level avatar)
        {
            cssdbEntities context = new cssdbEntities(m_vConnectionString);
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            player p = context.player.Find(avatar.GetPlayerAvatar().GetId());
            if (p != null)
            {
                p.LastUpdateTime = avatar.GetTime();
                p.AccountStatus = avatar.GetAccountStatus();
                p.AccountPrivileges = avatar.GetAccountPrivileges();
                p.IPAddress = avatar.GetIPAddress();
                p.Avatar = avatar.GetPlayerAvatar().SaveToJSON();
                p.GameObjects = avatar.SaveToJSON();
                context.Entry(p).State = EntityState.Modified;
            }
            else
            {
                context.player.Add(
                    new player
                    {
                        PlayerId = avatar.GetPlayerAvatar().GetId(),
                        AccountStatus = avatar.GetAccountStatus(),
                        AccountPrivileges = avatar.GetAccountPrivileges(),
                        LastUpdateTime = avatar.GetTime(),
                        IPAddress = avatar.GetIPAddress(),
                        Avatar = avatar.GetPlayerAvatar().SaveToJSON(),
                        GameObjects = avatar.SaveToJSON()
                    }
                    );
            }
            context.SaveChanges();
        }

        public void Save(List<Level> avatars)
        {
            try
            {
                using (cssdbEntities context = new cssdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    int transactionCount = 0;
                    foreach (Level pl in avatars)
                        lock (pl)
                        {
                            player p = context.player.Find(pl.GetPlayerAvatar().GetId());
                            if (p != null)
                            {
                                p.LastUpdateTime = pl.GetTime();
                                p.AccountStatus = pl.GetAccountStatus();
                                p.AccountPrivileges = pl.GetAccountPrivileges();
                                p.IPAddress = pl.GetIPAddress();
                                p.Avatar = pl.GetPlayerAvatar().SaveToJSON();
                                p.GameObjects = pl.SaveToJSON();
                                context.Entry(p).State = EntityState.Modified;
                            }
                            else
                                context.player.Add(
                                    new player
                                    {
                                        PlayerId = pl.GetPlayerAvatar().GetId(),
                                        AccountStatus = pl.GetAccountStatus(),
                                        AccountPrivileges = pl.GetAccountPrivileges(),
                                        LastUpdateTime = pl.GetTime(),
                                        IPAddress = pl.GetIPAddress(),
                                        Avatar = pl.GetPlayerAvatar().SaveToJSON(),
                                        GameObjects = pl.SaveToJSON()
                                    }
                                    );
                        }
                    transactionCount++;
                    if (transactionCount >= 100)
                    {
                        context.SaveChanges();
                        transactionCount = 0;
                    }
                     context.SaveChanges();
                }
                _Logger.Print("     All players in memory has been saved to database at " + DateTime.Now,Types.INFO);
            }
            catch (Exception ex)
            {
                _Logger.Print("     An exception occured during Save processing for avatars :", Types.ERROR);
            }
        }

        /// <summary>
        ///     This function save a specific alliance in the database.
        /// </summary>
        /// <param name="alliances">The Alliance of the alliance.</param>
        public void Save(List<Alliance> alliances)
        {
            try
            {
                using (cssdbEntities context = new cssdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    int transactionCount = 0;
                    foreach (Alliance alliance in alliances)
                        lock (alliance)
                        {
                            clan c = context.clan.Find((int) alliance.GetAllianceId());
                            if (c != null)
                            {
                                c.LastUpdateTime = DateTime.Now;
                                c.Data = alliance.SaveToJSON();
                                context.Entry(c).State = EntityState.Modified;
                            }
                            else
                            {
                                context.clan.Add(
                                    new clan
                                    {
                                        ClanId = alliance.GetAllianceId(),
                                        LastUpdateTime = DateTime.Now,
                                        Data = alliance.SaveToJSON()
                                    }
                                    );
                            }
                        }
                    transactionCount++;
                    if (transactionCount >= 100)
                    {
                        context.SaveChanges();
                        context.SaveChanges();
                        context.SaveChanges();
                        transactionCount = 0;
                    }
                    context.SaveChanges();
                }
                _Logger.Print("      All alliances in memory has been saved to database at " + DateTime.Now, Types.INFO);
            }
            catch (Exception ex)
            {
                _Logger.Print("     An exception occured during Save processing for alliances :", Types.ERROR);
            }
        }

        public void CheckConnection()
        {
            try
            {
                using (cssdbEntities db = new cssdbEntities(m_vConnectionString))
                {
                    (from ep in db.player select (long?) ep.PlayerId ?? 0).DefaultIfEmpty().Max();
                }
            }
            catch (EntityException)
            {
                if (ConfigurationManager.AppSettings["databaseConnectionName"] == "mysql")
                {
                    _Logger.Print("     An exception occured when connecting to the MySQL Server.",Types.ERROR);
                    _Logger.Print("     Please check your database configuration !",Types.ERROR);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                else
                {
                    _Logger.Print("     An exception occured when connecting to the SQLite database.", Types.ERROR);
                    _Logger.Print("     Please check your database configuration !", Types.ERROR);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            catch (Exception)
            {
                _Logger.Print("     An exception occured when connecting to the SQL Server.", Types.ERROR);
                _Logger.Print("     Please check your database configuration !", Types.ERROR);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        #endregion Public Methods
    }
}
