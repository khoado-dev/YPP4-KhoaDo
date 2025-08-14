using UnitTestForTrello.Controllers;
using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class WorkspaceControllerTest
    {
        private WorkspaceController? _workspaceController;

        private const int loggeddInUserId = 1;
        private const int workspaceId = 1;

        [TestInitialize]
        public void Setup()
        {
            _workspaceController = TestStartUp.ResolveSingleton<WorkspaceController>();
        }

        [TestMethod]
        public void GetWorkspacesByUserIdTest()
        {
            int expectedNumberOfWorkspaces = 2;

            var actualResult = _workspaceController?.GetWorkspacesByUserId(loggeddInUserId).ToList();
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedNumberOfWorkspaces, actualResult.Count);
        }

        [TestMethod]
        public void GetWorkspaceTypesTest()
        {
            int expectedNumberOfWorkspaceTypes = 9;
            var actualResult = _workspaceController?.GetWorkspaceTypes().ToList();
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedNumberOfWorkspaceTypes, actualResult.Count);
        }

        [TestMethod]
        public void GetWorkspaceDetailByIdTest()
        {
            var expectedDTO = new WorkspaceDetailDTO
            {
                WorkspaceId = 1,
                WorkspaceName = "Workspace 1",
                LogoUrl = "logo1.png",
                ShortName = "WS1",
                Website = "https://workspace1.com",
                WorkspaceDescription = "Description for Workspace 1"
            };

            var actualResult = _workspaceController?.GetWorkspaceDetailById(workspaceId);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedDTO.WorkspaceId, actualResult.WorkspaceId);
            Assert.AreEqual(expectedDTO.WorkspaceName, actualResult.WorkspaceName);
            Assert.AreEqual(expectedDTO.LogoUrl, actualResult.LogoUrl);
            Assert.AreEqual(expectedDTO.ShortName, actualResult.ShortName);
            Assert.AreEqual(expectedDTO.Website, actualResult.Website);
            Assert.AreEqual(expectedDTO.WorkspaceDescription, actualResult.WorkspaceDescription);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //TestStartUp.ResetDatabase();
        }
    }
}
