using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Routers;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;

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
            _router = TestStartup.CreateRouter();
        }

        [TestMethod]
        public void GetWorkspacesByUserId()
        {
            int expectedNumberOfWorkspaces = 2;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/users/{loggeddInUserId}/workspaces"
                // hoặc: "/users/workspaces?userId=1"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<WorkspaceDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfWorkspaces, result.Count);
        }

        [TestMethod]
        public void GetWorkspaceTypes()
        {
            int expectedNumberOfWorkspaceTypes = 9;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = "/workspaces/types"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<WorkspaceTypeDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfWorkspaceTypes, result.Count);
        }

        [TestMethod]
        public void GetWorkspaceDetailById()
        {
            var expected = new WorkspaceDetailDTO
            {
                WorkspaceId = 1,
                WorkspaceName = "Workspace 1",
                LogoUrl = "logo1.png",
                ShortName = "WS1",
                Website = "https://workspace1.com",
                WorkspaceDescription = "Description for Workspace 1"
            };

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/workspaces/{workspaceId}/detail"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var actual = (WorkspaceDetailDTO)res.Body!;
            Assert.AreEqual(expected.WorkspaceId, actual.WorkspaceId);
            Assert.AreEqual(expected.WorkspaceName, actual.WorkspaceName);
            Assert.AreEqual(expected.LogoUrl, actual.LogoUrl);
            Assert.AreEqual(expected.ShortName, actual.ShortName);
            Assert.AreEqual(expected.Website, actual.Website);
            Assert.AreEqual(expected.WorkspaceDescription, actual.WorkspaceDescription);
        }
    }
}
