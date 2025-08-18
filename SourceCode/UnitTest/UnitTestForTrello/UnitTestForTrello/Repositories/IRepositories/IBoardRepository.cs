using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IBoardRepository
    {
        public IEnumerable<StarredBoardDTO> GetStarredBoards(int userId);
        public IEnumerable<RecentBoardDTO> GetRecentBoards(int userId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsAsMember(int userId, int workspaceId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsAsOwner(int loggeddInUserId, int workspaceId);
    }
}
