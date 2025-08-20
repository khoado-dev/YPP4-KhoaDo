using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class NotificationRouterTest
    {
        private Router _router = null!;
        private const int userId = 1;

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
        }

        [TestMethod]
        public void GetUnreadNotificationsTest()
        {
            int expectedCount = 1; // từ SeedNotifications()

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/notifications?userId={userId}&isRead=false"
            };

            var res = _router.Handle(req);

            // Assert
            var list = ((IEnumerable<NotificationDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, list.Count);
            Assert.IsTrue(list.All(n => n.UserId == userId && n.IsRead == 0));
            Assert.IsTrue(list.All(n => !string.IsNullOrWhiteSpace(n.Username)));
        }

        [TestMethod]
        public void GetReadNotificationsTest()
        {
            // Arrange
            int expectedCount = 1; // từ SeedNotifications()

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/notifications?userId={userId}&isRead=true"
            };

            // Act
            var res = _router.Handle(req);

            // Assert
            var list = ((IEnumerable<NotificationDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, list.Count);
            Assert.IsTrue(list.All(n => n.UserId == userId && n.IsRead == 1));
            Assert.IsTrue(list.All(n => !string.IsNullOrWhiteSpace(n.ActivityDescription)));
        }
    }
}
