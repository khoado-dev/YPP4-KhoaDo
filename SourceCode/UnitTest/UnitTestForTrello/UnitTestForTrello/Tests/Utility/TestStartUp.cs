using Dapper;
using Microsoft.Data.Sqlite;
using PureDI;
using System.Data;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Routers;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello;

[TestClass]
public static class TestStartUp
{
    private static ServiceCollection? _services;
    private static ServiceProvider? _root;
    private static SqliteConnection? _conn;

    #region Setup DI & DB
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        _services = new ServiceCollection();

        _services.AddSingleton<ICustomCache>(new CustomCache(TimeSpan.FromMinutes(1))); //Add custom cache with 1 minute Time To Live(TTL).

        _conn = TestDatabaseHelper.GetInMemoryDatabaseConnection();
        if (_conn?.State != ConnectionState.Open) _conn?.Open();
        if (_conn is SqliteConnection) //Add No Lock for SQLite because SQLite does not support it in query.
        {
            _conn.Execute("PRAGMA journal_mode=WAL;"); //WAL helps readers not block writers and vice versa on file-backed DBs.
            _conn.Execute("PRAGMA synchronous=NORMAL;"); // - synchronous=NORMAL balances durability vs. speed for tests.
            _conn.Execute("PRAGMA busy_timeout = 5000;"); // - busy_timeout reduces SQLITE_BUSY errors when short locks happen.
        }
        _services.AddSingleton<IDbConnection>(_conn!);

        TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
        TestDatabaseHelper.SeedAllData();

        _services.AddScoped<IBoardRepository, BoardRepository>();
        _services.AddScoped<IBoardService, BoardService>();
        _services.AddTransient<BoardController>();

        _services.AddScoped<ICardRepository, CardRepository>();
        _services.AddScoped<ICardService, CardService>();
        _services.AddTransient<CardController>();

        _services.AddScoped<IMemberRepository, MemberRepository>();
        _services.AddScoped<IMemberService, MemberService>();
        _services.AddTransient<MemberController>();

        _services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        _services.AddScoped<IWorkspaceService, WorkspaceService>();
        _services.AddTransient<WorkspaceController>();

        _services.AddScoped<IUserRepository, UserRepository>();
        _services.AddScoped<IUserService, UserService>();
        _services.AddTransient<UserController>();

        _root = new ServiceProvider(_services);
    }
    #endregion

    public static IServiceScope CreateScope() => _root!.CreateScope();
    public static Router CreateRouter() => RouteConfig.Create(CreateScope);   

    #region Reset & Closse DB

    public static void ResetDatabase()
    {
        TestDatabaseHelper.ClearData();
        TestDatabaseHelper.SeedAllData();
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        (_root as IDisposable)?.Dispose();
        _root = null;
        _services = null;

        _conn?.Close();
        _conn?.Dispose();
        _conn = null;
    }
    #endregion
}
