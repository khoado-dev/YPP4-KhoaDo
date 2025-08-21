using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;
using UnitTestForTrello.Models; // chỗ enum OwnerType

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class SettingRouterTest
    {
        private Router _router = null!;
        private const int ownerId = 1; // khớp seed

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
        }

        // ===== WORKSPACE =====
        [TestMethod]
        public void GetWorkspaceSettingValuesTest()
        {
            int expectedCount = 2;
            var ownerType = OwnerType.WORKSPACE.ToString();

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/settings/values?ownerType={ownerType}&ownerId={ownerId}"
            };

            var res = _router.Handle(req);
            var list = ((IEnumerable<SettingKeyValueDTO>)res.Data!).ToList();

            Assert.AreEqual(expectedCount, list.Count);
        }

        [TestMethod]
        public void GetWorkspaceSettingOptionsTest()
        {
            int expectedCount = 2;
            var ownerType = OwnerType.WORKSPACE.ToString();

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/settings/options?ownerType={ownerType}"
            };

            var res = _router.Handle(req);
            var list = ((IEnumerable<SettingKeyOptionDTO>)res.Data!).ToList();

            Assert.AreEqual(expectedCount, list.Count);
        }

        // ===== BOARD =====
        [TestMethod]
        public void GetBoardSettingValuesTest()
        {
            int expectedCount = 2;
            var ownerType = OwnerType.BOARD.ToString();

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/settings/values?ownerType={ownerType}&ownerId={ownerId}"
            };

            var res = _router.Handle(req);
            var list = ((IEnumerable<SettingKeyValueDTO>)res.Data!).ToList();

            Assert.AreEqual(expectedCount, list.Count);
        }

        [TestMethod]
        public void GetBoardSettingOptionsTest()
        {
            int expectedCount = 2; // CardLayout có 2 option
            var ownerType = OwnerType.BOARD.ToString();

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/settings/options?ownerType={ownerType}"
            };

            var res = _router.Handle(req);
            var list = ((IEnumerable<SettingKeyOptionDTO>)res.Data!).ToList();

            Assert.AreEqual(expectedCount, list.Count);
        }

        // ===== USER =====
        [TestMethod]
        public void GetUserSettingValuesTest()
        {
            int expectedCount = 2; // Language, EmailNotifications
            var ownerType = OwnerType.USER.ToString();

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/settings/values?ownerType={ownerType}&ownerId={ownerId}"
            };

            var res = _router.Handle(req);
            var list = ((IEnumerable<SettingKeyValueDTO>)res.Data!).ToList();

            Assert.AreEqual(expectedCount, list.Count);
        }

        [TestMethod]
        public void GetUserSettingOptionsTest()
        {
            int expectedCount = 2; // Language có 2 option
            var ownerType = OwnerType.USER.ToString();

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/settings/options?ownerType={ownerType}"
            };

            var res = _router.Handle(req);
            var list = ((IEnumerable<SettingKeyOptionDTO>)res.Data!).ToList();

            Assert.AreEqual(expectedCount, list.Count);
        }
    }
}
