using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SuperMarioBros.Source.Services
{
    internal class TimerService : IDisposable
    {
        private static readonly Lazy<TimerService> _instance = new Lazy<TimerService>(() => new TimerService());
        private ConcurrentDictionary<Guid, CancellationTokenSource> _timers = new ConcurrentDictionary<Guid, CancellationTokenSource>();
        private bool _disposed;

        public static TimerService Instance => _instance.Value;

        private TimerService()
        {
            Console.WriteLine("TimerService initialized." + _instance.ToString());
        }

        public Guid StartTimer(float duration, Action onTimerComplete)
        {
            Guid timerId = Guid.NewGuid();
            var cts = new CancellationTokenSource();
            _timers[timerId] = cts;

            Console.WriteLine($"Timer {timerId} started for duration {duration} seconds.");

            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(duration), cts.Token).ConfigureAwait(false);
                    if (!cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Timer {timerId} completed.");
                        onTimerComplete?.Invoke();
                    }
                    else
                    {
                        Console.WriteLine($"Timer {timerId} was cancelled.");
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine($"Timer {timerId} was cancelled due to TaskCanceledException.");
                }
                finally
                {
                    _timers.TryRemove(timerId, out _);
                    cts.Dispose();
                }
            }, cts.Token).ConfigureAwait(false);

            return timerId;
        }

        public void StopTimer(Guid timerId)
        {
            if (_timers.TryRemove(timerId, out var cts))
            {
                Console.WriteLine($"Timer {timerId} stopped.");
                cts.Cancel();
                cts.Dispose();
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
                foreach (var cts in _timers.Values)
                {
                    cts.Cancel();
                    cts.Dispose();
                }
                _timers.Clear();
                _disposed = true;
                Console.WriteLine("TimerService disposed." + _disposed);
            }
        }
    }
}
