using System.Reflection;

namespace CustomMVC.Core.Routing
{
    public sealed record Endpoint(Type ControllerType, MethodInfo Action);
}
