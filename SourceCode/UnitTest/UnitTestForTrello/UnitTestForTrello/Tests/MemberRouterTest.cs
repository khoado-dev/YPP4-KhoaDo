using UnitTestForTrello.Models;
using UnitTestForTrello.Routers;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class MemberRouterTest
    {
        private Router _router = null!;

        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartUp.CreateRouter();
        }

        [TestMethod]
        public void GetMembersByBoardId()
        {
            int expectedNumberOfMembersInBoard = 3;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/boards/{boardId}/members"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<object>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfMembersInBoard, result.Count);
        }

        [TestMethod]
        public void GetMembersByCardId()
        {
            int expectedNumberOfMembersInCard = 2;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/members"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<object>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfMembersInCard, result.Count);
        }

        [TestMethod]
        public void GetSelectableMembersByCardId()
        {
            int expectedNumberOfSelectableMembersInCard = 8;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/members/selectable"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<object>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfSelectableMembersInCard, result.Count);
        }
    }
}
