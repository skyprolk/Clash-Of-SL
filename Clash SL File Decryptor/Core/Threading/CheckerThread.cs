using System.Threading;

namespace CSFD.Core.Threading
{
    internal class CheckerThread
    {
        public static void Start()
        {
            Thread T = new Thread(() =>
            {
                DirChecker.Check();
            }); T.Start();
        }
    }
}
