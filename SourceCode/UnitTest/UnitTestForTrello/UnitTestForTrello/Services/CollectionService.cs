using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;
namespace UnitTestForTrello.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository;

        public CollectionService(ICollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
        }

        public IEnumerable<BoardWithCollectionDTO>? GetBoardsWithCollectionsInWorkspace(int workspaceId)
        {
            return _collectionRepository.GetBoardsWithCollectionsInWorkspace(workspaceId);
        }

        public IEnumerable<CollectionDTO>? GetCollectionsByWorkspace(int workspaceId)
        {
            return _collectionRepository.GetCollectionsByWorkspace(workspaceId);
        }
    }
}