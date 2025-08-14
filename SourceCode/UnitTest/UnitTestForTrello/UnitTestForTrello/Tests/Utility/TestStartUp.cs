using Microsoft.Data.Sqlite;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello;

[TestClass]
public class TestStartUp
{
    private static Dictionary<Type, object> _singletons = new();

    public static SqliteConnection? Connection;

    #region Setup DI & DB
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        Connection = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
        TestDatabaseHelper.SeedAllData(Connection);

        RegisterSingleton<IUserRepository>(new UserRepository(Connection));
        RegisterSingleton<IUserService>(new UserService(ResolveSingleton<IUserRepository>()));
        RegisterSingleton<UserController>(new UserController(ResolveSingleton<IUserService>()));

        RegisterSingleton<ICardRepository>(new CardRepository(Connection));
        RegisterSingleton<ICardService>(new CardService(ResolveSingleton<ICardRepository>()));
        RegisterSingleton<CardController>(new CardController(ResolveSingleton<ICardService>()));

        RegisterSingleton<IBoardRepository>(new BoardRepository(Connection));
        RegisterSingleton<IBoardService>(new BoardService(ResolveSingleton<IBoardRepository>()));
        RegisterSingleton<BoardController>(new BoardController(ResolveSingleton<IBoardService>()));

        RegisterSingleton<IWorkspaceRepository>(new WorkspaceRepository(Connection));
        RegisterSingleton<IWorkspaceService>(new WorkspaceService(ResolveSingleton<IWorkspaceRepository>()));
        RegisterSingleton<WorkspaceController>(new WorkspaceController(ResolveSingleton<IWorkspaceService>()));

        RegisterSingleton<IMemberRepository>(new MemberRepository(Connection));
        RegisterSingleton<IMemberService>(new MemberService(ResolveSingleton<IMemberRepository>()));
        RegisterSingleton<MemberController>(new MemberController(ResolveSingleton<IMemberService>()));

    }
    #endregion

    #region Register & Resolve Singleton

    public static void RegisterSingleton<T>(T instance)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance), "Instance cannot be null.");
        _singletons[typeof(T)] = instance;
    }

    public static T ResolveSingleton<T>()
    {
        if (_singletons.TryGetValue(typeof(T), out var instance))
            return (T)instance;

        throw new InvalidOperationException($"Service {typeof(T).Name} not registered.");
    }

    #endregion

    #region Reset & Closse DB

    public static void ResetDatabase()
    {
        if (Connection == null)
            throw new InvalidOperationException("Database connection is not initialized.");
        TestDatabaseHelper.ClearData(Connection);
        TestDatabaseHelper.SeedAllData(Connection);
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        Connection?.Close();
        Connection?.Dispose();
    }
    #endregion
}
