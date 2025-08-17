using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Routers;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;
namespace UnitTestForTrello;

[TestClass]
public class BoardRouterTest
{
    private Router _router = null!;

    private const int userId = 1;
    private const int workspaceId = 1;
    private const string ACTIVE_STATUS = "active";

    [TestInitialize]
    public void Setup()
    {
        _router = TestStartUp.CreateRouter();
    }

    [TestMethod]
    public void GetStarredBoards()
    {
        int expectedCount = 2;

        var req = new Request()
        {
            Method = HttpMethod.GET,
            Path = $"/boards/starred/{userId}"
        };

        var res = _router.Handle(req);

        Assert.AreEqual(HttpStatus.OK, res.StatusCode);     
        var result = ((IEnumerable<StarredBoardDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
        Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_STATUS && b.StarredBoardsStatus));
    }

    [TestMethod]
    public void GetRecentlyBoards()
    {
        int expectedCount = 2;

        var req = new Request()
        {
            Method = HttpMethod.GET,
            Path = $"/boards/recent/{userId}"
        };

        var res = _router.Handle(req);

        Assert.AreEqual(HttpStatus.OK, res.StatusCode);
        var result = ((IEnumerable<RecentlyBoardDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
        Assert.IsTrue(result.All(b => b.BoardStatus == ACTIVE_STATUS));
    }


    [TestMethod]
    public void GetBoardsWhereUserIsMemberInWorkspace()
    {
        int expectedCount = 2;

        var req = new Request()
        {
            Method = HttpMethod.GET,
            Path = $"/workspaces/{workspaceId}/users/{userId}/boards/member"
        };

        var res = _router.Handle(req);

        Assert.AreEqual(HttpStatus.OK, res.StatusCode);
        var result = ((IEnumerable<BoardWithWorkspaceDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
    }


    [TestMethod]
    public void GetBoardsWhereUserIsOwnerInWorkspace()
    {
        int expectedCount = 2;

        var req = new Request()
        {
            Method = HttpMethod.GET,
            Path = $"/workspaces/{workspaceId}/users/{userId}/boards/owner"
        };

        var res = _router.Handle(req);

        Assert.AreEqual(HttpStatus.OK, res.StatusCode);
        var result = ((IEnumerable<BoardWithWorkspaceDTO>)res.Body!).ToList();
        Assert.AreEqual(expectedCount, result.Count);
    }

}
