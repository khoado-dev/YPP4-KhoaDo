using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories
{
    public class BoardService
    {
        private readonly BoardRepository _boardRepository;

        public BoardService(BoardRepository boardRepository)
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
