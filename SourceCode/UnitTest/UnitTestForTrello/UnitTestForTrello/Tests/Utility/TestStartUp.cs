using Microsoft.Data.Sqlite;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.CustomDI;
using UnitTestForTrello.Routers;
using UnitTestForTrello.Tests;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello
{
    [TestClass]
    public class TestStartup
    {
        private static SqliteConnection? _conn;
        private static CustomCache? cache;
        private static Router? router;

        public static SqliteConnection? Conn { get => _conn; set => _conn = value; }
        public static CustomCache Cache { get => cache; set => cache = value; }
        public static Router? Router { get => router; private set => router = value; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext _)
        {
            cache = new CustomCache();
            _conn = TestDatabase.OpenAndInit();
            router = RouteConfig.Create();
        }

        public static void ResetDatabase() => TestDatabase.Reset();

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestDatabase.Dispose();
            _conn = null;
        }
    }
}
