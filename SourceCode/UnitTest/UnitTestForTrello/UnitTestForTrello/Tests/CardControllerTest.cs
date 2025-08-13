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
        private IDbTransaction? _transaction;
        private CardController? _cardController;

        private const int boardId = 1;

        [TestInitialize]
        public void Setup()
        {
            (_connection, _transaction) = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedAllData(_connection, _transaction);

            ICardRepository cardRepository = new CardRepository(_connection, _transaction);
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
            _transaction?.Rollback();
            _transaction?.Dispose();
            _connection?.Close();
        }
    }
}
