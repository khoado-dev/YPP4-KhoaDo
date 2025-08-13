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
    public class MemberControllerTest
    {
        private SqliteConnection? _connection;
        private IDbTransaction? _transaction;
        private MemberController? _memberController;

        private const int boardId = 1;

        [TestInitialize]
        public void Setup()
        {
            (_connection, _transaction) = TestDatabaseHelper.CreateInMemoryDatabaseAndSchema();
            TestDatabaseHelper.SeedAllData(_connection, _transaction);

            IMemberRepository memberRepository = new MemberRepository(_connection, _transaction);
            IMemberService memberService = new MemberService(memberRepository);
            _memberController = new MemberController(memberService);
        }

        [TestMethod]
        public void GetMemberByBoardIdTest()
        {
            int expectedNumberOfMembersInBoard = 3;
            var result = _memberController?.GetMembersByBoardId(boardId).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == expectedNumberOfMembersInBoard);
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
