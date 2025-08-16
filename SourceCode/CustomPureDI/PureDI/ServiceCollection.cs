using System.Collections.ObjectModel;
namespace PureDI
{
    // Stores and adds service metadata into the collection.
    public class ServiceCollection : Collection<ServiceDescriptor>, IServiceCollection
    {
        public ServiceCollection AddTransient<TService, TImpl>() where TImpl : TService
        { 
            Add(ServiceDescriptor.Transient<TService, TImpl>()); 
            return this; 
        }

        public ServiceCollection AddScoped<TService, TImpl>() where TImpl : TService
        { 
            Add(ServiceDescriptor.Scoped<TService, TImpl>()); 
            return this; 
        }

        public ServiceCollection AddSingleton<TService, TImpl>() where TImpl : TService
        { 
            Add(ServiceDescriptor.Singleton<TService, TImpl>()); 
            return this; 
        }

        public ServiceCollection AddTransient<TService>(Func<IServiceProvider, TService> factory)
        { 
            Add(ServiceDescriptor.Transient(factory)); 
            return this; 
        }

        public ServiceCollection AddScoped<TService>(Func<IServiceProvider, TService> factory)
        { 
            Add(ServiceDescriptor.Scoped(factory)); 
            return this; 
        }

        public ServiceCollection AddSingleton<TService>(TService instance)
        { 
            Add(ServiceDescriptor.Singleton(instance)); 
            return this; 
        }
    }
}
