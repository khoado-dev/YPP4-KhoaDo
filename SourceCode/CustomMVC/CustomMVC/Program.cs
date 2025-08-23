using CustomMVC.Core.Http;

namespace CustomMVC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var prefixes = new[] 
            { 
                "http://localhost:5000/" 
            };

            var server = new HttpServer(
                prefixes,
                app: async ctx =>
                {
                    if (ctx.Request.Path.Equals("/hello", StringComparison.OrdinalIgnoreCase))
                        await ctx.Response.WriteAsync("Hello");
                    else
                    {
                        ctx.Response.StatusCode = 404;
                        await ctx.Response.WriteAsync("Not Found");
                    }
                }
            );

            Console.WriteLine("Listening at http://localhost:5000");
            await server.StartAsync();
        }
    }
}
