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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using ThreadState = System.Threading.ThreadState;
using Timer = System.Timers.Timer;
using CSS.Core;

namespace CSS.Core.Threading
{
    internal class MemoryThread
    {
        #region Private Properties

        /// <summary>
        ///     Variable holding the thread itself
        /// </summary>
        static Thread T { get; set; }

        #endregion Private Properties

        #region Private Methods

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize,
            UIntPtr maximumWorkingSetSize);

        #endregion Private Methods

        #region Public Methods

        public static void Start()
        {
            T = new Thread(() =>
            {
                var t = new Timer();
                t.Interval = 600; // 0,6 Seconds
                t.Elapsed += (s, a) =>
                {
                    GC.Collect(GC.MaxGeneration);
                    GC.WaitForPendingFinalizers();
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr) 0xFFFFFFFF,
                        (UIntPtr) 0xFFFFFFFF);
                };
                t.Enabled = true;
            });
            T.Start();
        }

        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }

        #endregion Public Methods
    }

    internal class PerformanceInfo
    {
        #region Public Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        #endregion Public Structs

        #region Public Methods

        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo();

        public static long GetPhysicalAvailableMemoryInMiB()
        {
            var pi = new PerformanceInformation();
            if (GetPerformanceInfo())
                return Convert.ToInt64(pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576);
            return -1;
        }

        public static long GetTotalMemoryInMiB()
        {
            var pi = new PerformanceInformation();
            if (GetPerformanceInfo())
                return Convert.ToInt64(pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576);
            return -1;
        }

        #endregion Public Methods
    }
}
