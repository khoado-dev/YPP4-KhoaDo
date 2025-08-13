using Microsoft.Data.Sqlite;
using System;
using System.Data;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    [TestClass]
    public class CardControllerTest
    {
        private SqliteConnection? _connection;
        private CardController? _cardController;

        private const int boardId = 1;
        private const int cardId = 1;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedAllData(_connection);

            ICardRepository cardRepository = new CardRepository(_connection);
            ICardService cardService = new CardService(cardRepository);
            _cardController = new CardController(cardService);
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

        [TestCleanup]
        public void Cleanup()
        {
            _connection?.Close();
        }
    }
}
