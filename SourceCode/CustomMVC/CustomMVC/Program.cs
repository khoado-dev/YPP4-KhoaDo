using CustomMVC.Core.Http;
using CustomMVC.Core.Routing;
using HttpMethod = CustomMVC.Core.Http.HttpMethod;

namespace CustomMVC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 1) Initialize router and register routes
            var router = new RouteTable();

            // Simple route: GET /hello → returns "Hello"
            router.Map(HttpMethod.GET, "/hello", async (ctx, rv) =>
            {
                await ctx.Response.WriteAsync("Hello");
            });

            // Route with a route value: GET /users/{id}
            // Example: /users/42 → routeValues["id"] = "42"
            router.Map(HttpMethod.GET, "/users/{id}", async (ctx, rv) =>
            {
                var id = rv["id"];
                await ctx.Response.WriteAsync($"User {id}");
            });

            // 2) Create and start the HTTP server
            // For each request: try to match a route
            // If matched → call the route handler
            // If not matched → return 404 Not Found
            var server = new HttpServer(
                new[] { "http://localhost:5000/" },
                app: async ctx =>
                {
                    var matched = router.Match(ctx.Request.Method, ctx.Request.Path);
                    if (matched is null)
                    {
                        ctx.Response.StatusCode = 404;
                        await ctx.Response.WriteAsync("Not Found");
                        return;
                    }

                    var (entry, routeValues) = matched.Value;
                    await entry.Handler(ctx, routeValues);
                }
            );

            Console.WriteLine("Listening at http://localhost:5000");
            await server.StartAsync();
        }
    }
}
