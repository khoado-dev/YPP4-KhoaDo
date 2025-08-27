using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface ICollectionService
    {
        IEnumerable<BoardWithCollectionDTO>? GetBoardsWithCollectionsInWorkspace(int workspaceId);
        IEnumerable<CollectionDTO>? GetCollectionsByWorkspace(int workspaceId);
    }
}
