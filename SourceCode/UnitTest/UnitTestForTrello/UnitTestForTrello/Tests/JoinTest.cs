using System.Data.Common;
using UnitTestForTrello.Models;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Tests;

namespace UnitTestForTrello;

[TestClass]
public class JoinTest : DatabaseTestBase
{
    private JoinRepository _joinRepository;

    [TestInitialize]
    public void Setup()
    {
        base.BeginTransaction();
        _joinRepository = new JoinRepository(_connection!, _transaction!);
    }

    [TestMethod]
    public void TestBoardInnerJoinWorkspace()
    {
        var boards = new List<Board>
        {
            new Board { Id = 1, BoardName = "Board1", WorkspaceId = 1 },
            new Board { Id = 2, BoardName = "Board2", WorkspaceId = 2 },
            new Board { Id = 3, BoardName = "Board3", WorkspaceId = 3 }
        };

        var workspaces = new List<Workspace>
        {
            new Workspace { Id = 1, WorkspaceName = "Workspace1" },
            new Workspace { Id = 2, WorkspaceName = "Workspace2" }
        };

        var result = _joinRepository.BoardInnerJoinWorkspace(boards, workspaces);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Workspace1", result[0].Workspace.WorkspaceName);
        Assert.AreEqual("Workspace2", result[1].Workspace.WorkspaceName);
    }

    [TestMethod]
    public void TestBoardLeftJoinWorkspace()
    {
        var boards = new List<Board>
        {
            new Board { Id = 1, BoardName = "Board1", WorkspaceId = 1 },
            new Board { Id = 2, BoardName = "Board2", WorkspaceId = 2 },
            new Board { Id = 3, BoardName = "Board3", WorkspaceId = 3 }
        };

        var workspaces = new List<Workspace>
        {
            new Workspace { Id = 1, WorkspaceName = "Workspace1" },
            new Workspace { Id = 2, WorkspaceName = "Workspace2" }
        };

        var result = _joinRepository.BoardLeftJoinWorkspace(boards, workspaces);

        Assert.AreEqual(3, result.Count);
        Assert.IsNotNull(result[0].Workspace);
        Assert.IsNotNull(result[1].Workspace);
        Assert.IsNull(result[2].Workspace);
    }
    [TestMethod]
    public void TestWorkspaceRightJoinBoard()
    {
        var boards = new List<Board>
        {
            new Board { Id = 1, BoardName = "Board1", WorkspaceId = 1 },
            new Board { Id = 2, BoardName = "Board2", WorkspaceId = 2 },
            new Board { Id = 3, BoardName = "Board3", WorkspaceId = 3 },
            new Board { Id = 4, BoardName = "Board4", WorkspaceId = 1 }
        };

        var workspaces = new List<Workspace>
        {
            new Workspace { Id = 1, WorkspaceName = "Workspace1" },
            new Workspace { Id = 2, WorkspaceName = "Workspace2" },
            new Workspace { Id = 4, WorkspaceName = "Workspace4" }
        };

        var result = _joinRepository.WorkspaceRightJoinBoard(boards, workspaces);

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(2, result[0].Boards.Count); // Workspace1 has Board1, Board4
        Assert.AreEqual("Board1", result[0].Boards[0].BoardName);
        Assert.AreEqual(1, result[1].Boards.Count); // Workspace2 has Board2
        Assert.AreEqual("Board2", result[1].Boards[0].BoardName);
        Assert.AreEqual(0, result[2].Boards.Count); // Workspace4 has no boards
    }


    [TestMethod]
    public void TestBoardFullOuterJoinWorkspace()
    {
        var boards = new List<Board>
            {
                new Board { Id = 1, BoardName = "Board1", WorkspaceId = 1 },
                new Board { Id = 2, BoardName = "Board2", WorkspaceId = 2 },
                new Board { Id = 3, BoardName = "Board3", WorkspaceId = 3 }
            };

        var workspaces = new List<Workspace>
            {
                new Workspace { Id = 1, WorkspaceName = "Workspace1" },
                new Workspace { Id = 2, WorkspaceName = "Workspace2" },
                new Workspace { Id = 4, WorkspaceName = "Workspace4" }
            };

        var result = _joinRepository.BoardFullOuterJoinWorkspace(boards, workspaces);

        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("Workspace1", result[0].Workspace?.WorkspaceName);
        Assert.AreEqual("Workspace2", result[1].Workspace?.WorkspaceName);
        Assert.IsNull(result[2].Workspace); // Board3 has no workspace
        Assert.AreEqual("Workspace4", result[3].Workspace?.WorkspaceName); // Workspace4 has no board
    }

    [TestMethod]
    public void TestBoardCrossJoinWorkspace()
    {
        var boards = new List<Board>
            {
                new Board { Id = 1, BoardName = "Board1", WorkspaceId = 1 },
                new Board { Id = 2, BoardName = "Board2", WorkspaceId = 2 }
            };

        var workspaces = new List<Workspace>
            {
                new Workspace { Id = 1, WorkspaceName = "Workspace1" },
                new Workspace { Id = 2, WorkspaceName = "Workspace2" }
            };

        var result = _joinRepository.BoardCrossJoinWorkspace(boards, workspaces);

        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("Workspace1", result[0].Workspace.WorkspaceName);
        Assert.AreEqual("Workspace2", result[1].Workspace.WorkspaceName);
        Assert.AreEqual("Workspace1", result[2].Workspace.WorkspaceName);
        Assert.AreEqual("Workspace2", result[3].Workspace.WorkspaceName);
    }

}
