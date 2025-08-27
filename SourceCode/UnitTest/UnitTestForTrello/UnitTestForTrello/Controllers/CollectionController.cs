using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class CollectionController
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
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