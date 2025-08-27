using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface ICollectionRepository
    {
        IEnumerable<BoardWithCollectionDTO>? GetBoardsWithCollectionsInWorkspace(int workspaceId);
        IEnumerable<CollectionDTO>? GetCollectionsByWorkspace(int workspaceId);
    }
}
