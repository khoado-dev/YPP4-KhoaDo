using PureDI;
using UnitTestForTrello.Controllers;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class MemberControllerTest
    {
        private IServiceScope _scope = null!;
        private MemberController _controller = null!;

        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _scope = TestStartUp.CreateScope();
            _controller = (MemberController)_scope.ServiceProvider.GetService(typeof(MemberController))!;
        }

        [TestMethod]
        public void GetMembersByBoardIdTest()
        {
            int expectedNumberOfMembersInBoard = 3;
            var result = _controller?.GetMembersByBoardId(boardId).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == expectedNumberOfMembersInBoard);
        }

        [TestMethod]
        public void GetMembersByCardIdTest()
        {
            int expectedNumberOfMembersInCard = 2;
            var result = _controller?.GetMembersByCardId(cardId).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == expectedNumberOfMembersInCard);
        }

        [TestMethod]
        public void GetSelectableMembersByCardIdTest()
        {
            int expectedNumberOfSelectableMembersInCard = 8;
            var result = _controller?.GetSelectableMembersByCardId(cardId).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == expectedNumberOfSelectableMembersInCard);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //TestStartUp.ResetDatabase();
        }
    }
}
