using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services;

namespace UnitTestForTrello.Controllers
{
    public class CollectionController
    {
        private readonly CollectionService _collectionService;

        public CollectionController(CollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public IEnumerable<BoardWithCollectionDTO>? GetBoardsWithCollectionsInWorkspace(int workspaceId)
        {
            return _collectionService.GetBoardsWithCollectionsInWorkspace(workspaceId);
        }

        public IEnumerable<CollectionDTO>? GetCollectionsByWorkspace(int workspaceId)
        {
            return _collectionService.GetCollectionsByWorkspace(workspaceId);
        }
    }
}