using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SuperMarioBros.Source.Services
{
    internal class TimerService : IDisposable
    {
        private static readonly Lazy<TimerService> _instance = new Lazy<TimerService>(() => new TimerService());
        private Dictionary<Guid, Timer> _timers = new Dictionary<Guid, Timer>();
        private bool _disposed;

        public static TimerService Instance => _instance.Value;

        private TimerService()
        {
            Console.WriteLine("TimerService initialized." + _instance);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            List<Guid> completedTimers = new List<Guid>();

            foreach (var timer in _timers.Values)
            {
                timer.RemainingTime -= deltaTime;
                if (timer.RemainingTime <= 0)
                {
                    timer.Callback?.Invoke();
                    completedTimers.Add(timer.Id);
                }
            }

            foreach (var timerId in completedTimers)
            {
                _timers.Remove(timerId);
            }
        }

        public Guid StartTimer(float duration, Action onTimerComplete)
        {
            Guid timerId = Guid.NewGuid();
            var timer = new Timer(timerId, duration, onTimerComplete);
            _timers[timerId] = timer;

            Console.WriteLine($"Timer {timerId} started for duration {duration} seconds.");

            return timerId;
        }

        public void StopTimer(Guid timerId)
        {
            if (_timers.Remove(timerId))
            {
                Console.WriteLine($"Timer {timerId} stopped.");
            }
            else
            {
                Console.WriteLine($"Timer {timerId} not found.");
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _timers.Clear();
                _disposed = true;
                Console.WriteLine("TimerService disposed." + _disposed);
            }
        }

        private class Timer
        {
            public Guid Id { get; }
            public float RemainingTime { get; set; }
            public Action Callback { get; }

            public Timer(Guid id, float duration, Action callback)
            {
                Id = id;
                RemainingTime = duration;
                Callback = callback;
            }
        }
    }
}
