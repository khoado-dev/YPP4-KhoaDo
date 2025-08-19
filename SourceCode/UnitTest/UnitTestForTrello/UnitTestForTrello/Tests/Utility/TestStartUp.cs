using System.Data;
using Microsoft.Data.Sqlite;
using PureDI;
using UnitTestForTrello.Routers;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello
{
    [TestClass]
    public class TestStartup
    {
        private static IServiceCollection? _services;
        private static ServiceProvider? _root;
        private static SqliteConnection? _conn;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext _)
        {
            _conn = TestDatabase.OpenAndInit();

            int cacheSweepMinutes = 15;
            _services = new ServiceCollection()
                .AddSingleton<ICustomCache>(new CustomCache(TimeSpan.FromMinutes(cacheSweepMinutes)))
                .AddSingleton<IDbConnection>(_conn)
                .AddBoardModule()
                .AddCardModule()
                .AddMemberModule()
                .AddWorkspaceModule()
                .AddUserModule();

            _root = new ServiceProvider(_services);
        }

        private static IServiceScope CreateScope() => _root!.CreateScope();
        public static Router CreateRouter() => RouteConfig.Create(CreateScope);

        public static void ResetDatabase() => TestDatabase.Reset();

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            (_root as IDisposable)?.Dispose();
            _root = null;
            _services = null;

            TestDatabase.Dispose();
            _conn = null;
        }
    }
}
