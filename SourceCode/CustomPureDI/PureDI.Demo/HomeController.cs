namespace PureDI.Demo
{
    // Simulates an ASP.NET Core controller using DI
    public class HomeController
    {
        private readonly IGreetingService _greeting;
        private readonly IRequestContext _ctx;

        public HomeController(IGreetingService greeting, IRequestContext ctx)
        {
            _greeting = greeting;
            _ctx = ctx;
        }

        public string Index(string user)
        {
            return $"{_greeting.Greet(user)} | CorrelationId: {_ctx.CorrelationId}";
        }
    }
}
