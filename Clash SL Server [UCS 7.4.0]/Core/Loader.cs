namespace CSS.Core
{
    using CSS.Core.Checker;
    using CSS.Database;
    using CSS.Core.Events;
    using CSS.Core.Settings;
    using CSS.Core.Threading;
    using CSS.Helpers;
    using CSS.Packets;
    using CSS.WebAPI;
    internal class Loader
    {
        internal CSVManager CsvManager;
        internal ConnectionBlocker ConnectionBlocker;
        internal DirectoryChecker DirectoryChecker;
        internal API API;
        internal Redis Redis;
        internal Logger Logger;
        internal ParserThread Parser;
        internal ResourcesManager ResourcesManager;
        internal ObjectManager ObjectManager;
        internal CommandFactory CommandFactory;
        internal MessageFactory MessageFactory;
        internal MemoryThread MemThread;
        internal LicenseChecker LicenseChecker;
        internal EventsHandler Events;

        public Loader()
        {
            // CSV Files and Logger
            this.Logger = new Logger();
            this.DirectoryChecker = new DirectoryChecker();
            this.CsvManager = new CSVManager();

            this.ConnectionBlocker = new ConnectionBlocker();
            if (Utils.ParseConfigBoolean("UseWebAPI"))
                this.API = new API();


            // Core
            this.LicenseChecker = new LicenseChecker();
            this.ResourcesManager = new ResourcesManager();
            this.ObjectManager = new ObjectManager();
            this.Events = new EventsHandler();
            if (Constants.UseCacheServer)
                this.Redis = new Redis();


            this.CommandFactory = new CommandFactory();

            this.MessageFactory = new MessageFactory();

            // Optimazions
            this.MemThread = new MemoryThread();

            // User
            this.Parser = new ParserThread();

        }
    }
}
