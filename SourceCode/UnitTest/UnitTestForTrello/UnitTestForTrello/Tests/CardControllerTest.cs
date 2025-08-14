using UnitTestForTrello.Controllers;
using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class CardControllerTest
    {
        private CardController? _cardController;

        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _cardController = TestStartUp.ResolveSingleton<CardController>();
        }

        [TestMethod]
        public void GetCardSummariesByBoardIdTest()
        {
            int expectedNumberOfCards = 3;
            var actualResult = _cardController?.GetCardSummariesByBoardId(boardId).ToList();

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

            var actualResult = _cardController?.GetCardDetailByCardId(expectedCard.CardId);

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

            var actualResult = _cardController?.GetCardDetailByBoardId(boardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfCards, actualResult.Count);
            Assert.IsTrue(actualResult.All(c => c.BoardId == boardId));
        }

        [TestMethod]
        public void GetCardLabelsByCardIdTest()
        {
            int expctedNumberOfLabelInCard = 2;

            var actualResult = _cardController?.GetCardLabelsByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfLabelInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetCardCommentsAndReactionsCountByCardIdTest()
        {
            int expctedNumberOfReactionEachCommentInCard = 3;

            var actualResult = _cardController?.GetCardCommentsAndReactionsCountByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfReactionEachCommentInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetActivitiesByCardIdTest()
        {
            int expctedNumberOfActivityInCard = 2;

            var actualResult = _cardController?.GetActivitiesByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfActivityInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetCustomFieldsByCardIdTest()
        {
            int expctedNumberOfCustomFieldsInCard = 4;

            var actualResult = _cardController?.GetCustomFieldsByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfCustomFieldsInCard, actualResult.Count);
        }

        [TestMethod]
        public void GetCustomFieldValuesByCardIdTest()
        {
            int expctedNumberOfCustomFieldsWithValuesInCard = 3;

            var actualResult = _cardController?.GetCustomFieldValuesByCardId(cardId).ToList();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expctedNumberOfCustomFieldsWithValuesInCard, actualResult.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //TestStartUp.ResetDatabase();
        }
    }
}
