namespace UnitTestForTrello.Tests.Utility
{
    public interface ICustomCache
    {
        bool TryGetValue<T>(string key, out T? value);
        void Set<T>(string key, T value, TimeSpan ttl);
        void Remove(string key);
    }

}
