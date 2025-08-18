using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class BoardController
    {
        private readonly IBoardService _boardService;
        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }
        public IEnumerable<StarredBoardDTO> GetStarredBoards(int userId)
        {
            return _boardService.GetStarredBoards(userId);
        }

        public IEnumerable<RecentBoardDTO> GetRecentBoards(int userId)
        {
            return _boardService.GetRecentBoards(userId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsMemberInWorkspace(int userId, int workspaceId)
        {
            return _boardService.GetBoardsAsMember(userId, workspaceId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsOwnerInWorkspace(int userId, int workspaceId)
        {
            return _boardService.GetBoardsAsOwner(userId, workspaceId);
        }
    }
}
