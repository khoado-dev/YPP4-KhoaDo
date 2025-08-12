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
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            _connection.Execute(@"
                CREATE TABLE Board (
                    Id INTEGER PRIMARY KEY,
                    BoardName TEXT,
                    BoardDescription TEXT,
                    CreatedAt TEXT,
                    BackgroundUrl TEXT,
                    BoardStatus TEXT,
                    WorkspaceId INTEGER
                );
            ", transaction: _transaction);

            _connection.Execute(@"
                CREATE TABLE UserStarredBoard (
                    UserId INTEGER,
                    BoardId INTEGER,
                    CreatedAt TEXT,
                    StarredBoardsStatus INTEGER
                );
            ", transaction: _transaction);

            _connection.Execute(@"
                INSERT INTO Board (Id, BoardName, BoardDescription, CreatedAt, BackgroundUrl, BoardStatus, WorkspaceId)
                VALUES (1, 'Test Board 1', 'Description', datetime('now'), 'url1', 'active', 1),
                       (2, 'Inactive Board', 'Description', datetime('now'), 'url2', 'archived', 1)
            ", transaction: _transaction);

            _connection.Execute(@"
                INSERT INTO UserStarredBoard (UserId, BoardId, CreatedAt, StarredBoardsStatus)
                VALUES (1, 1, datetime('now'), 1),
                       (1, 2, datetime('now'), 1)
            ", transaction: _transaction);

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
