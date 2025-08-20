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
            int expectedNumberOfMembersInBoard = 3;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/workspaces/{workspaceId}/members"
            };

            var res = _router.Handle(req);

            
            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedNumberOfMembersInBoard, result.Count);
        }

        [TestMethod]
        public void GetMembersByBoardId()
        {
            int expectedNumberOfMembersInBoard = 3;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/boards/{boardId}/members"
            };

            var res = _router.Handle(req);

            
            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedNumberOfMembersInBoard, result.Count);
        }

        [TestMethod]
        public void GetMembersByCardId()
        {
            int expectedNumberOfMembersInCard = 2;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/cards/{cardId}/members"
            };

            var res = _router.Handle(req);

            
            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedNumberOfMembersInCard, result.Count);
        }

        [TestMethod]
        public void GetSelectableMembersByCardId()
        {
            int expectedNumberOfSelectableMembersInCard = 8;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/cards/{cardId}/members/selectable"
            };

            var res = _router.Handle(req);

            
            var result = ((IEnumerable<object>)res.Data!).ToList();
            Assert.AreEqual(expectedNumberOfSelectableMembersInCard, result.Count);
        }
    }
}
