using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class WorkspaceRouterTest
    {
        private Router _router = null!;

        private const int loggeddInUserId = 1;
        private const int workspaceId = 1;

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
        }
        [TestMethod]
        public void GetWorkspacesByUserId()
        {
            int expectedNumberOfWorkspaces = 2;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/workspaces/by-user?userId={loggeddInUserId}"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<WorkspaceDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedNumberOfWorkspaces, result.Count);
        }

        [TestMethod]
        public void GetWorkspaceTypes()
        {
            int expectedNumberOfWorkspaceTypes = 9;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = "/workspaces/types"
            };

            var res = _router.Handle(req);

            var result = ((IEnumerable<WorkspaceTypeDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedNumberOfWorkspaceTypes, result.Count);
        }

        [TestMethod]
        public void GetWorkspaceDetailById()
        {
            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/workspaces/detail?workspaceId={workspaceId}"
            };

            var res = _router.Handle(req);

            var actual = (WorkspaceDetailDTO)res.Data!;
            Assert.AreEqual(workspaceId, actual.WorkspaceId);
        }
    }
}
