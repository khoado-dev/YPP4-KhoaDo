using CustomMVC.Core.Http;
using CustomMVC.Core.Routing;
using CustomMVC.Samples;
using HttpMethod = CustomMVC.Core.Http.HttpMethod;

namespace CustomMVC
{
    internal class Program
    {
        private const string DefaultUrl = "http://localhost:5000/";
        static async Task Main(string[] args)
        {
            // 1) Initialize router and register routes
            var router = new RouteTable();

            router.Map(HttpMethod.GET, "/hello", async (ctx, rv) =>
            {
                await ctx.Response.WriteAsync("Hello");
            });

            // map tới controller/action
            router.Map(HttpMethod.GET, "/users/find", typeof(UsersController), nameof(UsersController.Find));
            router.Map(HttpMethod.GET, "/users/{id}", typeof(UsersController), nameof(UsersController.Show));

            var server = new HttpServer(
                new[] { DefaultUrl },
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
                });

            Console.WriteLine($"Listening at {DefaultUrl}");
            await server.StartAsync();
        }
    }
}
