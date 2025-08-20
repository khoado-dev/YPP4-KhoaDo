using System.Reflection;

namespace UnitTestForTrello.CustomDI
{
    // Purpose: Use reflection and recursion to create instances of types, resolving dependencies automatically.
    public static class ReflectionFactory
    {
        private static readonly Dictionary<Type, object> _instances = new();

        private static readonly object _lock = new();

        public static T Get<T>() => (T)Get(typeof(T));

        private static object Get(Type type)
        {
            // if has instance then return it
            lock (_lock)
            {
                if (_instances.TryGetValue(type, out var existing))
                    return existing;

                // otherwise, create a new instance
                var created = Create(type);
                _instances[type] = created;
                return created;
            }
        }

        public static T Create<T>() => (T)Create(typeof(T));

        private static object Create(Type type) // type = BoardController
        {
            // Get constructor has most parameter to inject dependencies
            var ctor = type
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault(); //top1 descending

            if (ctor == null)
                throw new InvalidOperationException($"Type {type} doesn't have public constructor.");

            //have contructor
            var paramInfos = ctor.GetParameters();
            if (paramInfos.Length == 0) //ctor no parameters
                return Activator.CreateInstance(type)!; // new BoardController

            // recurse to create instances for each parameter type
            var args = paramInfos
                .Select(p => Create(p.ParameterType)) //create instance for each parameter type
                .ToArray();

            return Activator.CreateInstance(type, args)!; // create instance with the parameters and return it
        }
    }
}