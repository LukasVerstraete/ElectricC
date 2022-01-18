using System;
using System.Diagnostics;

namespace ElectricC.Core
{
    public class FrameTimer
    {
        public static FrameTimer Timer = new FrameTimer();

        private long lastFrameTicks = 0;
        private long currentFrameTicks = 0;
        private Stopwatch stopwatch;

        private FrameTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void EndFrame()
        {
            lastFrameTicks = currentFrameTicks;
            currentFrameTicks = stopwatch.ElapsedTicks;
            
        }

        public float DeltaTime()
        {
            return (currentFrameTicks - lastFrameTicks) / (float)TimeSpan.TicksPerSecond;
        }
    }
}
