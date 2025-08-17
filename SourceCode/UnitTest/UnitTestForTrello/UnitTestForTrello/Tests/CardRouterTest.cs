using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Routers;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class CardRouterTest
    {
        private Router _router = null!;

        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _router = TestStartUp.CreateRouter();
        }
        [TestMethod]
        public void GetCardDetailByCardId()
        {
            var expected = new CardDetailDTO
            {
                CardId = 1,
                CardTitle = "Card 1",
                CardDescription = "Description for Card 1",
                CardLocation = "List 1",
                StageTitle = "To Do"
            };

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{expected.CardId}/detail"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var actual = (CardDetailDTO)res.Body!;
            Assert.AreEqual(expected.CardId, actual.CardId);
            Assert.AreEqual(expected.CardTitle, actual.CardTitle);
            Assert.AreEqual(expected.CardDescription, actual.CardDescription);
            Assert.AreEqual(expected.CardLocation, actual.CardLocation);
            Assert.AreEqual(expected.StageTitle, actual.StageTitle);
        }

        [TestMethod]
        public void GetCardDetailByBoardId()
        {
            int expectedNumberOfCards = 3;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/boards/{boardId}/cards/detail"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<CardDetailDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfCards, result.Count);
            Assert.IsTrue(result.All(c => c.BoardId == boardId));
        }

        [TestMethod]
        public void GetCardLabelsByCardId()
        {
            int expectedNumberOfLabels = 2;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/labels"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<CardLabelDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfLabels, result.Count);
        }

        [TestMethod]
        public void GetCardCommentsAndReactionsCountByCardId()
        {
            int expectedNumberOfReactionEachComment = 3;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/comments/reactions"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<CardCommentWithReactionCountDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfReactionEachComment, result.Count);
        }

        [TestMethod]
        public void GetActivitiesByCardId()
        {
            int expectedNumberOfActivities = 2;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/activities"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<CardActivityDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfActivities, result.Count);
        }

        [TestMethod]
        public void GetCustomFieldsByCardId()
        {
            int expectedNumberOfCustomFields = 4;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/custom-fields"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<CardCustomFieldDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfCustomFields, result.Count);
        }

        [TestMethod]
        public void GetCustomFieldValuesByCardId()
        {
            int expectedNumberOfCustomFieldValues = 3;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/custom-field-values"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<CardCustomFieldValueDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfCustomFieldValues, result.Count);
        }

        [TestMethod]
        public void GetAttachmentsByCardId()
        {
            int expectedNumberOfAttachments = 3;

            var req = new Request
            {
                Method = HttpMethod.GET,
                Path = $"/cards/{cardId}/attachments"
            };

            var res = _router.Handle(req);

            Assert.AreEqual(HttpStatus.OK, res.StatusCode);
            var result = ((IEnumerable<CardAttachmentDTO>)res.Body!).ToList();
            Assert.AreEqual(expectedNumberOfAttachments, result.Count);
        }
    }
}
