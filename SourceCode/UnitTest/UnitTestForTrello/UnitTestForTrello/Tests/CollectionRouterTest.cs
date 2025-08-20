using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class CollectionRouterTest
    {
        private Router _router = null!;
        private const int ws1 = 1;
        private const int ws2 = 2;

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
            // Nếu cần reset DB mỗi test:
            // TestDatabase.Reset();
        }

        [TestMethod]
        public void GetBoardsWithCollectionsInWorkspaceTest()
        {
            // Arrange
            int expectedCount = 3; // (1,Backlog), (1,Sprint), (2,Sprint)

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/boards/with-collections?workspaceId={ws1}"
            };

            // Act
            var res = _router.Handle(req);

            // Assert
            var result = ((IEnumerable<BoardWithCollectionDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsTrue(result.All(x => x.WorkspaceId == ws1));
            Assert.IsTrue(result.All(x => x.BoardId > 0 && x.CollectionId > 0));
        }

        [TestMethod]
        public void GetCollectionsByWorkspaceTest()
        {
            // Arrange
            int expectedCount = 2; // Ideas, Roadmap

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/collections/by-workspace?workspaceId={ws2}"
            };

            // Act
            var res = _router.Handle(req);

            // Assert
            var result = ((IEnumerable<CollectionDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsTrue(result.All(x => x.WorkspaceId == ws2));
            Assert.IsTrue(result.All(x => x.CollectionId > 0));
        }
    }
}
