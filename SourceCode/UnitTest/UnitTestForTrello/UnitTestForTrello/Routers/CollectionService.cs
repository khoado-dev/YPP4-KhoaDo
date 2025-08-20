using UnitTestForTrello.Models.DTOs;
namespace UnitTestForTrello.Routers
{
    public class CollectionService
    {
        private readonly CollectionRepository _collectionRepository;

        public CollectionService(CollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
        }

        internal IEnumerable<BoardWithCollectionDTO>? GetBoardsWithCollectionsInWorkspace(int workspaceId)
        {
            return _collectionRepository.GetBoardsWithCollectionsInWorkspace(workspaceId);
        }

        internal IEnumerable<CollectionDTO>? GetCollectionsByWorkspace(int workspaceId)
        {
            return _collectionRepository.GetCollectionsByWorkspace(workspaceId);
        }
    }
}