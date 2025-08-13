using Microsoft.Data.Sqlite;
using System;
using System.Data;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class BoardControllerTest
    {
        private SqliteConnection? _connection;
        private IDbTransaction? _transaction;
        private BoardController? _boardController;

        private const int loggeddInUserId = 1;
        private const string ACTIVE_BOARD_STATUS = "active";

        [TestInitialize]
        public void Setup()
        {
            (_connection, _transaction) = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedBoards(_connection, _transaction);
            TestDatabaseHelper.SeedOwnerTypes(_connection, _transaction);
            TestDatabaseHelper.SeedUserStarredBoards(_connection, _transaction);
            TestDatabaseHelper.SeedUserViewHistories(_connection, _transaction);

            TestDatabaseHelper.SeedWorkspaces(_connection, _transaction);
            TestDatabaseHelper.SeedMembersOfBoard(_connection, _transaction);

            IBoardRepository boardRepository = new BoardRepository(_connection, _transaction);
            IBoardService boardService = new BoardService(boardRepository);
            _boardController = new BoardController(boardService);
        }

        [TestMethod]
        public void GetStarredBoardsTest()
        {
            var result = _boardController?.GetStarredBoards(loggeddInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_BOARD_STATUS & b.StarredBoardsStatus==true));
        }

        [TestMethod]
        public void GetRecentlyBoardsTest()
        {
            var result = _boardController?.GetRecentlyBoards(loggeddInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_BOARD_STATUS));
        }

        [TestMethod]
        public void GetBoardsWithWorkspaceByUserTest()
        {
            var result = _boardController?.GetBoardsWithWorkspaceByUser(loggeddInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetOwnedMemberBoardsInWorkspaceTest()
        {
            int workspaceId = 1;
            var result = _boardController?.GetOwnedMemberBoardsInWorkspace(loggeddInUserId, workspaceId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
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
