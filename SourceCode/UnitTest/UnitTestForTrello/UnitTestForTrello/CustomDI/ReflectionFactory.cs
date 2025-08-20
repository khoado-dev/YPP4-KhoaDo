using System.Reflection;

namespace UnitTestForTrello.CustomDI
{
    // Purpose: Use reflection and recursion to create instances of types, resolving dependencies automatically.
    public static class ReflectionFactory
    {
        public static T Create<T>() => (T)Create(typeof(T));

        public static object Create(Type type) // type = BoardController
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