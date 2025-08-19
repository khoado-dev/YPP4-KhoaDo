using System.Collections.Concurrent;

namespace UnitTestForTrello.Tests.Utility
{
    public class CustomCache : ICustomCache, IDisposable
    {
        private readonly ConcurrentDictionary<string, Entry> _store = new(); // thread-safe store
        private readonly Timer _sweeper;
        private readonly TimeSpan _sweepInterval;

        private sealed class Entry
        {
            public object Value { get; init; } = default!;
            public Type Type { get; init; } = default!;
            public DateTimeOffset ExpiresAt { get; init; }
        }

        public CustomCache(TimeSpan? sweepInterval = null)
        {
            int defaultSweepMinutes = 1;
            _sweepInterval = sweepInterval ?? TimeSpan.FromMinutes(defaultSweepMinutes);
            _sweeper = new Timer(Sweep, null, _sweepInterval, _sweepInterval);
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            value = default;
            if (!_store.TryGetValue(key, out var entry))
                return false;

            if (DateTimeOffset.UtcNow >= entry.ExpiresAt)
            {
                // expired → remove and miss
                _store.TryRemove(key, out _);
                return false;
            }

            // type-safe cast
            if (entry.Type != typeof(T))
                return false;

            value = (T)entry.Value;
            return true;
        }

        public void Set<T>(string key, T value, TimeSpan ttl)
        {
            var e = new Entry
            {
                Value = value!,
                Type = typeof(T),
                ExpiresAt = DateTimeOffset.UtcNow.Add(ttl)
            };
            _store[key] = e; // upsert
        }

        public void Remove(string key) => _store.TryRemove(key, out _);
        private void Sweep(object? state)
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var kvp in _store)
            {
                if (now >= kvp.Value.ExpiresAt)
                {
                    _store.TryRemove(kvp.Key, out _);
                }
            }
        }

        public void Dispose() => _sweeper.Dispose();
    }
}
