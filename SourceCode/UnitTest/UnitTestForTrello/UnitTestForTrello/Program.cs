using System.Net;
namespace UnitTestForTrello
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var prefixes = new[] { "http://localhost:5000/" };
            var listener = new HttpListener();
            foreach (var p in prefixes) listener.Prefixes.Add(p);
            listener.Start();
            Console.WriteLine($"API listening on {string.Join(", ", prefixes)}");
            TestStartup.AssemblyInit();
            var router = TestStartup.Router;

            while (true)
            {
                var ctx = await listener.GetContextAsync();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (ctx.Request.HttpMethod == "OPTIONS")
                        {
                            HttpResponseAdapter.WriteCors(ctx.Response);
                            ctx.Response.StatusCode = 204;
                            ctx.Response.Close();
                            return;
                        }

                        var reqDto = HttpRequestAdapter.From(ctx.Request);
                        var resDto = router.Handle(reqDto);

                        await HttpResponseAdapter.WriteJson(ctx.Response, resDto);
                    }
                    catch (Exception ex)
                    {
                        await HttpResponseAdapter.WriteProblem(ctx.Response, 500, "ServerError", ex.Message);
                    }
                    finally
                    {
                        ctx.Response.Close();
                    }
                });
            }
        }
    }
}