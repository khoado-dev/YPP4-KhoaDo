using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Services.IServices
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public IEnumerable<StarredBoardDTO> GetStarredBoards(int userId)
        {
            return _boardRepository.GetStarredBoardsByUser(userId);
        }

        public IEnumerable<RecentlyBoardDTO> GetRecentlyBoards(int userId)
        {
            return _boardRepository.GetRecentlyBoardsByUser(userId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWithWorkspaceByUser(int userId)
        {
            return _boardRepository.GetBoardsWithWorkspaceByUser(userId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsByUserIdAndWorkspaceId(int loggeddInUserId, int workspaceId)
        {
            return _boardRepository.GetBoardsByUserIdAndWorkspaceId(loggeddInUserId, workspaceId);
        }
    }
}
