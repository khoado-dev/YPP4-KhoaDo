using Dapper;
using Microsoft.Data.Sqlite;
using PureDI;
using System.Data;
using UnitTestForTrello.Routers;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello;

[TestClass]
public static class TestStartUp
{
    private static IServiceCollection? _services;
    private static ServiceProvider? _root;
    private static SqliteConnection? _conn;

    #region Setup DI & DB
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        int cacheSweepMinutes = 15;

        _services = new ServiceCollection()
        .AddSingleton<ICustomCache>(new CustomCache(TimeSpan.FromMinutes(cacheSweepMinutes)))
        .AddSingleton<IDbConnection>(InitSqlite())
        .AddBoardModule()
        .AddCardModule()
        .AddMemberModule()
        .AddWorkspaceModule()
        .AddUserModule();

        _root = new ServiceProvider(_services);
    }

    private static IDbConnection InitSqlite()
    {
        int defaultTimeout = 5000; // seconds

        var conn = TestDatabaseHelper.GetInMemoryDatabaseConnection();
        if (conn?.State != ConnectionState.Open) conn?.Open();
        if (conn is SqliteConnection)
        {
            conn.Execute("PRAGMA journal_mode=WAL;");
            conn.Execute("PRAGMA synchronous=NORMAL;");
            conn.Execute("PRAGMA read_uncommitted = ON;");
            conn.Execute($"PRAGMA busy_timeout={defaultTimeout};");
        }
        TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
        TestDatabaseHelper.SeedAllData();
        return conn!;
    }
    #endregion

    private static IServiceScope CreateScope() => _root!.CreateScope();
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
