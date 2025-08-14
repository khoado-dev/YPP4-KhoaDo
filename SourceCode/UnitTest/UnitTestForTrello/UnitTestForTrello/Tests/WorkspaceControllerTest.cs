using Microsoft.Data.Sqlite;
using System;
using System.Data;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class WorkspaceControllerTest
    {
        private SqliteConnection? _connection;
        private WorkspaceController? _workspaceController;

        private const int loggeddInUserId = 1;
        private const int workspaceId = 1;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedWorkspaces(_connection);
            TestDatabaseHelper.SeedOwnerTypes(_connection);
            TestDatabaseHelper.SeedMembersOfWorkspace(_connection);
            TestDatabaseHelper.SeedWorkspaceTypes(_connection);

            IWorkspaceRepository workspaceRepository = new WorkspaceRepository(_connection);
            IWorkspaceService workspaceService = new WorkspaceService(workspaceRepository);
            _workspaceController = new WorkspaceController(workspaceService);
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
            _connection?.Close();
        }
    }
}
