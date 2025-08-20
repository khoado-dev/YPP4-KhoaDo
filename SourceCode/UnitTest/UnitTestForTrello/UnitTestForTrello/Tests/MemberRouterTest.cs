using UnitTestForTrello.Models;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class MemberRouterTest
    {
        private Router _router = null!;

        private const int workspaceId = 1;
        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
        }
        [TestMethod]
        public void GetMembersByWorkspaceId()
        {
            int expectedMembersCount = 3;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/members/by-workspace?workspaceId={workspaceId}"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedMembersCount, result.Count);
        }

        [TestMethod]
        public void GetMembersByBoardId()
        {
            int expectedMembersCount = 3;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/members/by-board?boardId={boardId}"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedMembersCount, result.Count);
        }

        [TestMethod]
        public void GetMembersByCardId()
        {
            int expectedMembersCount = 2;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/members/by-card?cardId={cardId}"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedMembersCount, result.Count);
        }

        [TestMethod]
        public void GetSelectableMembersByCardId()
        {
            int expectedMembersCount = 8;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/members/selectable?cardId={cardId}"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedMembersCount, result.Count);
        }

    }
}
