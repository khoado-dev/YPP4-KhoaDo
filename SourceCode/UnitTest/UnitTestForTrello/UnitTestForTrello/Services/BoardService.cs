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
            return _boardRepository.GetStarredBoards(userId);
        }

        public IEnumerable<RecentBoardDTO> GetRecentBoards(int userId)
        {
            return _boardRepository.GetRecentBoards(userId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsAsMember(int userId, int workspaceId)
        {
            return _boardRepository.GetBoardsAsMember(userId, workspaceId);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsAsOwner(int userId, int workspaceId)
        {
            return _boardRepository.GetBoardsAsOwner(userId, workspaceId);
        }
    }
}
