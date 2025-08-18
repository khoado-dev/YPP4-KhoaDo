namespace PureDI
{
    // Minimal service collection interface: stores metadata + registration methods
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        // Transient
        ServiceCollection AddTransient<TService, TImpl>() where TImpl : TService;
        ServiceCollection AddTransient<TImpl>() where TImpl : class;
        ServiceCollection AddTransient<TService>(Func<IServiceProvider, TService> factory);

        // Scoped
        ServiceCollection AddScoped<TService, TImpl>() where TImpl : TService;
        ServiceCollection AddScoped<TService>(Func<IServiceProvider, TService> factory);

        // Singleton
        ServiceCollection AddSingleton<TService, TImpl>() where TImpl : TService;
        ServiceCollection AddSingleton<TService>(TService instance);
    }
}
