using PureDI;
using UnitTestForTrello.Controllers;
using static System.Formats.Asn1.AsnWriter;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class BoardControllerTest
    {
        private IServiceScope _scope = null!;
        private BoardController _controller = null!;

        private const int loggeddInUserId = 1;
        private const int workspaceId = 1;
        private const string ACTIVE_BOARD_STATUS = "active";

        [TestInitialize]
        public void Setup()
        {
            _scope = TestStartUp.CreateScope();
            _controller = (BoardController)_scope.ServiceProvider.GetService(typeof(BoardController))!;
        }

        [TestMethod]
        public void GetStarredBoardsTest()
        {
            int expectedNumberOfStarredBoards = 2;
            var result = _controller?.GetStarredBoards(loggeddInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfStarredBoards, result.Count);
            Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_BOARD_STATUS & b.StarredBoardsStatus==true));
        }

        [TestMethod]
        public void GetRecentlyBoardsTest()
        {
            int expectedNumberOfRecentlyBoards = 2;
            var result = _controller?.GetRecentlyBoards(loggeddInUserId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfRecentlyBoards, result.Count);
            Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_BOARD_STATUS));
        }

        [TestMethod]
        public void GetBoardsWhereUserIsMemberInWorkspaceTest()
        {
            int expectedNumberOfBoards = 2;
            var result = _controller?.GetBoardsWhereUserIsMemberInWorkspace(loggeddInUserId, workspaceId).ToList();
            
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfBoards, result.Count);
        }

        [TestMethod]
        public void GetBoardsWhereUserIsOwnerInWorkspaceTest()
        {
            int expectedNumberOfBoards = 2;
            var result = _controller?.GetBoardsWhereUserIsOwnerInWorkspace(loggeddInUserId, workspaceId).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfBoards, result.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose();
        }
    }
}
