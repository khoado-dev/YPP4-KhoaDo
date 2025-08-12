using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.SqlClient;
using UnitTestForTrello.Repositories;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class BoardServiceTest
    {
        private SqliteConnection _connection;
        private IDbTransaction _transaction;
        private BoardService _boardService;

        [TestInitialize]
        public void Setup()
        {
            (_connection, _transaction) = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.AddSeedTestData(_connection, _transaction);

            var boardRepository = new BoardRepository(_connection, _transaction);
            _boardService = new BoardService(boardRepository);
        }

        [TestMethod]
        public void GetStarredActiveBoards()
        {
            int loggedIngInUserId = 1;

            var result = _boardService.GetStarredBoards(loggedIngInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.All(b => b.BoardStatus == "active"));
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _connection?.Close();
        }
    }
}
