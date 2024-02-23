using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct HL_BaseGlobalVars
{
    public void Construct(HL_EngineInfo _EngineInfo)
    {
        FrameTime = 0;
        CurrentTime = 0;
        TickCount = 0;
        SystemDelta = 0;
        TotalFrameTime = 0;
        AvgFrameRate = 0;
        FrameCount = 0;
        NextSleep = 0;
        EngineInfo = _EngineInfo;
        IntervalPerTick = (1000.0 / (double)EngineInfo.TickRate);
    }

    public double FrameTime;
    public double TotalFrameTime;
    public double LastSleepDelta;
    public double SystemDelta;
    public int FrameRate;
    public int AvgFrameRate;
    public double CurrentTime;
    public int FrameCount;
    public double IntervalPerTick;
    public int TickCount;
    public int NextSleep;
    public HL_EngineInfo EngineInfo;
}

namespace TextBasedGameEngine.Engine
{
    public class HL_GlobalVarsManager
    {
        public long DateTickBegin = 0;

        private Stopwatch StopWatch;

        public HL_BaseGlobalVars GVars;
       
        private List<int> AverageFrameRate;
        public void Initialize(HL_EngineInfo InitInfo)
        {
            AverageFrameRate = new List<int>();
            StopWatch = new Stopwatch();
            DateTickBegin = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; 
            GVars.Construct(InitInfo);
        }
        public void OnFrameStart()
        {
            StopWatch.Stop();
            GVars.SystemDelta = StopWatch.Elapsed.TotalMilliseconds;
            StopWatch.Reset();

            StopWatch.Start();

            GVars.NextSleep = 0; 

            GVars.CurrentTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - DateTickBegin;
        }

        public void OnTick()
        {
            GVars.TickCount++;
        }
        public void OnFrameEnd()
        {
            StopWatch.Stop();

            GVars.FrameTime = StopWatch.Elapsed.TotalMilliseconds;
            GVars.TotalFrameTime = GVars.FrameTime + GVars.SystemDelta + GVars.LastSleepDelta;
            GVars.FrameRate = (int)(1.0 / GVars.TotalFrameTime);

            if (AverageFrameRate.Count > 100)
            {
                AverageFrameRate.RemoveAt(0);
            }

            AverageFrameRate.Add(GVars.FrameRate);

            int TotalFrameRateForAverage = 0;

            foreach (var frame in AverageFrameRate)
                TotalFrameRateForAverage += frame;

            GVars.AvgFrameRate = (int)((double)TotalFrameRateForAverage / (double)AverageFrameRate.Count);

            if (GVars.FrameRate > GVars.EngineInfo.MaxFrameRate)
            {
                double TargetFrameTime = 1.0 / GVars.EngineInfo.MaxFrameRate;
                int ResolvedSleep = (int)Math.Floor(TargetFrameTime * 1000.0 - (GVars.TotalFrameTime * 1000.0));
                 
                if (ResolvedSleep > 0)
                    GVars.NextSleep = ResolvedSleep;
            }

            GVars.FrameCount++;

            GVars.LastSleepDelta = 0;

            if (GVars.NextSleep > 0)
            {
                StopWatch.Reset();
                StopWatch.Start();
                System.Threading.Thread.Sleep(GVars.NextSleep);
                StopWatch.Stop();
                GVars.LastSleepDelta = GVars.NextSleep - StopWatch.ElapsedMilliseconds;
            }

            StopWatch.Reset();
            StopWatch.Start();
        }
    }
}
