using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

namespace PureDI
{
    // Minimal DI container: Singleton / Scoped / Transient + constructor injection
    public sealed class ServiceProvider : IServiceProvider, IServiceScopeFactory, IDisposable, IAsyncDisposable
    {
        private readonly IReadOnlyList<ServiceDescriptor> _descriptors;
        private readonly ConcurrentDictionary<Type, object> _singletons = new(); // root singletons
        private static readonly AsyncLocal<ResolutionState?> _state = new(); // per-flow resolution state (an toàn khi chạy song song)
        private bool _disposed;

        private static ResolutionState S => _state.Value ??= new ResolutionState();

        public ServiceProvider(IEnumerable<ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors.ToList();

            // preload instance singletons
            foreach (var d in _descriptors.Where(d => d.ImplementationInstance is not null))
                _singletons[d.ServiceType] = d.ImplementationInstance!;
        }

        // ===== IServiceProvider =====
        public object? GetService(Type serviceType)
        {
            ThrowIfDisposed();

            var descriptor = FindDescriptorFor(serviceType);
            if (descriptor is null)
            {
                // self-binding for concretes
                if (!serviceType.IsAbstract && !serviceType.IsInterface)
                    return CreateByType(serviceType, scopeCache: null);

                return null;
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

        // ===== IServiceScopeFactory =====
        public IServiceScope CreateScope() => new ServiceScope(this);

        // ---------- descriptor lookup (supports assignable) ----------
        private ServiceDescriptor? FindDescriptorFor(Type serviceType)
        {
            // exact
            var d = _descriptors.LastOrDefault(x => x.ServiceType == serviceType);
            if (d is not null) return d;

            // implementation assignable to requested service
            d = _descriptors.LastOrDefault(x =>
                x.ImplementationType is not null && serviceType.IsAssignableFrom(x.ImplementationType));
            if (d is not null) return d;

            // instance compatible with requested service
            d = _descriptors.LastOrDefault(x =>
                x.ImplementationInstance is not null && serviceType.IsInstanceOfType(x.ImplementationInstance));
            if (d is not null) return d;

            // broader service type
            //d = _descriptors.LastOrDefault(x => serviceType.IsAssignableFrom(x.ServiceType));
            return d;
        }

        // ---------- resolution core ----------
        private object ResolveSingleton(ServiceDescriptor d)
            => _singletons.GetOrAdd(d.ServiceType, _ => CreateInstance(d, scopeCache: null));

        internal object ResolveInScope(ServiceDescriptor d, Dictionary<Type, object> scopeCache)
        {
            return d.Lifetime switch
            {
                ServiceLifetime.Singleton => ResolveSingleton(d), // always from root
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
            // circular guard per-flow
            if (!S.Constructing.Add(implType))
            {
                var path = S.Trace.Count > 0
                    ? string.Join(" -> ", S.Trace.Reverse().Select(t => t.FullName)) + " -> " + implType.FullName
                    : implType.FullName!;
                throw new InvalidOperationException($"Circular dependency detected: {path}");
            }
            S.Trace.Push(implType);

            try
            {
                var ctors = implType
                    .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .OrderByDescending(c => c.GetParameters().Length)
                    .ToArray();

                if (ctors.Length == 0)
                    throw new InvalidOperationException($"Type {implType} has no public constructor.");

                // thử lần lượt các ctor (greedy -> less greedy), nhưng bỏ qua ctor "tự kiểu mình"
                foreach (var ctor in ctors)
                {
                    var ps = ctor.GetParameters();

                    // tránh self-injection qua chính kiểu mình
                    if (ps.Any(p => p.ParameterType.IsAssignableFrom(implType)))
                        continue;

                    var args = new object?[ps.Length];
                    var ok = true;

                    for (int i = 0; i < ps.Length; i++)
                    {
                        try
                        {
                            args[i] = ResolveParameter(ps[i].ParameterType, scopeCache);
                        }
                        catch
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok) return ctor.Invoke(args!);
                }

                throw new InvalidOperationException($"No satisfiable public constructor found for {implType}.");
            }
            finally
            {
                S.Trace.Pop();
                S.Constructing.Remove(implType);
            }
        }


        private object ResolveParameter(Type parameterType, Dictionary<Type, object>? scopeCache)
        {
            // chặn self-injection (kiểu hiện tại yêu cầu chính nó)
            if (S.Trace.Count > 0)
            {
                var current = S.Trace.Peek();
                if (parameterType.IsAssignableFrom(current))
                    throw new InvalidOperationException($"Self injection: {current} requires {parameterType}");
            }

            // cho phép inject IServiceProvider
            if (parameterType == typeof(IServiceProvider))
                return SelectProvider(scopeCache);

            var d = FindDescriptorFor(parameterType);

            if (d is null)
            {
                // self-bind concrete
                if (!parameterType.IsAbstract && !parameterType.IsInterface)
                    return CreateByType(parameterType, scopeCache);

                throw new InvalidOperationException($"No service registered for type {parameterType}.");
            }

            if (scopeCache is null)
            {
                // resolving at root
                return d.Lifetime switch
                {
                    ServiceLifetime.Singleton => ResolveSingleton(d),
                    ServiceLifetime.Transient => CreateInstance(d, null),
                    ServiceLifetime.Scoped => throw new InvalidOperationException(
                        $"Scoped service {d.ServiceType} can't be resolved from root."),
                    _ => throw new NotSupportedException()
                };
            }

            return ResolveInScope(d, scopeCache);
        }

        private IServiceProvider SelectProvider(Dictionary<Type, object>? scopeCache)
            => scopeCache is null ? this : new ScopedServiceProvider(this, scopeCache);

        // ---------- disposal ----------
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            foreach (var obj in _singletons.Values)
            {
                try
                {
                    if (obj is IAsyncDisposable ad) ad.DisposeAsync().AsTask().GetAwaiter().GetResult();
                    if (obj is IDisposable d) d.Dispose();
                }
                catch { /* ignore on dispose */ }
            }
            _singletons.Clear();
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ServiceProvider));
        }

        // ---------- nested providers/scopes ----------
        private sealed class ScopedServiceProvider : IServiceProvider
        {
            private readonly ServiceProvider _root;
            internal readonly Dictionary<Type, object> _scopeCache;

            public ScopedServiceProvider(ServiceProvider root, Dictionary<Type, object> scopeCache)
            {
                _root = root; _scopeCache = scopeCache;
            }

            public object? GetService(Type serviceType)
            {
                _root.ThrowIfDisposed();

                var d = _root.FindDescriptorFor(serviceType);
                if (d is null)
                {
                    if (!serviceType.IsAbstract && !serviceType.IsInterface)
                        return _root.CreateByType(serviceType, _scopeCache);
                    return null;
                }

                return _root.ResolveInScope(d, _scopeCache);
            }
        }

        private sealed class ServiceScope : IServiceScope
        {
            private readonly ServiceProvider _root;
            private bool _disposed;
            private readonly Dictionary<Type, object> _scopedCache = new();

            public ServiceScope(ServiceProvider root)
            {
                _root = root;
                ServiceProvider = new ScopedServiceProvider(root, _scopedCache);
            }

            public IServiceProvider ServiceProvider { get; }

            public void Dispose()
            {
                if (_disposed) return;

                foreach (var obj in _scopedCache.Values)
                {
                    try
                    {
                        if (obj is IAsyncDisposable ad) ad.DisposeAsync().AsTask().GetAwaiter().GetResult();
                        if (obj is IDisposable d) d.Dispose();
                    }
                    catch { /* ignore */ }
                }

                _scopedCache.Clear();
                _disposed = true;
            }

            public ValueTask DisposeAsync()
            {
                Dispose();
                return ValueTask.CompletedTask;
            }
        }
    }
}
