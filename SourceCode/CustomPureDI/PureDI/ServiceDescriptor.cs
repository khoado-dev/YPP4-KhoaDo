namespace PureDI
{
    // Create metadata about the service registration. Describe the way to create service instance.
    public sealed class ServiceDescriptor
    {
        //Type of service being registered. Such as an interface or a abstract class.
        public Type ServiceType { get; }
        //The type of the implementation.
        public Type? ImplementationType { get; }

        //The factory function to create the service instance.
        public Func<IServiceProvider, object>? ImplementationFactory { get; }

        // The instance of the service if it is already created
        public object? ImplementationInstance { get; }

        // The lifetime of the service registration.
        public ServiceLifetime Lifetime { get; }

        // Implementation by type
        private ServiceDescriptor(Type serviceType, Type implType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            ImplementationType = implType;
            Lifetime = lifetime;
        }

        // Implementation by factory
        private ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            ImplementationFactory = factory;
            Lifetime = lifetime;
        }

        // Implementation by existing instance (Singleton style)
        private ServiceDescriptor(Type serviceType, object instance)
        {
            ServiceType = serviceType;
            ImplementationInstance = instance;
            Lifetime = ServiceLifetime.Singleton;
        }

        public static ServiceDescriptor Transient<TService, TImpl>() where TImpl : TService
        {
            // Create a instance of ServiceDescriptor for transient service registration.
            var serviceDescriptor = new ServiceDescriptor(typeof(TService), typeof(TImpl), ServiceLifetime.Transient);
            return serviceDescriptor;
        }

        public static ServiceDescriptor Scoped<TService, TImpl>() where TImpl : TService
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(TService), typeof(TImpl), ServiceLifetime.Scoped);
            return serviceDescriptor;
        }

        public static ServiceDescriptor Singleton<TService, TImpl>() where TImpl : TService
            => new(typeof(TService), typeof(TImpl), ServiceLifetime.Singleton);

        // IServiceProvider provide the service instance stored in IServiceCollection.
        public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> factory)
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(TService), sp => factory(sp)!, ServiceLifetime.Transient);
            return serviceDescriptor;
        }
            //=> new(typeof(TService), sp => factory(sp)!, ServiceLifetime.Transient);

        public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService> factory)
            => new(typeof(TService), sp => factory(sp)!, ServiceLifetime.Scoped);

        public static ServiceDescriptor Singleton<TService>(TService instance)
            => new(typeof(TService), instance!);
    }
}
