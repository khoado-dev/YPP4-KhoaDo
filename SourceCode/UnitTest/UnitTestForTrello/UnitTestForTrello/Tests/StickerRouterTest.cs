using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class StickerRouterTest
    {
        private Router _router = null!;

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartup.Router!;
        }

        [TestMethod]
        public void GetNonCustomStickersTest()
        {
            // Arrange
            // SeedStickers(): 3 non-custom (Ids: 1,2,3) thuộc 'Emojis' & 'Animals'
            int expectedCount = 3;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = "/stickers/non-custom"
            };

            // Act
            var res = _router.Handle(req);

            // Assert
            var result = ((IEnumerable<StickerDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsTrue(result.All(s => !string.Equals(s.DisplayValue, "Custom Stickers", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void GetCustomStickersByUserTest()
        {
            // Arrange
            // SeedStickers(): userId=1 có 2 custom (Ids: 10, 11)
            int expectedCount = 2;
            int userId = 1;

            var req = new RequestDTO
            {
                Method = RequestMethod.GET,
                Path = $"/stickers/custom?userId={userId}"
            };

            // Act
            var res = _router.Handle(req);

            // Assert
            var result = ((IEnumerable<StickerDTO>)res.Data!).ToList();
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsTrue(result.All(s =>
                string.Equals(s.DisplayValue, "Custom Stickers", StringComparison.OrdinalIgnoreCase)
                && s.CreatedBy == userId));
        }

    }
}
