using System;
using System.Timers;

namespace SuperMarioBros.Source.Services
{
    public static class TimerService
    {
        public static void SetTimeout(Action action, TimeSpan delay)
        {
            using Timer timer = new Timer(delay.TotalMilliseconds) { AutoReset = false };
            timer.Elapsed += (sender, e) =>
            {
                action();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
