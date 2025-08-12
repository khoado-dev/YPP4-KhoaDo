using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Repositories
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
    }
}
