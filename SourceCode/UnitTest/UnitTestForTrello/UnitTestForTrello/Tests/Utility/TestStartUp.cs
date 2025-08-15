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

        AddSingleton<IUserRepository>(new UserRepository(_connection!));
        AddSingleton<IUserService>(new UserService(GetSingleton<IUserRepository>()));
        AddSingleton<UserController>(new UserController(GetSingleton<IUserService>()));

        AddSingleton<ICardRepository>(new CardRepository(_connection!));
        AddSingleton<ICardService>(new CardService(GetSingleton<ICardRepository>()));
        AddSingleton<CardController>(new CardController(GetSingleton<ICardService>()));

        AddSingleton<IBoardRepository>(new BoardRepository(_connection!));
        AddSingleton<IBoardService>(new BoardService(GetSingleton<IBoardRepository>()));
        AddSingleton<BoardController>(new BoardController(GetSingleton<IBoardService>()));

        AddSingleton<IWorkspaceRepository>(new WorkspaceRepository(_connection!));
        AddSingleton<IWorkspaceService>(new WorkspaceService(GetSingleton<IWorkspaceRepository>()));
        AddSingleton<WorkspaceController>(new WorkspaceController(GetSingleton<IWorkspaceService>()));

        AddSingleton<IMemberRepository>(new MemberRepository(_connection!));
        AddSingleton<IMemberService>(new MemberService(GetSingleton<IMemberRepository>()));
        AddSingleton<MemberController>(new MemberController(GetSingleton<IMemberService>()));

    }
    #endregion

    #region Register & Resolve Singleton

    public static void AddSingleton<T>(T instance)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance), "Instance cannot be null.");
        _singletons[typeof(T)] = instance;
    }

    public static T GetSingleton<T>()
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
