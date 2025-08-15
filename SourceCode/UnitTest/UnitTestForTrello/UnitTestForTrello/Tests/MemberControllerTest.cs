using UnitTestForTrello.Controllers;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class MemberControllerTest
    {
        private MemberController? _memberController;

        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _memberController = TestStartUp.GetSingleton<MemberController>();
        }

        [TestMethod]
        public void GetMembersByBoardIdTest()
        {
            int expectedNumberOfMembersInBoard = 3;
            var result = _memberController?.GetMembersByBoardId(boardId).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == expectedNumberOfMembersInBoard);
        }

        [TestMethod]
        public void GetMembersByCardIdTest()
        {
            int expectedNumberOfMembersInCard = 2;
            var result = _memberController?.GetMembersByCardId(cardId).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == expectedNumberOfMembersInCard);
        }

        [TestMethod]
        public void GetSelectableMembersByCardIdTest()
        {
            int expectedNumberOfSelectableMembersInCard = 8;
            var result = _memberController?.GetSelectableMembersByCardId(cardId).ToList();

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
