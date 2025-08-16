using PureDI;
using PureDI.Demo;

// 1) Build our manual DI container (similar to .NET's ServiceCollection/Provider)
var services = new ServiceCollection()
    // Singleton: same instance for whole app
    .AddSingleton<IClock, SystemClock>()
    // Scoped: a new instance per scope (per-request style)
    .AddScoped<IRequestContext, RequestContext>()
    // Transient: new instance every resolution
    .AddTransient<IGreetingService, GreetingService>()
    // Controllers can be registered explicit, or resolved as a concrete type later
    .AddTransient<HomeController>(sp => new HomeController(
        (IGreetingService)sp.GetService(typeof(IGreetingService))!,
        (IRequestContext)sp.GetService(typeof(IRequestContext))!
    ));

// Build root provider
using var provider = new ServiceProvider(services);

// 2) Resolve from ROOT (Singleton/Transient OK; Scoped is NOT allowed from root)
var clockFromRoot = (IClock)provider.GetService(typeof(IClock))!;
Console.WriteLine($"[ROOT] UtcNow: {clockFromRoot.UtcNow:O}");

// 3) Create first scope (simulate Request #1)
using (var scope = ((IServiceScopeFactory)provider).CreateScope())
{
    var sp = scope.ServiceProvider;

    var controller1 = (HomeController)sp.GetService(typeof(HomeController))!;
    Console.WriteLine("[SCOPE #1] " + controller1.Index("Alice"));

    var controller1b = (HomeController)sp.GetService(typeof(HomeController))!;
    Console.WriteLine("[SCOPE #1 again] " + controller1b.Index("Bob"));
    // Note: IRequestContext is Scoped, so CorrelationId should be the SAME within this scope.
}

// 4) Create second scope (simulate Request #2)
using (var scope2 = ((IServiceScopeFactory)provider).CreateScope())
{
    var sp2 = scope2.ServiceProvider;

    var controller2 = (HomeController)sp2.GetService(typeof(HomeController))!;
    Console.WriteLine("[SCOPE #2] " + controller2.Index("Charlie"));
    // Note: IRequestContext is Scoped, so CorrelationId should be DIFFERENT vs Scope #1.
}

Console.WriteLine("Done.");
