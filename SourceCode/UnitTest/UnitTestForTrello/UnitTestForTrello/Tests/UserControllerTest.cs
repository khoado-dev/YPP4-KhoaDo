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
    public class UserControllerTest
    {
        private SqliteConnection? _connection;
        private UserController? _userController;

        private const string loggeddInUserEmail = "james85@booth-daniels.net";

        [TestInitialize]
        public void Setup()
        {
            // 1. Lấy controller singleton từ TestStartUp
            _userController = TestStartUp.ResolveSingleton<UserController>();
        }

        [TestMethod]
        public void GetUserByEmailTest()
        {
            var result = _userController?.GetUserByEmail(loggeddInUserEmail);

            Assert.IsNotNull(result);
            Assert.AreEqual(loggeddInUserEmail, result.Email);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestStartUp.ResetDatabase();
        }
    }
}
