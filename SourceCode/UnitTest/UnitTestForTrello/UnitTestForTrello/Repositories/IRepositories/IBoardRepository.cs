using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IBoardRepository
    {
        public IEnumerable<StarredBoardDTO> GetStarredBoardsByUser(int userId);
        public IEnumerable<RecentlyBoardDTO> GetRecentlyBoardsByUser(int userId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWithWorkspaceByUser(int userId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsByUserIdAndWorkspaceId(int loggeddInUserId, int workspaceId);
    }
}
