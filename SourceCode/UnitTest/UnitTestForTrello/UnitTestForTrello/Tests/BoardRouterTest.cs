using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests;

[TestClass]
public class BoardRouterTest
{
    private Router _router = null!;

    private const int userId = 1;
    private const int workspaceId = 1;

    [TestInitialize]
    public void Setup()
    {
        _router = TestStartup.Router!;
    }

    [TestMethod]
    public void GetStarredBoardsTest()
    {
        int expectedCount = 2;

        var req = new RequestDTO
        {
            Method = RequestMethod.GET,
            Path = $"/boards/starred?userId={userId}"
        };

        var res = _router.Handle(req);

        var result = ((IEnumerable<StarredBoardDTO>)res.Data!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
        Assert.IsTrue(result.All(b => b.BoardStatus == BoardStatus.ACTIVE.ToString() && b.StarredBoardsStatus));
    }

    [TestMethod]
    public void GetRecentBoardsTest()
    {
        int expectedCount = 2;

        var req = new RequestDTO
        {
            Method = RequestMethod.GET,
            Path = $"/boards/recent?userId={userId}"
        };

        var res = _router.Handle(req);

        var result = ((IEnumerable<RecentBoardDTO>)res.Data!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
        Assert.IsTrue(result.All(b => string.Equals(b.BoardStatus, BoardStatus.ACTIVE.ToString())));
    }

    [TestMethod]
    public void GetBoardsAsMemberTest()
    {
        int expectedCount = 2;
        string membership = "member";

        var req = new RequestDTO
        {
            Method = RequestMethod.GET,
            Path = $"/boards?workspaceId={workspaceId}&userId={userId}&membership={membership}"
        };

        var res = _router.Handle(req);

        var result = ((IEnumerable<BoardWithWorkspaceDTO>)res.Data!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
    }

    [TestMethod]
    public void GetBoardsAsOwnerTest()
    {
        int expectedCount = 2;
        string membership = "owner";

        var req = new RequestDTO
        {
            Method = RequestMethod.GET,
            Path = $"/boards?workspaceId={workspaceId}&userId={userId}&membership={membership}"
        };

        var res = _router.Handle(req);

        var result = ((IEnumerable<BoardWithWorkspaceDTO>)res.Data!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
    }

}
