using Microsoft.Data.Sqlite;
using System;
using System.Data;
using UnitTestForTrello.Controllers;
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
        private IDbTransaction? _transaction;
        private WorkspaceController? _workspaceController;

        private const int loggeddInUserId = 1;

        [TestInitialize]
        public void Setup()
        {
            (_connection, _transaction) = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedWorkspaces(_connection, _transaction);
            TestDatabaseHelper.SeedOwnerTypes(_connection, _transaction);
            TestDatabaseHelper.SeedMembersOfWorkspace(_connection, _transaction);
            TestDatabaseHelper.SeedWorkspaceTypes(_connection, _transaction);

            IWorkspaceRepository workspaceRepository = new WorkspaceRepository(_connection, _transaction);
            IWorkspaceService workspaceService = new WorkspaceService(workspaceRepository);
            _workspaceController = new WorkspaceController(workspaceService);
        }

        [TestMethod]
        public void GetWorkspaceByUserIdTest()
        {
            var result = _workspaceController?.GetWorkspacesByUserId(loggeddInUserId).ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetWorkspaceTypesTest()
        {
            var result = _workspaceController?.GetWorkspaceTypes().ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result.Count);
        }



        [TestCleanup]
        public void Cleanup()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _connection?.Close();
        }
    }
}
