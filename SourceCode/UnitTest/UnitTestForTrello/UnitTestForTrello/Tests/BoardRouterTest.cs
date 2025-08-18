using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Routers;
using UnitTestForTrello.Tests.Utility;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;

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
        _router = TestStartUp.CreateRouter();
    }


    [TestMethod]
    public void GetStarredBoardsTest()
    {
        int expectedCount = 2;

        var req = new Request
        {
            Method = HttpMethod.GET,
            Path = $"/boards/starred/{userId}"
        };

        var res = _router.Handle(req);

        Assert.AreEqual(HttpStatus.OK, res.StatusCode);
        var result = ((IEnumerable<StarredBoardDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
        Assert.IsTrue(result.All(b => b.BoardStatus == BoardStatus.ACTIVE.ToString() && b.StarredBoardsStatus));
    }

    [TestMethod]
    public void GetRecentBoardsTest()
    {
        int expectedCount = 2;

        var req = new Request
        {
            Method = HttpMethod.GET,
            Path = $"/boards/recent?userId={userId}"
        };

        var res = _router.Handle(req);

        Assert.AreEqual(HttpStatus.OK, res.StatusCode);
        var result = ((IEnumerable<RecentBoardDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
        Assert.IsTrue(result.All(b => string.Equals(b.BoardStatus, BoardStatus.ACTIVE.ToString())));
    }
    [TestMethod]
    public void GetBoardsAsMemberTest()
    {
        int expectedCount = 2;

        var req = new Request
        {
            Method = HttpMethod.GET,
            Path = $"/workspaces/users/boards/member",
            Params = { 
                ["workspaceId"] = workspaceId.ToString(),
                ["userId"] = userId.ToString()
            }
        };
        var res = _router.Handle(req);


        Assert.AreEqual(HttpStatus.OK, res.StatusCode);
        var result = ((IEnumerable<BoardWithWorkspaceDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
    }

    [TestMethod]
    public void GetBoardsAsOwnerTest()
    {
        int expectedCount = 2;

        var req = new Request
        {
            Method = HttpMethod.GET,
            Path = $"/workspaces/{workspaceId}/users/{userId}/boards/owner",
        };

        var res = _router.Handle(req);

        Assert.AreEqual(HttpStatus.OK, res.StatusCode);
        var result = ((IEnumerable<BoardWithWorkspaceDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
    }
}
