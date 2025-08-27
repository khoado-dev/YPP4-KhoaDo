using System.Reflection;

namespace UnitTestForTrello.CustomDI
{
    // Purpose: Use reflection and recursion to create and resolve type instances automatically.
    // Supports interface-to-implementation mapping, factories, singletons, and transients.
    public enum ServiceLifetime { Singleton, Transient }

    public static class ReflectionFactory
    {
        private static readonly Dictionary<Type, object> _instances = new(); // Cached singleton instances
        private static readonly Dictionary<Type, Type> _implMap = new();      // Service -> Implementation type mapping
        private static readonly Dictionary<Type, ServiceLifetime> _lifetimes = new(); // Service -> Lifetime
        private static readonly object _lock = new();

        // ====== Registration API ======
        public static void Register<TService, TImpl>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TImpl : TService
            => Register(typeof(TService), typeof(TImpl), lifetime);

        public static void Register(Type service, Type implementation, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            lock (_lock)
            {
                _implMap[service] = implementation;
                _lifetimes[service] = lifetime;
                _instances.Remove(service); // Clear old instance if lifetime/impl changed
            }
        }

        public static void RegisterInstance<TService>(TService instance)
        {
            lock (_lock)
            {
                _lifetimes[typeof(TService)] = ServiceLifetime.Singleton;
                _instances[typeof(TService)] = instance!; // Save given instance
            }
        }

        // ====== Resolve ======
        public static T Get<T>() => (T)Get(typeof(T));

        public static object Get(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            lock (_lock)
            {
                // 1) Return cached singleton if exists
                if (_instances.TryGetValue(type, out var existing))
                    return existing;

                // 2) Otherwise create a new instance
                var created = Create(type);

                // 3) Cache instance if lifetime is Singleton
                var lifetime = _lifetimes.TryGetValue(type, out var lt) ? lt : ServiceLifetime.Singleton;
                if (lifetime == ServiceLifetime.Singleton)
                    _instances[type] = created; //cache intance with singleton lifetime

                return created;
            }
        }

        public static T Create<T>() => (T)Create(typeof(T));

        private static object Create(Type type)
        {
            // 1) Resolve service type to its concrete implementation
            var concrete = ResolveImplementationType(type);

            // 2) Select the constructor with the most parameters
            var ctor = concrete
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault()
                ?? throw new InvalidOperationException($"Type {concrete} doesn't have a public constructor.");

            var paramInfos = ctor.GetParameters();
            if (paramInfos.Length == 0)
                return Activator.CreateInstance(concrete)!; // Simple new instance

            // 3) Recursively resolve constructor parameters
            var args = paramInfos.Select(p => Get(p.ParameterType)).ToArray();
            return Activator.CreateInstance(concrete, args)!;
        }


        private static Type ResolveImplementationType(Type service)
        {
            return _implMap.TryGetValue(service, out var impl) ? impl : service;
        }
    }
}