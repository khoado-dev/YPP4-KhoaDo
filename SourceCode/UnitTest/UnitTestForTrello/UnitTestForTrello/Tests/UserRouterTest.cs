using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class UserRouterTest
    {
        private Router _router = null!;
        private const string loggeddInUserEmail = "james85@booth-daniels.net";

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
        }

        [TestMethod]
        public void GetUserByEmailTest()
        {
            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/users/by-email?email={loggeddInUserEmail}"
            };

            var res = _router.Handle(req);

            var user = (UserDTO)res.Data!;
            Assert.IsNotNull(user);
            Assert.AreEqual(loggeddInUserEmail, user.Email);
        }

    }
}
