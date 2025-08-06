using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestForTrello.Models;

namespace UnitTestForTrello.Repositories
{
    public class JoinRepository
    {
        private SqlConnection sqlConnection;
        private SqlTransaction sqlTransaction;

        public JoinRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            this.sqlConnection = sqlConnection;
            this.sqlTransaction = sqlTransaction;
        }

        public List<Board> BoardInnerJoinWorkspace(List<Board> boards, List<Workspace> workspaces)
        {
            var result = new List<Board>();
            for (int i = 0; i < boards.Count; i++)
            {
                Board currentBoard = boards[i];
                for (int j = 0; j < workspaces.Count; j++)
                {
                    Workspace currentWorkspace = workspaces[j];
                    if (currentBoard.WorkspaceId == currentWorkspace.Id)
                    {
                        currentBoard.Workspace = currentWorkspace;
                        result.Add(currentBoard);
                    }
                }
            }
            return result;
        }
        public List<Board> BoardLeftJoinWorkspace(List<Board> boards, List<Workspace> workspaces)
        {
            var result = new List<Board>();
            for (int i = 0; i < boards.Count; i++)
            {
                Board board = boards[i];
                Workspace? matchedWorkspace = null;
                for (int j = 0; j < workspaces.Count; j++)
                {
                    Workspace ws = workspaces[j];
                    if (board.WorkspaceId == ws.Id)
                    {
                        matchedWorkspace = ws;
                        break;
                    }
                }
                board.Workspace = matchedWorkspace;
                result.Add(board);
            }
            return result;
        }
        public List<Workspace> WorkspaceRightJoinBoard(List<Board> boards, List<Workspace> workspaces)
        {
            var result = new List<Workspace>();
            for (int i = 0; i < workspaces.Count; i++)
            {
                Workspace workspace = workspaces[i];
                List<Board> matchedBoards = new List<Board>();
                for (int j = 0; j < boards.Count; j++)
                {
                    Board board = boards[j];
                    if (board.WorkspaceId == workspace.Id)
                    {
                        matchedBoards.Add(board);
                    }
                }
                workspace.Boards = matchedBoards;
                result.Add(workspace);
            }
            return result;
        }
        public List<Board> BoardFullOuterJoinWorkspace(List<Board> boards, List<Workspace> workspaces)
        {
            var result = new List<Board>();
            var workspaceIds = new HashSet<int>(workspaces.Select(w => w.Id));
            var boardIds = new HashSet<int>(boards.Select(b => b.WorkspaceId ?? -1));

            // Left join part
            foreach (var board in boards)
            {
                var ws = workspaces.FirstOrDefault(w => w.Id == board.WorkspaceId);
                board.Workspace = ws;
                result.Add(board);
            }

            // Right join part (boards for workspaces with no matching board)
            foreach (var workspace in workspaces)
            {
                if (!boardIds.Contains(workspace.Id))
                {
                    var board = new Board
                    {
                        Workspace = workspace
                    };
                    result.Add(board);
                }
            }

            return result;
        }

        public List<Board> BoardCrossJoinWorkspace(List<Board> boards, List<Workspace> workspaces)
        {
            var result = new List<Board>();
            foreach (var board in boards)
            {
                foreach (var workspace in workspaces)
                {
                    var newBoard = new Board
                    {
                        Id = board.Id,
                        BoardName = board.BoardName,
                        BoardDescription = board.BoardDescription,
                        CreatedAt = board.CreatedAt,
                        CreatedBy = board.CreatedBy,
                        UpdatedAt = board.UpdatedAt,
                        UpdatedBy = board.UpdatedBy,
                        BackgroundUrl = board.BackgroundUrl,
                        WorkspaceId = board.WorkspaceId,
                        Workspace = workspace
                    };
                    result.Add(newBoard);
                }
            }
            return result;
        }
        
    }
}
