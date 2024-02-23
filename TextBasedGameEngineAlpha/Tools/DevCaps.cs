using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal sealed class HL_DevTime : IDisposable
    {
        
        [StructLayout(LayoutKind.Sequential)]
        private struct TIMECAPS
        {
            internal int wPeriodMin;
            internal int wPeriodMax;
        }

        private static TIMECAPS devCaps;

        private static int inTimePeriod;

        private readonly int period;

        private int disposed;

        [DllImport("winmm.dll", ExactSpelling = true)]
        private static extern int timeGetDevCaps(ref TIMECAPS ptc, int cbtc);

        [DllImport("winmm.dll", ExactSpelling = true)]
        private static extern int timeBeginPeriod(int uPeriod);

        [DllImport("winmm.dll", ExactSpelling = true)]
        private static extern int timeEndPeriod(int uPeriod);

        static HL_DevTime()
        {
            int result = timeGetDevCaps(ref devCaps, Marshal.SizeOf(typeof(TIMECAPS)));
            if (result != 0)
            {
                throw new InvalidOperationException("The request to get time capabilities was not completed because an unexpected error with code " + result + " occured.");
            }
        }

        internal HL_DevTime(int period)
        {
            if (Interlocked.Increment(ref inTimePeriod) != 1)
            {
                Interlocked.Decrement(ref inTimePeriod);
                throw new NotSupportedException("The process is already within a time period. Nested time periods are not supported.");
            }

            if (period < devCaps.wPeriodMin || period > devCaps.wPeriodMax)
            {
                throw new ArgumentOutOfRangeException("period", "The request to begin a time period was not completed because the resolution specified is out of range.");
            }

            int result = timeBeginPeriod(period);
            if (result != 0)
            {
                throw new InvalidOperationException("The request to begin a time period was not completed because an unexpected error with code " + result + " occured.");
            }

            this.period = period;
        }

        internal static int MinimumPeriod
        {
            get
            {
                return devCaps.wPeriodMin;
            }
        }

        internal static int MaximumPeriod
        {
            get
            {
                return devCaps.wPeriodMax;
            }
        }

        internal int Period
        {
            get
            {
                if (this.disposed > 0)
                {
                    throw new ObjectDisposedException("The time period instance has been disposed.");
                }

                return this.period;
            }
        }

        public void Dispose()
        {
            if (Interlocked.Increment(ref this.disposed) == 1)
            {
                timeEndPeriod(this.period);
                Interlocked.Decrement(ref inTimePeriod);
            }
            else
            {
                Interlocked.Decrement(ref this.disposed);
            }
        }
    }
}
