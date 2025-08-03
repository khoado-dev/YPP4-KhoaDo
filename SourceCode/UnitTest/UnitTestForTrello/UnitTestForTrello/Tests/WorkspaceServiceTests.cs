using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using UnitTestForTrello.Models;
using UnitTestForTrello.Models.Interface;
using UnitTestForTrello.Services;

namespace UnitTestForTrello
{
    [TestClass]
    public class WorkspaceServiceTests
    {
        private Mock<IDbTemplate> mockDbTemplate;
        private WorkspaceService workspaceService;
        private Workspace sampleWorkspace;

        [TestInitialize]
        public void Setup()
        {
            mockDbTemplate = new Mock<IDbTemplate>();
            workspaceService = new WorkspaceService(mockDbTemplate.Object);

            sampleWorkspace = new Workspace
            {
                Id = 1,
                Name = "Test Workspace",
                Description = "Sample description",
                Type = WorkspaceTypeEnum.HUMAN_RESOURCES
            };
        }

        [TestMethod]
        public void TestGetWorkspaceByName_Found()
        {
            mockDbTemplate.Setup(t => t.QueryForObject(
                It.IsAny<string>(),
                It.IsAny<Func<IDataReader, Workspace>>(),
                "Test Workspace"
            )).Returns(sampleWorkspace);

            var result = workspaceService.GetWorkspaceByName("Test Workspace");

            Assert.IsNotNull(result);
            Assert.AreEqual("Test Workspace", result.Name);
        }

        [TestMethod]
        public void TestGetWorkspaceByName_NotFound()
        {
            mockDbTemplate.Setup(t => t.QueryForObject(
                It.IsAny<string>(),
                It.IsAny<Func<IDataReader, Workspace>>(),
                "Unknown"
            )).Throws(new KeyNotFoundException());

            Assert.ThrowsException<KeyNotFoundException>(() =>
            {
                workspaceService.GetWorkspaceByName("Unknown");
            });
        }

        [TestMethod]
        public void TestGetAllWorkspaces()
        {
            mockDbTemplate.Setup(t => t.Query(
                It.IsAny<string>(),
                It.IsAny<Func<IDataReader, Workspace>>()
            )).Returns(new List<Workspace> { sampleWorkspace });

            var result = workspaceService.GetAllWorkspaces();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test Workspace", result[0].Name);
        }

        [TestMethod]
        public void TestUpdateWorkspace()
        {
            mockDbTemplate.Setup(t => t.Update(
                It.IsAny<string>(),
                It.IsAny<object[]>()
            )).Returns(1);

            int rows = workspaceService.UpdateWorkspace(1, "Updated", "Updated desc", WorkspaceTypeEnum.HUMAN_RESOURCES);

            Assert.AreEqual(1, rows);
        }

        [TestMethod]
        public void TestDeleteWorkspace()
        {
            mockDbTemplate.Setup(t => t.Update(
                It.IsAny<string>(),
                1
            )).Returns(1);

            int rows = workspaceService.DeleteWorkspace(1);

            Assert.AreEqual(1, rows);
        }
    }
}
