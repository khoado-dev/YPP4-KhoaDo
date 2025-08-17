using Microsoft.Data.Sqlite;
using PureDI;
using System.Data;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;
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
        if (_conn?.State != ConnectionState.Open) _conn?.Open();
        _services.AddSingleton<IDbConnection>(_conn!);

        TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
        TestDatabaseHelper.SeedAllData();

        _services.AddScoped<IBoardRepository, BoardRepository>();
        _services.AddScoped<IBoardService, BoardService>();
        _services.AddTransient<BoardController>();

        _services.AddScoped<ICardRepository, CardRepository>();
        _services.AddScoped<ICardService, CardService>();
        _services.AddTransient<CardController>();

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
