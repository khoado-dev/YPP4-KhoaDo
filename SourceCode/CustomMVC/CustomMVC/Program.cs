using CustomMVC.App.Repositories;
using CustomMVC.App.Repositories.IRepository;
using CustomMVC.App.Service;
using CustomMVC.App.Service.IService;
using CustomMVC.Core.DI;
using CustomMVC.Core.Http;
using CustomMVC.Core.Routing;
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
            RouteScanner.Build(router, typeof(Program).Assembly);
            //default route
            router.Map(HttpMethod.GET, "/", async (ctx, rv) =>
            {
                await ctx.Response.WriteAsync("Hello World");
            });

            ConfigureDependencies();

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

        private static void ConfigureDependencies()
        {
            ReflectionFactory.Register<IUserService, UserService>();
            ReflectionFactory.Register<IUserRepository, UserRepository>();
        }
    }
}
