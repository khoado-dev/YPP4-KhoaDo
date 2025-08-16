using Microsoft.Data.Sqlite;
using PureDI;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
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

        _conn = TestDatabaseHelper.GetInMemoryDatabaseConnection();
        if (_conn?.State != System.Data.ConnectionState.Open) _conn?.Open();
        _services.AddSingleton<SqliteConnection>(_conn!);

        TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
        TestDatabaseHelper.SeedAllData();

        // 2) Repositories — Scoped
        _services.AddScoped<IBoardRepository>(sp => new BoardRepository(
            (SqliteConnection)sp.GetService(typeof(SqliteConnection))!
        ));

        // 3) Services — Scoped
        _services.AddScoped<IBoardService>(sp => new BoardService(
            (IBoardRepository)sp.GetService(typeof(IBoardRepository))!)
        );

        // 4) Controllers — Transient (hoặc Scoped nếu bạn muốn 1 controller/test)
        _services.AddTransient<BoardController>(sp => new BoardController(
            (IBoardService)sp.GetService(typeof(IBoardService))!)
        );

        _root = new ServiceProvider(_services);
    }
    #endregion

    public static IServiceScope CreateScope() => _root!.CreateScope();

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
