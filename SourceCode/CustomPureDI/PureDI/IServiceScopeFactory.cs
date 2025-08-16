namespace PureDI
{
    // Factory to create scopes.
    public interface IServiceScopeFactory
    {
        IServiceScope CreateScope();
    }
}
