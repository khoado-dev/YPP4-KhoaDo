namespace PureDI
{
    // Represents a scope for Scoped services. Define scoppe 
    public interface IServiceScope : IDisposable, IAsyncDisposable
    {
        IServiceProvider ServiceProvider { get; }
    }
}
