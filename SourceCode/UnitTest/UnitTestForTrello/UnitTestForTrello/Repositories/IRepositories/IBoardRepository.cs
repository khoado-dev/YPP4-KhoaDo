using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IBoardRepository
    {
        public IEnumerable<StarredBoardDTO> GetStarredBoardsByUser(int userId);
        public IEnumerable<RecentlyBoardDTO> GetRecentlyBoardsByUser(int userId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsMemberInWorkspace(int userId, int workspaceId);
        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsOwnerInWorkspace(int loggeddInUserId, int workspaceId);
    }
}
