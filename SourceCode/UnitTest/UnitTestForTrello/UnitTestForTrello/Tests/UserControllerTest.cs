using PureDI;
using UnitTestForTrello.Controllers;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        private IServiceScope _scope = null!;
        private UserController _controller = null!;

        private const string loggeddInUserEmail = "james85@booth-daniels.net";

        [TestInitialize]
        public void Setup()
        {
            _scope = TestStartUp.CreateScope();
            _controller = (UserController)_scope.ServiceProvider.GetService(typeof(UserController))!;
        }

        [TestMethod]
        public void GetUserByEmailTest()
        {
            var result = _controller?.GetUserByEmail(loggeddInUserEmail);

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
