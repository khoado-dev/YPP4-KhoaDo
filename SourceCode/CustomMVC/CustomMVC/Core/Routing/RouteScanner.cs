using System.Reflection;

namespace CustomMVC.Core.Routing
{
    public static class RouteScanner
    {
        public static void Build(RouteTable router, params Assembly[] assemblies)
        {
            // Choose target assemblies (fallback to executing assembly)
            var targetAssemblies = (assemblies != null && assemblies.Length > 0)
                ? assemblies
                : new[] { Assembly.GetExecutingAssembly() };

            // Build all route entries via LINQ (no explicit for/foreach)
            var entries =
                targetAssemblies
                // 1) Flatten all types across assemblies
                .SelectMany(a => a.GetTypes())
                // 2) Keep only controller types
                .Where(IsController)
                // 3) Project controller metadata
                .Select(t => new
                {
                    Type = t,
                    ControllerToken = TrimControllerSuffix(t.Name).ToLowerInvariant(),
                    // Class-level prefixes; default to "{controller}" when none present
                    ClassPrefixes = t.GetCustomAttributes<RouteAttribute>(true)
                                     .Select(a => a.Template)
                                     .DefaultIfEmpty("{controller}")
                })
                // 4) Expand to public instance methods
                .SelectMany(x =>
                    x.Type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                          .Select(m => new
                          {
                              x.Type,
                              x.ControllerToken,
                              x.ClassPrefixes,
                              MethodInfo = m,
                              HttpAttrs = m.GetCustomAttributes<HttpMethodAttribute>(true),
                              MethodRoutes = m.GetCustomAttributes<RouteAttribute>(true)
                                              .Select(a => a.Template)
                          })
                )
                // 5) Only methods annotated with any Http* attribute
                .Where(mi => mi.HttpAttrs.Any())
                // 6) For each Http* attribute, combine class-prefix + method template(s)
                .SelectMany(mi =>
                    mi.HttpAttrs.SelectMany(httpAttr =>
                        // Use method-level routes if any; otherwise fall back to the Http* template
                        (mi.MethodRoutes.Any() ? mi.MethodRoutes : new[] { httpAttr.Template })
                        // Cross join with all class-level prefixes, then normalize and replace tokens
                        .SelectMany(methodTemplate =>
                            mi.ClassPrefixes
                              .Select(prefix => Combine(prefix, methodTemplate))
                              .Select(full => ReplaceTokens(full, mi.ControllerToken))
                              .Select(full => new
                              {
                                  httpAttr.Method,
                                  Path = full,
                                  mi.Type,
                                  MethodName = mi.MethodInfo.Name
                              })
                        )
                    )
                );

            // 7) Materialize and register with router (no explicit foreach; side-effect inside Select)
            _ = entries
                .Select(e => { router.Map(e.Method, e.Path, e.Type, e.MethodName); return 0; })
                .ToList();
        }

        // A controller is a non-abstract class whose name ends with "Controller"
        private static bool IsController(Type t)
            => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Controller", StringComparison.Ordinal);

        // Trim "Controller" suffix: "UsersController" -> "Users"
        private static string TrimControllerSuffix(string name)
            => name.EndsWith("Controller", StringComparison.Ordinal)
                ? name[..^"Controller".Length] : name;

        // Join two route fragments and normalize the result
        private static string Combine(string a, string b)
        {
            var left = a?.Trim() ?? "";
            var right = b?.Trim() ?? "";
            if (left.Length == 0) return Normalize(right);
            if (right.Length == 0) return Normalize(left);
            return Normalize($"{left}/{right}");
        }

        // Ensure single leading slash and collapse duplicate slashes
        private static string Normalize(string path)
        {
            path = path.Replace("//", "/");
            if (!path.StartsWith("/")) path = "/" + path;
            return path;
        }

        // Replace token {controller} with the actual controller token
        private static string ReplaceTokens(string template, string controllerToken)
            => template.Replace("{controller}", controllerToken, StringComparison.OrdinalIgnoreCase);
    }
}
