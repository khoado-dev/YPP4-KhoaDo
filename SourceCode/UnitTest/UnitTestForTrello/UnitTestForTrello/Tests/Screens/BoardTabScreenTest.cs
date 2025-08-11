using System.Data.Common;
using System.Data.SqlClient;
using UnitTestForTrello.Models;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.Screens;

namespace UnitTestForTrello.Tests.Screens;

[TestClass]
public class BoardTabScreenTest : DatabaseTestBase
{
    private BoardTabScreen _repo;

    [TestInitialize]
    public void Setup()
    {
        base.BeginTransaction();
        _repo = new BoardTabScreen(_connection!, _transaction!);
    }

    [TestMethod]
    public void GetStarredBoardsByUser()
    {
        // Arrange
        int userId = 1;

        // Act
        List<Board> result = _repo.GetStarredBoardsByUser(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Count > 0);
    }
}
