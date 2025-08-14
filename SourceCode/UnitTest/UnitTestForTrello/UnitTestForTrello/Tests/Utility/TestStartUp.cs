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

    public static SqliteConnection? _connection;

    #region Setup DI & DB
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
        TestDatabaseHelper.SeedAllData();

        _connection = TestDatabaseHelper.GetInMemoryDatabaseConnection();

        RegisterSingleton<IUserRepository>(new UserRepository(_connection!));
        RegisterSingleton<IUserService>(new UserService(ResolveSingleton<IUserRepository>()));
        RegisterSingleton<UserController>(new UserController(ResolveSingleton<IUserService>()));

        RegisterSingleton<ICardRepository>(new CardRepository(_connection!));
        RegisterSingleton<ICardService>(new CardService(ResolveSingleton<ICardRepository>()));
        RegisterSingleton<CardController>(new CardController(ResolveSingleton<ICardService>()));

        RegisterSingleton<IBoardRepository>(new BoardRepository(_connection!));
        RegisterSingleton<IBoardService>(new BoardService(ResolveSingleton<IBoardRepository>()));
        RegisterSingleton<BoardController>(new BoardController(ResolveSingleton<IBoardService>()));

        RegisterSingleton<IWorkspaceRepository>(new WorkspaceRepository(_connection!));
        RegisterSingleton<IWorkspaceService>(new WorkspaceService(ResolveSingleton<IWorkspaceRepository>()));
        RegisterSingleton<WorkspaceController>(new WorkspaceController(ResolveSingleton<IWorkspaceService>()));

        RegisterSingleton<IMemberRepository>(new MemberRepository(_connection!));
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
        TestDatabaseHelper.ClearData();
        TestDatabaseHelper.SeedAllData();
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
    #endregion
}
