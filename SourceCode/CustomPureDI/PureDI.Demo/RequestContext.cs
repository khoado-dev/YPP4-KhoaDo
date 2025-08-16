namespace PureDI.Demo
{
    public class RequestContext : IRequestContext
    {
        public Guid CorrelationId { get; } = Guid.NewGuid();
    }
}
