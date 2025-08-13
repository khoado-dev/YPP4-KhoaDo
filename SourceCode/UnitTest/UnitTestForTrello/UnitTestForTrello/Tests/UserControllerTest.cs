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
            _connection = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedAllData(_connection);

            IUserRepository userRepository = new UserRepository(_connection);
            IUserService userService = new UserService(userRepository);
            _userController = new UserController(userService);
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
            _connection?.Close();
        }
    }
}
