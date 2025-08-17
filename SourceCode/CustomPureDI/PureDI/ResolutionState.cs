namespace PureDI
{
    public sealed class ResolutionState
    {
        public HashSet<Type> Constructing { get; } = new();
        public Stack<Type> Trace { get; } = new();
    }
}
