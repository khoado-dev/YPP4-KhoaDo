using System.Collections.Concurrent;
using System.Reflection;

namespace PureDI
{
    // Minimal DI container supporting Singleton/Scoped/Transient + constructor injection.
    public sealed class ServiceProvider : IServiceProvider, IServiceScopeFactory, IDisposable, IAsyncDisposable
    {
        private readonly IReadOnlyList<ServiceDescriptor> _descriptors;
        private readonly ConcurrentDictionary<Type, object> _singletons = new();      // root singletons
        private readonly HashSet<Type> _callStackGuard = new();                       // circular dep guard
        private bool _disposed;                                                       // disposed flag 

        // Root provider
        public ServiceProvider(IEnumerable<ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors.ToList();
            // Pre-store instance singletons
            foreach (var d in _descriptors.Where(d => d.ImplementationInstance is not null))
                //if the type already has instance, store it in the singletons dictionary.
                _singletons[d.ServiceType] = d.ImplementationInstance!;
        }

        public object? GetService(Type serviceType)
        {
            ThrowIfDisposed();

            var descriptor = _descriptors.LastOrDefault(d => d.ServiceType == serviceType);
            if (descriptor is null)
            {
                // Support resolving non-registered concrete types (optional).
                if (!serviceType.IsAbstract && !serviceType.IsInterface)
                    return CreateByType(serviceType, scopeCache: null);

                return null; // not found
            }

            return descriptor.Lifetime switch
            {
                ServiceLifetime.Singleton => ResolveSingleton(descriptor),
                ServiceLifetime.Scoped => throw new InvalidOperationException(
                     "Scoped service requested from root provider. Use a scope: CreateScope()."),
                ServiceLifetime.Transient => CreateInstance(descriptor, scopeCache: null),
                _ => null
            };
        }

        // IServiceScopeFactory
        public IServiceScope CreateScope() => new ServiceScope(this, _descriptors);

        private object ResolveSingleton(ServiceDescriptor d)
        {
            return _singletons.GetOrAdd(d.ServiceType, _ => CreateInstance(d, scopeCache: null));
        }

        internal object ResolveInScope(ServiceDescriptor d, Dictionary<Type, object> scopeCache)
        {
            return d.Lifetime switch
            {
                ServiceLifetime.Singleton => _singletons.GetOrAdd(d.ServiceType, _ => CreateInstance(d, scopeCache)),
                ServiceLifetime.Scoped => scopeCache.TryGetValue(d.ServiceType, out var existing)
                                                ? existing
                                                : (scopeCache[d.ServiceType] = CreateInstance(d, scopeCache)),
                ServiceLifetime.Transient => CreateInstance(d, scopeCache),
                _ => throw new NotSupportedException()
            };
        }

        private object CreateInstance(ServiceDescriptor d, Dictionary<Type, object>? scopeCache)
        {
            if (d.ImplementationInstance is not null) return d.ImplementationInstance!;
            if (d.ImplementationFactory is not null) return d.ImplementationFactory(SelectProvider(scopeCache));

            if (d.ImplementationType is null)
                throw new InvalidOperationException($"Descriptor for {d.ServiceType} is incomplete.");

            return CreateByType(d.ImplementationType, scopeCache);
        }

        private object CreateByType(Type implType, Dictionary<Type, object>? scopeCache)
        {
            // Choose the “greediest” public constructor.
            var ctor = implType
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

            if (ctor is null)
                throw new InvalidOperationException($"Type {implType} has no public constructor.");

            // Circular dependency guard
            if (!_callStackGuard.Add(implType))
                throw new InvalidOperationException($"Circular dependency detected around {implType}.");

            try
            {
                var args = ctor.GetParameters()
                               .Select(p => ResolveParameter(p.ParameterType, scopeCache))
                               .ToArray();
                var instance = Activator.CreateInstance(implType, args)!;
                return instance;
            }
            finally
            {
                _callStackGuard.Remove(implType);
            }
        }

        private object ResolveParameter(Type parameterType, Dictionary<Type, object>? scopeCache)
        {
            // Find matching descriptor (last registration wins)
            var d = _descriptors.LastOrDefault(x => x.ServiceType == parameterType);

            if (d is null)
            {
                // Allow resolving concrete types that aren't registered
                if (!parameterType.IsAbstract && !parameterType.IsInterface)
                    return CreateByType(parameterType, scopeCache);

                throw new InvalidOperationException(
                    $"No service registered for type {parameterType}.");
            }

            if (scopeCache is null)
            {
                // From root
                return d.Lifetime == ServiceLifetime.Scoped
                    ? throw new InvalidOperationException($"Scoped service {d.ServiceType} can't be resolved from root.")
                    : ResolveInScope(d, new Dictionary<Type, object>()); // transient or singleton ok
            }

            return ResolveInScope(d, scopeCache);
        }

        private IServiceProvider SelectProvider(Dictionary<Type, object>? scopeCache)
            => scopeCache is null ? this : new ScopedServiceProvider(this, scopeCache);

        public void Dispose() => _disposed = true;
        public ValueTask DisposeAsync() { _disposed = true; return ValueTask.CompletedTask; }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ServiceProvider));
        }

        // Internal helper provider that resolves within a given scope cache
        private sealed class ScopedServiceProvider : IServiceProvider
        {
            private readonly ServiceProvider _root;
            private readonly Dictionary<Type, object> _scopeCache;

            public ScopedServiceProvider(ServiceProvider root, Dictionary<Type, object> scopeCache)
            {
                _root = root; _scopeCache = scopeCache;
            }

            public object? GetService(Type serviceType)
            {
                var d = _root._descriptors.LastOrDefault(x => x.ServiceType == serviceType);
                if (d is null)
                {
                    if (!serviceType.IsAbstract && !serviceType.IsInterface)
                        return _root.CreateByType(serviceType, _scopeCache);
                    return null;
                }
                return _root.ResolveInScope(d, _scopeCache);
            }
        }

        // Concrete scope type
        private sealed class ServiceScope : IServiceScope
        {
            private readonly ServiceProvider _root;
            private readonly Dictionary<Type, object> _scopedCache = new();
            private bool _disposed;

            public ServiceScope(ServiceProvider root, IReadOnlyList<ServiceDescriptor> _)
            { _root = root; ServiceProvider = new ScopedServiceProvider(root, _scopedCache); }

            public IServiceProvider ServiceProvider { get; }

            public void Dispose()
            {
                if (_disposed) return;
                // Dispose scoped instances if IDisposable/IAsyncDisposable
                foreach (var obj in _scopedCache.Values)
                {
                    if (obj is IAsyncDisposable ad) ad.DisposeAsync().AsTask().GetAwaiter().GetResult();
                    if (obj is IDisposable d) d.Dispose();
                }
                _disposed = true;
            }

            public ValueTask DisposeAsync()
            {
                if (_disposed) return ValueTask.CompletedTask;
                foreach (var obj in _scopedCache.Values)
                {
                    if (obj is IAsyncDisposable ad) ad.DisposeAsync().AsTask().GetAwaiter().GetResult();
                    if (obj is IDisposable d) d.Dispose();
                }
                _disposed = true;
                return ValueTask.CompletedTask;
            }
        }
    }
}
