using CustomMVC.Core.DI;
using CustomMVC.Core.Http;
using CustomMVC.Core.Mvc;
using CustomMVC.Core.Mvc.Results;

namespace CustomMVC.Core.Routing
{
    public static class ActionInvoker
    {
        private static readonly Dictionary<Type, Func<string, object?>> Parsers = new Dictionary<Type, Func<string, object?>>()
        {
            { typeof(string), v => v },
            { typeof(int), v => int.TryParse(v, out var i) ? i : null },
            { typeof(int?), v => int.TryParse(v, out var i) ? (int?)i : null },
            { typeof(long), v => long.TryParse(v, out var l) ? l : null },
            { typeof(long?), v => long.TryParse(v, out var l) ? (long?)l : null },
            { typeof(bool), v => bool.TryParse(v, out var b) ? b : null },
            { typeof(bool?), v => bool.TryParse(v, out var b) ? (bool?)b : null },
        };

        private static readonly List<(Type key, Func<object, IActionResult> factory)> ResultMap = new()
        {
            (typeof(IActionResult), v => (IActionResult)v),
            (typeof(string),       v => new ContentResult((string)v)),
            (typeof(object),       v => new JsonResult(v)) // fallback
        };

        public static async Task InvokeAsync(HttpContext ctx, Endpoint ep, IDictionary<string, string> routeValues)
        {
            // Create controller
            var controller = (ControllerBase)ReflectionFactory.Get(ep.ControllerType);
            controller.HttpContext = ctx;

            var method = ep.Action;
            var paramInfos = method.GetParameters();
            var args = new object?[paramInfos.Length];

            for (int i = 0; i < paramInfos.Length; i++)
            {
                var p = paramInfos[i];

                args[i] =
                    routeValues.TryGetValue(p.Name!, out var fromRoute) ? ConvertSimple(fromRoute, p.ParameterType) :
                    ctx.Request.Query.TryGetValue(p.Name!, out var fromQuery) ? ConvertSimple(fromQuery, p.ParameterType) :
                    p.HasDefaultValue ? p.DefaultValue :
                    p.ParameterType.IsValueType ? ReflectionFactory.Get(p.ParameterType) :
                    null;
            }

            ctx.Items["__actionName"] = ep.Action.Name; //for find name of View

            // Invoke method of controller
            var result = method.Invoke(controller, args);

            var t = result?.GetType();

            var factory = ResultMap
                .OrderByDescending(e => e.key == t)                  
                .ThenByDescending(e => e.key.IsAssignableFrom(t))  
                .Select(e => e.factory)
                .First();

            await factory(result!).ExecuteAsync(ctx);

        }

        private static object? ConvertSimple(string value, Type t)
        {
            return Parsers.TryGetValue(t, out var parser)
            ? parser(value)
            : value; // fallback
        }
    }
}
