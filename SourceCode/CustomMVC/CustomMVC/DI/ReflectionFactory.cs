using System.Reflection;

namespace CustomMVC.DI
{
    // Purpose: Use reflection and recursion to create and resolve type instances automatically.
    // Supports interface-to-implementation mapping, factories, singletons, and transients.
    public enum ServiceLifetime { Singleton, Transient }

    public static class ReflectionFactory
    {
        private static readonly Dictionary<Type, object> _instances = new(); // Cached singleton instances
        private static readonly Dictionary<Type, Type> _implMap = new();      // Service -> Implementation type mapping
        private static readonly Dictionary<Type, Func<object>> _factories = new(); // Service -> Factory function
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

        public static void RegisterFactory<TService>(Func<object> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            lock (_lock)
            {
                _factories[typeof(TService)] = factory;
                _lifetimes[typeof(TService)] = lifetime;
                _instances.Remove(typeof(TService)); // Clear old instance
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
            lock (_lock)
            {
                // 1) Return cached singleton if exists
                if (_instances.TryGetValue(type, out var existing))
                    return existing;

                // 2) Otherwise create a new instance
                var created = Create(type);

                // 3) Cache instance if lifetime is Singleton
                var lifetime = GetLifetimeFor(type);
                if (lifetime == ServiceLifetime.Singleton)
                    _instances[type] = created;

                return created;
            }
        }

        public static T Create<T>() => (T)Create(typeof(T));

        private static object Create(Type type)
        {
            // 0) Use factory if registered
            if (_factories.TryGetValue(type, out var factory))
                return factory();

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

        // ====== Helpers ======
        private static ServiceLifetime GetLifetimeFor(Type serviceOrImpl)
        {
            if (serviceOrImpl is null) return ServiceLifetime.Singleton;

            // Check if lifetime registered for this service
            if (_lifetimes.TryGetValue(serviceOrImpl, out var lt))
                return lt;

            // Otherwise, check if the implementation belongs to a registered service
            var serviceKey = _implMap.FirstOrDefault(kv =>
                kv.Value == serviceOrImpl ||
                (kv.Key.IsGenericTypeDefinition && serviceOrImpl.IsGenericType &&
                 kv.Value.IsGenericTypeDefinition && serviceOrImpl.GetGenericTypeDefinition() == kv.Value)
            ).Key;

            return _lifetimes.TryGetValue(serviceKey, out var lt2) ? lt2 : ServiceLifetime.Singleton;
        }

        private static Type ResolveImplementationType(Type service)
        {
            // If interface/abstract, resolve to implementation
            if (service.IsInterface || service.IsAbstract)
            {
                // 1) Direct mapping
                if (_implMap.TryGetValue(service, out var impl))
                    return impl;

                // 2) Open generic mapping (e.g., IRepository<T> -> Repository<T>)
                if (service.IsGenericType)
                {
                    var def = service.GetGenericTypeDefinition();
                    if (_implMap.TryGetValue(def, out var openImpl))
                    {
                        if (!openImpl.IsGenericTypeDefinition)
                            throw new InvalidOperationException($"Implementation {openImpl} must be an open generic for {def}.");

                        return openImpl.MakeGenericType(service.GetGenericArguments());
                    }
                }

                throw new InvalidOperationException($"No implementation registered for service: {service}.");
            }

            // If it's a concrete type, return itself
            return service;
        }
    }
}