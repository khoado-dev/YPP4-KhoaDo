using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Routers;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;

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
            _router = TestStartup.CreateRouter();
        }

        [TestMethod]
        public void GetUserByEmail()
        {
            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/users?email={Uri.EscapeDataString(loggeddInUserEmail)}"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var user = (UserDTO)res.Body!;
            Assert.IsNotNull(user);
            Assert.AreEqual(loggeddInUserEmail, user.Email);
        }
    }
}
