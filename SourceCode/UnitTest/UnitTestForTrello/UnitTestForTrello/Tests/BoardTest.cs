using UnitTestForTrello.Models;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Tests;

namespace UnitTestForTrello;

[TestClass]
public class BoardTest : DatabaseTestBase
{
    private BoardRepository _boardRepository;

    [TestInitialize]
    public void Setup()
    {
        base.BeginTransaction();
        _boardRepository = new BoardRepository(_connection!, _transaction!);
    }

    [TestMethod]
    public void CreateBoardTest()
    {
        var board = new Board
        {
            BoardName = "Test Board",
            BoardDescription = "Test Desc",
            WorkspaceId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            BackgroundUrl = "http://background"
        };

        int id = _boardRepository.Create(board);
        var newBoard = _boardRepository.GetById(id);
        Assert.IsNotNull(newBoard);
        Assert.AreEqual(board.BoardName, newBoard?.BoardName);
    }

    [TestMethod]
    public void GetBoardByIdTest()
    {
        var board = new Board
        {
            BoardName = "GetById Board",
            BoardDescription = "Desc",
            WorkspaceId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            BackgroundUrl = "http://background"
        };
        int id = _boardRepository.Create(board);

        var result = _boardRepository.GetById(id);
        Assert.IsNotNull(result);
        Assert.AreEqual(board.BoardName, result?.BoardName);
    }

    [TestMethod]
    public void GetAllBoardsTest()
    {
        var before = _boardRepository.GetAll().Count;

        var board = new Board
        {
            BoardName = "AllBoardsTest",
            BoardDescription = "Desc",
            WorkspaceId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            BackgroundUrl = "http://background"
        };
        _boardRepository.Create(board);

        var after = _boardRepository.GetAll().Count;

        Assert.IsTrue(after == before + 1);
    }

    [TestMethod]
    public void UpdateBoardTest()
    {
        var board = new Board
        {
            BoardName = "UpdateBoardTest",
            BoardDescription = "Desc",
            WorkspaceId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            BackgroundUrl = "http://background"
        };
        int id = _boardRepository.Create(board);

        var toUpdate = _boardRepository.GetById(id);
        Assert.IsNotNull(toUpdate);

        toUpdate.BoardName = "Updated Name";
        toUpdate.UpdatedAt = DateTime.UtcNow;
        toUpdate.UpdatedBy = 2;

        var updated = _boardRepository.Update(toUpdate);

        Assert.IsTrue(updated);

        var afterUpdate = _boardRepository.GetById(id);
        Assert.AreEqual(toUpdate.BoardName, afterUpdate?.BoardName);
        Assert.AreEqual(2, afterUpdate?.UpdatedBy);
    }

    [TestMethod]
    public void DeleteBoardTest()
    {
        var board = new Board
        {
            BoardName = "DeleteBoardTest",
            BoardDescription = "Desc",
            WorkspaceId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = 1,
            BackgroundUrl = "http://background"
        };
        int id = _boardRepository.Create(board);

        var deleted = _boardRepository.Delete(id);

        Assert.IsTrue(deleted);

        var afterDelete = _boardRepository.GetById(id);
        Assert.IsNull(afterDelete);
    }
}
