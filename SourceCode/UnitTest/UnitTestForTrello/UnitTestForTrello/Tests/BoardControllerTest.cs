using Microsoft.Data.Sqlite;
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
        private BoardController? _boardController;

        private const int loggeddInUserId = 1;
        private const int workspaceId = 1;
        private const string ACTIVE_BOARD_STATUS = "active";

        [TestInitialize]
        public void Setup()
        {
            _connection= TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedAllData(_connection);

            IBoardRepository boardRepository = new BoardRepository(_connection);
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
        public void GetBoardsWhereUserIsMemberInWorkspaceTest()
        {
            var result = _boardController?.GetBoardsWhereUserIsMemberInWorkspace(loggeddInUserId, workspaceId).ToList();
            
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetBoardsWhereUserIsOwnerInWorkspaceTest()
        {
            var result = _boardController?.GetBoardsWhereUserIsOwnerInWorkspace(loggeddInUserId, workspaceId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection?.Close();
        }
    }
}
