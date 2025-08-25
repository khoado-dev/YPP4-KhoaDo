using System.Reflection;
using CustomMVC.Core.Http;
using CustomMVC.Mvc;
using CustomMVC.Mvc.Results;

namespace CustomMVC.Core.Routing
{
    public static class ActionInvoker
    {
        public static async Task InvokeAsync(HttpContext ctx, Endpoint ep, IDictionary<string, string> routeValues)
        {
            // Create controller
            var controller = (ControllerBase)Activator.CreateInstance(ep.ControllerType)!;
            controller.HttpContext = ctx;

            var method = ep.Action;
            var paramInfos = method.GetParameters();
            var args = new object?[paramInfos.Length];

            // Bind params from route -> query
            for (int i = 0; i < paramInfos.Length; i++)
            {
                var p = paramInfos[i];
                if (routeValues.TryGetValue(p.Name!, out var fromRoute))
                {
                    args[i] = ConvertSimple(fromRoute, p.ParameterType);
                    continue;
                }

                if (ctx.Request.Query.TryGetValue(p.Name!, out var fromQuery))
                {
                    args[i] = ConvertSimple(fromQuery, p.ParameterType);
                    continue;
                }

                // default
                args[i] = p.HasDefaultValue
                    ? p.DefaultValue
                    : (p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null);
            }

            // Invoke
            var result = method.Invoke(controller, args);

            // Normalize output
            if (result is IActionResult ar)
                await ar.ExecuteAsync(ctx);
            else if (result is string s)
                await new ContentResult(s).ExecuteAsync(ctx);
            else
                await new JsonResult(result!).ExecuteAsync(ctx);
        }

        private static object? ConvertSimple(string value, Type t)
        {
            if (t == typeof(string)) return value;
            if (t == typeof(int) || t == typeof(int?)) return int.TryParse(value, out var i) ? i : null;
            if (t == typeof(long) || t == typeof(long?)) return long.TryParse(value, out var l) ? l : null;
            if (t == typeof(bool) || t == typeof(bool?)) return bool.TryParse(value, out var b) ? b : null;
            return value; // fallback
        }
    }
}
