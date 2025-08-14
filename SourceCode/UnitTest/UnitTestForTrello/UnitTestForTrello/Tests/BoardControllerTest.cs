using UnitTestForTrello.Controllers;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class BoardControllerTest
    {
        private BoardController? _boardController;

        private const int loggeddInUserId = 1;
        private const int workspaceId = 1;
        private const string ACTIVE_BOARD_STATUS = "active";

        [TestInitialize]
        public void Setup()
        {
            _boardController = TestStartUp.ResolveSingleton<BoardController>();
        }

        [TestMethod]
        public void GetStarredBoardsTest()
        {
            int expectedNumberOfStarredBoards = 2;
            var result = _boardController?.GetStarredBoards(loggeddInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfStarredBoards, result.Count);
            Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_BOARD_STATUS & b.StarredBoardsStatus==true));
        }

        [TestMethod]
        public void GetRecentlyBoardsTest()
        {
            int expectedNumberOfRecentlyBoards = 2;
            var result = _boardController?.GetRecentlyBoards(loggeddInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfRecentlyBoards, result.Count);
            Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_BOARD_STATUS));
        }

        [TestMethod]
        public void GetBoardsWhereUserIsMemberInWorkspaceTest()
        {
            int expectedNumberOfBoards = 2;
            var result = _boardController?.GetBoardsWhereUserIsMemberInWorkspace(loggeddInUserId, workspaceId).ToList();
            
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfBoards, result.Count);
        }

        [TestMethod]
        public void GetBoardsWhereUserIsOwnerInWorkspaceTest()
        {
            int expectedNumberOfBoards = 2;
            var result = _boardController?.GetBoardsWhereUserIsOwnerInWorkspace(loggeddInUserId, workspaceId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfBoards, result.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //TestStartUp.ResetDatabase();
        }
    }
}
