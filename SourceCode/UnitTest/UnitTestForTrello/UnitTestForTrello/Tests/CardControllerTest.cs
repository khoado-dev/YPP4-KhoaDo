using Microsoft.Data.Sqlite;
using System;
using System.Data;
using UnitTestForTrello.Controllers;
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

        [TestCleanup]
        public void Cleanup()
        {
            _connection?.Close();
        }
    }
}
