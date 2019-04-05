using System;
using System.Timers;

namespace Npuzzle
{
    public class Timer
    {
        private static int Time { get; set; }
        private static System.Timers.Timer SyncTimer;

        public Timer()
        {
            Time = 0;
            SetSyncTimer();
            Console.WriteLine("Start search\n");
			Console.Write($"Waiting for result ({Time++})");
        }

        private void SetSyncTimer()
        {
            // Create a timer with a five second interval.
            SyncTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            SyncTimer.Elapsed += SynchronizeCache;
            SyncTimer.AutoReset = true;
            SyncTimer.Enabled = true;
        }

        private void SynchronizeCache(Object source, ElapsedEventArgs e)
        {
            if (Helper.Loading)
            {
                int currentLineCursor = Console.CursorTop;

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Waiting for result ({Time++})");
                Console.SetCursorPosition(0, currentLineCursor);
            }
        }
    }
}