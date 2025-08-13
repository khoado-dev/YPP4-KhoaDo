using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Tests
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IWorkspaceRepository _workspaceRepository;

        public WorkspaceService(IWorkspaceRepository workspaceRepository)
        {
            _workspaceRepository = workspaceRepository;
        }

        public WorkspaceDetailDTO GetWorkspaceDetailById(int workspaceId)
        {
            return _workspaceRepository.GetWorkspaceDetailById(workspaceId);
        }

        public IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId)
        {
            return _workspaceRepository.GetWorkspacesByUserId(userId);
        }

        public IEnumerable<WorkspaceTypeDTO> GetWorkspaceTypes()
        {
            return _workspaceRepository.GetWorkspaceTypes();
        }
    }
}