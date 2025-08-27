using CustomMVC.Core.Http;
using CustomMVC.Core.Routing;
using CustomMVC.DI;
using CustomMVC.Mvc.Views;
using CustomMVC.Samples;
using HttpMethod = CustomMVC.Core.Http.HttpMethod;

namespace CustomMVC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var prefixes = new[] { "http://localhost:5000/" };
            // 1) Initialize router and register routes
            var router = new RouteTable();

            router.Map(HttpMethod.GET, "/", async (ctx, rv) =>
            {
                await ctx.Response.WriteAsync("Hello World");
            });

            ReflectionFactory.Register<IUserService, UserService>();
            ReflectionFactory.Register<IUserRepository, UserRepository>();

            // map to controller/action
            router.Map(HttpMethod.GET, "/users", typeof(UsersController), nameof(UsersController.GetUsers));
            router.Map(HttpMethod.GET, "/users/{email}", typeof(UsersController), nameof(UsersController.GetUserByEmail));
            router.Map(HttpMethod.GET, "/users/{id}/profile", typeof(UsersController), nameof(UsersController.Profile));

            var server = new HttpServer(
                prefixes,
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

            Console.WriteLine($"Listening at {prefixes[0]}");
            await server.StartAsync();
        }
    }
}
