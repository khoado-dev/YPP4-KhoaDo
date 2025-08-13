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

        public IEnumerable<RecentlyBoardDTO> GetRecentlyBoards(int userId)
        {
            return _boardService.GetRecentlyBoards(userId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsMemberInWorkspace(int userId, int workspaceId)
        {
            return _boardService.GetBoardsWhereUserIsMemberInWorkspace(userId, workspaceId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsOwnerInWorkspace(int userId, int workspaceId)
        {
            return _boardService.GetBoardsWhereUserIsOwnerInWorkspace(userId, workspaceId);
        }
    }
}
