using UnitTestForTrello.Controllers;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController? _userController;

        private const string loggeddInUserEmail = "james85@booth-daniels.net";

        [TestInitialize]
        public void Setup()
        {
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
            //TestStartUp.ResetDatabase();
        }
    }
}
