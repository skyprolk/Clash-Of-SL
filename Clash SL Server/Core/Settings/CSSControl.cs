using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSS.Core.Threading;
using CSS.Core.Web;
using static System.Console;
using static CSS.Core.Logger;

namespace CSS.Core.Settings
{
    internal class CSSControl
    {
        public static void CSSClose()
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
            }

            Environment.Exit(0);
        }

        public static void CSSRestart()
        {
            new Thread(() =>
            {
                Say("Restarting CSS...");
                Thread.Sleep(200);
                Process.Start("CSS.exe");
                Environment.Exit(0);
            }).Start();
        }
    }
}
