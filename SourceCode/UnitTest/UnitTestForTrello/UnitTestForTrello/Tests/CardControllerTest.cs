using PureDI;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Models.DTOs;
using static System.Formats.Asn1.AsnWriter;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class CardControllerTest
    {
        private IServiceScope _scope = null!;
        private CardController _controller = null!;

        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _scope = TestStartUp.CreateScope();
            _controller = (CardController)_scope.ServiceProvider.GetService(typeof(CardController))!;
        }

        [TestMethod]
        public void GetCardSummariesByBoardIdTest()
        {
            int expectedNumberOfCards = 3;
            var actualResult = _controller?.GetCardSummariesByBoardId(boardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedNumberOfCards, actualResult.Count);
        }

        [TestMethod]
        public void GetCardDetailByCardIdTest()
        {

            var expectedCard = new CardDetailDTO
            {
                CardId = 1,
                CardTitle = "Card 1",
                CardDescription = "Description for Card 1",
                CardLocation = "List 1",
                StageTitle = "To Do"
            };

            var actualResult = _controller?.GetCardDetailByCardId(expectedCard.CardId);

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedCard.CardId, actualResult.CardId);
            Assert.AreEqual(expectedCard.CardTitle, actualResult.CardTitle);
            Assert.AreEqual(expectedCard.CardDescription, actualResult.CardDescription);
            Assert.AreEqual(expectedCard.CardLocation, actualResult.CardLocation);
            Assert.AreEqual(expectedCard.StageTitle, actualResult.StageTitle);
        }

        [TestMethod]
        public void GetCardDetailByBoardIdTest()
        {
            int expctedNumberOfCards = 3;

            var actualResult = _controller?.GetCardDetailByBoardId(boardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfCards, actualResult.Count);
            Assert.IsTrue(actualResult.All(c => c.BoardId == boardId));
        }

        [TestMethod]
        public void GetCardLabelsByCardIdTest()
        {
            int expctedNumberOfLabelInCard = 2;

            var actualResult = _controller?.GetCardLabelsByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfLabelInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetCardCommentsAndReactionsCountByCardIdTest()
        {
            int expctedNumberOfReactionEachCommentInCard = 3;

            var actualResult = _controller?.GetCardCommentsAndReactionsCountByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfReactionEachCommentInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetActivitiesByCardIdTest()
        {
            int expctedNumberOfActivityInCard = 2;

            var actualResult = _controller?.GetActivitiesByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfActivityInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetCustomFieldsByCardIdTest()
        {
            int expctedNumberOfCustomFieldsInCard = 4;

            var actualResult = _controller?.GetCustomFieldsByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfCustomFieldsInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetCustomFieldValuesByCardIdTest()
        {
            int expctedNumberOfCustomFieldsWithValuesInCard = 3;

            var actualResult = _controller?.GetCustomFieldValuesByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfCustomFieldsWithValuesInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetAttachmentsByCardIdTest()
        {
            int expctedNumberOfAttachmentsInCard = 3;

            var actualResult = _controller?.GetAttachmentsByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfAttachmentsInCard, actualResult.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //TestStartUp.ResetDatabase();
        }
    }
}
