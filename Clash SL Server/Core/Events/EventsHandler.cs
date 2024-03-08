
namespace CSS.Core.Events
{

    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using System.Linq;
    using System.Threading;

    internal class EventsHandler
    {
        internal static EventHandler EHandler;

        internal delegate void EventHandler(Logic.Enums.Exits Type = Logic.Enums.Exits.CTRL_CLOSE_EVENT);

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsHandler"/> class.
        /// </summary>
        internal EventsHandler()
        {
            EventsHandler.EHandler += this.Handler;
            EventsHandler.SetConsoleCtrlHandler(EventsHandler.EHandler, true);
        }
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Enabled);

        internal void ExitHandler()
        {
            try
            {
                if (ResourcesManager.m_vInMemoryLevels.Count > 0)
                {
                    Parallel.ForEach(ResourcesManager.m_vInMemoryLevels.Values.ToList(), (_Player) =>
                    {
                        if (_Player != null)
                        {
                            ResourcesManager.LogPlayerOut(_Player);
                        }
                    });
                }


                if (ResourcesManager.GetInMemoryAlliances().Count > 0)
                {
                    Parallel.ForEach(ResourcesManager.GetInMemoryAlliances(), (_Player) =>
                    {
                        if (_Player != null)
                        {
                            ResourcesManager.RemoveAllianceFromMemory(_Player.m_vAllianceId);
                        }
                    });
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mmh, something happen when we tried to save everything.");
            }
        }

        internal void Handler(Logic.Enums.Exits Type = Logic.Enums.Exits.CTRL_CLOSE_EVENT)
        {
            Logger.Say("CSS is shutting down", true);
            new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                }
            }).Start();
            this.ExitHandler();
        }
    }
}