using System;
using System.Configuration;
using System.Linq;
using System.Management;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using static CSS.Core.Logger;
using static System.Console;
using CSS.Core;
using CSS.Core.Network;
using CSS.Core.Settings;
using CSS.Logic;
using CSS.Packets.Messages.Server;
using CSS.Core.Checker;
using CSS.Core.Web;

namespace CSS.Helpers
{
    internal class ParserThread
    {
        static bool MaintenanceMode = false;

        static int Time;
    
        static Thread T { get; set; }

        static ParserThread()
        {
            T = new Thread((ThreadStart)(() =>
            {
                while (true)
                {
                    string entry = Console.ReadLine().ToLower();
                    switch (entry)
                    {
                        case "/help":
                            Print("------------------------------------------------------------------------------>");
                            Say("/status            - Shows the actual CSS status.");
                            Say("/clear             - Clears the console screen.");
                            Say("/gui               - Shows the CSS Graphical User Interface.");
                            Say("/restart           - Restarts CSS instantly.");
                            Say("/shutdown          - Shuts CSS down instantly.");
                            Say("/banned            - Writes all Banned IP's into the Console.");
                            Say("/addip             - Add an IP to the Blacklist");
                            Say("/maintenance       - Begin Server Maintenance.");
                            Say("/saveall           - Saves everything in memory to the Database");
                            Say("/dl csv            - Downloads latest CSV Files (if Fingerprint is up to Date).");
                            Say("/info              - Shows the CSS Informations.");
                            Say("/info 'command'    - More Info On a Command. Ex: /info gui");
                            Print("------------------------------------------------------------------------------>");
                            break;

                        case "/info":
                            Console.WriteLine("------------------------------------->");
                            Say($"CSS Version:         {Constants.Version}");
                            Say($"Build:               {Constants.Build}");
                            Say($"LicenseID:           {Constants.LicensePlanID}");
                            Say($"CoC Version from CSS: {VersionChecker.LatestCoCVersion()}");
                            Say($"CSSS  - {DateTime.Now.Year}");
                            Console.WriteLine("------------------------------------->");
                            break;

                        case "/dl csv":
                            CSVManager.DownloadLatestCSVFiles();
                            break;

                        case "/info dl csv":
                            Print("------------------------------------------------------------------------------>");
                            Say(@"/dl csv > Downloads COC Assets such as CSVs and if enabled:");
                            Say(@"     - Logic,");
                            Say(@"     - Sound Files ");
                            Say(@"     - SCs");
                            Print("------------------------------------------------------------------------------>");
                            break;

                        case "/banned":
                            Console.WriteLine("------------------------------------->");
                            Say("Banned IP Addresses:");
                            ConnectionBlocker.GetBannedIPs();
                            Console.WriteLine("------------------------------------->");
                            break;

                        case "/addip":
                            Console.WriteLine("------------------------------------->");
                            Console.Write("IP: ");
                            string s = Console.ReadLine();
                            ConnectionBlocker.AddNewIpToBlackList(s);
                            Console.WriteLine("------------------------------------->");
                            break;

                        case "/saveall":
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("----------------------------------------------------->");
                            Say($"Starting saving of all Players... ({ResourcesManager.m_vInMemoryLevels.Count})");
                            Resources.DatabaseManager.Save(ResourcesManager.m_vInMemoryLevels.Values.ToList()).Wait();
                            Say("Finished saving of all Players!");
                            Say($"Starting saving of all Alliances... ({ResourcesManager.GetInMemoryAlliances().Count})");
                            Resources.DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances()).Wait();
                            Say("Finished saving of all Alliances!");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("----------------------------------------------------->");
                            Console.ResetColor();
                            break;

                        

                        case "/maintenance":
                            StartMaintenance();
                            break;

                        case "/info maintenance":
                            Print("------------------------------------------------------------------------------>");
                            Say(@"/maintenance > Enables Maintenance which will do the following:");
                            Say(@"     - All Online Users will be notified (Attacks will be disabled),");
                            Say(@"     - All new connections get a Maintenace Message at the Login. ");
                            Say(@"     - After 5min all Players will be kicked.");
                            Say(@"     - After the Maintenance Players will be able to connect again.");
                            Print("------------------------------------------------------------------------------>");
                            break;

                        case "/status":
                            Say($"Please wait retrieving CSS Server status");
                            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
                            var cpuTimes = searcher.Get()
                                .Cast<ManagementObject>()
                                .Select(mo => new
                                {
                                    Name = mo["Name"],
                                    Usage = mo["PercentProcessorTime"]
                                }
                                )
                                .ToList();
                            var query = cpuTimes.Where(x => x.Name.ToString() == "_Total").Select(x => x.Usage);
                            var CPUParcentage = query.SingleOrDefault();
                            Print("------------------------------------------------------->");
                            Say($"CPU Usage:                {CPUParcentage}%");
                            Say($"RAM Usage:                {Performances.GetUsedMemory()}%");
                            Say($"Time:                     {DateTime.Now}");
                            Say($"IP Address:               {Dns.GetHostByName(Dns.GetHostName()).AddressList[0]}");
                            Say($"Online Players:           {ResourcesManager.m_vOnlinePlayers.Count}");
                            Say($"Connected Players:        {ResourcesManager.GetConnectedClients().Count}");
                            Say($"In Memory Players:        {ResourcesManager.m_vInMemoryLevels.Values.ToList().Count}");
                            Say($"In Memory Alliances:      {ResourcesManager.GetInMemoryAlliances().Count}");
                            Say($"Client Version:           {ConfigurationManager.AppSettings["ClientVersion"]}");
                            Print("------------------------------------------------------->");
                            break;

                        case "/info status":
                            Print("----------------------------------------------------------------->");
                            Say(@"/status > Shows current state of server including:");
                            Say(@"     - Online Status");
                            Say(@"     - Server IP Address");
                            Say(@"     - Amount of Online Players");
                            Say(@"     - Amount of Connected Players");
                            Say(@"     - Amount of Players in Memory");
                            Say(@"     - Amount of Alliances in Memory");
                            Say(@"     - Clash of Clans Version.");
                            Print("----------------------------------------------------------------->");
                            break;

                        case "/clear":
                            Clear();
                            break;

                        case "/info shutdown":
                            Print("---------------------------------------------------------------------------->");
                            Say(@"/shutdown > Shuts Down CSS instantly after doing the following:");
                            Say(@"     - Throws all Players an 'Client Out of Sync Message'");
                            Say(@"     - Disconnects All Players From the Server");
                            Say(@"     - Saves all Players in Database");
                            Say(@"     - Shutsdown CSS.");
                            Print("---------------------------------------------------------------------------->");
                            break;

                        case "/gui":
                            Application.Run(new CSSUI());
                            break;

                        case "/info gui":
                            Print("------------------------------------------------------------------------------->");
                            Say(@"/gui > Starts the CSS Gui which includes many features listed here:");
                            Say(@"     - Status Controler/Manager");
                            Say(@"     - Player Editor");
                            Say(@"     - config.css editor.");
                            Print("------------------------------------------------------------------------------->");
                            break;

                        case "/restart":
                            CSSControl.CSSRestart();
                            break;

                        case "/shutdown":
                            CSSControl.CSSClose();
                            break;

                        case "/info restart":
                            Print("---------------------------------------------------------------------------->");
                            Say(@"/restart > Restarts CSS instantly after doing the following:");
                            Say(@"     - Throws all Players an 'Client Out of Sync Message'");
                            Say(@"     - Disconnects All Players From the Server");
                            Say(@"     - Saves all Players in Database");
                            Say(@"     - Restarts CSS.");
                            Print("---------------------------------------------------------------------------->");
                            break;

                        default:
                            Say("Unknown command, type \"/help\" for a list containing all available commands.");
                            break;
                    }
               }
            })); 
            T.Start();
        }

        static System.Timers.Timer Timer = new System.Timers.Timer();
        static System.Timers.Timer Timer2 = new System.Timers.Timer();
                
        public static void StartMaintenance()
        {
            Print("------------------------------------------------------------------->");
            Say("Please type in now your Time for the Maintenance");
            Say("(Seconds): ");
            String newTime = ReadLine();
            Time = Convert.ToInt32(((newTime + 0) + 0) + 0);
            Say("Server will be restarted in 5min and will start with the");
            Say("Maintenance Mode (" + Time + ")");
            Print("------------------------------------------------------------------->");

            foreach (Level p in ResourcesManager.m_vOnlinePlayers)
            {
                Processor.Send(new ShutdownStartedMessage(p.Client));
            }

            Timer.Elapsed += ShutdownMessage;
            Timer.Interval = 30000;
            Timer.Start();
            Timer2.Elapsed += ActivateFullMaintenance;
            Timer2.Interval = 300000;
            Timer2.Start();
            MaintenanceMode = true;
        }
        
        private static void ShutdownMessage(object sender, EventArgs e)
        {
            foreach(Level p in ResourcesManager.m_vOnlinePlayers)
            {
                Processor.Send(new ShutdownStartedMessage(p.Client));
            }
        }

        static System.Timers.Timer Timer3 = new System.Timers.Timer();

        private static void ActivateFullMaintenance(object sender, EventArgs e)
        {
            Timer.Stop();
            Timer2.Stop();
            Timer3.Elapsed += DisableMaintenance;
            Timer3.Interval = Time;
            Timer3.Start();
            ForegroundColor = ConsoleColor.Yellow;
            Say("Full Maintenance has been started!");
            ResetColor();
            if (Time >= 7000)
            {
                Say();
                Error("Please type in a valid time!");
                Error("20min = 1200, 10min = 600");
                Say();
                StartMaintenance();
            }

            foreach(Level p in ResourcesManager.m_vInMemoryLevels.Values.ToList())
            {
                Processor.Send(new OutOfSyncMessage(p.Client));
                ResourcesManager.DropClient(p.Client.SocketHandle);
            }
            Resources.DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances());
        }

        private static void DisableMaintenance(object sender, EventArgs e)
        {             
            Time = 0;
            MaintenanceMode = false;
            Timer3.Stop();
            Say("Maintenance Mode has been stopped.");
        }

        public static bool GetMaintenanceMode() => MaintenanceMode;
        
        public static void SetMaintenanceMode(bool m) => MaintenanceMode = m;

        public static int GetMaintenanceTime() => Time;
    }
}
