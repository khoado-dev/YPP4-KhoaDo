using System.Net;

namespace CustomMVC.Core.Http
{
    public sealed class HttpServer : IDisposable
    {
        private readonly HttpListener _listener = new(); // HTTP listener to handle incoming requests
        private readonly Func<HttpContext, Task> _app; // Application delegate to handle requests
        private bool _running; // Flag to indicate if the server is running

        public HttpServer(string[] prefixes, Func<HttpContext, Task> app)
        {
            foreach (var p in prefixes)
            {
                _listener.Prefixes.Add(p);
            }

            _app = app;
        }

        public async Task StartAsync(CancellationToken ct = default)
        {
            _listener.Start(); // Start the HTTP listener
            _running = true;

            while (_running && !ct.IsCancellationRequested)
            {
                var ctx = await _listener.GetContextAsync(); // Wait for an incoming request

                // Router handle context here instead of below

                _ = Task.Run(async () =>
                {
                    var httpCtx = new HttpContext(new HttpRequest(ctx.Request), new HttpResponse(ctx.Response)); // create custom HttpContext by request and response of HttpListenerContext

                    try
                    {
                        await _app(httpCtx);
                    }
                    catch (Exception ex)
                    {
                        await httpCtx.Response.WriteAsync($"Internal Error: {ex.Message}", "text/plain");
                    }
                    finally
                    {
                        ctx.Response.OutputStream.Close();
                    }
                }, ct);
            }
        }

        public void Dispose() 
        { 
            _running = false;
            _listener.Close(); 
        }
    }
}
