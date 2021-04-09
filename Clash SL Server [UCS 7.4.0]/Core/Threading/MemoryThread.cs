using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CSS.Core.Settings;
using CSS.Logic;
using CSS.Packets;

namespace CSS.Core.Threading
{
    internal class MemoryThread : IDisposable
    {
        private System.Timers.Timer _Timer = null;
        private Thread _Thread             = null;

        public MemoryThread()
        {
            _Thread = new Thread(() =>
            { 
                _Timer = new System.Timers.Timer();
                _Timer.Interval = Constants.CleanInterval;
                _Timer.Elapsed += ((s, a) => Clean());
                _Timer.Start();
            });

            _Thread.Priority = ThreadPriority.Lowest;

            _Thread.Start();
        }

        public static void Clean()
        {
            try
            {
                foreach (Level _Player in ResourcesManager.m_vInMemoryLevels.Values.ToList())
                {
                    if (!_Player.Client.IsClientSocketConnected())
                    {
                        _Player.Client.Socket.Close();
                        ResourcesManager.DropClient(_Player.Client.SocketHandle);
                    }
                }

                int c = ResourcesManager.m_vOnlinePlayers.Count;
                Console.Title = Program.Title + c;
                Program.OP = c;

                GC.Collect(GC.MaxGeneration);
                GC.WaitForPendingFinalizers();
            }
            catch (Exception)
            {
                
            }
        }

        public void Dispose()
        {
            _Timer.Stop();
            _Thread.Abort();
        }
    }
}
